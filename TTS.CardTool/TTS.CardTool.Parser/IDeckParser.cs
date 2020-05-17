using TTS.CardTool.Model;

namespace TTS.CardTool.Parser
{
    public interface IDeckParser
    {
        Deck Parse(string decklist);
    }
}
