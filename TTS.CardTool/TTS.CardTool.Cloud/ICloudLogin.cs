using System;

namespace TTS.CardTool.Cloud
{
    public interface ICloudLogin
    {
        event Action LoggedIn;
        event Action LoggedOut;

        void LogIn(string username, string password);
        void LogOut();
    }
}
