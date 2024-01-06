using HarmonyLib;

namespace Spectrals.DinkumBetterVanilla.Redux.Patches
{
    // Token: 0x02000007 RID: 7
    [HarmonyPatch(typeof(GiveNPC))]
    public class GiveNPCPatches
    {
        // Token: 0x06000011 RID: 17 RVA: 0x000027F0 File Offset: 0x000009F0
        [HarmonyPrefix]
        [HarmonyPatch("donateItemToMuseum")]
        private static void DonateItemToMuseumPrefix()
        {
            // FIX#XXX: Updated Inventory reference to Instance. - VaterOtter 12/29/2023
            foreach (InventorySlot inventorySlot in Inventory.Instance.invSlots)
            {
                bool flag = inventorySlot.isSelectedForGive();
                if (flag)
                {
                    GiveNPCPatches.ITEM_NO = inventorySlot.itemNo;
                    GiveNPCPatches.ITEM_STACK = inventorySlot.stack - 1;
                    break;
                }
            }
        }

        // Token: 0x06000012 RID: 18 RVA: 0x00002843 File Offset: 0x00000A43
        [HarmonyPostfix]
        [HarmonyPatch("donateItemToMuseum")]
        private static void DonateItemToMuseumPostfix()
        {
            // FIX#XXX: Update Inventory reference to Instance. - VaterOtter 12/29/2023
            Inventory.Instance.addItemToInventory(GiveNPCPatches.ITEM_NO, GiveNPCPatches.ITEM_STACK, true);
        }

        // Token: 0x04000025 RID: 37
        private static int ITEM_NO;

        // Token: 0x04000026 RID: 38
        private static int ITEM_STACK;
    }
}
