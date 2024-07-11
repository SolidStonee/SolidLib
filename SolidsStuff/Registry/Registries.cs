using LethalLib.Extras;
using UnityEngine;

namespace SolidLib.Registry
{
    public static class Registries
    {
        public static Registry<GameObject> GameObjectRegistry { get; private set; }
        public static Registry<AudioClip> SoundRegistry { get; private set; }
        public static Registry<Material> MaterialRegistry { get; private set; }
        public static Registry<Item> ItemRegistry { get; private set; }
        public static Registry<EnemyType> EnemyRegistry { get; private set; }
        
        public static Registry<SpawnableOutsideObjectDef> OutsideMapObjectRegistry { get; private set; }
        
        public static Registry<SpawnableMapObjectDef> InsideMapObjectRegistry { get; private set; }
        public static Registry<TerminalNode> TerminalNodeRegistry { get; private set; }
        public static Registry<TerminalKeyword> TerminalKeywordRegistry { get; private set; }

        public static void InitRegistries()
        {
            GameObjectRegistry = RegistryManager.GetRegistry<GameObject>();
            SoundRegistry = RegistryManager.GetRegistry<AudioClip>();
            MaterialRegistry = RegistryManager.GetRegistry<Material>();
            ItemRegistry = RegistryManager.GetRegistry<Item>();
            EnemyRegistry = RegistryManager.GetRegistry<EnemyType>();
            OutsideMapObjectRegistry = RegistryManager.GetRegistry<SpawnableOutsideObjectDef>();
            InsideMapObjectRegistry = RegistryManager.GetRegistry<SpawnableMapObjectDef>();
            TerminalNodeRegistry = RegistryManager.GetRegistry<TerminalNode>();
            TerminalKeywordRegistry = RegistryManager.GetRegistry<TerminalKeyword>();
        }
    }
}