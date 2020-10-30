namespace get_links_from_appsettings
{
    using System.Linq;
    
    public class UriNameHelper
    {
        public static string CleanupUriName(string input)
        {
            var tree = input.Split(":");
            if (!tree.Any())
            {
                return input;
            }

            string returnValue;
            if (int.TryParse(tree.Last(), out _) && tree.Length >= 2)
            {
                returnValue = $"{tree[^2]}{tree.Last()}";
            }
            else
            {
                returnValue = tree.Last();
            }

            return returnValue
                .Replace("BaseUri", string.Empty)
                .Replace("Uri", string.Empty);
        }
    }
}
