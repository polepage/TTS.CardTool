﻿using Prism.Ioc;
using Prism.Modularity;

namespace TTS.CardTool.Converter
{
    [ModuleDependency("TTS.CardTool.Processor.Module")]
    public class Module : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider) { }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IDeckConverter, DeckConverter>();
        }
    }
}
