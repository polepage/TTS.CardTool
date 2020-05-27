using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Windows;
using System.Windows.Input;
using TTS.CardTool.UI.Events;
using WPF.Utils.Dialogs;

namespace TTS.CardTool.UI.ViewModel
{
    class OutputsViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IDialogService _dialogService;

        public OutputsViewModel(IEventAggregator eventAggregator, IDialogService dialogService)
        {
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;

            _eventAggregator.GetEvent<PostOutput>().Subscribe(OutputReceived);
        }

        private DelegateCommand _saveCommand;
        public ICommand SaveCommand => _saveCommand ??= new DelegateCommand(Save);

        private DelegateCommand _copyCommand;
        public ICommand CopyCommand => _copyCommand ??= new DelegateCommand(Copy);

        private string _ttsData;
        public string TTSData
        {
            get => _ttsData;
            set => SetProperty(ref _ttsData, value);
        }

        private void Save()
        {
            IDialogParameters dialogParameters = new DialogParameters
            {
                { DialogParams.Title, "Select Output Target" },
                { DialogParams.File.Filter, "Text Files (*.txt)|*.txt" }
            };

            _dialogService.ShowSaveDialog(dialogParameters, dialogResult =>
            {
                string filePath = dialogResult.Parameters.GetValue<string>(DialogParams.File.Target);
                using var stream = System.IO.File.CreateText(filePath);
                stream.Write(TTSData);
            });
        }

        private void Copy()
        {
            Clipboard.SetText(TTSData);
        }

        private void OutputReceived(string output)
        {
            TTSData = output;
        }
    }
}
