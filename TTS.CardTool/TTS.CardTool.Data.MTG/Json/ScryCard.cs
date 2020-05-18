using Newtonsoft.Json;
using System.Collections.Generic;

namespace TTS.CardTool.Data.MTG.Json
{
    class ScryCard
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("layout")]
        public string Layout { get; set; }

        [JsonProperty("image_uris")]
        public ScryImages ImageUris { get; set; }

        [JsonProperty("set")]
        public string Set { get; set; }

        [JsonProperty("all_parts")]
        public List<ScryReference> RelatedCards { get; set; }

        [JsonProperty("card_faces")]
        public List<ScryCardFace> CardFaces { get; set; }
    }
}
