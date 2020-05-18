using Newtonsoft.Json;

namespace TTS.CardTool.Data.MTG.Json
{
    class ScryCardFace
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("image_uris")]
        public ScryImages ImageUris { get; set; }
    }
}
