using Newtonsoft.Json;
using System.Collections.Generic;

namespace TTS.CardTool.Processor.Json
{
    class DBCard
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("pack_cards")]
        public List<DBPack> Packs { get; set; }

        [JsonProperty("side")]
        public string Side { get; set; }
    }
}
