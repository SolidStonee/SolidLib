using System.Collections.Generic;
using System.Reflection;
using BepInEx.Configuration;

namespace SolidLib
{

    public class LibConfig
    {

        public ConfigEntry<bool> extendedLogging;

        public LibConfig(ConfigFile cfg)
        {
            extendedLogging = cfg.Bind("General",
                "ExtendedLogging",
                false,
                "Extra logging for debugging");

            ClearUnusedEntries(cfg);
        }

        private void ClearUnusedEntries(ConfigFile cfg)
        {
            PropertyInfo orphanedEntriesProp =
                cfg.GetType().GetProperty("OrphanedEntries", BindingFlags.NonPublic | BindingFlags.Instance);
            var orphanedEntries = (Dictionary<ConfigDefinition, string>)orphanedEntriesProp.GetValue(cfg, null);
            orphanedEntries.Clear(); // Clear orphaned entries (Unbinded/Abandoned entries)
            cfg.Save(); // Save the config file to save these changes
        }
    }
}