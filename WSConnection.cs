using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace UnityPackages.WebSockets {
  public class WSConnection {

    private ClientWebSocket clientWebSocket;
    private Uri uri;

    private Action onConnect;
    private Action<string> onError;

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
        if (this.clientWebSocket.State == WebSocketState.Open) {
          // ArraySegment<byte> bytesToSend = new ArraySegment<byte> (
          // 	Encoding.UTF8.GetBytes ("hello fury from unity")
          // );
          // await this.clientWebSocket.SendAsync (
          // 	bytesToSend,
          // 	WebSocketMessageType.Text,
          // 	true,
          // 	CancellationToken.None
          // );
          // ArraySegment<byte> bytesReceived = new ArraySegment<byte> (new byte[1024]);
          // WebSocketReceiveResult result = await this.clientWebSocket.ReceiveAsync (
          // 	bytesReceived,
          // 	CancellationToken.None
          // );
          // Debug.Log (Encoding.UTF8.GetString (bytesReceived.Array, 0, result.Count));
        }
        this.onConnect ();
      } catch (Exception e) {
        this.onError (e.Message);
        if (e.InnerException != null) {
          this.onError (e.InnerException.Message);
        }
      }
    }

    public void OnConnected (Action onConnect) {
      this.onConnect = onConnect;
    }

    public void OnError (Action<string> onError) {
      this.onError = onError;
    }
  }
}