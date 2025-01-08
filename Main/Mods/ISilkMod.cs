using UnityEngine;

namespace Silk
{
    public abstract class SilkMod : MonoBehaviour
    {
        // Initialize the mod
        public abstract void Initialize();

        // Unload the mod
        public abstract void Unload();
    }
}