using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TTS.CardTool.Processor.Json;
using TTS.CardTool.Processor.Options;
using TTS.CardTool.Processor.Parser;

namespace TTS.CardTool.Processor
{
    class DeckProcessor : IProcessor
    {
        private static readonly string _cacheFile = "cache.json";

        private readonly IProcessorOptions _options;

        private readonly HttpClient _client;
        private Cache _cache;

        public DeckProcessor(IProcessorOptions options)
        {
            _options = options;

            _client = new HttpClient
            {
                BaseAddress = new Uri(@"https://api.fiveringsdb.com")
            };
        }

        public async Task<Deck> CreateDeck(string decklist)
        {
            var parsedCards = ParseList(decklist);
            await ValidateCache(parsedCards);
            
            var (conflict, dynasty, identity) = CreateCards(parsedCards);
            return new Deck(new List<Pile>
            {
                new Pile("Conflict", _options.CardBacks["conflict"].Path, conflict),
                new Pile("Dynasty", _options.CardBacks["dynasty"].Path, dynasty),
                new Pile("Identity", _options.CardBacks["identity"].Path, identity)
            });
        }

        private List<ParsedCard> ParseList(string decklist)
        {
            var deck = new List<ParsedCard>();

            using var reader = new StringReader(decklist);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();

                // Find count 
                int count = -1;
                for (int i = 0; i < line.Length; i++)
                {
                    if (!char.IsDigit(line[i]))
                    {
                        int.TryParse(line.Substring(0, i), out count);
                        line = line.Substring(i).Trim();
                        break;
                    }
                }

                if (count <= 0)
                {
                    // Line was not a valid card 
                    continue;
                }

                // Parse name 
                int setStart = line.LastIndexOf('(');
                string name = line.Substring(0, setStart).Trim();
                line = line.Substring(setStart).Trim();

                // Parse set 
                string set = line[1..^1];

                deck.Add(new ParsedCard
                {
                    Count = count,
                    Name = name,
                    Set = set
                });
            }

            FixSetNames(deck);
            return deck;
        }

        private void FixSetNames(IEnumerable<ParsedCard> cards)
        {
            // This method is used to fix some incoherent set names between jigoku, bushi builder and five rings DB
            // It even fixes a set name discrepency between five rings DB database and five ring DB deckbuilder exporter.
            foreach (ParsedCard card in cards.Where(c => _options.SetMap.ContainsKey(c.Set)))
            {
                card.Set = _options.SetMap[card.Set];
            }
        }

        private async Task ValidateCache(IEnumerable<ParsedCard> cards)
        {
            await EnsureCache();
            await EnsureSets(cards.Select(c => c.Set));
        }

        private async Task EnsureCache()
        {
            if (_cache == null)
            {
                if (File.Exists(_cacheFile))
                {
                    await LoadCache();
                }
                else
                {
                    await UpdateCache();
                }
            }
        }

        private async Task EnsureSets(IEnumerable<string> sets)
        {
            if (sets.Distinct().Except(_cache.CachedData.Keys).Count() > 0)
            {
                await UpdateCache(true);
            }
        }

        private async Task LoadCache()
        {
            using var stream = File.OpenText(_cacheFile);
            string json = await stream.ReadToEndAsync();
            _cache = JsonConvert.DeserializeObject<Cache>(json);
        }

        private async Task UpdateCache(bool validateSets = false)
        {
            bool outdated = _cache != null && DateTime.Now > _cache.LastUpdate + TimeSpan.FromHours(24);

            if (_cache != null && !outdated && !validateSets)
            {
                return;
            }

            HttpResponseMessage setResponse = await _client.GetAsync(@"/packs");
            if (!setResponse.IsSuccessStatusCode)
            {
                throw new HttpRequestException("FiveRingsDB failed to handle request.");
            }

            string setsJson = await setResponse.Content.ReadAsStringAsync();
            DBSets sets = JsonConvert.DeserializeObject<DBSets>(setsJson);

            if (_cache != null && !outdated && validateSets && sets.Sets.Select(p => p.Name).Except(_cache.CachedData.Keys).Count() == 0)
            {
                return;
            }

            HttpResponseMessage response = await _client.GetAsync(@"/cards");
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("FiveRingsDB failed to handle request.");
            }

            string json = await response.Content.ReadAsStringAsync();
            DBCards cards = JsonConvert.DeserializeObject<DBCards>(json);

            _cache = new Cache
            {
                CachedData = new Dictionary<string, Dictionary<string, CacheCard>>(),
                LastUpdate = DateTime.Now
            };

            var setMap = new Dictionary<string, string>();
            foreach (DBSet set in sets.Sets)
            {
                _cache.CachedData.Add(set.Name, new Dictionary<string, CacheCard>());
                setMap.Add(set.Id, set.Name);
            }

            foreach (DBCard card in cards.Cards)
            {
                _cache.CachedData[setMap[card.Packs[0].Set.Id]].Add(card.Name, new CacheCard
                {
                    Id = card.Id,
                    ImageUrl = card.Packs[0].ImageUrl,
                    Side = card.Side
                });
            }

            string cacheJson = JsonConvert.SerializeObject(_cache);
            using var stream = File.CreateText(_cacheFile);
            stream.Write(cacheJson);
        }

        private (IEnumerable<Card> conflict, IEnumerable<Card> dynasty, IEnumerable<Card> identity) CreateCards(IEnumerable<ParsedCard> cards)
        {
            var conflict = new List<Card>();
            var dynasty = new List<Card>();
            var identity = new List<Card>();
            var errors = new List<ParsedCard>();

            foreach (ParsedCard card in cards)
            {
                if (_cache.CachedData.TryGetValue(card.Set, out var set) && set.TryGetValue(card.Name, out var cacheCard))
                {
                    var newCard = new Card(card.Count, card.Name, cacheCard.ImageUrl);
                    switch (cacheCard.Side)
                    {
                        case "conflict":
                            conflict.Add(newCard);
                            break;
                        case "dynasty":
                            dynasty.Add(newCard);
                            break;
                        default:
                            identity.Add(newCard);
                            break;
                    }
                }
                else
                {
                    errors.Add(card);
                }
            }

            if (errors.Count > 0)
            {
                StringBuilder builder = new StringBuilder()
                    .AppendLine("Cards not found:");
                foreach (ParsedCard card in errors)
                {
                    builder.AppendLine($"{card.Name} ({card.Set})");
                }

                throw new KeyNotFoundException(builder.ToString());
            }

            return (conflict, dynasty, identity);
        }
    }
}
