<div align="center">

![readme splash](https://raw.githubusercontent.com/jeffreylanters/unity-web-sockets/master/.github/WIKI/repository-readme-splash.png)

[![license](https://img.shields.io/badge/mit-license-red.svg?style=for-the-badge)](https://github.com/jeffreylanters/unity-web-sockets/blob/master/LICENSE.md)
[![openupm](https://img.shields.io/npm/v/nl.elraccoone.web-sockets?label=UPM&registry_uri=https://package.openupm.com&style=for-the-badge&color=232c37)](https://openupm.com/packages/nl.elraccoone.web-sockets/)
[![build](https://img.shields.io/badge/build-passing-brightgreen.svg?style=for-the-badge)](https://github.com/jeffreylanters/unity-web-sockets/actions)
[![deployment](https://img.shields.io/badge/state-success-brightgreen.svg?style=for-the-badge)](https://github.com/jeffreylanters/unity-web-sockets/deployments)
[![stars](https://img.shields.io/github/stars/jeffreylanters/unity-web-sockets.svg?style=for-the-badge&color=fe8523&label=stargazers)](https://github.com/jeffreylanters/unity-web-sockets/stargazers)
[![awesome](https://img.shields.io/badge/listed-awesome-fc60a8.svg?style=for-the-badge)](https://github.com/jeffreylanters/awesome-unity-packages)
[![size](https://img.shields.io/github/languages/code-size/jeffreylanters/unity-web-sockets?style=for-the-badge)](https://github.com/jeffreylanters/unity-web-sockets/blob/master/Runtime)
[![sponsors](https://img.shields.io/github/sponsors/jeffreylanters?color=E12C9A&style=for-the-badge)](https://github.com/sponsors/jeffreylanters)
[![donate](https://img.shields.io/badge/donate-paypal-F23150?style=for-the-badge)](https://paypal.me/jeffreylanters)

Unity Web Sockets provides a wrapper for using Web Sockets, an advanced technology that allows real-time interactive communication between the client browser and a server. It uses a completely different protocol that allows bidirectional data flow, making it unique against HTTP.

[**Installation**](#installation) &middot;
[**Documentation**](#documentation) &middot;
[**License**](./LICENSE.md)

**Made with &hearts; by Jeffrey Lanters**

</div>

# Installation

### Using the Unity Package Manager

Install the latest stable release using the Unity Package Manager by adding the following line to your `manifest.json` file located within your project's Packages directory, or by adding the Git URL to the Package Manager Window inside of Unity.

```json
"nl.elraccoone.web-sockets": "git+https://github.com/jeffreylanters/unity-web-sockets"
```

### Using OpenUPM

The module is availble on the OpenUPM package registry, you can install the latest stable release using the OpenUPM Package manager's Command Line Tool using the following command.

```sh
openupm add nl.elraccoone.web-sockets
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
