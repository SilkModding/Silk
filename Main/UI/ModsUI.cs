using System.Collections.Generic;
using UnityEngine;

namespace Silk
{
    public class ModsUI
    {
        public static void Initialize()
        {
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

            var modList = new List<(string title, string[] authors, string version, string gameVersion, string id)>
            {
                ("Silk", new[] {"Abstractmelon"}, "1.0.0", "1.6a - QOL", "silk"),
            };

            foreach (var mod in Loader.LoadedMods)
            {
                modList.Add((mod.ModName, mod.ModAuthors, mod.ModVersion, mod.ModGameVersion, mod.ModId));
            }

            foreach (var mod in modList)
            {
                ModsMenu.instance.CreateButton(mod.title, () => {
                    var ui = Announcer.ModsPopup(mod.title);
                    ui.CreateDivider();
                    ui.CreateParagraph($"Authors: {string.Join(", ", mod.authors)}");
                    ui.CreateParagraph($"Version: {mod.version}");
                    ui.CreateParagraph($"Designed for SpiderHeck version: {mod.gameVersion}");
                    ui.CreateParagraph($"ID: {mod.id}");
                });
            }
        }
    }
}

