using System;
using System.Collections.Generic;
using UnityEngine;

namespace SolidLib.Registry
{

    public static class RegistryManager
    {
        private static readonly Dictionary<Type, object> Registries = new Dictionary<Type, object>();

        public static Registry<T> GetRegistry<T>()
        {
            var type = typeof(T);
            if (!Registries.TryGetValue(type, out var registry))
            {
                
                registry = new Registry<T>();
                Registries[type] = registry;
                SolidLib.Log.LogInfo($"[Registry] Initializing {type.Name} registry holder");
            }
            return (Registry<T>)registry;
        }
    }
    
    public class MapObjectConfig
    {
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public string AssetName { get; set; }
        public bool Outside { get; set; }
        public int SpawnAmount { get; set; }
        
    }

    public class EnemyConfig
    {
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public string AssetName { get; set; }
        public string? TerminalNodeAsset { get; set; }
        public string? TerminalKeywordAsset { get; set; }
        public string SpawnWeights { get; set; }
        public float PowerLevel { get; set; }
        public int MaxSpawnCount { get; set; }
    }

    public class ItemConfig
    {
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public string AssetName { get; set; }
        
        public string SpawnWeights { get; set; }
        public string? TerminalNodeAsset { get; set; }
        public bool IsShopItem { get; set; }
        
        public int ItemCost { get; set; }
    }

    public static class AssetLoader
    {
        public static T Load<T>(AssetBundle bundle, string assetName) where T : UnityEngine.Object
        {
            // This method loads an asset of type T from an AssetBundle
            return bundle.LoadAsset<T>(assetName);
        }
    }



}
