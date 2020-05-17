using System.Collections.Generic;

namespace TTS.CardTool.Model
{
    public abstract class Deck
    {
        public abstract int TotalCount { get; }
        public abstract IEnumerable<Card> AllCards { get; }
    }
}
