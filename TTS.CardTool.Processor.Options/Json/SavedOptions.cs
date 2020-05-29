using Newtonsoft.Json;
using System.Collections.Generic;

namespace TTS.CardTool.Processor.Options.Json
{
    class SavedOptions
    {
        [JsonProperty("backs")]
        public Dictionary<string, SavedBack> CardBacks { get; set; }

        [JsonProperty("sets")]
        public Dictionary<string, string> SetMap { get; set; }
    }
}
