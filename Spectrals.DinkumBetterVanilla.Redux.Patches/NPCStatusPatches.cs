using System;
using HarmonyLib;

namespace Spectrals.DinkumBetterVanilla.Redux.Patches
{
    // Token: 0x0200000D RID: 13
    [HarmonyPatch(typeof(NPCStatus))]
    public class NPCStatusPatches
    {
        // Token: 0x06000022 RID: 34 RVA: 0x00003090 File Offset: 0x00001290
        [HarmonyPrefix]
        [HarmonyPatch("addToRelationshipLevel")]
        private static void AddToRelationshipLevel(ref int toAdd)
        {
            bool flag = toAdd > 0;
            if (flag)
            {
                toAdd = (int)Math.Floor((double)((float)toAdd * Plugin.Instance.RelationshipGainCoefficient.Value));
            }
        }
    }
}
