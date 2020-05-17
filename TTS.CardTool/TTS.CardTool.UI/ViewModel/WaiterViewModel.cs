using Prism.Events;
using Prism.Mvvm;
using TTS.CardTool.UI.Events;

namespace TTS.CardTool.UI.ViewModel
{
    class WaiterViewModel: BindableBase
    {
        public WaiterViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<ShowWaiter>().Subscribe(Show);
            eventAggregator.GetEvent<HideWaiter>().Subscribe(Hide);
            eventAggregator.GetEvent<UpdateWaiterStatus>().Subscribe(UpateStatus);
            eventAggregator.GetEvent<UpdateWaiterValue>().Subscribe(UpdateValue);
        }

        private bool _isVisible;
        public bool IsVisible
        {
            get => _isVisible;
            private set => SetProperty(ref _isVisible, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            private set => SetProperty(ref _name, value);
        }

        private bool _isIndeterminate;
        public bool IsIndeterminate
        {
            get => _isIndeterminate;
            private set => SetProperty(ref _isIndeterminate, value);
        }

        private int _value;
        public int Value
        {
            get => _value;
            private set => SetProperty(ref _value, value);
        }

        private int _maximum;
        public int Maximum
        {
            get => _maximum;
            private set => SetProperty(ref _maximum, value);
        }

        private void Show()
        {
            IsVisible = true;
        }

        private void Hide()
        {
            IsVisible = false;
        }

        private void UpateStatus(WaiterStatus status)
        {
            IsIndeterminate = status.IsIndeterminate;
            Name = status.Text;

            if (!IsIndeterminate)
            {
                Maximum = status.Maximum;
                Value = 0;
            }
        }

        private void UpdateValue(int status)
        {
            if (status < 0)
            {
                IsIndeterminate = true;
            }
            else
            {
                IsIndeterminate = false;
                Value = status;
            }
        }
    }
}
