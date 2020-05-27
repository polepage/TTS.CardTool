using Newtonsoft.Json;
using System.Collections.Generic;

namespace TTS.CardTool.Processor.Json
{
    class ScryCollectionParameters
    {
        [JsonProperty("identifiers")]
        public List<ScryCollectionIdentifier> Identifiers { get; set; }
    }
}
