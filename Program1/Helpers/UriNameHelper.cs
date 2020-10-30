namespace Project1
{
    using System.Linq;
    
    public class UriNameHelper
    {
        public static string CleanupConfigKey(string input)
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
                .Replace("BaseUrl", string.Empty)
                .Replace("Uri", string.Empty)
                .Replace("Url", string.Empty);
        }
    }
}
