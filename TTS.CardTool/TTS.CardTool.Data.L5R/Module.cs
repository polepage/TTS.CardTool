using Prism.Ioc;
using Prism.Modularity;

namespace TTS.CardTool.Data.L5R
{
    public class Module : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider) { }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IDataDownloader, DataDownloader>();
        }
    }
}
