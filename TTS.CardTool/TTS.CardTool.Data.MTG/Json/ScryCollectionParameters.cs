using Newtonsoft.Json;
using System.Collections.Generic;

namespace TTS.CardTool.Data.MTG.Json
{
    class ScryCollectionParameters
    {
        [JsonProperty("identifiers")]
        public List<ScryCollectionIdentifier> Identifiers { get; set; }
    }
}
