using System;
using System.Threading.Tasks;
using TTS.CardTool.Model;

namespace TTS.CardTool.Output
{
    public interface IDeckOutput
    {
        Task CreateOutput(string targetFolder, string baseName, Deck deck);
    }
}
