using System.Collections.Generic;
using Silk;

namespace Silk.Mods
{
    public static class Manager
    {
        public static List<SilkMod> Mods = new List<SilkMod>();

        public static void MainLoop()
        {
            while (true) 
            {
                foreach (var mod in Mods)
                {
                    mod.Update();
                }
            }
        }
    }
}