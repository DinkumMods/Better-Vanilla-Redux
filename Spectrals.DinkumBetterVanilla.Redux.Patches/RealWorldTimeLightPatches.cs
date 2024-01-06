using System;
using HarmonyLib;

namespace Spectrals.DinkumBetterVanilla.Redux.Patches
{
    // Token: 0x0200000E RID: 14
    [HarmonyPatch(typeof(RealWorldTimeLight))]
    public class RealWorldTimeLightPatches
    {
        // Token: 0x06000024 RID: 36 RVA: 0x000030C8 File Offset: 0x000012C8
        [HarmonyPostfix]
        [HarmonyPatch("Start")]
        private static void Start(RealWorldTimeLight __instance)
        {
            bool isServer = __instance.isServer;
            if (isServer)
            {
                bool flag = Math.Abs(__instance.getCurrentSpeed() - Plugin.Instance.InGameTimeSpeed.Value) > 0.01f;
                if (flag)
                {
                    __instance.changeSpeed(Plugin.Instance.InGameTimeSpeed.Value);
                }
            }
        }
    }
}
