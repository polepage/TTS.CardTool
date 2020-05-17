using Prism.Ioc;
using Prism.Modularity;

namespace TTS.CardTool.Process
{
    public class Module : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider) { }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ICreationProcess, CreationProcess>();
        }
    }
}
