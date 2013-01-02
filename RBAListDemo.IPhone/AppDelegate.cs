using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace RBAListDemo.IPhone
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;


		public UINavigationController RootNavigationController {get;set;}
		public UIWindow MainWindow {get;set;}
		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{

			// create a new window instance based on the screen size
			MainWindow = new UIWindow(UIScreen.MainScreen.Bounds);

			var currentHomeUiViewController = new SplashViewController();
			RootNavigationController = new UINavigationController(currentHomeUiViewController);
			MainWindow.RootViewController = RootNavigationController;
			
			var titleTextAttributes = new UITextAttributes();
			titleTextAttributes.TextColor = UIColor.FromRGB(25, 83, 135);
			titleTextAttributes.TextShadowColor = UIColor.Clear;
			titleTextAttributes.Font = UIFont.SystemFontOfSize(16);
			
//			if (IsIOS5OrGreater)
//			{
//				UINavigationBar.Appearance.SetTitleTextAttributes(titleTextAttributes);
//				UINavigationBar.Appearance.SetBackgroundImage(UIImage.FromBundle("/Images/top_bar_bg"), UIBarMetrics.Default);
//			}
//			
		
			MainWindow.MakeKeyAndVisible();

			return true;
		}
	}
}

