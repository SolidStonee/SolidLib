using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using SolidLib.Registry;
using SolidLib.Utils.AssetLoading;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Dissonance;
using LethalLib.Modules;
using UnityEngine;
using UnityEngine.Assertions;
using LogLevel = BepInEx.Logging.LogLevel;

namespace SolidLib
{

    public static class PluginInformation
    {
        public const string PLUGIN_GUID = "solidlib";
        public const string PLUGIN_NAME = "Solid's Library";
        public const string PLUGIN_VERSION = "1.2.0";
    }

    [BepInPlugin(PluginInformation.PLUGIN_GUID, PluginInformation.PLUGIN_NAME, PluginInformation.PLUGIN_VERSION)]
    [BepInDependency(LethalLib.Plugin.ModGUID)]
    public class SolidLib : BaseUnityPlugin
    {
        public static ManualLogSource Log;
        private readonly Harmony harmony = new Harmony(PluginInformation.PLUGIN_GUID);
        internal static LibConfig BoundConfig { get; private set; } = null!;

        private AssetBundle solidLibBundle;

        public static GameObject UtilsPrefab;

        private void Awake()
        {

            Log = Logger;
            Log.LogInfo($"Initializing Library");
            
            BoundConfig = new LibConfig(base.Config);
            
            InitializeNetworkBehaviours();

            Registries.InitRegistries();
            
            LoadAllIngameAssetsIntoRegistry();

            solidLibBundle = BundleUtils.LoadBundleFromInternalAssembly("solidlibassets");

            UtilsPrefab = solidLibBundle.LoadAsset<GameObject>("SolidLibUtils.prefab");
            NetworkPrefabs.RegisterNetworkPrefab(UtilsPrefab); //make sure to register the utils

            harmony.PatchAll(Assembly.GetExecutingAssembly());
            //these were to just test
            // var itemConfigs = new List<ItemConfig>
            // {
            //     new ItemConfig
            //     {
            //         Name = "Grape",
            //         AssetName = "GrapeObj.asset",
            //         Enabled = true,
            //         IsShopItem = false,
            //         SpawnWeights = "Modded:80,Vanilla:80",
            //
            //     },
            //     new ItemConfig
            //     {
            //         Name = "Glue",
            //         AssetName = "GlueObj.asset",
            //         Enabled = true,
            //         IsShopItem = true,
            //         SpawnWeights = "Modded:80,Vanilla:80",
            //         ItemCost = 2,
            //     }
            // };
            //
            // var enemyConfigs = new List<EnemyConfig>
            // {
            //     new EnemyConfig
            //     {
            //         Name = "Duck",
            //         AssetName = "DuckEnemyType.asset",
            //         Enabled = true,
            //         MaxSpawnCount = 1,
            //         PowerLevel = 1,
            //         SpawnWeights = "Modded:50,Vanilla:80"
            //     }
            // };
            //
            //  var mapObjectConfigs = new List<MapObjectConfig>
            //  {
            //      new MapObjectConfig
            //      {
            //          Name = "SeaMine",
            //          AssetName = "Seamine",
            //          Enabled = true,
            //          SpawnAmount = 10,
            //          Outside = false
            //      }
            //  };
            
            // Initialize registries
            // ItemInitializer.Initialize(bundle, itemConfigs);
            // MapObjectInitializer.Initialize(bundle, mapObjectConfigs);
        }

        public static void LogExtended(object log)
        {
            if(BoundConfig.extendedLogging.Value)
                Log.LogInfo(log);
        }

        private int materialCount = 0;

        private void LoadAllIngameAssetsIntoRegistry()
        {
            
            /*
            foreach (UnityEngine.Object go in Resources.FindObjectsOfTypeAll(typeof(UnityEngine.Object)) as UnityEngine.Object[])
            {
                GameObject cGO = go as GameObject;
                if (cGO != null && !EditorUtility.IsPersistent(cGO.transform.root.gameObject) && !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
                    
            }
            
            GameObject[] allGameObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (GameObject gameObject in allGameObjects)
            {
                Registries.GameObjectRegistry.Register(gameObject.name, gameObject);
            }
            AudioClip[] allAudioClips = Resources.FindObjectsOfTypeAll<AudioClip>();
            foreach (AudioClip clip in allAudioClips)
            {
                Registries.SoundRegistry.Register(clip.name, clip);
            }
            Material[] allMaterials = Resources.FindObjectsOfTypeAll<Material>();
            foreach (Material material in allMaterials)
            {
                //Filter Unneeded shiat
                
                
                Registries.MaterialRegistry.Register(material.name, material);
                
                materialCount++;
                LogExtended($"Registered Material {name}");
            }
            
            LogExtended($"Registered {materialCount} materials on startup");
            */
            
        }

        private static void InitializeNetworkBehaviours()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                foreach (var method in methods)
                {
                    var attributes = method.GetCustomAttributes(typeof(RuntimeInitializeOnLoadMethodAttribute), false);
                    if (attributes.Length > 0)
                    {
                        method.Invoke(null, null);
                    }
                }
            }
        }

    }
}