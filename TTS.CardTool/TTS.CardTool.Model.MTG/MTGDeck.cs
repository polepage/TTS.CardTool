using System.Collections.Generic;
using System.Linq;

namespace TTS.CardTool.Model.MTG
{
    public class MTGDeck : Deck
    {
        public MTGDeck()
        {
            Cards = new List<Card>();
            Related = new List<Card>();
        }

        public IList<Card> Cards { get; }
        public IList<Card> Related { get; }

        public override int TotalCount => Cards.Count + Related.Count;
        public override IEnumerable<Card> AllCards => Cards.Concat(Related);
    }
}
