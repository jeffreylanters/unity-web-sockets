<div align="center">

<img src="https://raw.githubusercontent.com/elraccoone/unity-web-sockets/master/.github/WIKI/logo.jpg" height="100px"></br>

# Web Sockets

[![npm](https://img.shields.io/badge/upm-1.0.5-232c37.svg?style=for-the-badge)]()
[![license](https://img.shields.io/badge/license-Custom-%23ecc531.svg?style=for-the-badge)](./LICENSE.md)
[![npm](https://img.shields.io/badge/sponsor-donate-E12C9A.svg?style=for-the-badge)](https://paypal.me/jeffreylanters)
[![npm](https://img.shields.io/github/stars/elraccoone/unity-web-sockets.svg?style=for-the-badge)]()

Unity Web Sockets provides a wrapper for using Web Sockets, an advanced technology that allows real-time interactive communication between the client browser and a server. It uses a completely different protocol that allows bidirectional data flow, making it unique against HTTP.

When using this Unity Package, make sure to **Star** this repository. When using any of the packages please make sure to give credits to **Jeffrey Lanters / El Raccoone** somewhere in your app or game. **It it prohibited to distribute, sublicense, and/or sell copies of the Software!**

**&Lt;**
[**Installation**](#installation) &middot;
[**Documentation**](#documentation) &middot;
[**License**](./LICENSE.md) &middot;
[**Sponsor**](https://paypal.me/jeffreylanters)
**&Gt;**

**Made with &hearts; by Jeffrey Lanters**

</div>

## Installation

Install using the Unity Package Manager. add the following line to your `manifest.json` file located within your project's packages directory.

```json
"nl.elraccoone.web-sockets": "git+https://github.com/elraccoone/unity-web-sockets"
```

## Documentation

### Example Usage

```cs
using UnityEngine;
using ElRaccoone.WebSockets;

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
    // A provided editor script closes all connections automatically when you
    // exit play mode. Use this method to close the connection manually.
    this.wsConnection.Disconnect ();
  }
}
```
