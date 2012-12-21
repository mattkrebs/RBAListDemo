using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
//using Microsoft.WindowsAzure.MobileServices;
using RBAListDemo.IPhone;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.WindowsAzure.MobileServices;
using RBAList.Core.Models;

namespace Sample
{
	public partial class SampleViewController : UIViewController
	{
		public class  ItemsTable  : UITableView 
		{

		}

		private static readonly MobileServiceClient MobileService = new MobileServiceClient (/* MOBILE SERVICE URL */);
		private readonly IMobileServiceTable<Item> todoTable = MobileService.GetTable<ItemsTable>();


		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}
		
		public SampleViewController (IntPtr handle) : base (handle)
		{
		}
		
		private TodoController itemController;
		
		#region View lifecycle
		
		private UIAlertView prompt;
		partial void Add (NSObject sender)
		{
			if (this.prompt != null)
				this.prompt.Dispose ();
			
			this.prompt = new UIAlertView ("Add Item", String.Empty, null, "Cancel", "Add");
			this.prompt.AlertViewStyle = UIAlertViewStyle.PlainTextInput;
			this.prompt.Clicked += (s, e) => 
			{
				if (e.ButtonIndex == 1)
				{
					UITextField text = prompt.GetTextField (0);
					todoTable.InsertAsync (new Item { Text = text.Text })
						.ContinueWith (t => this.itemController.RefreshAsync(), scheduler);
				}
			};
			
			this.prompt.Show();
		}
		
		partial void Refresh (NSObject sender)
		{
			this.itemController.RefreshAsync();
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			this.itemController = new TodoController (ItemsTable, this.todoTable);
			this.itemController.IsUpdatingChanged += (sender, e) =>
			{
				RefreshButton.Enabled =
					AddButton.Enabled =
						!this.itemController.IsUpdating;
			};
			
			ItemsTable.Source = this.itemController;
		}
		
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			
			// Clear any references to subviews of the main view in order to
			// allow the Garbage Collector to collect them sooner.
			//
			// e.g. myOutlet.Dispose (); myOutlet = null;
			
			//ReleaseDesignerOutlets ();
		}
		
#endregion
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			if (UserInterfaceIdiomIsPhone) {
				return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
			} else {
				return true;
			}
		}
		
		private TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
	}
	
	public class TodoController
		: UITableViewSource
	{
		public TodoController (UITableView view, IMobileServiceTable<Item> table)
		{
			this.tableView = view;
			this.table = table;
			RefreshAsync();
		}
		
		public event EventHandler IsUpdatingChanged;
		
		public bool IsUpdating
		{
			get { return this.isUpdating; }
			private set
			{
				this.isUpdating = value;
				
				var changed = IsUpdatingChanged;
				if (changed != null)
					changed (this, EventArgs.Empty);
			}
		}
		
		public void RefreshAsync()
		{
			IsUpdating = true;
//			this.table.Where (ti => !ti.Complete).ToListAsync()
//				.ContinueWith (t =>
//				               {
//					this.items = t.Result;
//					this.tableView.ReloadData();
//					IsUpdating = false;
//				}, scheduler);
		}
		
		public void Insert (Item item)
		{
			IsUpdating = true;
			
			NSIndexPath path = NSIndexPath.Create (this.items.Count);
			this.items.Add (item);
			
			this.tableView.InsertRows (new[] { path }, UITableViewRowAnimation.Automatic);
			
			this.table.InsertAsync (item).ContinueWith (t =>
			                                            {
				if (t.IsFaulted)
				{
					this.items.Remove (item);
					this.tableView.DeleteRows (new[] { path }, UITableViewRowAnimation.Automatic);
				}
				
				IsUpdating = false;
			}, scheduler);
		}
		
		public override int RowsInSection (UITableView tableview, int section)
		{
			if (this.items == null)
				return 0;
			
			return this.items.Count;
		}
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell ("todo");
			if (cell == null)
				cell = new UITableViewCell (UITableViewCellStyle.Default, "todo");
			
			var item = this.items[indexPath.Row];
//			cell.Accessory = item.Complete ? UITableViewCellAccessory.Checkmark : UITableViewCellAccessory.None;
			cell.TextLabel.Text = item.Name;
			
			return cell;
		}
		
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			IsUpdating = true;
			
			Item item = this.items[indexPath.Row];
//			item.Complete = true;
			
			this.tableView.ReloadRows (new[] { indexPath }, UITableViewRowAnimation.Automatic);
			this.table.UpdateAsync (item)
				.ContinueWith (t => 
				               {
					this.items.RemoveAt (indexPath.Row);
					this.tableView.ReloadData();
					
					IsUpdating = false;
				}, scheduler);
		}
		
		private bool isUpdating;
		private UITableView tableView;
		private IMobileServiceTable<Item> table;
		private List<Item> items;
		private readonly TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
	}
}

