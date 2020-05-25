using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Windows.Input;
using TTS.CardTool.UI.Password;
using WPF.Utils.Dialogs;
using LocalDialogParams = TTS.CardTool.UI.Dialogs.DialogParams;

namespace TTS.CardTool.UI.ViewModel
{
    class ConnectionDialogViewModel: BaseDialogViewModel, IPasswordDialog
    {
        private Func<string> _getPassword;

        private DelegateCommand _acceptCommand;
        public ICommand AcceptCommand => _acceptCommand ??= new DelegateCommand(Accept);

        private string _username;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);
            Title = parameters.GetValue<string>(DialogParams.Title);
        }

        private void Accept()
        {
            IDialogResult result = new DialogResult(ButtonResult.OK);
            result.Parameters.Add(LocalDialogParams.LogIn.Username, Username);
            result.Parameters.Add(LocalDialogParams.LogIn.Password, _getPassword?.Invoke());

            RaiseRequestClose(result);
        }

        public void SetPasswordAccessor(Func<string> getPassword)
        {
            _getPassword = getPassword;
        }
    }
}
