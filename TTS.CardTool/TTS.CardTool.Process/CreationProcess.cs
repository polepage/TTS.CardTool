using System;
using System.Threading.Tasks;
using TTS.CardTool.Data;
using TTS.CardTool.Downloader;
using TTS.CardTool.Model;
using TTS.CardTool.Output;
using TTS.CardTool.Parser;

namespace TTS.CardTool.Process
{
    class CreationProcess : ICreationProcess
    {
        private readonly IDeckParser _parser;
        private readonly IDataDownloader _dataDownloader;
        private readonly IImageDownloader _imageDownloader;
        private readonly IDeckOutput _deckOutput;

        public CreationProcess(IDeckParser parser, IDataDownloader dataDownloader, IImageDownloader imageDownloader, IDeckOutput deckOutput)
        {
            _parser = parser;
            _dataDownloader = dataDownloader;
            _imageDownloader = imageDownloader;
            _deckOutput = deckOutput;
        }

        public async Task Create(string decklist, string targetFolder, string baseName, IProgress<(bool, string, int)> progress, IProgress<int> stepProgress)
        {
            progress.Report((true, "Parsing Deck List", 0));
            Deck deck = _parser.Parse(decklist);

            progress.Report((true, "Downloading Card Data", 0));
            await _dataDownloader.UpdateDeck(deck);

            progress.Report((false, "Downloading Card Images", deck.TotalCount));
            await _imageDownloader.DownloadImages(deck, stepProgress);

            progress.Report((true, "Creating TTS Data", 0));
            await _deckOutput.CreateOutput(targetFolder, baseName, deck);

            progress.Report((true, "Removing Temporary Files", 0));
            _imageDownloader.DeleteImages();
        }
    }
}
