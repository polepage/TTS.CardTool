using System;
using System.Threading.Tasks;
using TTS.CardTool.Model;

namespace TTS.CardTool.Downloader
{
    public interface IImageDownloader
    {
        Task DownloadImages(Deck deck, IProgress<int> progress);
        void DeleteImages();
    }
}
