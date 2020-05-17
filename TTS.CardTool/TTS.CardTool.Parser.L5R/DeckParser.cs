using System.IO;
using TTS.CardTool.Model;
using TTS.CardTool.Model.L5R;

namespace TTS.CardTool.Parser.L5R
{
    class DeckParser : IDeckParser
    {
        public Deck Parse(string decklist)
        {
            var deck = new L5RDeck();

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

                deck.Base.Add(new Card
                {
                    Count = count,
                    Name = name,
                    Set = set
                });
            }

            return deck;
        }
    }
}
