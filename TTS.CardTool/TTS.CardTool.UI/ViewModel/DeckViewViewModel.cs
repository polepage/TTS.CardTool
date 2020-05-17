using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TTS.CardTool.Process;
using TTS.CardTool.UI.Events;
using WPF.Utils.Dialogs;

namespace TTS.CardTool.UI.ViewModel
{
    class DeckViewViewModel: BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IDialogService _dialogService;
        private readonly ICreationProcess _process;

        public DeckViewViewModel(IEventAggregator eventAggregator, IDialogService dialogService, ICreationProcess process)
        {
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            _process = process;
        }

        private DelegateCommand _openListCommand;
        public ICommand OpenListCommand => _openListCommand ?? (_openListCommand = new DelegateCommand(OpenList));

        private DelegateCommand _createImageCommand;
        public ICommand CreateImageCommand => _createImageCommand ?? (_createImageCommand = new DelegateCommand(CreateImage));

        private string _text;
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        private void OpenList()
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
                    Text = stream.ReadToEnd();
                }
            });
        }

        private async void CreateImage()
        {
            IProgress<(bool, string, int)> progress = new Progress<(bool indetermintate, string text, int maximum)>(s =>
            {
                if (s.indetermintate)
                {
                    _eventAggregator.GetEvent<UpdateWaiterStatus>().Publish(WaiterStatus.Indeterminate(s.text));
                }
                else
                {
                    _eventAggregator.GetEvent<UpdateWaiterStatus>().Publish(WaiterStatus.Range(s.text, s.maximum));
                }
            });

            IProgress<int> stepProgress = new Progress<int>(p =>
            {
                _eventAggregator.GetEvent<UpdateWaiterValue>().Publish(p);
            });

            _eventAggregator.GetEvent<ShowWaiter>().Publish();
            _eventAggregator.GetEvent<UpdateWaiterStatus>().Publish(WaiterStatus.Indeterminate());
            await _process.Create(Text, progress, stepProgress);
            _eventAggregator.GetEvent<HideWaiter>().Publish();
        }
    }
}
