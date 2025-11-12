// using GameNetcodeStuff;
// using HarmonyLib;
// using SolidLib.Components;
// using System;
// using UnityEngine;
//
// namespace SolidLib.Patches
// {
//     [HarmonyPatch(typeof(PlayerControllerB))]
//     internal class PlayerControllerB_Patch
//     {
//         [HarmonyPatch(nameof(PlayerControllerB.PlayFootstepSound))]
//         [HarmonyPrefix]
//         public static bool SPlayFootstepSound_Prefix(PlayerControllerB __instance)
//         {
//             try
//             {
//                 Ray interactRay = new Ray(__instance.thisPlayerBody.position + Vector3.up, -Vector3.up);
//                 RaycastHit hit;
//                 AudioClip[] clips;
//                 if (Physics.Raycast(interactRay, out hit, 6f, StartOfRound.Instance.walkableSurfacesMask,
//                         QueryTriggerInteraction.Ignore) || hit.collider.CompareTag(StartOfRound.Instance
//                         .footstepSurfaces[__instance.currentFootstepSurfaceIndex].surfaceTag))
//                 {
//
//
//                 }
//                 int num;
//                 if (hit.collider.gameObject.GetComponent<API_FootStepSurface>() != null)
//                 {
//                     var footstepSurface = hit.collider.gameObject.GetComponent<API_FootStepSurface>();
//                     clips = footstepSurface.clips;
//                     num = UnityEngine.Random.Range(0, footstepSurface.clips.Length);
//                     if (num == __instance.previousFootstepClip)
//                     {
//                         num = (num + 1) % footstepSurface.clips.Length;
//                     }
//                 }
//                 else
//                 {
//                     num = UnityEngine.Random.Range(0, StartOfRound.Instance.footstepSurfaces[__instance.currentFootstepSurfaceIndex].clips.Length);
//                     clips = StartOfRound.Instance.footstepSurfaces[__instance.currentFootstepSurfaceIndex].clips;
//
//                     for (int i = 0; i < StartOfRound.Instance.footstepSurfaces.Length; i++)
//                     {
//                         if (hit.collider.CompareTag(StartOfRound.Instance.footstepSurfaces[i].surfaceTag))
//                         {
//                             __instance.currentFootstepSurfaceIndex = i;
//                             break;
//                         }
//                     }
//
//                     if (num == __instance.previousFootstepClip)
//                     {
//                         num = (num + 1) % StartOfRound.Instance.footstepSurfaces[__instance.currentFootstepSurfaceIndex].clips.Length;
//                     }
//                 }
//
//                 __instance.movementAudio.pitch = UnityEngine.Random.Range(0.93f, 1.07f);
//                 bool flag = ((!__instance.IsOwner) ? __instance.playerBodyAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Sprinting") : __instance.isSprinting);
//                 float num2 = 0.9f;
//                 if (!flag)
//                 {
//                     num2 = 0.6f;
//                 }
//
//
//                 __instance.movementAudio.PlayOneShot(clips[num], num2);
//                 __instance.previousFootstepClip = num;
//                 WalkieTalkie.TransmitOneShotAudio(__instance.movementAudio, clips[num], num2);
//             }
//             catch(Exception e)
//             {
//                 //SolidLib.Log.LogInfo("FootStep error you can ignore this");
//             }
//
//             //Cancel out the original code
//             return false;
//         }
//     }
// }
