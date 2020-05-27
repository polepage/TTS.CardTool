using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Threading.Tasks;
using TTS.CardTool.Processor;

namespace TTS.CardTool.Converter
{
    class DeckConverter : IDeckConverter
    {
        private readonly IProcessor _deckProcessor;

        public DeckConverter(IProcessor deckProcessor)
        {
            _deckProcessor = deckProcessor;
        }

        public async Task<string> Convert(string decklist)
        {
            if (decklist == null)
            {
                return null;
            }

            Deck deck = await _deckProcessor.CreateDeck(decklist);

            return JsonConvert.SerializeObject(deck, Formatting.Indented, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }
    }
}
