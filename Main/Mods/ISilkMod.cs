namespace Silk
{
    public interface SilkMod
    {
        // Initialize the mod
        void Initialize();

        // Update the mod      
        void Update();

        // Register the mod
        void RegisterMod();

        // Unload the mod
        void Unload();
    }
}



