using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using Microsoft.WindowsAzure.MobileServices;
using RBAList.Core;
using RBAList.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBAListDemo.Android
{
    
    public class ItemAdapter : BaseAdapter<ItemViewModel>
    {
        public ItemAdapter(Context context)
        {
            this.inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
           
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

        public override ItemViewModel this[int position]
        {
            get { return this.items[position]; }
        }

        public override long GetItemId(int position)
        {
            return this.items[position].Item.Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ItemViewModel product = this.items[position];

            View view = this.inflater.Inflate(Resource.Layout.ItemListItemView, null);


            ImageView image = view.FindViewById<ImageView>(Resource.Id.imgItem);
            TextView name = view.FindViewById<TextView>(Resource.Id.txtName);
            TextView description = view.FindViewById<TextView>(Resource.Id.txtDescription);
            TextView amount = view.FindViewById<TextView>(Resource.Id.txtPrice);

            name.Text = product.Item.Name;
            description.Text = product.Item.Description;
            amount.Text = string.Format("${0:0.00}", product.Item.AskingPrice);

            if (product.ItemImage != null)
                image.SetImageDrawable((BitmapDrawable)MediaFileHelper.Convert(product.ItemImage.ImageBase64, typeof(BitmapDrawable), null, CultureInfo.CurrentCulture));


            return view;
        }

        public void RefreshAsync(List<ItemViewModel> returnedItems)
        {
            IsUpdating = true;

            this.items = returnedItems;
            NotifyDataSetChanged();
          
        }

       

        private List<ItemViewModel> items = new List<ItemViewModel>();

        private readonly LayoutInflater inflater;
        private readonly TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
        private bool isUpdating;
    }
}
