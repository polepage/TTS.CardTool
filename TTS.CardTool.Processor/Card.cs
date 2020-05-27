namespace TTS.CardTool.Processor
{
    public class Card
    {
        public Card(int count, string name, string image)
        {
            Count = count;
            Name = name;
            ImageUrl = image;
        }

        public int Count { get; }
        public string Name { get; }
        public string ImageUrl { get; }
    }
}
