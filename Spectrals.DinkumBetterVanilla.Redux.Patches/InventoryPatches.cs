using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TMPro;
using UnityEngine;

namespace Spectrals.DinkumBetterVanilla.Redux.Patches
{
    // Token: 0x02000009 RID: 9
    [HarmonyPatch(typeof(Inventory))]
    internal class InventoryPatches
    {
        // Token: 0x06000016 RID: 22 RVA: 0x000028FC File Offset: 0x00000AFC
        [HarmonyPostfix]
        [HarmonyPatch("Start")]
        private static void Start()
        {
            // FIX#XXX Updated ot use Instance - VaterOtter 12/29/2023
            foreach (InventoryItem inventoryItem in Inventory.Instance.allItems)
            {
                bool flag = inventoryItem.getItemId() > 0 && ((Plugin.Instance.IsFishStackable.Value && inventoryItem.fish) || (Plugin.Instance.IsBugStackable.Value && inventoryItem.bug) || (Plugin.Instance.IsUnderwaterCreatureStackable.Value && inventoryItem.underwaterCreature) || Plugin.Instance.TrapIds.Contains(inventoryItem.getItemId()));
                if (flag)
                {
                    inventoryItem.isStackable = true;
                    inventoryItem.maxStack = 10000;
                    inventoryItem.isUniqueItem = false;
                }
            }
        }

        // Token: 0x06000017 RID: 23 RVA: 0x000029D0 File Offset: 0x00000BD0
        [HarmonyPostfix]
        [HarmonyPatch("fillHoverDescription")]
        [HarmonyPriority(600)]
        private static void FillHoverDescription(ref Inventory __instance, InventorySlot rollOverSlot)
        {
            List<string> list = new List<string>();
            bool flag = !Harmony.HasAnyPatches("Octr_ValueTooltip") && Plugin.Instance.ShouldShowItemValue.Value && (!Plugin.Instance.ItemValueRequiredCommerceLicence.Value || (Plugin.Instance.ItemValueRequiredCommerceLicence.Value && LicenceManager.manage.allLicences[8].isUnlocked && LicenceManager.manage.allLicences[8].getCurrentLevel() > 0));
            if (flag)
            {
                float baseStackValue = (float)((!rollOverSlot.itemInSlot.checkIfStackable()) ? rollOverSlot.itemInSlot.value : (rollOverSlot.itemInSlot.value * rollOverSlot.stack));
                int unlockedLicenseLevel = LicenceManager.manage.allLicences[8].isUnlocked ? LicenceManager.manage.allLicences[8].getCurrentLevel() : 0;
                float licenseLevelBonus = 10f * (float)unlockedLicenseLevel / 100f;
                int stackTotalValue = Mathf.RoundToInt(baseStackValue * (1f + licenseLevelBonus));
                int valuePerItem = Mathf.RoundToInt(stackTotalValue / rollOverSlot.stack);
                // FIX#XXX: updated reference to use proper call of MoneyAmountColorTag - VaterOtter 12/29/2023
                // Feature#001: Added Cost per item line
                //if
                //list.Add(UIAnimationManager.manage.MoneyAmountColorTag("<sprite=11>" + valuePerItem.ToString() + " each"));
                list.Add(UIAnimationManager.manage.MoneyAmountColorTag("<sprite=11> " + stackTotalValue.ToString() + " stack"));
            }
            bool flag2 = !Harmony.HasAnyPatches("Spicy.MuseumTooltip") && Plugin.Instance.ShouldShowIfItemIsDonatedToMuseum.Value;
            if (flag2)
            {
                int num5 = 0;
                int num6 = 0;
                int num7 = 0;
                bool value = Plugin.Instance.IsDonatedToMuseumRequiredSomeDonations.Value;
                if (value)
                {
                    foreach (bool flag3 in MuseumManager.manage.bugsDonated)
                    {
                        bool flag4 = flag3;
                        if (flag4)
                        {
                            num5++;
                        }
                    }
                    foreach (bool flag5 in MuseumManager.manage.fishDonated)
                    {
                        bool flag6 = flag5;
                        if (flag6)
                        {
                            num6++;
                        }
                    }
                    foreach (bool flag7 in MuseumManager.manage.underWaterCreaturesDonated)
                    {
                        bool flag8 = flag7;
                        if (flag8)
                        {
                            num7++;
                        }
                    }
                }
                bool flag9 = rollOverSlot.itemInSlot.bug;
                if (flag9)
                {
                    bool flag10 = !Plugin.Instance.IsDonatedToMuseumRequiredSomeDonations.Value || (Plugin.Instance.IsDonatedToMuseumRequiredSomeDonations.Value && num5 >= 20);
                    if (flag10)
                    {
                        int num8 = MuseumManager.manage.allBugs.IndexOf(rollOverSlot.itemInSlot);
                        bool flag11 = MuseumManager.manage.bugsDonated[num8];
                        if (flag11)
                        {
                            list.Add("<sprite=17> Donated to Museum");
                        }
                        else
                        {
                            list.Add("<sprite=16> Not donated to Museum");
                        }
                    }
                }
                else
                {
                    bool flag12 = rollOverSlot.itemInSlot.fish;
                    if (flag12)
                    {
                        bool flag13 = !Plugin.Instance.IsDonatedToMuseumRequiredSomeDonations.Value || (Plugin.Instance.IsDonatedToMuseumRequiredSomeDonations.Value && num6 >= 20);
                        if (flag13)
                        {
                            int num9 = MuseumManager.manage.allFish.IndexOf(rollOverSlot.itemInSlot);
                            bool flag14 = MuseumManager.manage.fishDonated[num9];
                            if (flag14)
                            {
                                list.Add("<sprite=17> Donated to Museum");
                            }
                            else
                            {
                                list.Add("<sprite=16> Not donated to Museum");
                            }
                        }
                    }
                    else
                    {
                        bool flag15 = rollOverSlot.itemInSlot.underwaterCreature;
                        if (flag15)
                        {
                            bool flag16 = !Plugin.Instance.IsDonatedToMuseumRequiredSomeDonations.Value || (Plugin.Instance.IsDonatedToMuseumRequiredSomeDonations.Value && num7 >= 10);
                            if (flag16)
                            {
                                int num10 = MuseumManager.manage.allUnderWaterCreatures.IndexOf(rollOverSlot.itemInSlot);
                                bool flag17 = MuseumManager.manage.underWaterCreaturesDonated[num10];
                                if (flag17)
                                {
                                    list.Add("<sprite=17> Donated to Museum");
                                }
                                else
                                {
                                    list.Add("<sprite=16> Not donated to Museum");
                                }
                            }
                        }
                    }
                }
            }
            bool flag18 = list.Count > 0;
            if (flag18)
            {
                InventoryPatches.UpdateItemDescription(__instance, rollOverSlot, list);
            }
        }

        // Token: 0x06000018 RID: 24 RVA: 0x00002DE8 File Offset: 0x00000FE8
        private static void UpdateItemDescription(Inventory inventory, InventorySlot inventorySlot, List<string> descriptionsToAdd)
        {
            // FIX#XXX: Fixed reference to get proper Itemid - VaterOtter 12/30/2023
            inventory.InvDescriptionText.text = (inventorySlot.itemInSlot.getItemDescription(inventorySlot.itemInSlot.getItemId()) ?? "");
            foreach (string str in descriptionsToAdd)
            {
                TextMeshProUGUI invDescriptionText = inventory.InvDescriptionText;
                invDescriptionText.text = invDescriptionText.text + "\n" + str;
            }
        }

        // Token: 0x04000027 RID: 39
        private const int CommerceLicence = 8;
    }
}
