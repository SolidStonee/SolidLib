using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using SolidLib.Utils.Extensions;

namespace SolidLib.Patches { 


    [HarmonyPatch(typeof(StartOfRound))]
    static class StartOfRoundPatch
    {

        [HarmonyPatch(nameof(StartOfRound.Awake))]
        [HarmonyPostfix]
        public static void StartOfRound_Start(ref StartOfRound __instance)
        {
            __instance.NetworkObject.OnSpawn(CreateNetworkManager);
        }

        private static void CreateNetworkManager()
        {
            if (StartOfRound.Instance.IsServer || StartOfRound.Instance.IsHost)
            {
                if (Utils.SolidLibUtils.Instance == null)
                {
                    GameObject utilsInstance = GameObject.Instantiate(SolidLib.UtilsPrefab);
                    SceneManager.MoveGameObjectToScene(utilsInstance, StartOfRound.Instance.gameObject.scene);
                    utilsInstance.GetComponent<NetworkObject>().Spawn();
                    SolidLib.Log.LogInfo($"Created SolidLibUtils. Scene is: '{utilsInstance.scene.name}'");
                }
                else
                {
                    SolidLib.Log.LogWarning("SolidLibUtils already exists?");
                }
            }
        }
    }
}
