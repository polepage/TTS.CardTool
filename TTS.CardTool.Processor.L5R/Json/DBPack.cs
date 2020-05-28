using Newtonsoft.Json;

namespace TTS.CardTool.Processor.Json
{
    class DBPack
    {
        [JsonProperty("pack")]
        public DBSet Set { get; set; }

        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }
    }
}
