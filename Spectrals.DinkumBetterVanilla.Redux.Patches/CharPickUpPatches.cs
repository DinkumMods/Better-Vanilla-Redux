using HarmonyLib;

namespace Spectrals.DinkumBetterVanilla.Redux.Patches
{
    // Token: 0x02000004 RID: 4
    [HarmonyPatch(typeof(CharPickUp))]
    public class CharPickUpPatches
    {
        // Token: 0x06000006 RID: 6 RVA: 0x000025C4 File Offset: 0x000007C4
        [HarmonyPostfix]
        [HarmonyPatch("Start")]
        private static void Start(CharPickUp __instance)
        {
            CharPickUpPatches.IS_AUTO_PICK_UP_ENABLED = Plugin.Instance.IsAutoPickUpEnabled.Value;
        }

        // Token: 0x06000007 RID: 7 RVA: 0x000025DC File Offset: 0x000007DC
        [HarmonyPrefix]
        [HarmonyPatch("Update")]
        private static void Update(CharPickUp __instance)
        {
            bool flag = !CharPickUpPatches.TMP_DISABLED && CharPickUpPatches.IS_AUTO_PICK_UP_ENABLED;
            if (flag)
            {
                __instance.holdingPickUp = true;
            }
            else
            {
                CharPickUpPatches.TMP_DISABLED = false;
            }
        }

        // Token: 0x06000008 RID: 8 RVA: 0x00002610 File Offset: 0x00000810
        [HarmonyPrefix]
        [HarmonyPatch("pickUp")]
        private static void PickUp(CharPickUp __instance)
        {
            CharPickUpPatches.TMP_DISABLED = true;
            __instance.holdingPickUp = false;
        }

        // Token: 0x06000009 RID: 9 RVA: 0x00002610 File Offset: 0x00000810
        [HarmonyPrefix]
        [HarmonyPatch("pressX")]
        private static void PressX(CharPickUp __instance)
        {
            CharPickUpPatches.TMP_DISABLED = true;
            __instance.holdingPickUp = false;
        }

        // Token: 0x0600000A RID: 10 RVA: 0x00002610 File Offset: 0x00000810
        [HarmonyPrefix]
        [HarmonyPatch("pressY")]
        private static void PressY(CharPickUp __instance)
        {
            CharPickUpPatches.TMP_DISABLED = true;
            __instance.holdingPickUp = false;
        }

        // Token: 0x0600000B RID: 11 RVA: 0x00002620 File Offset: 0x00000820
        public static void OnAutoPickUpKeyCodePressed()
        {
            CharPickUpPatches.IS_AUTO_PICK_UP_ENABLED = !CharPickUpPatches.IS_AUTO_PICK_UP_ENABLED;
            NotificationManager.manage.createChatNotification("Automatically Pick-Up is now " + (CharPickUpPatches.IS_AUTO_PICK_UP_ENABLED ? "enabled" : "disabled") + ".", false);
        }

        // Token: 0x04000022 RID: 34
        private static bool IS_AUTO_PICK_UP_ENABLED;

        // Token: 0x04000023 RID: 35
        private static bool TMP_DISABLED;
    }
}
