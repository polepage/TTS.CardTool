using Newtonsoft.Json;

namespace TTS.CardTool.Data.L5R.Json
{
    class FRDBPack
    {
        [JsonProperty("pack")]
        public FRDBSet Set { get; set; }

        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }
    }
}
