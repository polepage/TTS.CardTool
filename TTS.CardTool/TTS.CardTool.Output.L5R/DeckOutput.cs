using System.Threading.Tasks;
using TTS.CardTool.Model;
using TTS.CardTool.Model.L5R;

namespace TTS.CardTool.Output.L5R
{
    class DeckOutput : DeckOutputBase
    {
        public override async Task CreateOutput(string targetFolder, string baseName, Deck deck)
        {
            await Task.Run(() => CreateOutput(targetFolder, baseName, (L5RDeck)deck));
        }

        private void CreateOutput(string targetFolder, string baseName, L5RDeck deck)
        {
            OutputDeck(targetFolder, baseName + "_dynasty", deck.Dynasty, "dynastyback.png");
            OutputDeck(targetFolder, baseName + "_conflict", deck.Conflict, "conflictback.png");
            if (deck.Others.Count > 0)
            {
                OutputDeck(targetFolder, baseName, deck.Others, "othersback.png");
            }
        }
    }
}
