using Newtonsoft.Json;
using System.Collections.Generic;

namespace TTS.CardTool.Processor.Json
{
    class DBCards
    {
        [JsonProperty("records")]
        public List<DBCard> Cards { get; set; }
    }
}
