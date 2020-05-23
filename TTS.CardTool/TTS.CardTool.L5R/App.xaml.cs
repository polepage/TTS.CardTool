using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using System.Windows;
using TTS.CardTool.UI.View;
using WPF.Utils.Modularity;

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
                .AddModule<UI.Module>("TTS.CardTool.UI.Module", dependsOn: ModuleDependency.GetDependencies<UI.Module>())
                .AddModule<Process.Module>("TTS.CardTool.Process.Module", dependsOn: ModuleDependency.GetDependencies<Process.Module>())
                .AddModule<Parser.L5R.Module>("TTS.CardTool.Parser.Module", dependsOn: ModuleDependency.GetDependencies<Parser.L5R.Module>())
                .AddModule<Data.L5R.Module>("TTS.CardTool.Data.Module", dependsOn: ModuleDependency.GetDependencies<Data.L5R.Module>())
                .AddModule<Downloader.Module>("TTS.CardTool.Downloader.Module", dependsOn: ModuleDependency.GetDependencies<Downloader.Module>())
                .AddModule<Output.L5R.Module>("TTS.CardTool.Output.Module", dependsOn: ModuleDependency.GetDependencies<Output.L5R.Module>());
        }
    }
}
