using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Windows;
using TTS.CardTool.UI.Dialogs;
using TTS.CardTool.UI.Navigation;
using TTS.CardTool.UI.View;
using TTS.CardTool.UI.ViewModel;

namespace TTS.CardTool.UI
{
    [ModuleDependency("TTS.CardTool.Converter.Module")]
    [ModuleDependency("TTS.CardTool.Processor.Options.Module")]
    public class Module : IModule
    {
        public static void ConfigureViewModelLocator(Func<Type, object> resolver)
        {
            ViewModelLocationProvider.Register<MainWindow>(() => resolver(typeof(MainWindowViewModel)));
            ViewModelLocationProvider.Register<Inputs>(() => resolver(typeof(InputsViewModel)));
            ViewModelLocationProvider.Register<Outputs>(() => resolver(typeof(OutputsViewModel)));
            ViewModelLocationProvider.Register<Actions>(() => resolver(typeof(ActionsViewModel)));
            ViewModelLocationProvider.Register<Waiter>(() => resolver(typeof(WaiterViewModel)));
            ViewModelLocationProvider.Register<OptionDialog>(() => resolver(typeof(OptionDialogViewModel)));
        }

        public static Window CreateMainWindow(string title)
        {
            return new MainWindow
            {
                Title = title
            };
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            IRegionManager regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(NavigationParams.InputRegion, typeof(Inputs));
            regionManager.RegisterViewWithRegion(NavigationParams.OutputRegion, typeof(Outputs));
            regionManager.RegisterViewWithRegion(NavigationParams.ActionRegion, typeof(Actions));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<OptionDialog, OptionDialogViewModel>(DialogParams.Options.Name);
        }
    }
}
