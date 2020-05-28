using Newtonsoft.Json;

namespace TTS.CardTool.Processor.Json
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
