using Newtonsoft.Json;
using System.Collections.Generic;

namespace TTS.CardTool.Data.L5R.Json
{
    class FRDBSets
    {
        [JsonProperty("records")]
        public List<FRDBSet> Sets { get; set; }
    }
}
