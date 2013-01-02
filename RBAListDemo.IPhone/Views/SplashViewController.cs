
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using RBAList.Core.Models;
using RBAList.Core;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace RBAListDemo.IPhone
{
	public partial class SplashViewController : UIViewController
	{
		public SplashViewController () : base ("SplashViewController", null)
		{
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			var rbalogo = new UIImageView();
			rbalogo.Image = UIImage.FromBundle("/Images/sprite");
			rbalogo.Frame = new RectangleF(10, 8, 170, 43);
			View.Add(rbalogo);

			var btnFacebook = new UIButton(new RectangleF(60,60,200,20));
			btnFacebook.SetTitle("Facebook Login", UIControlState.Normal);
			btnFacebook.TouchUpInside += FacebookClickEvent;

			View.Add(btnFacebook);
			// Perform any additional setup after loading the view, typically from a nib.
		}
		
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			
			// Clear any references to subviews of the main view in order to
			// allow the Garbage Collector to collect them sooner.
			//
			// e.g. myOutlet.Dispose (); myOutlet = null;
			
			ReleaseDesignerOutlets ();
		}
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			CheckLogin();
		}
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}

		void FacebookClickEvent (object sender, EventArgs e)
		{
			Login(new LoginPlatform() { Name = "Facebook", Provider = MobileServiceAuthenticationProvider.Facebook });
		}
		//Could be shared by 
		public void Login(LoginPlatform platform)
		{
			RBAListPresenter.Current.Logout();
			RBAListPresenter.Current.MobileService.LoginAsync(this,platform.Provider).ContinueWith((t) => 
			{
				HandleLoginResult(t, platform);
			}, TaskScheduler.FromCurrentSynchronizationContext());
			
		}
		//Most of this could be shared
		public void HandleLoginResult (System.Threading.Tasks.Task<Microsoft.WindowsAzure.MobileServices.MobileServiceUser> t, LoginPlatform platform)
		{
			if (t.Status == TaskStatus.RanToCompletion && t.Result != null && !string.IsNullOrEmpty(t.Result.UserId))
			{
				//Save our app settings for next launch
				var settings = SettingsPresenter.Current;
				
				settings.UserId = t.Result.UserId;
				
				if (platform != null)
					settings.AuthenticationProvider = (int)platform.Provider;
				
				settings.Save();

					//Navigate to the Lists view
					//RequestNavigate<WishListsViewModel>();
				var homecontroller = new HomeViewController();
				this.NavigationController.PushViewController(homecontroller, true);
				
			}
			else
			{
			
				//Show Error
				//ReportError("Login Failed!");
			}
		}

		//Could be shared
		public void CheckLogin()
		{
			var settings = SettingsPresenter.Current;
			if (settings.AuthenticationProvider < 0) return;
			
			var provider = (MobileServiceAuthenticationProvider)Enum.Parse(typeof(MobileServiceAuthenticationProvider), settings.AuthenticationProvider.ToString());
			
			Login(new LoginPlatform { Provider = provider, Name = string.Empty });
		}
	}
}

