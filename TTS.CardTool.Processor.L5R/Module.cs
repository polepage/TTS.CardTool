using Prism.Ioc;
using Prism.Modularity;

namespace TTS.CardTool.Processor
{
    public class Module : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider) { }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IProcessor, DeckProcessor>();
        }
    }
}
