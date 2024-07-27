using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace SolidLib.Patches
{
    internal class TimeOfDay_Patch
    {
        [HarmonyPatch(nameof(TimeOfDay.PlayerSeesNewTimeOfDay))]
        [HarmonyPrefix]
        public static bool SeesNewTimeOfDay_Patch(TimeOfDay __instance)
        {
            if (!GameNetworkManager.Instance.localPlayerController.isInsideFactory && !GameNetworkManager.Instance.localPlayerController.isInHangarShipRoom && __instance.playersManager.shipHasLanded)
            {
                __instance.dayModeLastTimePlayerWasOutside = __instance.dayMode;
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
