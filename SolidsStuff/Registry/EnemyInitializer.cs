using System.Collections.Generic;
using System.IO;
using System.Reflection;
using LethalLib.Modules;
using SolidLib.Utils;
using SolidLib.Utils.AssetLoading;
using UnityEngine;

namespace SolidLib.Registry
{
    public static class EnemyInitializer
    {

        public static void Initialize(string bundleName, List<EnemyConfig> enemyConfigs)
        {
            var bundleLoader = AssetBundleLoader.LoadBundleFromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location), bundleName));
            Initialize(bundleLoader.Bundle, enemyConfigs);
        }
        
        
        public static void Initialize(AssetBundle bundle, List<EnemyConfig> enemyConfigs)
        {
            
            if (bundle == null)
            {
                SolidLib.Log.LogInfo($"[Registry] Bundle {bundle.name} was not found for EnemyRegistry, Cancelling registration for this bundle!!!");
                return;
            }
            SolidLib.Log.LogInfo($"[Registry] Initalizing Enemys for " + Assembly.GetCallingAssembly().GetName());

            foreach (var config in enemyConfigs)
            {
                var enemyType = AssetLoader.Load<EnemyType>(bundle, config.AssetName);
                var terminalNode = AssetLoader.Load<TerminalNode>(bundle, config.TerminalNodeAsset);
                var terminalKeyword = AssetLoader.Load<TerminalKeyword>(bundle, config.TerminalKeywordAsset);

                Registries.EnemyRegistry.Register(config.Name, enemyType);
                Registries.TerminalNodeRegistry.Register(config.Name, terminalNode);
                Registries.TerminalKeywordRegistry.Register(config.Name, terminalKeyword);

                RegisterEnemyWithConfig(config, enemyType, terminalNode, terminalKeyword);
            }
        }

        private static void RegisterEnemyWithConfig(EnemyConfig config, EnemyType enemyType, TerminalNode terminalNode, TerminalKeyword terminalKeyword)
        {
            enemyType.MaxCount = config.MaxSpawnCount;
            enemyType.PowerLevel = config.PowerLevel;

            if (config.Enabled)
            {
                var (spawnRateByLevelType, spawnRateByCustomLevelType) = SolidLibUtils.ConfigParsing(config.SpawnWeights);
                
                Enemies.RegisterEnemy(enemyType, spawnRateByLevelType, spawnRateByCustomLevelType, terminalNode, terminalKeyword);
            }
            else
            {
                Enemies.RegisterEnemy(enemyType, 0, Levels.LevelTypes.All, terminalNode, terminalKeyword);
            }
        }

    }
}
