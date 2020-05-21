using System;
using System.Threading.Tasks;
using TTS.CardTool.Model;
using TTS.CardTool.Model.MTG;

namespace TTS.CardTool.Output.MTG
{
    class DeckOutput : DeckOutputBase
    {
        public override async Task CreateOutput(string targetFolder, string baseName, Deck deck)
        {
            await Task.Run(() => CreateOutput(targetFolder, baseName, (MTGDeck)deck));
        }

        private void CreateOutput(string targetFolder, string baseName, MTGDeck deck)
        {
            OutputDeck(targetFolder, baseName, deck.Cards, "mtgback.png");
            if (deck.Related.Count > 0)
            {
                OutputDeck(targetFolder, baseName + "_Tokens", deck.Related, "mtgback.png");
            }
        }
    }
}
