using System.Collections.Generic;
using Microsoft.WindowsAzure.MobileServices;
using RBAList.Core.Models;

namespace RBAList.Core
{
    public class LoginViewModel
    {
        #region Variables

        //private List<LoginPlatform> _platforms;

        #endregion


        #region Properties

        private List<LoginPlatform> Platforms { get; set; }

        #endregion


        #region Constructors

        public LoginViewModel()
        {
            var settings = SettingsPresenter.Current;
            settings.Load();

            Platforms = new List<LoginPlatform> {
                new LoginPlatform {
                    Name = "Twitter",
                    Provider = MobileServiceAuthenticationProvider.Twitter
                },
                new LoginPlatform {
                    Name = "Facebook",
                    Provider = MobileServiceAuthenticationProvider.Facebook
                },
                new LoginPlatform {
                    Name = "Google",
                    Provider = MobileServiceAuthenticationProvider.Google
                },
                new LoginPlatform {
                    Name = "Microsoft",
                    Provider = MobileServiceAuthenticationProvider.MicrosoftAccount
                },
            };
        }

        #endregion
    }
}