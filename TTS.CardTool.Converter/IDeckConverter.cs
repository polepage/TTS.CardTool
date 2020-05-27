using System.Threading.Tasks;

namespace TTS.CardTool.Converter
{
    public interface IDeckConverter
    {
        Task<string> Convert(string decklist);
    }
}
