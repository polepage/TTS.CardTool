using System.Threading.Tasks;

namespace TTS.CardTool.Processor
{
    public interface IProcessor
    {
        Task<Deck> CreateDeck(string decklist);
    }
}
