using System.Collections.Generic;
using HarmonyLib;

namespace Spectrals.DinkumBetterVanilla.Redux.Patches
{
    // Token: 0x0200000A RID: 10
    [HarmonyPatch(typeof(MailManager))]
    internal class MailManagerPatches
    {
        // Token: 0x0600001A RID: 26 RVA: 0x00002E7C File Offset: 0x0000107C
        [HarmonyPrefix]
        [HarmonyPatch("sendDailyMail")]
        private static void SendDailyMail(MailManager __instance)
        {
            bool flag = MailManagerPatches.ANIMAL_CAPTURED_REWARD_AMOUNT > 0;
            if (flag)
            {
                // FIX#XXX: Use Explicit cast on Letter.LetterType below - VaterOtter 12/29/2023
                // FIX#XXX: Updated to use Instance reference. - VaterOtter 12/29/2023
                __instance.tomorrowsLetters.Add(new Letter(-1, (Letter.LetterType)4, Inventory.Instance.getInvItemId(Inventory.Instance.moneyItem), MailManagerPatches.ANIMAL_CAPTURED_REWARD_AMOUNT));
                foreach (KeyValuePair<int, int> keyValuePair in MailManagerPatches.ANIMAL_TRAP_USED)
                {
                    // FIX#XXX: Use Explicit cast on Letter.LetterType below - VaterOtter 12/29/2023
                    __instance.tomorrowsLetters.Add(new Letter(-1, (Letter.LetterType)5, keyValuePair.Key, keyValuePair.Value));
                }
                MailManagerPatches.ANIMAL_CAPTURED_REWARD_AMOUNT = 0;
                MailManagerPatches.ANIMAL_TRAP_USED.Clear();
            }
        }

        // Token: 0x0600001B RID: 27 RVA: 0x00002F3C File Offset: 0x0000113C
        [HarmonyPrefix]
        [HarmonyPatch("sendAnAnimalCapturedLetter")]
        private static bool SendAnAnimalCapturedLetter(int rewardAmount, int trapType)
        {
            //FIX#XXX: Added Explicit cast below - VaterOtter 12/29/2023
            DailyTaskGenerator.generate.doATask((DailyTaskGenerator.genericTaskType)22, 1);
            MailManagerPatches.ANIMAL_CAPTURED_REWARD_AMOUNT += rewardAmount;
            bool flag = MailManagerPatches.ANIMAL_TRAP_USED.ContainsKey(trapType);
            if (flag)
            {
                Dictionary<int, int> animal_TRAP_USED = MailManagerPatches.ANIMAL_TRAP_USED;
                int num = animal_TRAP_USED[trapType];
                animal_TRAP_USED[trapType] = num + 1;
            }
            else
            {
                MailManagerPatches.ANIMAL_TRAP_USED.Add(trapType, 1);
            }
            return false;
        }

        // Token: 0x04000028 RID: 40
        private static int ANIMAL_CAPTURED_REWARD_AMOUNT = 0;

        // Token: 0x04000029 RID: 41
        private static Dictionary<int, int> ANIMAL_TRAP_USED = new Dictionary<int, int>();
    }
}
