using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;

namespace TTS.CardTool.UI.ViewModel
{
    class CloudConnectionViewModel: BindableBase
    {
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
            // Temp
            IsLoggedIn = true;
            Status = "Online as Bob";


        }

        private void LogOut()
        {
            // Temp
            IsLoggedIn = false;
            Status = "Offline";
        }
    }
}
