using System.Collections.Generic;

namespace TTS.CardTool.Processor
{
    public class Deck
    {
        public Deck(IEnumerable<Pile> piles)
        {
            Piles = new List<Pile>(piles);
        }

        public IEnumerable<Pile> Piles { get; }
    }
}
