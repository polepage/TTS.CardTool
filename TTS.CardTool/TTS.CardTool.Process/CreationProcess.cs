using System;
using System.Threading.Tasks;
using TTS.CardTool.Data;
using TTS.CardTool.Model;
using TTS.CardTool.Parser;

namespace TTS.CardTool.Process
{
    class CreationProcess : ICreationProcess
    {
        private readonly IDeckParser _parser;
        private readonly IDataDownloader _dataDownloader;

        public CreationProcess(IDeckParser parser, IDataDownloader dataDownloader)
        {
            _parser = parser;
            _dataDownloader = dataDownloader;
        }

        public async Task Create(string decklist, IProgress<(bool, string, int)> progress, IProgress<int> stepProgress)
        {
            progress.Report((true, "Parsing Deck List", 0));
            Deck deck = _parser.Parse(decklist);

            progress.Report((true, "Downloading Card Data", 0));
            await _dataDownloader.UpdateDeck(deck);

            // Download Card Art
            // Create TTS Data
        }
    }
}
