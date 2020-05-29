using Prism.Ioc;
using Prism.Modularity;

namespace TTS.CardTool.Processor
{
    [ModuleDependency("TTS.CardTool.Processor.Options.Module")]
    public class Module : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider) { }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IProcessor, DeckProcessor>();
        }
    }
}
