#if UNITY_EDITOR

using UnityEditor;

namespace ElRaccoone.WebSockets {
  /// This helper class closes all connections when the UnityEditor stops playing.
  [InitializeOnLoadAttribute]
  public static class WSEditor {

    /// Binds the PlayModeStateChanged event.
    static WSEditor () {
      EditorApplication.playModeStateChanged += LogPlayModeState;
    }

    /// When the UnityEditor is exiting play mode, all websocket instances will
    /// disconnect immediatly.
    private static void LogPlayModeState (PlayModeStateChange state) {
      if (state == PlayModeStateChange.ExitingPlayMode)
        if (WSConnection.instances != null)
          foreach (var _wsConnectionInstance in WSConnection.instances)
            _wsConnectionInstance.Disconnect ();
    }
  }
}

#endif