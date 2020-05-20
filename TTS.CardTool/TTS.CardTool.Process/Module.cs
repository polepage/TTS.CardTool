using Prism.Ioc;
using Prism.Modularity;

namespace TTS.CardTool.Process
{
    [ModuleDependency("TTS.CardPool.Parser.Module")]
    [ModuleDependency("TTS.CardTool.Data.Module")]
    [ModuleDependency("TTS.CardTool.Downloader.Module")]
    public class Module : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider) { }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ICreationProcess, CreationProcess>();
        }
    }
}
