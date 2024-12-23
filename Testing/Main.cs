using UnityEngine;
using System.IO;
using System.Text;
using System;
using Silk;
using System.Collections;
using Logger = Silk.Logger;
using Object = UnityEngine.Object;

namespace TestMod
{
    [SilkMod("SilkMod", "Abstractmelon")]
    public class TestMod : SilkMod
    {
        public void Initialize()
        {
            Logger.LogInfo("Doin cool stuff");
        }
    }
}