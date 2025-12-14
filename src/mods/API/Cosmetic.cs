using System;
using UnityEngine;

namespace Silk.API
{
    public class Cosmetics
    {
        private static SpiderCustomizer GetCustomizer()
        {
            GameObject player = Player.GetLocalPlayer() 
                ?? throw new InvalidOperationException("Local player not available.");
            
            SpiderCustomizer customizer = player.GetComponent<SpiderCustomizer>();
            if (customizer == null)
                throw new InvalidOperationException("SpiderCustomizer component not found on local player.");

            return customizer;
        }

        public static void SetSpiderColor(Color primary, Color secondary, bool save = true)
        {
            GetCustomizer().SetSpiderColor(primary, secondary, save);
        }

        public static void SetSpiderHatDefaultConfig()
        {
            GetCustomizer().SetSpiderHatDefaultConfig();
        }

        public static void SetSpiderHat(HatConfig hatConfig, bool save = true)
        {
            GetCustomizer().SetSpiderHat(hatConfig, save);
        }

        public static void DestroyHat()
        {
            GetCustomizer().DestroyHat();
        }

        public static Color GetPrimaryColor()
        {
            return GetCustomizer().GetPrimaryColor();
        }

        public static Color GetSecondaryColor()
        {
            return GetCustomizer().GetSecondaryColor();
        }

        public static HatConfig GetHat()
        {
            return GetCustomizer().GetHat();
        }
    }
}
