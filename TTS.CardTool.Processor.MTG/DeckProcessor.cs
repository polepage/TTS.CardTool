using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TTS.CardTool.Processor.Json;
using TTS.CardTool.Processor.Parser;

namespace TTS.CardTool.Processor
{
    class DeckProcessor : IProcessor
    {
        private static readonly string Json = "application/json";
        private static readonly Encoding Encoding = Encoding.UTF8;

        private readonly HttpClient _client;

        public DeckProcessor()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(@"https://api.scryfall.com")
            };
        }

        public async Task<Deck> CreateDeck(string decklist)
        {
            var parsedCards = ParseList(decklist);
            var cardsData = await GetCardsData(parsedCards);
            var relatedCardsData = await GetRelatedCardsData(cardsData.SelectMany(sc => sc?.RelatedCards ?? Enumerable.Empty<ScryReference>()));

            return new Deck(new List<Pile>
            {
                new Pile("Main", "TODO", CreateMainCardPile(cardsData.Zip(parsedCards, (sc, pc) => (sc, pc.Count)))),
                new Pile("Tokens", "TODO", CreateTokenPile(cardsData
                                                            .Where(sc => sc.Layout == "transform")
                                                            .Concat(relatedCardsData)))
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

                // Parse set 
                int setStart = -1;
                int setEnd = line.LastIndexOf(')');
                string set = null;
                if (setEnd >= 0)
                {
                    setStart = line.LastIndexOf('(', setEnd);
                    set = line.Substring(setStart + 1, setEnd - setStart - 1);
                }

                // Parse name 
                string name;
                if (setStart < 0)
                {
                    name = line;
                }
                else
                {
                    name = line.Substring(0, setStart).Trim();
                }

                deck.Add(new ParsedCard
                {
                    Count = count,
                    Name = name,
                    Set = set
                });
            }

            return deck;
        }

        private async Task<List<ScryCard>> GetCardsData(IList<ParsedCard> parsedCards)
        {
                var results = new List<ScryCard>();
                var errors = new List<ScryCollectionIdentifier>();
                for (int i = 0; i < parsedCards.Count; i += 75)
                {
                    ScryCollectionResults collectionResults = await GetCardCollection(parsedCards.Skip(i).Take(75));
                    results.AddRange(collectionResults.Cards);
                    errors.AddRange(collectionResults.Errors);

                    // Polite Delay 
                    await Task.Delay(100);
                }

                if (errors.Count > 0)
                {
                    StringBuilder builder = new StringBuilder()
                        .AppendLine("Cards not found:");
                    foreach (ScryCollectionIdentifier id in errors)
                    {
                        if (id.Set != null)
                        {
                            builder.AppendLine($"{id.Name} ({id.Set.ToUpper()})");
                        }
                        else
                        {
                            builder.AppendLine(id.Name);
                        }
                    }

                    throw new KeyNotFoundException(builder.ToString());
                }

                return results;
        }

        private async Task<ScryCollectionResults> GetCardCollection(IEnumerable<ParsedCard> cards)
        {
            var collectionParams = new ScryCollectionParameters
            {
                Identifiers = cards.Select(c => new ScryCollectionIdentifier
                {
                    Name = c.Name,
                    Set = c.Set?.ToLower()
                }).ToList()
            };

            string jsonParams = JsonConvert.SerializeObject(collectionParams);
            HttpResponseMessage response = await _client.PostAsync(@"/cards/collection", new StringContent(jsonParams, Encoding, Json));

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Scryfall failed to handle request.");
            }

            string jsonResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ScryCollectionResults>(jsonResult);
        }

        private async Task<List<ScryCard>> GetRelatedCardsData(IEnumerable<ScryReference> related)
        {
            var results = new List<ScryCard>();
            foreach (ScryReference reference in related
                .Where(r => r.Component == "token" || r.Component == "meld_result")
                .Where(r => results.FirstOrDefault(sc => sc.Id.Equals(r.Id)) == null))
            {
                results.Add(await GetCardById(reference.Id));
                await Task.Delay(100);
            }

            return results;
        }

        private async Task<ScryCard> GetCardById(string id)
        {
            HttpResponseMessage response = await _client.GetAsync(@"/cards/" + id);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Scryfall failed to handle request.");
            }

            string jsonResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ScryCard>(jsonResult);
        }

        private List<Card> CreateMainCardPile(IEnumerable<(ScryCard card, int count)> cards)
        {
            var results = new List<Card>();
            foreach ((ScryCard card, int count) in cards)
            {
                results.Add(new Card(count,
                                     card.Name,
                                     card.Layout == "transform" ? card.CardFaces[0].ImageUris.Normal : card.ImageUris.Normal));
            }

            return results;
        }

        private List<Card> CreateTokenPile(IEnumerable<ScryCard> cards)
        {
            var results = new List<Card>();
            foreach (ScryCard card in cards)
            {
                string image;
                if (card.Layout == "transform")
                {
                    image = card.CardFaces[1].ImageUris.Normal;
                }
                else if (card.Layout == "double_faced_token")
                {
                    if (card.Name.Equals(card.CardFaces[0].Name))
                    {
                        image = card.CardFaces[0].ImageUris.Normal;
                    }
                    else
                    {
                        image = card.CardFaces[1].ImageUris.Normal;
                    }
                }
                else
                {
                    image = card.ImageUris.Normal;
                }

                results.Add(new Card(1, card.Name, image));
            }

            return results;
        }
    }
}
