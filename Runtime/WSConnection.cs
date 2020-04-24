using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace ElRaccoone.WebSockets {
  public class WSConnection {

    private int receiveChunkSize = 1024 * 10000;
    private int sendChunkSize = 1024 * 10000;

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
      var _buffer = new ArraySegment<byte> (new byte[1024]);
      var _bytes = new List<byte> ();

      WebSocketReceiveResult _result = null;

      do {
        _result = await this.clientWebSocket.ReceiveAsync (_buffer, CancellationToken.None);
        for (int i = 0; i < _result.Count; i++)
          _bytes.Add (_buffer.Array[i]);
      }
      while (!_result.EndOfMessage);
      var _text = Encoding.UTF8.GetString (_bytes.ToArray (), 0, _bytes.Count);

      this.onMessage (_text);
      this.AwaitWebSocketMessage ();
    }

    public void SetChunkSize (int receive, int send) {
      this.receiveChunkSize = receive;
      this.sendChunkSize = send;
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