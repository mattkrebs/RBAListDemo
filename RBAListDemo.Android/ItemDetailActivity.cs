using System.Globalization;
using Android.App;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using RBAList.Core;
using System.Threading.Tasks;
using System;
using Android.Graphics;
using Android.Views.Animations;

namespace RBAListDemo.Android
{
    [Activity(Label = "Loading Details....", NoHistory=true, Icon = "@drawable/icon")]
    public class ItemDetailActivity : Activity
    {
        #region Variables

        private ImageView _imgItem;
        private TextView _txtDescription;
        private TextView _txtName;
        private TextView _txtPrice;
        private TextView _txtRetail;
        private TranslateAnimation moveDownAnimation;
        private ProgressBar progresBar;
        #endregion


        #region Methods

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            //RequestWindowFeature(WindowFeatures.ActionBar);
            SetContentView(Resource.Layout.ItemDetails);

            _txtName = FindViewById<TextView>(Resource.Id.txtName);
            progresBar = FindViewById<ProgressBar>(Resource.Id.progressBar1);
            
            moveDownAnimation = new TranslateAnimation(0, 0, -200, 0);
            moveDownAnimation.Duration = 1000;
            moveDownAnimation.FillAfter = true;
            moveDownAnimation.Interpolator = new BounceInterpolator();
            _txtName.Visibility =  ViewStates.Gone;
            //_txtDescription = FindViewById<TextView>(Resource.Id.txtDescription);
            //_txtPrice = FindViewById<TextView>(Resource.Id.txtAsking);
            //_txtRetail = FindViewById<TextView>(Resource.Id.txtRetail);
            _imgItem = FindViewById<ImageView>(Resource.Id.imgItem);
            _imgItem.Visibility = ViewStates.Gone;
            _imgItem.Dispose();
        }


        protected override void OnResume()
        {
            base.OnResume();

            var product = RBAListPresenter.Current.CurrentViewModel;
            _txtName.Text = product.Item.Name;
            Title = product.Item.Name;
            //_txtDescription.Text = product.Item.Description;
            //_txtPrice.Text = string.Format("${0:0.00}", product.Item.AskingPrice);
            //_txtRetail.Text = string.Format("${0:0.00}", product.Item.RetailPrice);
            if (!String.IsNullOrEmpty(product.Item.ImageName))
            {
                Task.Factory.StartNew(() =>
                {
                    return BitmapFactory.DecodeStream(new Java.Net.URL("http://rbalist.blob.core.windows.net/rbalist/" + product.Item.ImageName).OpenStream());
                }).ContinueWith(t =>
                {
                    _imgItem.Visibility = ViewStates.Visible;
                    progresBar.Visibility = ViewStates.Gone;
                    
                    Bitmap b = (Bitmap)t.Result;
                    _imgItem.SetImageBitmap(b);
                    

                        
                    _txtName.Visibility =  ViewStates.Visible;
                    _txtName.StartAnimation(moveDownAnimation);



                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
           
        }

        #endregion
    }
}