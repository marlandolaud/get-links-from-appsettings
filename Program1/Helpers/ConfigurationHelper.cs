namespace Project1
{
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ConfigurationHelper : IConfigurationHelper
    {
        private Dictionary<string, string> kvp;

        private readonly IConfiguration configuration;

        public ConfigurationHelper(IConfiguration configuration)
        {
            kvp = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            this.configuration = configuration;
        }

        public Dictionary<string, string> GetUriFromConfigurationSection(IEnumerable<string> configurationSections)
        {
            if (configuration == null || configurationSections == null)
            {
                return kvp;
            }

            foreach (var section in configurationSections)
            {
                FindUrisPerConfigurationSection(section);
            }

            return kvp;
        }

        private void FindUrisPerConfigurationSection(string targetSection)
        {
            var itemsInConfigSection = configuration.GetSection(targetSection);
            if (itemsInConfigSection == null)
            {
                return;
            }

            foreach (var item in itemsInConfigSection.AsEnumerable())
            {
                AddUniqueUriHosts(item);
            }
        }

        private void AddUniqueUriHosts(KeyValuePair<string, string> item)
        {
            if (Uri.TryCreate(item.Value, UriKind.Absolute, out var uri))
            {
                var uriKeyName = UriNameHelper.CleanupConfigKey(item.Key);

                if (IsNotInCollection(uri, uriKeyName))
                {
                    kvp.Add(uriKeyName, uri.Host);
                }
            }
        }

        private bool IsNotInCollection(Uri uri, string uriKeyName)
        {
            return !kvp.ContainsValue(uri.Host) &&
                !kvp.ContainsKey(uriKeyName) &&
                !string.IsNullOrEmpty(uri.Host) &&
                !string.IsNullOrEmpty(uriKeyName);
        }
    }
}
