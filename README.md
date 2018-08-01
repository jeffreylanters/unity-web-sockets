# Web Sockets

Unity Web Sockets provides a wrapper for using Web Sockets, an advanced technology that allows real-time interactive communication between the client browser and a server. It uses a completely different protocol that allows bidirectional data flow, making it unique against HTTP.

## Install

```sh
$ git submodule add https://github.com/unity-packages/web-sockets Assets/packages/web-sockets
```

## Requirements

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
      Debug.Log ("Receives message " + message.name + "\nWith data " + message.data);
    });
    
    this.wsConnection.Connect ();
  }
  
  private void OnDestroy () {
    this.wsConnection.Disconnect ();
  }
}
```
