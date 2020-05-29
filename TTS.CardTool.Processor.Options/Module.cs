using Prism.Ioc;
using Prism.Modularity;

namespace TTS.CardTool.Processor.Options
{
    public class Module : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider) { }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IProcessorOptions, ProcessorOptions>();
        }
    }
}
