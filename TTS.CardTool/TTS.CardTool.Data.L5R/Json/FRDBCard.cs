using Newtonsoft.Json;
using System.Collections.Generic;

namespace TTS.CardTool.Data.L5R.Json
{
    class FRDBCard
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("pack_cards")]
        public List<FRDBPack> Packs { get; set; }

        [JsonProperty("side")]
        public string Side { get; set; }
    }
}
