using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows.Input;
using TTS.CardTool.Converter;
using TTS.CardTool.UI.Events;
using WPF.Utils.Dialogs;
using UIDialogParams = TTS.CardTool.UI.Dialogs.DialogParams;

namespace TTS.CardTool.UI.ViewModel
{
    class ActionsViewModel: BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IDeckConverter _deckConverter;
        private readonly IDialogService _dialogService;

        public ActionsViewModel(IEventAggregator eventAggregator, IDeckConverter deckConverter, IDialogService dialogService)
        {
            _eventAggregator = eventAggregator;
            _deckConverter = deckConverter;
            _dialogService = dialogService;

            _eventAggregator.GetEvent<PostInput>().Subscribe(InputReceived);
        }

        private DelegateCommand _convertCommand;
        public ICommand ConvertCommand => _convertCommand ??= new DelegateCommand(Convert);

        private DelegateCommand _settingsCommand;
        public ICommand SettingsCommand => _settingsCommand ??= new DelegateCommand(Settings);

        public void Convert()
        {
            _eventAggregator.GetEvent<RequestInput>().Publish();
        }

        private void Settings()
        {
            IDialogParameters dialogParameters = new DialogParameters
            {
                { DialogParams.Title, "Options" }
            };

            _dialogService.ShowDialog(UIDialogParams.Options.Name, dialogParameters, null);
        }

        private async void InputReceived(string input)
        {
            void showErrorMessage(string title, string content)
            {
                IDialogParameters dialogParameters = new DialogParameters
                {
                    { DialogParams.Title, title },
                    { DialogParams.Alert.Content, content },
                    { DialogParams.Alert.Image, DialogParams.Alert.AlertImage.Error }
                };

                _dialogService.ShowMessageBox(dialogParameters, null);
            };

            try
            {
                _eventAggregator.GetEvent<ShowWaiter>().Publish();

                string result = await _deckConverter.Convert(input);
                _eventAggregator.GetEvent<PostOutput>().Publish(result);
            }
            catch (HttpRequestException e)
            {
                showErrorMessage("Server Error", e.Message);
            }
            catch (KeyNotFoundException e)
            {
                showErrorMessage("Card Error", e.Message);
            }
            finally
            {
                _eventAggregator.GetEvent<HideWaiter>().Publish();
            }
        }
    }
}
