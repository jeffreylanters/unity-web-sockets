using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

namespace ElRaccoone.WebSockets {

  /// The WebSocket Connection class creates a WebSocket connection and handles
  /// all in and out going messages.
  public class WSConnection {

    /// Fixed chunk size for incomming messages.
    private int receiveChunkSize = 1024;

    /// Fixed check size for outgoing messages.
    private int sendChunkSize = 1024;

    /// Client WebSocket instance.
    private ClientWebSocket clientWebSocket;

    /// WebSocket server Uri.
    private Uri uri;

    /// Assigns the event listener raised when the WebSockets are connected.
    public void OnConnected (Action action) => this.hasOnConnected = (this.onConnect = action) != null;

    /// Defines whether the OnConnected event is assigned.
    private bool hasOnConnected;

    /// Callback Action invoked when the connection is estabalished.
    private Action onConnect;

    /// Assigns the event listener raised when the WebSockets are being disconnected.
    public void OnDisconnected (Action action) => this.hasOnDisconnected = (this.onDisconnected = action) != null;

    /// Defines whether the OnDisconnected event is assigned.
    private bool hasOnDisconnected;

    /// Callback Action invoked when the connection is ended.
    private Action onDisconnected;

    /// Assigns the event listener raised when the WebSockets do error.
    public void OnError (Action<string> action) => this.hasOnError = (this.onError = action) != null;

    /// Defines whether the OnError event is assigned.
    private bool hasOnError;

    /// Callback Action invoked when the connection did error.
    private Action<string> onError;

    /// Assigns the event listener raised when the WebSockets receive a message.
    public void OnMessage (Action<string> action) => this.hasOnMessage = (this.onMessage = action) != null;

    /// Defines whether the OnMessage event is assigned.
    private bool hasOnMessage;

    /// Callback Action invoked when the connection receives a message.
    private Action<string> onMessage;

    /// Defines whether the message queue is dispatching.
    private bool isMessageQueueDispatching = false;

    /// A queue containing all the messages that are queued to be sendt.
    private List<string> sendMessageQueue = new List<string> ();

    /// A list of all WebSocket Connection instances.
    public static List<WSConnection> instances = new List<WSConnection> ();

    /// Defines wheter the WebSocket is connected.
    public bool isConnected { private set; get; }

    /// Instanciates a new WebSocket connection.
    public WSConnection (string uri, string subProtocol = "Tls") {
      this.clientWebSocket = new ClientWebSocket ();
      this.clientWebSocket.Options.AddSubProtocol (subProtocol);
      this.uri = new Uri (uri);
      /// Adds the instance to the instaces array.
      WSConnection.instances.Add (this);
    }

    /// Connects to the WebSocket.
    public async void Connect () {
      this.clientWebSocket = new ClientWebSocket ();
      try {
        /// Tries connecting to the sockets async. No cancellation token will be
        /// provided. When connected, the onConnect event listener will be raised.
        await this.clientWebSocket.ConnectAsync (this.uri, CancellationToken.None);
        this.isConnected = true;
        if (this.hasOnConnected == true)
          this.onConnect ();
        /// Starts awaiting web socket messages.
        this.ReceiveWebSocketMessage ();
      } catch (Exception exception) {
        /// Whebnthe connection fails, the onError event listenr will be raised.
        if (this.hasOnError == true) {
          this.onError (exception.Message);
          if (exception.InnerException != null)
            this.onError (exception.InnerException.Message);
        }
      }
    }

    /// Sends a message using the dispatch queue. Messages will always be send
    /// in the order their added.
    public void SendMessage (string message) {
      /// When the web sockets are not connected, we'll stop sending.
      if (this.isConnected == false)
        return;
      /// Adds the message to the message queue.
      this.sendMessageQueue.Add (message);
      /// Starts the dispatch message queue if it's not already.
      if (this.isMessageQueueDispatching == false)
        this.DispatchMessageQueue ();
    }

    /// Dispatches the message queueu, this sends all the messages in the queue
    /// using a async method. Messages will be sendt in the order they're added.
    private async void DispatchMessageQueue () {
      this.isMessageQueueDispatching = true;
      while (true) {
        /// Sends the oldest message from the queueu and removed it afterwards.
        var _bytesToSend = new ArraySegment<byte> (
          Encoding.UTF8.GetBytes (this.sendMessageQueue[0]));
        await this.clientWebSocket.SendAsync (
          _bytesToSend, WebSocketMessageType.Text, true, CancellationToken.None);
        this.sendMessageQueue.RemoveAt (0);
        /// When the message queue is empty, we'll return out of the loop and
        /// set the flag to flase, ready for another message to be queued.
        if (this.sendMessageQueue.Count == 0) {
          this.isMessageQueueDispatching = false;
          return;
        }
      }
    }

    /// Receives WebSocket messages, keeps waiting async until messages are being
    /// received. When parsed and did not contain a close type, the method will
    /// be invoked again to wait for the next message.
    private async void ReceiveWebSocketMessage () {
      var _buffer = new ArraySegment<byte> (new byte[this.receiveChunkSize]);
      var _bytes = new List<byte> ();
      var _result = null as WebSocketReceiveResult;
      /// Awaits a message to be received async. When receiving it will loop
      /// until the message of the end of the message has been received.
      do {
        _result = await this.clientWebSocket.ReceiveAsync (_buffer, CancellationToken.None);
        /// When a message is received it will streaned to the bytes buffer.
        for (int i = 0; i < _result.Count; i++)
          _bytes.Add (_buffer.Array[i]);
        /// If the WebSocket dis connects while receiving a messages, the client
        /// will be disposed and the receiving will be stopped.
        if (this.isConnected == false) {
          this.clientWebSocket.Dispose ();
          return;
        }
      } while (!_result.EndOfMessage);
      /// When the message did receive a close event, the web socket will be set
      /// to disconnected, and the OnDisconnected event will be raised.
      if (_result.MessageType == WebSocketMessageType.Close) {
        if (this.hasOnDisconnected == true) {
          this.isConnected = false;
          this.onDisconnected ();
        }
      }
      /// When the message is of any other type, it will be parsed into a string
      /// and the onMessage event will be raised.
      else {
        if (this.hasOnMessage == true)
          this.onMessage (Encoding.UTF8.GetString (_bytes.ToArray (), 0, _bytes.Count));
        this.ReceiveWebSocketMessage ();
      }
    }

    /// Sets the receive and send chuck sizes. Keep in mind these should be to
    /// the power of two for the best results.
    public void SetChunkSize (int receive, int send) {
      this.receiveChunkSize = receive;
      this.sendChunkSize = send;
    }

    /// Disconnects from the WebSockets.
    public void Disconnect () {
      this.isConnected = false;
      if (this.hasOnDisconnected == true)
        this.onDisconnected ();
    }
  }
}
