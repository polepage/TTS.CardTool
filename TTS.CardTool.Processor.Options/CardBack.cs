namespace TTS.CardTool.Processor.Options
{
    public class CardBack
    {
        internal CardBack(string name, string path)
        {
            Name = name;
            Path = path;
        }

        public string Name { get; }
        public string Path { get; set; }
    }
}
