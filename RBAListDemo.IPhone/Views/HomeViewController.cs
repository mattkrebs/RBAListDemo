
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Microsoft.WindowsAzure.MobileServices;
using RBAList.Core.Models;


namespace RBAListDemo.IPhone
{
	public partial class HomeViewController : UIViewController
	{
		public HomeViewController () : base ("HomeViewController", null)
		{
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		//Sub Views 
		private UIScrollView _scrollViewMain = new UIScrollView();
		private UIView _MainView = new UIView();
		private UITableView _ItemTableView = new UITableView(new RectangleF(0,0,320,416), UITableViewStyle.Grouped);
		public ActiveItemTableController itemController;

	//	private static readonly MobileServiceClient MobileService = new MobileServiceClient(new Uri(""));
		private readonly IMobileServiceTable<Item> todoTable = RBAList.Core.RBAListRepository.MobileService.GetTable<Item>();

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();


			this.itemController = new ActiveItemTableController (_ItemTableView, this.todoTable);
//			this.itemController.IsUpdatingChanged += (sender, e) =>
//			{
//				RefreshButton.Enabled =
//					AddButton.Enabled =
//						!this.itemController.IsUpdating;
//			};

			_ItemTableView.Source = this.itemController;

			_MainView.AddSubview(_ItemTableView);
			this.View.AddSubview(_MainView);
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
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}
	}
}

