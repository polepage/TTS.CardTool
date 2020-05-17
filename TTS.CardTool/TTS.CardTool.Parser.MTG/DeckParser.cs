using System.IO;
using TTS.CardTool.Model;
using TTS.CardTool.Model.MTG;

namespace TTS.CardTool.Parser.MTG
{
    class DeckParser : IDeckParser
    {
        public Deck Parse(string decklist)
        {
            var deck = new MTGDeck();

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

                deck.Cards.Add(new Card
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
