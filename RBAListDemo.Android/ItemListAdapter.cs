using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using RBAList.Core;

namespace RBAListDemo.Android
{
    public class ItemAdapter : BaseAdapter<ItemViewModel>
    {
        #region Events & Delegates

        public event EventHandler IsUpdatingChanged;

        #endregion


        #region Variables

        private readonly LayoutInflater inflater;
        private bool isUpdating;
        private List<ItemViewModel> items = new List<ItemViewModel>();
        private readonly TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();

        #endregion


        #region Properties

        public bool IsUpdating
        {
            get { return isUpdating; }
            private set
            {
                isUpdating = value;

                var changed = IsUpdatingChanged;
                if (changed != null)
                {
                    changed(this, EventArgs.Empty);
                }
            }
        }

        public override bool HasStableIds
        {
            get { return true; }
        }

        public override int Count
        {
            get { return items.Count; }
        }

        public override ItemViewModel this[int position]
        {
            get { return items[position]; }
        }

        #endregion


        #region Constructors

        public ItemAdapter(Context context)
        {
            inflater = (LayoutInflater) context.GetSystemService(Context.LayoutInflaterService);

            RBAListPresenter.Current.GetItemsAsync(RefreshAsync);
        }

        #endregion


        #region Methods

        public override long GetItemId(int position)
        {
            return items[position].Item.Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ItemViewModel product = items[position];

            View view = inflater.Inflate(Resource.Layout.ItemListItemView, null);


            var image = view.FindViewById<ImageView>(Resource.Id.imgItem);
            var name = view.FindViewById<TextView>(Resource.Id.txtName);
            var description = view.FindViewById<TextView>(Resource.Id.txtDescription);
            var amount = view.FindViewById<TextView>(Resource.Id.txtPrice);

            name.Text = product.Item.Name;
            description.Text = product.Item.Description;
            amount.Text = string.Format("${0:0.00}", product.Item.AskingPrice);

            if (product.ItemImage != null)
            {
                image.SetImageDrawable((BitmapDrawable) MediaFileHelper.Convert(product.ItemImage.ImageBase64, typeof (BitmapDrawable), null, CultureInfo.CurrentCulture));
            }


            return view;
        }

        public void RefreshAsync(List<ItemViewModel> returnedItems)
        {
            IsUpdating = true;

            items = returnedItems;
            NotifyDataSetChanged();
        }

        #endregion
    }
}