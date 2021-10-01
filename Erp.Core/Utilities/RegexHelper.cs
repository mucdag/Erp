using System.Text.RegularExpressions;

namespace Core.Utilities
{
    public class RegexHelper
    {
        public static Match IpRegex(string address)
        {
            return Regex.Match(address, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
        }
    }
}
