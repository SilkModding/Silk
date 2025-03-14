using System;
using UnityEngine;

namespace Silk.API
{
    public class Cosmetic
    {
        public static void SetSpiderColor(Color primary, Color secondary, bool save = true)
        {
            SpiderCustomizer spiderCustomizer = Player.GetLocalPlayer().GetComponent<SpiderCustomizer>();
            spiderCustomizer.SetSpiderColor(primary, secondary, save);
        }

        public static void SetSpiderHatDefaultConfig()
        {
            SpiderCustomizer spiderCustomizer = Player.GetLocalPlayer().GetComponent<SpiderCustomizer>();
            spiderCustomizer.SetSpiderHatDefaultConfig();
        }

        public static void SetSpiderHat(HatConfig hatConfig, bool save = true)
        {
            SpiderCustomizer spiderCustomizer = Player.GetLocalPlayer().GetComponent<SpiderCustomizer>();
            spiderCustomizer.SetSpiderHat(hatConfig, save);
        }

        public static void DestroyHat()
        {
            SpiderCustomizer spiderCustomizer = Player.GetLocalPlayer().GetComponent<SpiderCustomizer>();
            spiderCustomizer.DestroyHat();
        }

        public static Color GetPrimaryColor()
        {
            SpiderCustomizer spiderCustomizer = Player.GetLocalPlayer().GetComponent<SpiderCustomizer>();
            return spiderCustomizer.GetPrimaryColor();
        }

        public static Color GetSecondaryColor()
        {
            SpiderCustomizer spiderCustomizer = Player.GetLocalPlayer().GetComponent<SpiderCustomizer>();
            return spiderCustomizer.GetSecondaryColor();
        }

        public static HatConfig GetHat()
        {
            SpiderCustomizer spiderCustomizer = Player.GetLocalPlayer().GetComponent<SpiderCustomizer>();
            return spiderCustomizer.GetHat();
        }
    }
}

