using System.Windows.Controls;
using TTS.CardTool.UI.Password;

namespace TTS.CardTool.UI.View
{
    /// <summary>
    /// Interaction logic for ConnectionDialog.xaml
    /// </summary>
    public partial class ConnectionDialog : StackPanel
    {
        public ConnectionDialog()
        {
            InitializeComponent();
            if (DataContext is IPasswordDialog passwordDialog)
            {
                passwordDialog.SetPasswordAccessor(GetPassword);
            }

            Unloaded += DialogUnloaded;
        }

        private string GetPassword()
        {
            string password = passwordBox.Password;
            passwordBox.Clear();
            return password;
        }

        private void DialogUnloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            passwordBox.Clear();
            passwordBox.SecurePassword.Dispose();
        }
    }
}
