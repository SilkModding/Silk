using UnityEngine;

namespace Silk
{
    /// <summary>
    /// The base class for all silk mods.
    /// </summary>
    public abstract class SilkMod : MonoBehaviour
    {
        /// <summary>
        /// Initializes the mod.
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Unloads the mod.
        /// </summary>
        public abstract void Unload();
    }
}

