using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Windows.Input;
using TTS.CardTool.Cloud;
using TTS.CardTool.UI.Dialogs;
using WPFDialogParams = WPF.Utils.Dialogs.DialogParams;

namespace TTS.CardTool.UI.ViewModel
{
    class CloudConnectionViewModel: BindableBase
    {
        private readonly IDialogService _dialogService;
        private readonly ICloudLogin _cloudLogin;

        public CloudConnectionViewModel(IDialogService dialogService, ICloudLogin cloudLogin)
        {
            _dialogService = dialogService;
            _cloudLogin = cloudLogin;

            _cloudLogin.LoggedIn += CloudLoggedIn;
            _cloudLogin.LoggedOut += CloudLoggedOut;
        }

        private DelegateCommand _logInCommand;
        public ICommand LogInCommand => _logInCommand ??= new DelegateCommand(LogIn);

        private DelegateCommand _logOutCommand;
        public ICommand LogOutCommand => _logOutCommand ??= new DelegateCommand(LogOut);

        private bool _isLoggedIn;
        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            private set => SetProperty(ref _isLoggedIn, value);
        }

        private string _status = "Offline";
        public string Status
        {
            get => _status;
            private set => SetProperty(ref _status, value);
        }

        private void LogIn()
        {
            IDialogParameters dialogParams = new DialogParameters
            {
                { WPFDialogParams.Title, "Log In to your Steam account." }
            };

            _dialogService.ShowDialog(DialogParams.LogIn.Name, dialogParams, dialogResult =>
            {
                if (dialogResult.Result == ButtonResult.OK)
                {
                    _cloudLogin.LogIn(dialogResult.Parameters.GetValue<string>(DialogParams.LogIn.Username),
                                      dialogResult.Parameters.GetValue<string>(DialogParams.LogIn.Password));
                }
            });
        }

        private void LogOut()
        {
            _cloudLogin.LogOut();   
        }

        private void CloudLoggedIn(string username)
        {
            IsLoggedIn = true;
            Status = $"Online as {username}";
        }

        private void CloudLoggedOut()
        {
            IsLoggedIn = false;
            Status = "Offline";
        }
    }
}
