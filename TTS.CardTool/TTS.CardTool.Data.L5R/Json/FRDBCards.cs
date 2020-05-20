using Newtonsoft.Json;
using System.Collections.Generic;

namespace TTS.CardTool.Data.L5R.Json
{
    class FRDBCards
    {
        [JsonProperty("records")]
        public List<FRDBCard> Cards { get; set; }
    }
}
