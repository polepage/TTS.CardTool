using System;
using System.Threading.Tasks;

namespace TTS.CardTool.Process
{
    public interface ICreationProcess
    {
        Task Create(string decklist, string targetFolder, string baseName, IProgress<(bool, string, int)> progress, IProgress<int> stepProgress);
    }
}
