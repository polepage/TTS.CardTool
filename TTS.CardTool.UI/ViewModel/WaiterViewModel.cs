using Prism.Events;
using Prism.Mvvm;
using TTS.CardTool.UI.Events;

namespace TTS.CardTool.UI.ViewModel
{
    class WaiterViewModel: BindableBase
    {
        public WaiterViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<ShowWaiter>().Subscribe(() => IsVisible = true);
            eventAggregator.GetEvent<HideWaiter>().Subscribe(() => IsVisible = false);
        }

        private bool _isVisible;
        public bool IsVisible
        {
            get => _isVisible;
            private set => SetProperty(ref _isVisible, value);
        }
    }
}
