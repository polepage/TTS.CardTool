using System.Collections.Generic;

namespace TTS.CardTool.Processor.Options
{
    public interface IProcessorOptions
    {
        IReadOnlyDictionary<string, CardBack> CardBacks { get; }
        IDictionary<string, string> SetMap { get; }

        void Save();
    }
}
