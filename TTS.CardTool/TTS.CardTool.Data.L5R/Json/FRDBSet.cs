using Newtonsoft.Json;

namespace TTS.CardTool.Data.L5R.Json
{
    class FRDBSet
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
