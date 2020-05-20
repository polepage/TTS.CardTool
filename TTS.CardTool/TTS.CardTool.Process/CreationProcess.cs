using System;
using System.Threading.Tasks;
using TTS.CardTool.Data;
using TTS.CardTool.Downloader;
using TTS.CardTool.Model;
using TTS.CardTool.Parser;

namespace TTS.CardTool.Process
{
    class CreationProcess : ICreationProcess
    {
        private readonly IDeckParser _parser;
        private readonly IDataDownloader _dataDownloader;
        private readonly IImageDownloader _imageDownloader;

        public CreationProcess(IDeckParser parser, IDataDownloader dataDownloader, IImageDownloader imageDownloader)
        {
            _parser = parser;
            _dataDownloader = dataDownloader;
            _imageDownloader = imageDownloader;
        }

        public async Task Create(string decklist, IProgress<(bool, string, int)> progress, IProgress<int> stepProgress)
        {
            progress.Report((true, "Parsing Deck List", 0));
            Deck deck = _parser.Parse(decklist);

            progress.Report((true, "Downloading Card Data", 0));
            await _dataDownloader.UpdateDeck(deck);

            progress.Report((false, "Downloading Card Images", deck.TotalCount));
            await _imageDownloader.DownloadImages(deck, stepProgress);

            // Create TTS Data

            progress.Report((true, "Removing Temporary Files", 0));
            _imageDownloader.DeleteImages();
        }
    }
}
