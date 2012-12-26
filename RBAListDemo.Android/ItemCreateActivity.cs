using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using System.IO;

using Android.App;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;

using RBAList.Core.Models;
using RBAList.Core;
using Android.Graphics;
using Android.OS;
using Xamarin.Media;
using Android.Content;

namespace RBAListDemo.Android
{
    [Activity(Label = "My Activity")]
    public class ItemCreateActivity : Activity
    {
        private EditText _txtName;
        private EditText _txtDescription;
        private EditText _txtPrice;
        private EditText _txtRetail;
        private Button _btnAdd;
        private Button _btnAddImage;
        private ImageView _imgProduct;
        private ProgressBar _progress;

        private Stream _mediaFile;
        private byte[] _bitmapData;
        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.ItemActivity);
            // Create your application here
            _txtName = FindViewById<EditText>(Resource.Id.txtName);
            _txtDescription = FindViewById<EditText>(Resource.Id.txtDescription);
            _txtPrice = FindViewById<EditText>(Resource.Id.txtAsking);
            _txtRetail = FindViewById<EditText>(Resource.Id.txtRetail);
            _progress = FindViewById<ProgressBar>(Resource.Id.progressBar1);
            
            _btnAdd = FindViewById<Button>(Resource.Id.btnAdd);
            _btnAdd.Click += _btnAdd_Click;
            _btnAddImage = FindViewById<Button>(Resource.Id.btnAddImage);
            _btnAddImage.Click +=
                (s, e) =>
                this.ShowQuestion("Add Photo", "Would you like to Choose an existing photo or Take a New one?", "Take New",
                                  "Choose Existing",
                                  () => AddPhoto(true), () => AddPhoto(false));
            _imgProduct = FindViewById<ImageView>(Resource.Id.imgProduct);
            
        }
    

        private void AddPhoto(bool takeNew)
        {
            var mediaFileSource = new MediaFileHelper();
            mediaFileSource.GetPhoto(takeNew, this).ContinueWith(t =>
            {
                var ex = t.Exception;

                if (t.Status != TaskStatus.RanToCompletion || t.Result == null) return;

                using (var mediaFile = t.Result)
                {
                    _mediaFile = mediaFile.GetStream();

                    Bitmap b = BitmapFactory.DecodeFile(t.Result.Path);
                    
                    _imgProduct.Visibility = ViewStates.Visible;
                    _progress.Visibility = ViewStates.Gone;
                    Bitmap scaledBitmap = scaleDown(b, 300, true);

                    MemoryStream stream = new MemoryStream();
                    scaledBitmap.Compress(Bitmap.CompressFormat.Jpeg, 70, stream);
                   _bitmapData = stream.ToArray();

                   
                    _imgProduct.SetImageBitmap(scaledBitmap);
                }
                       
                
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public static Bitmap scaleDown(Bitmap realImage, float maxImageSize, bool filter)
        {
            float ratio = Math.Min(
                    (float)maxImageSize / realImage.Width, (float)maxImageSize / realImage.Height);
            double width = Math.Round((float)ratio * realImage.Width);
            double height = Math.Round((float)ratio * realImage.Height);

            Bitmap newBitmap = Bitmap.CreateScaledBitmap(realImage, (int)width,
                    (int)height, filter);
            return newBitmap;
        }

        public void _btnAdd_Click(object sender, EventArgs e)
        {
            ItemViewModel itemViewModel = new ItemViewModel();

            Item item = new Item();
            item.Name = _txtName.Text;
            
            item.RetailPrice = double.Parse(_txtRetail.Text);
            item.AskingPrice = double.Parse(_txtPrice.Text);

            item.Description = _txtDescription.Text;
            item.CreatedDate = DateTime.Now;

            itemViewModel.Item = item;
            itemViewModel.AddPhoto(_bitmapData);            

            RBAListPresenter.Current.AddItemAsync(itemViewModel, SaveComplete);

          
        }

        public void SaveComplete()
        {
            Toast.MakeText(this, "Save Successful", ToastLength.Short).Show();
            Intent i = new Intent(this, typeof(ItemListActivity));
            i.AddFlags(ActivityFlags.ClearTop);
            StartActivity(i);
        }

       
    }
}