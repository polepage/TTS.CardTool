using Newtonsoft.Json;
using System.Collections.Generic;

namespace TTS.CardTool.Processor.Json
{
    class SetNameFixer
    {
        [JsonProperty("fixes")]
        public Dictionary<string, string> Fixes { get; set; }
    }
}
