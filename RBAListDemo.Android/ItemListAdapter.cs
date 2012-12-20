using Android.Content;
using Android.Views;
using Android.Widget;
using Microsoft.WindowsAzure.MobileServices;
using RBAList.Core;
using RBAList.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBAListDemo.Android
{
    public class ItemListAdapter : ArrayAdapter<Item>
    {
        private List<Item> items = new List<Item>();

        private readonly LayoutInflater inflater;
      //  private readonly IMobileServiceTable<Item> table;
     //   private readonly TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
        private bool isUpdating;

        public ItemListAdapter(Context context, int textViewResourceId, List<Item> items) : base(context, textViewResourceId, items)
        {
            this.items = items;
           
        }
        public override long GetItemId(int position)
        {
            return position;
        }
 
        public override View GetView(int position, View view, ViewGroup parent)
        {
            Item item = this.items[position];
            if (view == null)
            {
                var layoutInflater = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);
                view = layoutInflater.Inflate(Resource.Layout.ItemListItemView, null);
            }

            ImageView image = view.FindViewById<ImageView>(Resource.Id.imgItem);
            TextView name = view.FindViewById<TextView>(Resource.Id.txtName);
            TextView description = view.FindViewById<TextView>(Resource.Id.txtDescription);
            TextView amount = view.FindViewById<TextView>(Resource.Id.txtPrice);

            name.Text = item.Name;
            description.Text = item.Description;
            amount.Text = string.Format("${0:0.00}", item.AskingPrice);

            return view;
        }
       
        public override int Count
        {
            get 
            {
                int count = this.items.Count();
                return count;
            }
        }
    }
    public class ItemAdapter
        : BaseAdapter<Item>
    {
        public ItemAdapter(IMobileServiceTable<Item> table, Context context)
        {
            this.inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
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
                    changed(this, EventArgs.Empty);
            }
        }

        public override bool HasStableIds
        {
            get { return true; }
        }

        public override int Count
        {
            get { return this.items.Count; }
        }

        public override Item this[int position]
        {
            get { return this.items[position]; }
        }

        public override long GetItemId(int position)
        {
            return this.items[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Item item = this.items[position];

            View view = this.inflater.Inflate(Resource.Layout.ItemListItemView, null);


            ImageView image = view.FindViewById<ImageView>(Resource.Id.imgItem);
            TextView name = view.FindViewById<TextView>(Resource.Id.txtName);
            TextView description = view.FindViewById<TextView>(Resource.Id.txtDescription);
            TextView amount = view.FindViewById<TextView>(Resource.Id.txtPrice);

            name.Text = item.Name;
            description.Text = item.Description;
            amount.Text = string.Format("${0:0.00}", item.AskingPrice);

            return view;
        }

        public void RefreshAsync()
        {
            IsUpdating = true;
            
            this.table.Where(ti => !ti.Sold).ToListAsync()
                .ContinueWith(t =>
                {
                    this.items = t.Result;
                    NotifyDataSetChanged();
                    IsUpdating = false;
                }, scheduler);
        }
        
        //public void Insert(Item item)
        //{
        //    IsUpdating = true;
        //    this.items.Add(item);
        //    NotifyDataSetChanged();

        //    this.table.InsertAsync(item).ContinueWith(t =>
        //    {
        //        if (t.IsFaulted)
        //        {
        //            this.items.Remove(item);
        //            NotifyDataSetChanged();
        //        }

        //        IsUpdating = false;
        //    }, scheduler);
        //}

        private List<Item> items = new List<Item>();

        private readonly LayoutInflater inflater;
        private readonly IMobileServiceTable<Item> table;
        private readonly TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
        private bool isUpdating;
    }
}
