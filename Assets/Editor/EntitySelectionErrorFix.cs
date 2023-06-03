using UnityEditor;


// This script fixes exceptions thrown in Editor when you dispose world and inspector has selected entity from it
namespace Editor
{
    [InitializeOnLoad]
    public static class EntitySelectionErrorFix
    {
        static EntitySelectionErrorFix()
        {
            EditorApplication.playModeStateChanged += LogPlayModeState;
        }

        private static void LogPlayModeState(PlayModeStateChange state)
        {
            Selection.activeObject = null;
        }
    }
}