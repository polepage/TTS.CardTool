using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using TTS.CardTool.Model;

namespace TTS.CardTool.Downloader
{
    class ImageDownloader : IImageDownloader
    {
        public async Task DownloadImages(Deck deck, IProgress<int> progress)
        {
            int processed = 0;
            Directory.CreateDirectory("downloaded");

            using var client = new WebClient();
            foreach (Card card in deck.AllCards)
            {
                string imageName = Path.GetFileName(card.ImageUrl);
                int parameterIndex = imageName.IndexOf('?');
                if (parameterIndex >= 0)
                {
                    imageName = imageName.Substring(0, parameterIndex);
                }

                card.ImagePath = $"downloaded\\{imageName}";
                await client.DownloadFileTaskAsync(card.ImageUrl, card.ImagePath);

                processed++;
                progress.Report(processed);
            }
        }

        public void DeleteImages()
        {
            Directory.Delete("downloaded", true);
        }
    }
}
