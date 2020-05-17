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
    public class Module : IModule
    {
        public static void ConfigureViewModelLocator(Func<Type, object> resolver)
        {
            ViewModelLocationProvider.Register<MainWindow>(() => resolver(typeof(MainWindowViewModel)));
            ViewModelLocationProvider.Register<DeckView>(() => resolver(typeof(DeckViewViewModel)));
            ViewModelLocationProvider.Register<Waiter>(() => resolver(typeof(WaiterViewModel)));
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            IRegionManager regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(NavigationParams.DeckRegion, typeof(DeckView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry) { }
    }
}
