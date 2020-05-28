using Newtonsoft.Json;
using System.Collections.Generic;

namespace TTS.CardTool.Processor.Json
{
    class DBSets
    {
        [JsonProperty("records")]
        public List<DBSet> Sets { get; set; }
    }
}
