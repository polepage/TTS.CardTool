using System.Threading.Tasks;
using TTS.CardTool.Model;

namespace TTS.CardTool.Data
{
    public interface IDataDownloader
    {
        Task UpdateDeck(Deck deck);
    }
}
