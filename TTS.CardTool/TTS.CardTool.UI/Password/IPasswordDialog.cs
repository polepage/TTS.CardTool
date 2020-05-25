using System;

namespace TTS.CardTool.UI.Password
{
    interface IPasswordDialog
    {
        void SetPasswordAccessor(Func<string> getPassword);
    }
}
