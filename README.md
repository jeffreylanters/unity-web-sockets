<div align="center">

<img src="https://raw.githubusercontent.com/elraccoone/unity-web-sockets/master/.github/WIKI/logo-transparent.png" height="100px">

</br>

# Web Sockets

[![openupm](https://img.shields.io/npm/v/nl.elraccoone.web-sockets?label=UPM&registry_uri=https://package.openupm.com&style=for-the-badge&color=232c37)](https://openupm.com/packages/nl.elraccoone.web-sockets/)
[![](https://img.shields.io/github/stars/elraccoone/unity-web-sockets.svg?style=for-the-badge)]()
[![](https://img.shields.io/badge/build-passing-brightgreen.svg?style=for-the-badge)]()

Unity Web Sockets provides a wrapper for using Web Sockets, an advanced technology that allows real-time interactive communication between the client browser and a server. It uses a completely different protocol that allows bidirectional data flow, making it unique against HTTP.

**&Lt;**
[**Installation**](#installation) &middot;
[**Documentation**](#documentation) &middot;
[**License**](./LICENSE.md)
**&Gt;**

</br></br>

[![npm](https://img.shields.io/badge/sponsor_the_project-donate-E12C9A.svg?style=for-the-badge)](https://paypal.me/jeffreylanters)

Hi! My name is Jeffrey Lanters, thanks for checking out my modules! I've been a Unity developer for years when in 2020 I decided to start sharing my modules by open-sourcing them. So feel free to look around. If you're using this module for production, please consider donating to support the project. When using any of the packages, please make sure to **Star** this repository and give credit to **Jeffrey Lanters** somewhere in your app or game. Also keep in mind **it it prohibited to sublicense and/or sell copies of the Software in stores such as the Unity Asset Store!** Thanks for your time.

**&Lt;**
**Made with &hearts; by Jeffrey Lanters**
**&Gt;**

</br>

</div>

# Installation

Install the latest stable release using the Unity Package Manager by adding the following line to your `manifest.json` file located within your project's Packages directory.

```json
"nl.elraccoone.web-sockets": "git+https://github.com/elraccoone/unity-web-sockets"
```

# Documentation

## Example Usage

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
      Debug.Log ("Received message " + message);
    });

    this.wsConnection.Connect ();

    // Queue sending messages, these will always be send in this order.
    this.wsConnection.SendMessage("Hello,");
    this.wsConnection.SendMessage("World!");
  }

  private void OnDestroy () {
    // A provided editor script closes all connections automatically when you
    // exit play mode. Use this method to close the connection manually.
    this.wsConnection.Disconnect ();
  }
}
```
