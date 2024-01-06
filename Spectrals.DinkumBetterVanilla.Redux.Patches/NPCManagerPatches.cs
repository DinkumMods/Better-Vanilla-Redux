using HarmonyLib;

namespace Spectrals.DinkumBetterVanilla.Redux.Patches
{
    // Token: 0x0200000B RID: 11
    [HarmonyPatch(typeof(NPCManager))]
    public class NPCManagerPatches
    {
        // Token: 0x0600001E RID: 30 RVA: 0x00002FB8 File Offset: 0x000011B8
        [HarmonyPrefix]
        [HarmonyPatch("resetNPCRequestsForNewDay")]
        private static void ResetNPCRequestsForNewDay(NPCManager __instance)
        {
            bool flag = Plugin.Instance.RelationshipLostOnRequestNotCompleted.Value >= 0;
            if (flag)
            {
                for (int i = 0; i < __instance.NPCDetails.Length; i++)
                {
                    bool flag2 = __instance.npcStatus[i].acceptedRequest && !__instance.npcStatus[i].completedRequest;
                    if (flag2)
                    {
                        __instance.npcStatus[i].addToRelationshipLevel(-Plugin.Instance.RelationshipLostOnRequestNotCompleted.Value);
                        __instance.npcStatus[i].acceptedRequest = false;
                    }
                }
            }
        }
    }
}
