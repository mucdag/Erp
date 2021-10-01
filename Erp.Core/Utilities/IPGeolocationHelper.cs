using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Core.Utilities
{
    public class IPGeolocationHelper
    {
        public static async Task<IPAddressInfo> GetIPGeolocation(string ipAdress)
        {
            HttpClient httpClient = new HttpClient();
            var result = await httpClient.GetStringAsync($"http://api.ipstack.com/{ipAdress}?access_key=c7361b82290b60f76dd0c9fa903d53ed");
            return JsonConvert.DeserializeObject<IPAddressInfo>(result);
        }
    }

    public class IPAddressInfo
    {
        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("country_name")]
        public string CountryName { get; set; }

        [JsonProperty("city")]
        public string CityName { get; set; }

        [JsonProperty("latitude")]
        public string Latitude { get; set; }

        [JsonProperty("longitude")]
        public string Longitude { get; set; }
    }
}
