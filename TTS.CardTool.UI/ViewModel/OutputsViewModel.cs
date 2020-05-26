using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Windows;
using System.Windows.Input;
using WPF.Utils.Dialogs;

namespace TTS.CardTool.UI.ViewModel
{
    class OutputsViewModel : BindableBase
    {
        private readonly IDialogService _dialogService;

        public OutputsViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
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
    }
}
