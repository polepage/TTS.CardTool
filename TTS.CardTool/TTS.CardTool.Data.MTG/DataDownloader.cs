using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TTS.CardTool.Data.MTG.Json;
using TTS.CardTool.Model;
using TTS.CardTool.Model.MTG;

namespace TTS.CardTool.Data.MTG
{
    class DataDownloader : IDataDownloader
    {
        private static readonly string Json = "application/json";
        private static readonly Encoding Encoding = Encoding.UTF8;

        private readonly HttpClient _client;

        public DataDownloader()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(@"https://api.scryfall.com")
            };
        }

        public async Task UpdateDeck(Deck deck)
        {
            await UpdateDeck((MTGDeck)deck);
        }

        private async Task UpdateDeck(MTGDeck deck)
        {
            List<ScryCard> cardData = await GetCardsData(deck.Cards);
            UpdateCards(cardData, deck.Cards, deck.Related);
            List<ScryCard> relatedCardData = await GetRelatedCardsData(deck.Related);
            UpdateRelatedCards(relatedCardData, deck.Related);
        }

        private async Task<List<ScryCard>> GetCardsData(IList<Card> cards)
        {
            var results = new List<ScryCard>();
            var errors = new List<ScryCollectionIdentifier>();
            for (int i = 0; i < cards.Count; i += 75)
            {
                ScryCollectionResults collectionResults = await GetCardCollection(cards.Skip(i).Take(75));
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

        private async Task<List<ScryCard>> GetRelatedCardsData(IList<Card> related)
        {
            var results = new List<ScryCard>();
            foreach (Card card in related)
            {
                results.Add(await GetCardById(card));
                await Task.Delay(100);
            }

            return results;
        }

        private async Task<ScryCollectionResults> GetCardCollection(IEnumerable<Card> cards)
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

        private async Task<ScryCard> GetCardById(Card card)
        {
            HttpResponseMessage response = await _client.GetAsync(@"/cards/" + card.Id);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Scryfall failed to handle request.");
            }

            string jsonResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ScryCard>(jsonResult);
        }

        private void UpdateCards(IList<ScryCard> updates, IList<Card> cards, IList<Card> related)
        {
            if (updates.Count != cards.Count)
            {
                throw new ArgumentOutOfRangeException("Update cards must have both the same collecton size.");
            }

            for (int i = 0; i < updates.Count; i++)
            {
                UpdateCard(updates[i], cards[i], related);
            }
        }

        private void UpdateCard(ScryCard update, Card card, IList<Card> related)
        {
            card.Id = update.Id;
            card.Name = update.Name;
            card.Set = update.Set.ToUpper();
            card.ImageUrl = update.Layout == "transform" ? update.CardFaces[0].ImageUris.PNG : update.ImageUris.PNG;

            if (update.Layout == "transform")
            {
                ScryCardFace backFace = update.CardFaces[1];
                related.Add(new Card
                {
                    Id = card.Id,
                    Name = card.Name,
                    Set = card.Set,
                    Count = 1,
                    ImageUrl = backFace.ImageUris.PNG
                });
            }

            if (update.RelatedCards != null)
            {
                foreach (ScryReference reference in update.RelatedCards)
                {
                    if (reference.Component == "token" || reference.Component == "meld_result")
                    {
                        if (related.FirstOrDefault(c => c.Id.Equals(reference.Id)) == null)
                        {
                            related.Add(new Card
                            {
                                Id = reference.Id,
                                Name = reference.Name,
                                Count = 1
                            });
                        }
                    }
                }
            }
        }

        private void UpdateRelatedCards(IList<ScryCard> updates, IList<Card> related)
        {
            if (updates.Count != related.Count)
            {
                throw new ArgumentOutOfRangeException("Update cards must have both the same collecton size.");
            }

            for (int i = 0; i < updates.Count; i++)
            {
                if (related[i].ImageUrl == null)
                {
                    UpdateRelatedCard(updates[i], related[i]);
                }
            }
        }

        private void UpdateRelatedCard(ScryCard update, Card card)
        {
            card.Set = update.Set.ToUpper();

            if (update.Layout == "double_faced_token")
            {
                if (update.CardFaces[0].Name == card.Name)
                {
                    card.ImageUrl = update.CardFaces[0].ImageUris.PNG;
                }
                else
                {
                    card.ImageUrl = update.CardFaces[1].ImageUris.PNG;
                }
            }
            else
            {
                card.ImageUrl = update.ImageUris.PNG;
            }
        }
    }
}