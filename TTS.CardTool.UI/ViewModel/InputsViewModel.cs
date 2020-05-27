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
    class InputsViewModel: BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IDialogService _dialogService;

        public InputsViewModel(IEventAggregator eventAggregator, IDialogService dialogService)
        {
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;

            _eventAggregator.GetEvent<RequestInput>().Subscribe(InputRequested);
        }

        private DelegateCommand _loadCommand;
        public ICommand LoadCommand => _loadCommand ??= new DelegateCommand(Load);

        private DelegateCommand _pasteCommand;
        public ICommand PasteCommand => _pasteCommand ??= new DelegateCommand(Paste);

        private string _deckContent;
        public string DeckContent
        {
            get => _deckContent;
            set => SetProperty(ref _deckContent, value);
        }

        private void Load()
        {
            IDialogParameters dialogParameters = new DialogParameters
            {
                { DialogParams.Title, "Open Deck List" },
                { DialogParams.File.Filter, "Text Files (*.txt)|*.txt" }
            };

            _dialogService.ShowOpenDialog(dialogParameters, dialogResult =>
            {
                if (dialogResult.Result == ButtonResult.OK)
                {
                    string filePath = dialogResult.Parameters.GetValue<string>(DialogParams.File.Target);
                    using var stream = System.IO.File.OpenText(filePath);
                    DeckContent = stream.ReadToEnd();
                }
            });
        }

        private void Paste()
        {
            DeckContent = Clipboard.GetText();
        }

        private void InputRequested()
        {
            _eventAggregator.GetEvent<PostInput>().Publish(DeckContent);
        }
    }
}
