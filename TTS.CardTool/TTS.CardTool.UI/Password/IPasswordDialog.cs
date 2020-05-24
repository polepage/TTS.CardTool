using System;
using System.Security;

namespace TTS.CardTool.UI.Password
{
    interface IPasswordDialog
    {
        void SetPasswordAccessor(Func<SecureString> getPassword);
    }
}
