using System.Globalization;
using Android.App;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using RBAList.Core;

namespace RBAListDemo.Android
{
    [Activity(Label = "Loading Details....", Icon = "@drawable/icon")]
    public class ItemDetailActivity : Activity
    {
        #region Variables

        private ImageView _imgItem;
        private TextView _txtDescription;
        private TextView _txtName;
        private TextView _txtPrice;
        private TextView _txtRetail;

        #endregion


        #region Methods

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            RequestWindowFeature(WindowFeatures.ActionBar);
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

            var product = RBAListPresenter.Current.CurrentViewModel;
            _txtName.Text = product.Item.Name;
            Title = product.Item.Name;
            _txtDescription.Text = product.Item.Description;
            _txtPrice.Text = string.Format("${0:0.00}", product.Item.AskingPrice);
            _txtRetail.Text = string.Format("${0:0.00}", product.Item.RetailPrice);

            if (product.ItemImage != null)
            {
                var drawable = (BitmapDrawable) MediaFileHelper.Convert(product.ItemImage.ImageBase64, typeof (BitmapDrawable), null, CultureInfo.CurrentCulture);

                _imgItem.SetImageDrawable(drawable);
            }
        }

        #endregion
    }
}