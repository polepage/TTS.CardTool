using System.Collections.Generic;
using System.Linq;

namespace TTS.CardTool.Model.L5R
{
    public class L5RDeck : Deck
    {
        public L5RDeck()
        {
            Base = new List<Card>();
            Conflict = new List<Card>();
            Dynasty = new List<Card>();
            Provinces = new List<Card>();
            Identity = new List<Card>();
        }

        public IList<Card> Base { get; }
        public IList<Card> Conflict { get; }
        public IList<Card> Dynasty { get; }
        public IList<Card> Provinces { get; }
        public IList<Card> Identity { get; } // Stronghold + Role

        public override int TotalCount => Base.Count > 0 ? Base.Count : Conflict.Count + Dynasty.Count + Provinces.Count + Identity.Count;

        public override IEnumerable<Card> AllCards => Base.Count > 0 ? Base : Conflict
                                                                                .Concat(Dynasty)
                                                                                .Concat(Provinces)
                                                                                .Concat(Identity);
    }
}
