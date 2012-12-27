using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RBAList.Core.Models;
using Microsoft.WindowsAzure.MobileServices;

namespace RBAList.Core
{
    public class LoginViewModel
    {

        public LoginViewModel()
        {
            var settings = SettingsPresenter.Current;
            settings.Load();

            Platforms = new List<LoginPlatform>
				{
					new LoginPlatform {Name = "Twitter", Provider = MobileServiceAuthenticationProvider.Twitter},
					new LoginPlatform {Name = "Facebook", Provider = MobileServiceAuthenticationProvider.Facebook},
					new LoginPlatform {Name = "Google", Provider = MobileServiceAuthenticationProvider.Google},
					new LoginPlatform {Name = "Microsoft", Provider = MobileServiceAuthenticationProvider.MicrosoftAccount},
				};         
        }

        private List<LoginPlatform> _platforms;

        public List<LoginPlatform> Platforms {get;set;}

        

    }
}