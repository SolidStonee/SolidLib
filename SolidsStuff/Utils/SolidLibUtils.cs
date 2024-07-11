using LethalLib.Modules;
using SolidLib.Registry;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace SolidLib.Utils
{
    public class SolidLibUtils : NetworkBehaviour
    {
        static System.Random random;
        public static SolidLibUtils Instance { get; private set; }

        void Awake()
        {
            Instance = this;
        }

        private static Dictionary<ulong, Action<GameObject>> pendingSpawns = new Dictionary<ulong, Action<GameObject>>();

        [ServerRpc(RequireOwnership = false)]
        public void SpawnItemServerRpc(ulong requestId, string itemName, Vector3 position)
        {
            if (StartOfRound.Instance == null)
            {
                CompleteSpawn(requestId, null);
                return;
            }

            if (random == null)
            {
                random = new System.Random(StartOfRound.Instance.randomMapSeed + 85);
            }

            if (!Registries.ItemRegistry.TryGetValue(itemName, out Item item))
            {
                SolidLib.Log.LogInfo("[Spawner] Failed to get item from registry");
                CompleteSpawn(requestId, null);
                return;
            }

            GameObject go = Instantiate(item.spawnPrefab, position + Vector3.up, Quaternion.identity, StartOfRound.Instance.propsContainer.transform);
            go.GetComponent<NetworkObject>().TrySetParent(StartOfRound.Instance.propsContainer);
            go.GetComponent<NetworkObject>().Spawn();
            go.GetComponent<NetworkObject>().TrySetParent(StartOfRound.Instance.propsContainer);
            go.GetComponent<NetworkObject>().TrySetParent(StartOfRound.Instance.propsContainer);
            int value = random.Next(minValue: item.minValue, maxValue: item.maxValue);
            var scanNode = go.GetComponentInChildren<ScanNodeProperties>();
            scanNode.scrapValue = value;
            scanNode.subText = $"Value: ${value}";
            go.GetComponent<GrabbableObject>().scrapValue = value;

            SolidLib.Log.LogInfo("[Spawner] Syncing Spawned Object");
            UpdateScanNodeClientRpc(new NetworkObjectReference(go), value);
            CompleteSpawn(requestId, go);
        }

        private void CompleteSpawn(ulong requestId, GameObject go)
        {
            if (pendingSpawns.TryGetValue(requestId, out var callback))
            {
                pendingSpawns.Remove(requestId);
                callback?.Invoke(go);
            }
        }

        [ClientRpc]
        public void UpdateScanNodeClientRpc(NetworkObjectReference go, int value)
        {
            if (go.TryGet(out NetworkObject netObj))
            {
                var scanNode = netObj.GetComponentInChildren<ScanNodeProperties>();
                scanNode.scrapValue = value;
                scanNode.subText = $"Value: ${value}";
            }
        }

        public void SpawnItem(string itemName, Vector3 position, Action<GameObject> callback)
        {
            var requestId = NetworkManager.Singleton.LocalClientId;
            pendingSpawns[requestId] = callback;

            SpawnItemServerRpc(requestId, itemName, position);
        }

        public static (Dictionary<Levels.LevelTypes, int> spawnRateByLevelType, Dictionary<string, int> spawnRateByCustomLevelType) ConfigParsing(string configMoonRarity) {
            Dictionary<Levels.LevelTypes, int> spawnRateByLevelType = new Dictionary<Levels.LevelTypes, int>();
            Dictionary<string, int> spawnRateByCustomLevelType = new Dictionary<string, int>();

            foreach (string entry in configMoonRarity.Split(',').Select(s => s.Trim())) {
                string[] entryParts = entry.Split(':');

                if (entryParts.Length != 2) continue;

                string name = entryParts[0];
                int spawnrate;

                if (!int.TryParse(entryParts[1], out spawnrate)) continue;

                if (System.Enum.TryParse(name, true, out Levels.LevelTypes levelType))
                {
                    spawnRateByLevelType[levelType] = spawnrate;
                }
                else
                {
                    spawnRateByCustomLevelType[name] = spawnrate;
                }
            }
            return (spawnRateByLevelType, spawnRateByCustomLevelType);
        }
    }
}
