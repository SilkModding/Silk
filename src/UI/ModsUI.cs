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
            var modList = new List<(string title, string[] authors, string version, string gameVersion, string id, NetworkingType networkingType)>
            {
                ("Silk", new[] {"Abstractmelon"}, "1.0.0", "0.5.0", "silk", NetworkingType.None), // Add silk as an always loaded mod (This mod is silk so if this menu is here and silk isnt, something has gone horribly wrong)
            };

            // Add loaded mods
            foreach (var mod in Loader.LoadedMods)
            {
                modList.Add((mod.ModName, mod.ModAuthors, mod.ModVersion, mod.ModSilkVersion, mod.ModId, mod.ModNetworkingType));
            }

            // Create mods menu
            foreach (var mod in modList)
            {
                ModsMenu.instance.CreateButton(mod.title, () => {
                    var ui = Announcer.ModsPopup(mod.title);
                    ui.CreateDivider();
                    ui.CreateParagraph($"Authors: {string.Join(", ", mod.authors)}");
                    ui.CreateParagraph($"Version: {mod.version}");
                    ui.CreateParagraph($"Designed for SpiderHeck version: {mod.gameVersion}");
                    ui.CreateParagraph($"ID: {mod.id}");
                    ui.CreateParagraph($"Networking Type: {mod.networkingType}");
                });
            }
        }
    }
}

