using LethalLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using SolidLib.Registry;
using Unity.Netcode;
using UnityEngine;
using NetworkPrefabs = LethalLib.Modules.NetworkPrefabs;
using Utilities = LethalLib.Modules.Utilities;

namespace SolidLib.Utils.AssetLoading
{


    public class AssetBundleLoader
    {
        public static readonly Dictionary<string, AssetBundle> LoadedBundles = [];
        
        public AssetBundle Bundle { get; private set; }

        public AssetBundleLoader(string filePath, bool registerNetworkPrefabs = true, bool fixMixerGroups = true)
        {

            bool alreadyExists = LoadedBundles.TryGetValue(filePath, out var bundle);
            
            SolidLib.LogExtended($"[AssetBundle Loading] AlreadyExists: {alreadyExists}");
            
            if (!alreadyExists)
            {
                bundle = AssetBundle.LoadFromFile(filePath);
                if (bundle == null)
                {
                    SolidLib.Log.LogWarning($"[AssetBundle Loading] Asset bundle {filePath} was empty, Cancelling.");
                    return;
                }
                LoadedBundles.Add(filePath, bundle);
                SolidLib.LogExtended(
                    $"[AssetBundle Loading] {filePath} contains these objects: {string.Join(",", bundle.GetAllAssetNames())}");
            }
            else
            {
                SolidLib.LogExtended($"[AssetBundle Loading] Used cached {filePath}");
            }
            Bundle = bundle;

            if (alreadyExists)
            {
                SolidLib.Log.LogDebug($"Skipping registering stuff as {bundle.name} has already been loaded");
                return;
            }
            foreach (AudioClip clip in bundle.LoadAllAssets<AudioClip>())
            {
                if (Registries.SoundRegistry.Contains(clip.name)) return;
                
                Registries.SoundRegistry.Register(clip.name, clip);
                SolidLib.LogExtended($"[AssetBundle Loading] Registered AudioClip: {clip.name}");
            }
            foreach (GameObject gameObject in bundle.LoadAllAssets<GameObject>())
            {
                if (fixMixerGroups)
                {
                    Utilities.FixMixerGroups(gameObject);
                    SolidLib.LogExtended($"[AssetBundle Loading] Fixed Mixer Groups: {gameObject.name}");
                }
                Registries.GameObjectRegistry.Register(gameObject.name, gameObject);
                if (!registerNetworkPrefabs || gameObject.GetComponent<NetworkObject>() == null) continue;
                NetworkPrefabs.RegisterNetworkPrefab(gameObject);
                SolidLib.LogExtended($"[AssetBundle Loading] Registered Network Prefab: {gameObject.name}");
                
            }
            foreach (Item item in bundle.LoadAllAssets<Item>())
            {
                try
                {
                    if (!registerNetworkPrefabs || item.spawnPrefab.GetComponent<NetworkObject>() == null) continue;
                    NetworkPrefabs.RegisterNetworkPrefab(item.spawnPrefab);
                    SolidLib.LogExtended($"[AssetBundle Loading] Registered Network Prefab: {item.name}");
                }
                catch(Exception e)
                {
                    SolidLib.Log.LogError(e + $"\n\n There was an error loading the item: {item.name}, Is your spawnprefab null or is it something else?");
                }
                
            }
        }

        public static AssetBundleLoader LoadBundleFromFile(string filePath)
        {
            SolidLib.LogExtended("Start Loading bundle");
            return new AssetBundleLoader(filePath);
        }
        
        public static AssetBundle? GetLoadedAssetBundle(string bundleName)
        {
            bool gotBundle = LoadedBundles.TryGetValue((Path.Combine(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location), bundleName)), out var bundle);
            if(gotBundle )
            {
                return bundle;
            }
            return null;
        }
        
    }
}
