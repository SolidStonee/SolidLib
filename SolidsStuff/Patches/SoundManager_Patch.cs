using GameNetcodeStuff;
using HarmonyLib;
using SolidLib.Components;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace SolidLib.Patches
{
    [HarmonyPatch(typeof(SoundManager))]
    internal class SoundManager_Patch
    {
        [HarmonyPatch(nameof(SoundManager.PlayRandomOutsideMusic))]
        [HarmonyPrefix]
        public static bool SPlayRandomOutsideMusic_Prefix(SoundManager __instance, ref bool eveningMusic)
        {
            API_MoonMusic music = GameObject.FindAnyObjectByType<API_MoonMusic>();

            FieldInfo timeSincePlayingLastMusicField = AccessTools.Field(typeof(SoundManager), "timeSincePlayingLastMusic");
            float timeSincePlayingLastMusic = (float)timeSincePlayingLastMusicField.GetValue(__instance);

            if (timeSincePlayingLastMusic > 200f)
            {
                if(music != null)
                {
                    int num = UnityEngine.Random.Range(0, music.ambientDayCues.Length);
                    if (eveningMusic)
                    {
                        if (music.ambientEveningCues.Length != 0)
                        {
                            __instance.musicSource.clip = music.ambientEveningCues[num];
                        }
                    }
                    else
                    {
                        __instance.musicSource.clip = music.ambientDayCues[num];
                    }
                }
                else
                {
                    int num = UnityEngine.Random.Range(0, __instance.DaytimeMusic.Length);
                    if (eveningMusic)
                    {
                        if (__instance.EveningMusic.Length != 0)
                        {
                            __instance.musicSource.clip = __instance.EveningMusic[num];
                        }
                    }
                    else
                    {
                        __instance.musicSource.clip = __instance.DaytimeMusic[num];
                    }
                }
                
                __instance.musicSource.Play();
                __instance.playingOutsideMusic = true;
                timeSincePlayingLastMusic = 0f;
                timeSincePlayingLastMusicField.SetValue(__instance, timeSincePlayingLastMusic);
            }
            return false;
        }
    }
}
