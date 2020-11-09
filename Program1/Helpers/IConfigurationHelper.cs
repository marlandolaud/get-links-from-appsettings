using System.Collections.Generic;

namespace Project1
{
    public interface IConfigurationHelper
    {
        Dictionary<string, string> GetUriFromConfigurationSection(IEnumerable<string> configurationSections);
    }
}