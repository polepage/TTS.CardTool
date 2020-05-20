using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TTS.CardTool.Data.L5R.Json;
using TTS.CardTool.Model;
using TTS.CardTool.Model.L5R;

namespace TTS.CardTool.Data.L5R
{
    class DataDownloader : IDataDownloader
    {
        private static readonly string _cacheFile = "cache";

        private readonly HttpClient _client;
        private Cache _cache;

        public DataDownloader()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(@"https://api.fiveringsdb.com")
            };
        }

        public async Task UpdateDeck(Deck deck)
        {
            await UpdateDeck((L5RDeck)deck);
        }

        private async Task UpdateDeck(L5RDeck deck)
        {
            await EnsureCache();
            await EnsureSets(deck.Base.Select(c => c.Set));
            UpdateCards(deck);
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
            FRDBSets sets = JsonConvert.DeserializeObject<FRDBSets>(setsJson);

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
            FRDBCards cards = JsonConvert.DeserializeObject<FRDBCards>(json);

            _cache = new Cache
            {
                CachedData = new Dictionary<string, Dictionary<string, CacheCard>>(),
                LastUpdate = DateTime.Now
            };

            var setMap = new Dictionary<string, string>();
            foreach (FRDBSet set in sets.Sets)
            {
                _cache.CachedData.Add(set.Name, new Dictionary<string, CacheCard>());
                setMap.Add(set.Id, set.Name);
            }

            foreach (FRDBCard card in cards.Cards)
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

        private void UpdateCards(L5RDeck deck)
        {
            var errors = new List<Card>();
            foreach (Card card in deck.Base)
            {
                if (_cache.CachedData.TryGetValue(card.Set, out var set) && set.TryGetValue(card.Name, out var cacheCard))
                {
                    card.Id = cacheCard.Id;
                    card.ImageUrl = cacheCard.ImageUrl;

                    switch (cacheCard.Side)
                    {
                        case "conflict":
                            deck.Conflict.Add(card);
                            break;
                        case "dynasty":
                            deck.Dynasty.Add(card);
                            break;
                        default:
                            deck.Others.Add(card);
                            break;
                    }
                }
                else
                {
                    errors.Add(card);
                }
            }

            deck.Base.Clear();

            if (errors.Count > 0)
            {
                StringBuilder builder = new StringBuilder()
                    .AppendLine("Cards not found:");
                foreach (Card card in errors)
                {
                    builder.AppendLine($"{card.Name} ({card.Set})");
                }

                throw new KeyNotFoundException(builder.ToString());
            }
        }
    }
}
