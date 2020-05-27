using Newtonsoft.Json;

namespace TTS.CardTool.Processor.Json
{
    class ScryCardFace
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("image_uris")]
        public ScryImages ImageUris { get; set; }
    }
}
