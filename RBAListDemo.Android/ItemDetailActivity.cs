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
using RBAList.Core;
using Android.Graphics.Drawables;
using System.Globalization;

namespace RBAListDemo.Android
{
    [Activity(Label = "My Activity")]
    public class ItemDetailActivity : Activity
    {
        private TextView _txtRetail;
        private TextView _txtPrice;
        private TextView _txtDescription;
        private TextView _txtName;
        private ImageView _imgItem;

        

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.ItemDetails);

            _txtName = FindViewById<TextView>(Resource.Id.txtName);
            _txtDescription = FindViewById<TextView>(Resource.Id.txtDescription);
            _txtPrice = FindViewById<TextView>(Resource.Id.txtAsking);
            _txtRetail = FindViewById<TextView>(Resource.Id.txtRetail);
            _imgItem = FindViewById<ImageView>(Resource.Id.imgItem);


        }


        protected override void OnResume()
        {
            base.OnResume();

            ItemViewModel product = RBAListPresenter.Current.CurrentViewModel;
            _txtName.Text = product.Item.Name;
            _txtDescription.Text = product.Item.Description;
            _txtPrice.Text = string.Format("${0:0.00}", product.Item.AskingPrice);
            _txtRetail.Text = string.Format("${0:0.00}", product.Item.RetailPrice);

            if (product.ItemImage != null)
                _imgItem.SetImageDrawable((BitmapDrawable)MediaFileHelper.Convert(product.ItemImage.ImageBase64, typeof(BitmapDrawable), null, CultureInfo.CurrentCulture));



        }
    }
}