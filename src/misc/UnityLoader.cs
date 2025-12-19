namespace Silk
{
    internal class UnityLoader
    {
        public static void Initialize()
        {
            // NOW it's safe to reference Unity types
            Main.HookIntoSceneLoading();
        }
    }
}