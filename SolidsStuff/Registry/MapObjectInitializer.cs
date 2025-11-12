using System.Collections.Generic;
using System.IO;
using System.Reflection;
using LethalLib.Extras;
using LethalLib.Modules;
using SolidLib.Utils;
using SolidLib.Utils.AssetLoading;
using UnityEngine;


namespace SolidLib.Registry
{
    public class MapObjectInitializer
    {
        
        public static void Initialize(string bundleName, List<MapObjectConfig> mapObjectConfigs)
        {
            var bundleLoader = AssetBundleLoader.LoadBundleFromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location), bundleName));
            Initialize(bundleLoader.Bundle, mapObjectConfigs);
        }
        
        public static void Initialize(AssetBundle bundle, List<MapObjectConfig> mapObjectConfigs)
        {
            if (bundle == null)
            {
                SolidLib.Log.LogInfo($"[Registry] Bundle {bundle.name} was not found for MapObjectRegistry, Cancelling registration for this bundle!!!");
                return;
            }
            SolidLib.Log.LogInfo($"[Registry] Initalizing MapObjects for " + Assembly.GetCallingAssembly().GetName());
            foreach (var config in mapObjectConfigs)
            {
                if (!config.Enabled)
                    return;

                var mapObject = AssetLoader.Load<GameObject>(bundle, config.AssetName);
                RegisterMapObjectWithConfig(config, mapObject);
            }
        }

        public static void RegisterMapObjectWithConfig(MapObjectConfig config, GameObject mapObject)
        {
            if (config.Outside)
            {
                SpawnableOutsideObjectDef mapObjectDef = ScriptableObject.CreateInstance<SpawnableOutsideObjectDef>();
                mapObjectDef.spawnableMapObject = new SpawnableOutsideObjectWithRarity
                {
                    spawnableObject = ScriptableObject.CreateInstance<SpawnableOutsideObject>()
                };
                mapObjectDef.spawnableMapObject.spawnableObject.prefabToSpawn = mapObject;
                
                MapObjects.RegisterOutsideObject(mapObjectDef, Levels.LevelTypes.All, (level) => new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, Mathf.Clamp(config.SpawnAmount, 0, 1000))));
                Registries.OutsideMapObjectRegistry.Register(config.Name, mapObjectDef);
            }
            else
            {
                SpawnableMapObjectDef mapObjectDef = ScriptableObject.CreateInstance<SpawnableMapObjectDef>();
                mapObjectDef.spawnableMapObject = new SpawnableMapObject();
                mapObjectDef.spawnableMapObject.prefabToSpawn = mapObject;
                
                MapObjects.RegisterMapObject(mapObjectDef, Levels.LevelTypes.All, (level) => new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, Mathf.Clamp(config.SpawnAmount, 0, 1000))));
                Registries.InsideMapObjectRegistry.Register(config.Name, mapObjectDef);
            }
        }
        
    }
}