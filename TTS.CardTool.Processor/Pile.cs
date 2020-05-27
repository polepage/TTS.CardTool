using System.Collections.Generic;

namespace TTS.CardTool.Processor
{
    public class Pile
    {
        public Pile(string name, string image, IEnumerable<Card> cards)
        {
            Name = name;
            ImageUrl = image;
            Cards = new List<Card>(cards);
        }

        public string Name { get; }
        public string ImageUrl { get; }
        public IEnumerable<Card> Cards { get; }
    }
}
