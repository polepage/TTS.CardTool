using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TTS.CardTool.Model;

namespace TTS.CardTool.Output
{
    public abstract class DeckOutputBase : IDeckOutput
    {
        public abstract Task CreateOutput(string targetFolder, string baseName, Deck deck);

        protected void OutputDeck(string targetFolder, string fileName, IEnumerable<Card> cards, string cardBackPath)
        {
            var imagePaths = cards.SelectMany(c => Enumerable.Repeat(c.ImagePath, c.Count));
            var cardNames = cards.SelectMany(c => Enumerable.Repeat(c.Name, c.Count));

            if (cards.Select(c => c.Count).Sum() < 70)
            {
                OutputSingleDeck(targetFolder, fileName, imagePaths, cardNames, cardBackPath);
            }
            else
            {
                OutputNumberedDeck(targetFolder, fileName, imagePaths, cardNames, cardBackPath);
            }
        }

        private void OutputNumberedDeck(string targetFolder, string fileName, IEnumerable<string> imagePaths, IEnumerable<string> cardNames, string cardBackPath)
        {
            for (int i = 0, page = 1; i < imagePaths.Count(); i += 69, page++)
            {
                OutputSingleDeck(targetFolder, $"{fileName}_{page}", imagePaths.Skip(i).Take(69), cardNames.Skip(i).Take(69), cardBackPath);
            }
        }

        private void OutputSingleDeck(string targetFolder, string fileName, IEnumerable<string> imagePaths, IEnumerable<string> cardNames, string cardBackPath)
        {
            CreateFaceTexture(targetFolder + fileName + ".png", imagePaths, cardBackPath);
            CreateBackTexture(targetFolder + fileName + "_back.png", cardBackPath);
            CreateDeckFile(targetFolder + fileName + ".txt", cardNames);
        }

        private void CreateFaceTexture(string targetPath, IEnumerable<string> imagePaths, string cardBackPath)
        {
            Size expectedCardSize = GetImageSize(imagePaths.First());
            using var output = new Image<Rgba32>(expectedCardSize.Width * 10, expectedCardSize.Height * 7, new Rgba32(0, 0, 0));

            int x = 0;
            int y = 0;
            foreach (string image in imagePaths)
            {
                using var visual = Image.Load<Rgba32>(image);
                output.Mutate(o => o.DrawImage(visual, new Point(x, y), 1));

                x += expectedCardSize.Width;
                if (x >= expectedCardSize.Width * 10)
                {
                    x = 0;
                    y += expectedCardSize.Height;
                }
            }

            using (var visual = Image.Load<Rgba32>(cardBackPath))
            {
                visual.Mutate(o => o.Resize(expectedCardSize));
                output.Mutate(o => o.DrawImage(visual, new Point(expectedCardSize.Width * 9, expectedCardSize.Height * 6), 1));
            }

            output.Mutate(o => o.Resize(new Size(4096, 4096 * 7 * expectedCardSize.Height / (10 * expectedCardSize.Width))));

            using var outputStream = File.OpenWrite(targetPath);
            output.SaveAsPng(outputStream);
        }

        private void CreateBackTexture(string targetPath, string cardBackPath)
        {
            using var visual = Image.Load<Rgba32>(cardBackPath);
            using var output = File.OpenWrite(targetPath);
            visual.SaveAsPng(output);
        }

        private void CreateDeckFile(string targetPath, IEnumerable<string> cardNames)
        {
            using var stream = File.CreateText(targetPath);
            foreach (string name in cardNames)
            {
                stream.WriteLine(name);
            }
        }

        private Size GetImageSize(string path)
        {
            using var image = Image.Load<Rgba32>(path);
            return image.Size();
        }
    }
}
