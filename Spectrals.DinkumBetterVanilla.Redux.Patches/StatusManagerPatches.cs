using HarmonyLib;

namespace Spectrals.DinkumBetterVanilla.Redux.Patches
{
    // Token: 0x0200000F RID: 15
    [HarmonyPatch(typeof(StatusManager))]
    public class StatusManagerPatches
    {
        // Token: 0x06000026 RID: 38 RVA: 0x00003120 File Offset: 0x00001320
        [HarmonyPrefix]
        [HarmonyPatch("die")]
        private static bool Die(StatusManager __instance)
        {
            bool flag = Plugin.Instance.HealthLostWhenNoMoreStamina.Value > 0 && __instance.tired && __instance.connectedDamge.Networkhealth > 0;
            bool result;
            if (flag)
            {
                __instance.connectedDamge.changeHealth(-Plugin.Instance.HealthLostWhenNoMoreStamina.Value);
                result = false;
            }
            else
            {
                result = true;
            }
            return result;
        }
    }
}
