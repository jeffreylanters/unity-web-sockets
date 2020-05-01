using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

namespace ElRaccoone.WebSockets {
  public class WSConnection {

    private int receiveChunkSize = 1024 * 10000;
    private int sendChunkSize = 1024 * 10000;

    private ClientWebSocket clientWebSocket;
    private Uri uri;

    private bool hasOnConnected;
    private bool hasOnDisconnected;
    private bool hasOnError;
    private bool hasOnMessage;
    private Action onConnect;
    private Action onDisconnected;
    private Action<string> onError;
    private Action<string> onMessage;

    public static List<WSConnection> instances = new List<WSConnection> ();

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
      WSConnection.instances.Add (this);
    }

    public async void Connect () {
      this.clientWebSocket = new ClientWebSocket ();
      this.clientWebSocket.Options.AddSubProtocol ("Tls");
      try {
        await this.clientWebSocket.ConnectAsync (this.uri, CancellationToken.None);
        this.isConnected = true;
        if (this.hasOnConnected == true)
          this.onConnect ();
        this.AwaitWebSocketMessage ();
      } catch (Exception exception) {
        if (this.hasOnError == true) {
          this.onError (exception.Message);
          if (exception.InnerException != null)
            this.onError (exception.InnerException.Message);
        }
      }
    }

    public void Disconnect () {
      this.isConnected = false;
      if (this.hasOnDisconnected == true)
        this.onDisconnected ();
    }

    public async void SendMessage (string message) {
      if (this.isConnected == false)
        return;
      var _bytesToSend = new ArraySegment<byte> (
        Encoding.UTF8.GetBytes (message));
      await this.clientWebSocket.SendAsync (
        _bytesToSend,
        WebSocketMessageType.Text,
        true,
        CancellationToken.None);
    }

    private async void AwaitWebSocketMessage () {
      var _buffer = new ArraySegment<byte> (new byte[1024]);
      var _bytes = new List<byte> ();
      var _result = null as WebSocketReceiveResult;

      do {
        _result = await this.clientWebSocket.ReceiveAsync (_buffer, CancellationToken.None);
        for (int i = 0; i < _result.Count; i++)
          _bytes.Add (_buffer.Array[i]);
        if (this.isConnected == false) {
          this.clientWebSocket.Dispose ();
          return;
        }
      }
      while (!_result.EndOfMessage);

      if (_result.MessageType == WebSocketMessageType.Close) {
        if (this.hasOnDisconnected == true)
          this.onDisconnected ();
      } else {
        if (this.hasOnMessage == true)
          this.onMessage (Encoding.UTF8.GetString (_bytes.ToArray (), 0, _bytes.Count));
        this.AwaitWebSocketMessage ();
      }
    }

    public void SetChunkSize (int receive, int send) {
      this.receiveChunkSize = receive;
      this.sendChunkSize = send;
    }

    public void OnConnected (Action onConnect) {
      this.onConnect = onConnect;
      this.hasOnConnected = true;
    }

    public void OnDisconnected (Action onDisconnected) {
      this.onDisconnected = onDisconnected;
      this.hasOnDisconnected = true;
    }

    public void OnError (Action<string> onError) {
      this.onError = onError;
      this.hasOnError = true;
    }

    public void OnMessage (Action<string> onMessage) {
      this.onMessage = onMessage;
      this.hasOnMessage = true;
    }
  }
}