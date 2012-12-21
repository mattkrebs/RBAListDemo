
using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using Microsoft.WindowsAzure.MobileServices;
using RBAList.Core.Models;
using System.Threading.Tasks;

namespace RBAListDemo.IPhone
{
		public class ActiveItemTableController: UITableViewSource
		{
			public ActiveItemTableController (UITableView view, IMobileServiceTable<Item> table)
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

