using System;
using System.Linq;

namespace TTS.CardTool.Converter
{
    class DeckConverter : IDeckConverter
    {
        public string Convert(string decklist)
        {
            if (decklist == null)
            {
                return null;
            }

            return new string(decklist.ToCharArray().Reverse().ToArray());
        }
    }
}
