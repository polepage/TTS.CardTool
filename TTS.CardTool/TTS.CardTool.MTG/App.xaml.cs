using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using System.Windows;
using TTS.CardTool.UI.View;

namespace TTS.CardTool.MTG
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return new MainWindow
            {
                Title = "Magic Decklist to TTS"
            };
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry) { }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();
            UI.Module.ConfigureViewModelLocator(t => Container.Resolve(t));
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);

            moduleCatalog
                .AddModule<UI.Module>("TTS.CardTool.UI.Module");
        }
    }
}
