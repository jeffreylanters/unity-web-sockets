# Web Sockets

Unity Web Sockets provides an wrapper for advanced technology that allows real-time interactive communication between the client browser and a server. It uses a completely different protocol that allows bidirectional data flow, making it unique against HTTP.

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
    wsConnection.Connect ();

    wsConnection.OnConnected (() => {
      Debug.Log ("WS Connected!");
    });

    wsConnection.OnError (error => {
      Debug.Log ("WS Error " + error);
    });

    wsConnection.OnMessage (message => {
      Debug.Log ("Receives message " + message.name + "\nWith data " + message.data);
    });
  }
}
```
