using System;
using System.IO;

namespace ExampleMod
{
    public class ModEntryPoint
    {
        public static void Initialize()
        {
            File.WriteAllText("plugin.log", "Hello from ExampleMod!");
        }
    }
}
