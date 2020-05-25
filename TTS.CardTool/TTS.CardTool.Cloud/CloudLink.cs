using SteamKit2;
using System;
using System.Threading.Tasks;

namespace TTS.CardTool.Cloud
{
    class CloudLink : ICloudLogin
    {
        private readonly SteamClient _client;
        private readonly CallbackManager _callbackManager;

        private readonly SteamUser _user;

        private bool _isRunning;

        public CloudLink()
        {
            _client = new SteamClient();
            _callbackManager = new CallbackManager(_client);
            _user = _client.GetHandler<SteamUser>();

            _callbackManager.Subscribe<SteamClient.ConnectedCallback>(OnConnected);
            _callbackManager.Subscribe<SteamClient.DisconnectedCallback>(OnDisconnected);

            _callbackManager.Subscribe<SteamUser.LoggedOnCallback>(OnLoggedIn);
            _callbackManager.Subscribe<SteamUser.LoggedOffCallback>(OnLoggedOff);
        }

        public event Action LoggedIn;
        public event Action LoggedOut;

        public void LogIn(string username, string password)
        {
            _callbackManager.Subscribe<SteamClient.ConnectedCallback>(cb =>
            {
                _user.LogOn(new SteamUser.LogOnDetails
                {
                    Username = username,
                    Password = password
                });
            });

            _isRunning = true;
            _client.Connect();
            Task.Run(RunCallbacks);
        }

        public void LogOut()
        {
            _user.LogOff();
        }

        private void RunCallbacks()
        {
            while (_isRunning)
            {
                _callbackManager.RunWaitCallbacks(TimeSpan.FromSeconds(1));
            }
        }

        private void OnDisconnected(SteamClient.DisconnectedCallback callback)
        {
            // That will happen after logout anyway.
            LoggedOut?.Invoke();
        }

        private void OnLoggedIn(SteamUser.LoggedOnCallback callback)
        {
            if (callback.Result == EResult.OK)
            {
                
            }

        }

        private void OnLoggedOff(SteamUser.LoggedOffCallback _)
        {
            // Dont really care if it worked or not.
            // It will eventually disconnect anyway.
        }
    }
}
