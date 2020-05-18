using Newtonsoft.Json;
using System.Collections.Generic;

namespace TTS.CardTool.Data.MTG.Json
{
    class ScryCollectionResults
    {
        [JsonProperty("not_found")]
        public List<ScryCollectionIdentifier> Errors { get; set; }

        [JsonProperty("data")]
        public List<ScryCard> Cards { get; set; }
    }
}
