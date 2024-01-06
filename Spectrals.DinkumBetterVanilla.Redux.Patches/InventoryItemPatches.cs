using System.Linq;
using HarmonyLib;

namespace Spectrals.DinkumBetterVanilla.Redux.Patches
{
    // Token: 0x02000008 RID: 8
    [HarmonyPatch(typeof(InventoryItem))]
    internal class InventoryItemPatches
    {
        // Token: 0x06000014 RID: 20 RVA: 0x0000285C File Offset: 0x00000A5C
        [HarmonyPrefix]
        [HarmonyPatch("checkIfStackable")]
        private static bool checkIfStackable(InventoryItem __instance, ref bool __result)
        {
            bool flag = __instance.getItemId() > 0 && ((Plugin.Instance.IsFishStackable.Value && __instance.fish) || (Plugin.Instance.IsBugStackable.Value && __instance.bug) || (Plugin.Instance.IsUnderwaterCreatureStackable.Value && __instance.underwaterCreature) || Plugin.Instance.TrapIds.Contains(__instance.getItemId()));
            bool result;
            if (flag)
            {
                __result = true;
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
