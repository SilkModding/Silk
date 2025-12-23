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
        /// Unloads the mod. Override this method if your mod needs cleanup logic.
        /// </summary>
        public virtual void Unload()
        {
            // Default implementation does nothing - mods can override if needed
        }
    }
}

