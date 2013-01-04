using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System.Threading.Tasks;
using RBAList.Core;
using Microsoft.WindowsAzure.MobileServices;
using RBAList.Core.Models;

namespace RBAListDemo.Android
{
    [Activity(Label = "Login", MainLauncher = true, Icon = "@drawable/icon")]
    public class SplashScreen : Activity
    {
        #region Variables

        private Button _btnFacebook;
        private Button _btnMicrosoft;
        private Button _btnTwitter;

        #endregion


        #region Methods

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.splashscreen);
            SettingsPresenter.Current.Load();

            _btnFacebook = FindViewById<Button>(Resource.Id.btnFacebook);
            _btnFacebook.Click += _btnFacebook_Click;
            _btnTwitter = FindViewById<Button>(Resource.Id.btnTwitter);
            _btnTwitter.Click += _btnTwitter_Click;
            _btnMicrosoft = FindViewById<Button>(Resource.Id.btnMicrosoft);
            _btnMicrosoft.Click += _btnMicrosoft_Click;

            CheckLogin();
        }

        private void CheckLogin()
        {
            var settings = SettingsPresenter.Current;
            if (settings.AuthenticationProvider < 0)
            {
                return;
            }

            var provider = (MobileServiceAuthenticationProvider) Enum.Parse(typeof (MobileServiceAuthenticationProvider), settings.AuthenticationProvider.ToString());

            Login(new LoginPlatform {
                Provider = provider,
                Name = string.Empty
            });
        }

        private void HandleLoginResult(Task<MobileServiceUser> t, LoginPlatform platform = null)
        {
            if (t.Status == TaskStatus.RanToCompletion && t.Result != null && !string.IsNullOrEmpty(t.Result.UserId))
            {
                //Save our app settings for next launch
                var settings = SettingsPresenter.Current;

                settings.UserId = t.Result.UserId;

                if (platform != null)
                {
                    settings.AuthenticationProvider = (int) platform.Provider;
                }

                settings.Save();
                RunOnUiThread(() =>
                {
                    Toast.MakeText(this, "Login Successful", ToastLength.Long).Show();
                    //Navigate to the Lists view
                    //RequestNavigate<WishListsViewModel>();
                    var i = new Intent(this, typeof (ItemListActivity));
                    i.AddFlags(ActivityFlags.ClearTop);
                    StartActivity(i);
                });
            }
            else
            {
                RunOnUiThread(() => { Toast.MakeText(this, "Login Failed", ToastLength.Long).Show(); });
                //Show Error
                //ReportError("Login Failed!");
            }
        }

        private void Login(LoginPlatform platform)
        {
            RBAListPresenter.Current.Logout();
            RBAListPresenter.Current.MobileService.LoginAsync(this, platform.Provider).ContinueWith((t) => HandleLoginResult(t, platform));
        }

        private void _btnFacebook_Click(object sender, EventArgs e)
        {
            Login(new LoginPlatform {
                Name = "Facebook",
                Provider = MobileServiceAuthenticationProvider.Facebook
            });
        }

        private void _btnMicrosoft_Click(object sender, EventArgs e)
        {
            Login(new LoginPlatform {
                Name = "Microsoft",
                Provider = MobileServiceAuthenticationProvider.MicrosoftAccount
            });
        }

        private void _btnTwitter_Click(object sender, EventArgs e)
        {
            Login(new LoginPlatform {
                Name = "Twitter",
                Provider = MobileServiceAuthenticationProvider.Twitter
            });
        }

        #endregion
    }
}