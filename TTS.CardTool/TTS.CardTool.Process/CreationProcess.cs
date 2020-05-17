using System;
using System.Threading.Tasks;

namespace TTS.CardTool.Process
{
    class CreationProcess : ICreationProcess
    {
        public CreationProcess()
        {

        }

        public async Task Create(string decklist, IProgress<(bool, string, int)> progress, IProgress<int> stepProgress)
        {
            // Parse Decklist
            // Download Card Data
            // Download Card Art
            // Create TTS Data
        }
    }
}
