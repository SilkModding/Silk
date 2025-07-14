using System.Collections.Generic;
using UnityEngine;

namespace Silk
{
    public class ModsUI
    {
        /// <summary>
        /// Initializes the mods UI by enabling the mods button and creating a menu item for each loaded mod.
        /// </summary>
        /// <remarks>
        /// This function should only be called once, after the game has loaded.
        /// </remarks>
        public static void Initialize()
        {
            // Create mods UI
            Logger.LogInfo("Loading mods UI...");

            // Check if UI is already enabled to prevent redundant calls
            if (HudController.instance != null && !HudController.instance.modsButton.activeSelf)
            {
                Logger.LogInfo("Creating mods UI...");
                HudController.instance.EnableModsButton();
            }

            if (ModsMenu.instance == null)
            {
                Logger.LogInfo("Mods Menu not found, skipping mods UI initialization");
                return;
            }

            // Create mods list
            var modList = new List<SilkModData> { SilkModData.GetSilkModData() };
            modList.AddRange(Loader.LoadedMods);

            foreach (var mod in modList)
            {
                ModsMenu.instance.CreateButton(mod.ModName, () => {
                    var ui = Announcer.ModsPopup(mod.ModName);
                    ui.CreateDivider();
                    ui.CreateParagraph($"Authors: {string.Join(", ", mod.ModAuthors)}");
                    ui.CreateParagraph($"Version: {mod.ModVersion}");
                    ui.CreateParagraph($"Designed for SpiderHeck version: {mod.ModSilkVersion}");
                    ui.CreateParagraph($"ID: {mod.ModId}");
                    ui.CreateParagraph($"Networking Type: {mod.ModNetworkingType}");
                });
            }
        }
    }
}

