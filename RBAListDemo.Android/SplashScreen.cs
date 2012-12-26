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
using System.Threading.Tasks;
using System.Threading;
using RBAList.Core;
using Microsoft.WindowsAzure.MobileServices;
using RBAList.Core.Models;

namespace RBAListDemo.Android
{
    [Activity(Label = "RBA List", NoHistory=true, MainLauncher = true, Icon = "@drawable/icon")]
    public class SplashScreen : Activity
    {

        private Button _btnFacebook;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.splashscreen);


            _btnFacebook = FindViewById<Button>(Resource.Id.btnFacebook);
            _btnFacebook.Click += _btnFacebook_Click;

            //Task.Factory.StartNew(() =>
            //{
            //    Thread.Sleep(2000);
            //}).ContinueWith(t => {
                
            //    StartActivity(typeof(ItemListActivity));
            //});
            // Create your application here


        }

        void _btnFacebook_Click(object sender, EventArgs e)
        {
            Login(new LoginPlatform() { Name = "Twitter", Provider = MobileServiceAuthenticationProvider.Twitter });
        }


        public void Login(LoginPlatform platform)
        {
            RBAListPresenter.Current.MobileService.LoginAsync(this,platform.Provider).ContinueWith((t) => HandleLoginResult(t, platform));

        }
        void HandleLoginResult(Task<MobileServiceUser> t, LoginPlatform platform = null)
        {
           

            if (t.Status == TaskStatus.RanToCompletion && t.Result != null && !string.IsNullOrEmpty(t.Result.UserId))
            {
                //Save our app settings for next launch
                var settings = SettingsPresenter.Current;

                settings.UserId = t.Result.UserId;

                if (platform != null)
                    settings.AuthenticationProvider = (int)platform.Provider;

                settings.Save();
                Toast.MakeText(this, "Login Successful" , ToastLength.Long).Show();
                //Navigate to the Lists view
                //RequestNavigate<WishListsViewModel>();

            }
            else
            {
                Toast.MakeText(this, t.Exception.Message, ToastLength.Long).Show();
                //Show Error
                //ReportError("Login Failed!");
            }
        }

        public void CheckLogin()
        {
            var settings = SettingsPresenter.Current;

            if (settings.AuthenticationProvider < 0) return;

            var provider =
                (MobileServiceAuthenticationProvider)Enum.Parse(typeof(MobileServiceAuthenticationProvider),
                                                                settings.AuthenticationProvider.ToString());

            Login(new LoginPlatform { Provider = provider, Name = string.Empty });
        }
    }
}