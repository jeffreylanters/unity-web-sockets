using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace UnityPackages.WebSockets {
  public class WSConnection {

    private const int receiveChunkSize = 1024;
    private const int sendChunkSize = 1024;

    private ClientWebSocket clientWebSocket;
    private Uri uri;

    private Action onConnect;
    private Action onDisconnected;
    private Action<string> onError;
    private Action<string> onMessage;

    public bool isConnected;

    public class WSMessage {
      public string name;
      public string data;
      public WSMessage (string name, string data) {
        this.name = name;
        this.data = data;
      }
    }

    public WSConnection (string url) {
      this.clientWebSocket = new ClientWebSocket ();
      this.clientWebSocket.Options.AddSubProtocol ("Tls");
      this.uri = new Uri (url);
    }

    public async void Connect () {
      this.clientWebSocket = new ClientWebSocket ();
      this.clientWebSocket.Options.AddSubProtocol ("Tls");
      try {
        await this.clientWebSocket.ConnectAsync (this.uri, CancellationToken.None);
        this.isConnected = true;
        this.onConnect ();
        this.AwaitWebSocketMessage ();
      } catch (Exception exception) {
        this.onError (exception.Message);
        if (exception.InnerException != null)
          this.onError (exception.InnerException.Message);
      }
    }

    public void Disconnect () {
      // this.clientWebSocket.Abort ();
      // await this.clientWebSocket.CloseAsync (
      //   WebSocketCloseStatus.Empty, "",
      //   CancellationToken.None);
      this.clientWebSocket.Dispose ();
      // this.clientWebSocket = null;
      // await this.clientWebSocket.CloseOutputAsync (
      //   WebSocketCloseStatus.Empty, "",
      //   CancellationToken.None);
      this.isConnected = false;
      this.onDisconnected ();
    }

    public async void SendMessage (string message) {
      if (this.isConnected == false)
        return;
      ArraySegment<byte> bytesToSend = new ArraySegment<byte> (
        Encoding.UTF8.GetBytes (message));
      await this.clientWebSocket.SendAsync (
        bytesToSend,
        WebSocketMessageType.Text,
        true,
        CancellationToken.None);
    }

    private async void AwaitWebSocketMessage () {
      if (this.clientWebSocket.State == WebSocketState.Open) {
        var _bytesReceived = new ArraySegment<byte> (new byte[receiveChunkSize]);
        var _result = await this.clientWebSocket.ReceiveAsync (
          _bytesReceived,
          CancellationToken.None);
        var _data = Encoding.UTF8.GetString (
          _bytesReceived.Array, 0,
          _result.Count);
        this.onMessage (_data);
        this.AwaitWebSocketMessage ();
      } else {
        this.isConnected = false;
        this.onDisconnected ();
      }
    }

    public void OnConnected (Action onConnect) {
      this.onConnect = onConnect;
    }

    public void OnDisconnected (Action onDisconnected) {
      this.onDisconnected = onDisconnected;
    }

    public void OnError (Action<string> onError) {
      this.onError = onError;
    }

    public void OnMessage (Action<string> onMessage) {
      this.onMessage = onMessage;
    }
  }
}