using System;
using System.Threading.Tasks;

namespace TTS.CardTool.Process
{
    public interface ICreationProcess
    {
        Task Create(string decklist, IProgress<(bool, string, int)> progress, IProgress<int> stepProgress);
    }
}
