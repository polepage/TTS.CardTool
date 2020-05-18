using Newtonsoft.Json;

namespace TTS.CardTool.Data.MTG.Json
{
    class ScryCollectionIdentifier
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("set", NullValueHandling = NullValueHandling.Ignore)]
        public string Set { get; set; }
    }
}
