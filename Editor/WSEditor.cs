#if UNITY_EDITOR

using UnityEditor;

namespace ElRaccoone.WebSockets {
  // This helper class closes all connections when the Unity editor stops playing.
  [InitializeOnLoadAttribute]
  public static class WSEditor {
    static WSEditor () {
      EditorApplication.playModeStateChanged += LogPlayModeState;
    }

    private static void LogPlayModeState (PlayModeStateChange state) {
      if (state == PlayModeStateChange.ExitingPlayMode)
        if (WSConnection.instances != null)
          foreach (var _wsConnectionInstance in WSConnection.instances)
            _wsConnectionInstance.Disconnect ();
    }
  }
}

#endif