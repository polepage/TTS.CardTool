using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TTS.CardTool.Processor.Options.Json;

namespace TTS.CardTool.Processor.Options
{
    class ProcessorOptions : IProcessorOptions
    {
        private static readonly string _optionsFile = "processoroptions.json";

        private Dictionary<string, CardBack> _cardBacks;
        private Dictionary<string, string> _setMap;

        public ProcessorOptions()
        {
            Load();
        }

        public IReadOnlyDictionary<string, CardBack> CardBacks => _cardBacks;
        public IDictionary<string, string> SetMap => _setMap;

        public void Save()
        {
            string json = JsonConvert.SerializeObject(new SavedOptions
            {
                CardBacks = _cardBacks.ToDictionary(pair => pair.Key, pair => new SavedBack
                {
                    Name = pair.Value.Name,
                    Path = pair.Value.Path
                }),
                SetMap = new Dictionary<string, string>(_setMap)
            });

            using var stream = File.CreateText(_optionsFile);
            stream.Write(json);
        }

        private void Load()
        {
            using var stream = File.OpenText(_optionsFile);
            string json = stream.ReadToEnd();
            var savedOptions = JsonConvert.DeserializeObject<SavedOptions>(json);

            _cardBacks = new Dictionary<string, CardBack>(savedOptions.CardBacks
                .Select(sb => new KeyValuePair<string, CardBack>(sb.Key, new CardBack(sb.Value.Name, sb.Value.Path))));
            _setMap = new Dictionary<string, string>(savedOptions.SetMap);
        }
    }
}
