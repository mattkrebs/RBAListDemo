
using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using Microsoft.WindowsAzure.MobileServices;
using RBAList.Core.Models;
using System.Threading.Tasks;
using RBAList.Core;

namespace RBAListDemo.IPhone
{
		public class ActiveItemTableController: UITableViewSource
		{
			public ActiveItemTableController (UITableView view, List<Item> table)
			{
				this.tableView = view;
			   // this.items = table;
			RBAListPresenter.Current.GetItemsAsync(RefreshAsync);
				
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
			
		public void RefreshAsync(List<ItemViewModel> returnedItems)
			{
				IsUpdating = true;
				this.items = returnedItems;
				this.tableView.ReloadData();
			}
			
			public void Insert (Item item)
			{
				IsUpdating = true;
				
				NSIndexPath path = NSIndexPath.Create (this.items.Count);
				//this.items.Add (item);
				
				this.tableView.InsertRows (new[] { path }, UITableViewRowAnimation.Automatic);
				
				
				
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
				cell.TextLabel.Text = item.Item.Name;
				
				return cell;
			}
			
			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				IsUpdating = true;
				
				ItemViewModel item = this.items[indexPath.Row];
				//			item.Complete = true;
				
				
			}
			
			private bool isUpdating;
			private UITableView tableView;
			private List<ItemViewModel> items;
			private readonly TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
		}
	}

