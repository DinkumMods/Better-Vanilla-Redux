using HarmonyLib;

namespace Spectrals.DinkumBetterVanilla.Redux.Patches
{
    // Token: 0x02000006 RID: 6
    [HarmonyPatch(typeof(ChestWindow))]
    public class ChestWindowPatches
    {
        // Token: 0x0600000F RID: 15 RVA: 0x00002740 File Offset: 0x00000940
        [HarmonyPostfix]
        [HarmonyPatch("lockBugsAndFishFromChest")]
        private static void LockBugsAndFishFromChest()
        {
            // FIX#XX: Updated to use Instance - VaterOtter 12/29/2023
            foreach (InventorySlot inventorySlot in Inventory.Instance.invSlots)
            {
                // FIX#XXX: Fixed ref to Inventory.Instance - VaterOtter 12/30/2023
                bool flag = inventorySlot.itemNo >= 0 && ((Plugin.Instance.IsFishStorable.Value && Inventory.Instance.allItems[inventorySlot.itemNo].fish) || (Plugin.Instance.IsBugStorable.Value && Inventory.Instance.allItems[inventorySlot.itemNo].bug));
                if (flag)
                {
                    inventorySlot.clearDisable();
                }
            }
        }
    }
}
