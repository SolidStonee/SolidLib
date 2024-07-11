using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace SolidLib.Patches
{
    [HarmonyPatch(typeof(TimeOfDay))]
    internal class TimeOfDay_Patch
    {
        static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(TimeOfDay), "PlayerSeesNewTimeOfDay");
        }

        [HarmonyPrefix]
        public static bool PrefixMethod(TimeOfDay __instance)
        {
            if (!GameNetworkManager.Instance.localPlayerController.isInsideFactory && !GameNetworkManager.Instance.localPlayerController.isInHangarShipRoom && __instance.playersManager.shipHasLanded)
            {

                FieldInfo dayModeLastTimePlayerWasOutsideField = AccessTools.Field(typeof(TimeOfDay), "dayModeLastTimePlayerWasOutside");
                DayMode dayModeLastTimePlayerWasOutside = (DayMode)dayModeLastTimePlayerWasOutsideField.GetValue(__instance);
                dayModeLastTimePlayerWasOutside = __instance.dayMode;
                dayModeLastTimePlayerWasOutsideField.SetValue(__instance, dayModeLastTimePlayerWasOutside);
                HUDManager.Instance.SetClockIcon(__instance.dayMode);
                if (__instance.currentLevel.planetHasTime)
                {
                    __instance.PlayTimeMusicDelayed(__instance.timeOfDayCues[(int)__instance.dayMode], 0.5f, playRandomDaytimeMusic: true);
                }
            }

            return false; // Return false to skip the original method
        }
    }
}
