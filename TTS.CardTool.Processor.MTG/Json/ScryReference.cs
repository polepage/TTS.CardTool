using Newtonsoft.Json;

namespace TTS.CardTool.Processor.Json
{
    class ScryReference
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("component")]
        public string Component { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
