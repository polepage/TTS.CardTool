using System;

namespace TTS.CardTool.Cloud
{
    public interface ICloudLogin
    {
        event Action<string> LoggedIn;
        event Action LoggedOut;

        void LogIn(string username, string password);
        void LogOut();
    }
}
