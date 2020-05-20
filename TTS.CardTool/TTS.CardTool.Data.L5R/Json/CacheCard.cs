using Newtonsoft.Json;

namespace TTS.CardTool.Data.L5R.Json
{
    class CacheCard
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("iamge_url")]
        public string ImageUrl { get; set; }

        [JsonProperty("type")]
        public string Side { get; set; }
    }
}
