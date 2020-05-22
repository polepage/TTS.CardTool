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
        private static readonly int _columns = 10;
        private static readonly int _rows = 7;

        public abstract Task CreateOutput(string targetFolder, string baseName, Deck deck);

        protected void OutputDeck(string targetFolder, string fileName, IEnumerable<Card> cards, string cardBackPath)
        {
            var imagePaths = cards.SelectMany(c => Enumerable.Repeat(c.ImagePath, c.Count));
            var cardNames = cards.SelectMany(c => Enumerable.Repeat(c.Name, c.Count));

            if (cards.Select(c => c.Count).Sum() <= 70)
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
            for (int i = 0, page = 1; i < imagePaths.Count(); i += 70, page++)
            {
                OutputSingleDeck(targetFolder, $"{fileName}_{page}", imagePaths.Skip(i).Take(70), cardNames.Skip(i).Take(70), cardBackPath);
            }
        }

        private void OutputSingleDeck(string targetFolder, string fileName, IEnumerable<string> imagePaths, IEnumerable<string> cardNames, string cardBackPath)
        {
            CreateFaceTexture(targetFolder + fileName + ".png", imagePaths);
            CreateBackTexture(targetFolder + fileName + "_back.png", cardBackPath);
            CreateDeckFile(targetFolder + fileName + ".txt", cardNames);
        }

        private void CreateFaceTexture(string targetPath, IEnumerable<string> imagePaths)
        {
            Size expectedCardSize = GetImageSize(imagePaths.First());
            using var output = new Image<Rgba32>(expectedCardSize.Width * _columns, expectedCardSize.Height * _rows, new Rgba32(0, 0, 0));

            int x = 0;
            int y = 0;
            int actualRows = 1;
            int cardCount = 0;
            foreach (string image in imagePaths)
            {
                if (x >= expectedCardSize.Width * _columns)
                {
                    x = 0;
                    y += expectedCardSize.Height;
                    actualRows++;
                }

                using var visual = Image.Load<Rgba32>(image);
                output.Mutate(o => o.DrawImage(visual, new Point(x, y), 1));
                x += expectedCardSize.Width;
                cardCount++;
            }

            if (cardCount == 1)
            {
                output.Mutate(o => o.Crop(new Rectangle(0, 0, expectedCardSize.Width, expectedCardSize.Height)));
            }
            else
            {
                output.Mutate(o => o
                    .Crop(new Rectangle(0, 0, _columns * expectedCardSize.Width, actualRows * expectedCardSize.Height))
                    .Resize(new Size(4096, 4096 * actualRows * expectedCardSize.Height / (_columns * expectedCardSize.Width))));
            }

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
