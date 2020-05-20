using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TTS.CardTool.Data.L5R.Json
{
    class Cache
    {
        [JsonProperty("last_updated")]
        public DateTime LastUpdate { get; set; }

        [JsonProperty("data")]
        public Dictionary<string, Dictionary<string, CacheCard>> CachedData { get; set; }
    }
}
