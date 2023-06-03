public static class ApplicationQuitFix
{
    #if !UNITY_EDITOR
    [UnityEngine.RuntimeInitializeOnLoadMethod]
    private static void Init()
    {
        UnityEngine.Application.quitting += OnApplicationQuit;
    }

    private static void OnApplicationQuit()
    {
        if (!UnityEngine.Application.isEditor)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
    #endif
}