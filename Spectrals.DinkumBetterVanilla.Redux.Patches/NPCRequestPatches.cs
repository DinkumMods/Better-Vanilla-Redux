using HarmonyLib;

namespace Spectrals.DinkumBetterVanilla.Redux.Patches
{
    // Token: 0x0200000C RID: 12
    [HarmonyPatch(typeof(NPCRequest))]
    public class NPCRequestPatches
    {
        // Token: 0x06000020 RID: 32 RVA: 0x00003060 File Offset: 0x00001260
        [HarmonyPostfix]
        [HarmonyPatch("getRandomQuest")]
        private static void GetRandomQuest(NPCRequest __instance)
        {
            // FIX#XXX: Updated logic path for Bug and Fish Requests and set bool accordingly. - VaterOtter 12/29/2023
            bool flag = __instance.myRequestType == NPCRequest.requestType.BugRequest || __instance.myRequestType == NPCRequest.requestType.FishRequest;
            if (flag)
            {
                __instance.desiredAmount = 1;
            }
        }
    }
}
