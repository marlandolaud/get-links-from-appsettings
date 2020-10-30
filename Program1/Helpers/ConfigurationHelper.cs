namespace Project1
{
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class ConfigurationHelper
    {
        public static Dictionary<string, string> GetUriFromConfigurationSection(
            IConfiguration configuration,
            IEnumerable<string> configurationSections)
        {
            var kvp = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            if (configuration == null || configurationSections == null)
            {
                return kvp;
            }

            foreach (var section in configurationSections)
            {
                FindUriPerSection(configuration, kvp, section);
            }

            return kvp;
        }

        private static void FindUriPerSection(
            IConfiguration configuration, 
            Dictionary<string, string> kvp, 
            string targetSection)
        {
            var itemsInConfigSection = configuration.GetSection(targetSection);
            if (itemsInConfigSection == null)
            {
                return;
            }

            foreach (var item in itemsInConfigSection.AsEnumerable())
            {
                AddUniqueUriHosts(kvp, item);
            }
        }

        private static void AddUniqueUriHosts(
            Dictionary<string, string> kvp, 
            KeyValuePair<string, string> item)
        {
            if (Uri.TryCreate(item.Value, UriKind.Absolute, out var uri))
            {
                var uriKeyName = UriNameHelper.CleanupConfigKey(item.Key);

                if (!kvp.ContainsValue(uri.Host) && !kvp.ContainsKey(uriKeyName))
                {
                    kvp.Add(uriKeyName, uri.Host);
                }
            }
        }        
    }
}
