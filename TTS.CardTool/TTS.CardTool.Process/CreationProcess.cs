using System;
using System.Threading.Tasks;
using TTS.CardTool.Model;
using TTS.CardTool.Parser;

namespace TTS.CardTool.Process
{
    class CreationProcess : ICreationProcess
    {
        private readonly IDeckParser _parser;

        public CreationProcess(IDeckParser parser)
        {
            _parser = parser;
        }

        public async Task Create(string decklist, IProgress<(bool, string, int)> progress, IProgress<int> stepProgress)
        {
            progress.Report((true, "Parsing Deck List", 0));
            Deck deck = _parser.Parse(decklist);

            // Download Card Data
            // Download Card Art
            // Create TTS Data
        }
    }
}
