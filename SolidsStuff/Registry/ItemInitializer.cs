using System.Collections.Generic;
using System.IO;
using System.Reflection;
using LethalLib.Modules;
using SolidLib.Utils;
using SolidLib.Utils.AssetLoading;
using Steamworks.Ugc;
using UnityEngine;

namespace SolidLib.Registry
{
    public static class ItemInitializer
    {
        
        public static void Initialize(string bundleName, List<ItemConfig> itemConfigs)
        {
            var bundleLoader = AssetBundleLoader.LoadBundleFromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location), bundleName));
            Initialize(bundleLoader.Bundle, itemConfigs);
        }
        
        public static void Initialize(AssetBundle bundle, List<ItemConfig> itemConfigs)
        {
            if (bundle == null)
            {
                SolidLib.Log.LogInfo($"[Registry] Bundle {bundle.name} was not found for ItemRegistry, Cancelling registration for this bundle!!!");
                return;
            }
            SolidLib.Log.LogInfo($"[Registry] Initalizing Items for " + Assembly.GetCallingAssembly().GetName());
            foreach (var config in itemConfigs)
            {
                var item = AssetLoader.Load<Item>(bundle, config.AssetName);
                SolidLib.Log.LogInfo(item.name);
                Registries.ItemRegistry.Register(config.Name, item);
                TerminalNode? terminalNode = null;
                if (config.TerminalNodeAsset != null)
                    terminalNode = AssetLoader.Load<TerminalNode>(bundle, config.TerminalNodeAsset);
                
                RegisterItemWithConfig(config, item, terminalNode);
            }
        }

        private static void RegisterItemWithConfig(ItemConfig config, Item registeredItem, TerminalNode terminalNode)
        {
            if (config.Enabled)
            {
                if (config.IsShopItem) 
                {
                    Items.RegisterShopItem(registeredItem, null, null, terminalNode, config.ItemCost);
                }
                else
                {
                    RegisterScrapWithConfig(config.Enabled, config.SpawnWeights, registeredItem);
                }
            }
            else
            {
                Items.RegisterItem(registeredItem);
            }
        }

        private static void RegisterScrapWithConfig(bool enabled, string configMoonRarity, Item scrap)
        {
            if (enabled)
            {
                var (spawnRateByLevelType, spawnRateByCustomLevelType) = SolidLibUtils.ConfigParsing(configMoonRarity);
                Items.RegisterScrap(scrap, spawnRateByLevelType, spawnRateByCustomLevelType);
            }
            else
            {
                Items.RegisterScrap(scrap, 0, Levels.LevelTypes.All);
            }
        }

    }
}
