using Newtonsoft.Json;

namespace TTS.CardTool.Processor.Options.Json
{
    class SavedBack
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }
    }
}
