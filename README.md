# Web Sockets

Unity Web Sockets provides a wrapper for using Web Sockets, an advanced technology that allows real-time interactive communication between the client browser and a server. It uses a completely different protocol that allows bidirectional data flow, making it unique against HTTP.

> NOTE When using this Unity Package, make sure to **Star** this repository. When using any of the packages please make sure to give credits to **Jeffrey Lanters** somewhere in your app or game. **These packages are not allowed to be sold anywhere!**

## Install

```
"com.unity-packages.web-sockets": "git+https://github.com/unity-packages/web-sockets"
```

[Click here to read the Unity Packages installation guide](https://github.com/unity-packages/installation)

## Dependencies

- CSharp 4.x (You can change this in Unitys 'Player settings')

## Usage

```cs
using UnityEngine;
using UnityPackages.WebSockets;

public class SocketService : MonoBehaviour {

  private WSConnection wsConnection = new WSConnection ("wss://localhost:3000");

  private void Awake () {
    this.wsConnection.OnConnected (() => {
      Debug.Log ("WS Connected!");
    });

    this.wsConnection.OnDisconnected (() => {
      Debug.Log ("WS Disconnected!");
    });

    this.wsConnection.OnError (error => {
      Debug.Log ("WS Error " + error);
    });

    this.wsConnection.OnMessage (message => {
      Debug.Log ("Receives message " + message);
    });

    this.wsConnection.Connect ();

    // Sending messages
    this.wsConnection.SendMessage("Hi!");
  }

  private void OnDestroy () {
    this.wsConnection.Disconnect ();
  }
}
```
