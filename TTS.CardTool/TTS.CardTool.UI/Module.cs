using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using System;
using TTS.CardTool.UI.Navigation;
using TTS.CardTool.UI.View;
using TTS.CardTool.UI.ViewModel;

namespace TTS.CardTool.UI
{
    [ModuleDependency("TTS.CardTool.Process.Module")]
    [ModuleDependency("TTS.CardTool.Cloud.Module")]
    public class Module : IModule
    {
        public static void ConfigureViewModelLocator(Func<Type, object> resolver)
        {
            ViewModelLocationProvider.Register<MainWindow>(() => resolver(typeof(MainWindowViewModel)));
            ViewModelLocationProvider.Register<DeckView>(() => resolver(typeof(DeckViewViewModel)));
            ViewModelLocationProvider.Register<Waiter>(() => resolver(typeof(WaiterViewModel)));
            ViewModelLocationProvider.Register<CloudConnection>(() => resolver(typeof(CloudConnectionViewModel)));
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            IRegionManager regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(NavigationParams.DeckRegion, typeof(DeckView));
            regionManager.RegisterViewWithRegion(NavigationParams.CloudRegion, typeof(CloudConnection));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry) { }
    }
}
