using System.Collections.Generic;

namespace TTS.CardTool.Processor
{
    public class Deck
    {
        public Deck(IEnumerable<Pile> piles)
        {
            CardPiles = new List<Pile>(piles);
        }

        public IEnumerable<Pile> CardPiles { get; }
    }
}
