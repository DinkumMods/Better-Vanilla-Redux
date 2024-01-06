using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BepInEx.Core.Logging.Interpolation;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace Spectrals.DinkumBetterVanilla.Redux.Patches
{
    //// Token: 0x02000005 RID: 5
    [HarmonyPatch(typeof(CharMovement))]
    public class CharMovementPatches
    {
    //    // Token: 0x0600000D RID: 13 RVA: 0x00002668 File Offset: 0x00000868
    //    [HarmonyPostfix]
    //    [HarmonyPatch("Update")]
    //    private static void Update(CharMovement __instance)
    //    {
    //        bool value = Plugin.Instance.IsRestingEnabled.Value;

    //        if (value)
    //        {
    //            bool justSat = __instance.myPickUp.sittingPos != Vector3.zero;
    //            if (justSat)
    //            {
    //                CharMovementPatches.RESTING_TIMER = Plugin.Instance.RestingTick.Value;
    //            }
    //            else
    //            {
    //                bool flag = __instance.myPickUp.sitting || __instance.myPickUp.isLayingDown();
    //                if (flag)
    //                {
    //                    CharMovementPatches.RESTING_TIMER -= Time.deltaTime;
    //                    bool flag2 = CharMovementPatches.RESTING_TIMER <= 0f;
    //                    if (flag2)
    //                    {
    //                        StatusManager.manage.changeStatus(Plugin.Instance.HealthRestoreOnResting.Value, Plugin.Instance.StaminaRestoreOnResting.Value);
    //                        CharMovementPatches.RESTING_TIMER = Plugin.Instance.RestingTick.Value;
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    // Token: 0x04000024 RID: 36
    //    private static float RESTING_TIMER;
    }
}
