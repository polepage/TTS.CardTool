using Newtonsoft.Json;

namespace TTS.CardTool.Processor.Json
{
    class DBSet
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
