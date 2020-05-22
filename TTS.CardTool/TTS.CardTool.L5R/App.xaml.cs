using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using System.Windows;
using TTS.CardTool.UI.View;

namespace TTS.CardTool.L5R
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
                Title = "L5R Decklist to TTS"
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
                .AddModule<Output.L5R.Module>("TTS.CardTool.Output.Module")
                .AddModule<Downloader.Module>("TTS.CardTool.Downloader.Module")
                .AddModule<Data.L5R.Module>("TTS.CardTool.Data.Module")
                .AddModule<Parser.L5R.Module>("TTS.CardTool.Parser.Module")
                .AddModule<Process.Module>("TTS.CardTool.Process.Module")
                .AddModule<UI.Module>("TTS.CardTool.UI.Module");
        }
    }
}
