using System;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using RBAList.Core;
using RBAList.Core.Models;

namespace RBAListDemo.Android
{
    [Activity(Label = "New Item", Icon = "@drawable/icon")]
    public class ItemCreateActivity : Activity
    {
        #region Variables

        private byte[] _bitmapData;
        private Button _btnAdd;
        private Button _btnAddImage;
        private ImageView _imgProduct;
        private Stream _mediaFile;
        private EditText _txtDescription;
        private EditText _txtName;
        private EditText _txtPrice;
        private EditText _txtRetail;

        #endregion


        #region Methods

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.ItemActivity);
            // Create your application here
            _txtName = FindViewById<EditText>(Resource.Id.txtName);
            _txtDescription = FindViewById<EditText>(Resource.Id.txtDescription);
            _txtPrice = FindViewById<EditText>(Resource.Id.txtAsking);
            _txtRetail = FindViewById<EditText>(Resource.Id.txtRetail);

            _btnAdd = FindViewById<Button>(Resource.Id.btnAdd);
            _btnAdd.Click += _btnAdd_Click;
            _btnAddImage = FindViewById<Button>(Resource.Id.btnAddImage);
            _btnAddImage.Click += (s, e) => this.ShowQuestion("Add Photo", "Would you like to Choose an existing photo or Take a New one?", "Take New", "Choose Existing", () => AddPhoto(true), () => AddPhoto(false));
            _imgProduct = FindViewById<ImageView>(Resource.Id.imgProduct);
        }


        private void AddPhoto(bool takeNew)
        {
            var mediaFileSource = new MediaFileHelper();
            mediaFileSource.GetPhoto(takeNew, this).ContinueWith(t =>
            {
                var ex = t.Exception;

                if (t.Status != TaskStatus.RanToCompletion || t.Result == null)
                {
                    return;
                }

                using (var mediaFile = t.Result)
                {
                    _mediaFile = mediaFile.GetStream();

                    Bitmap b = BitmapFactory.DecodeFile(t.Result.Path);
                    Bitmap scaledBitmap = scaleDown(b, 960, true);
                    var stream = new MemoryStream();
                    scaledBitmap.Compress(Bitmap.CompressFormat.Jpeg, 70, stream);
                    _bitmapData = stream.ToArray();

                    RunOnUiThread(() => _imgProduct.SetImageBitmap(scaledBitmap));
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void SaveComplete()
        {
            Toast.MakeText(this, "Save Successful", ToastLength.Short).Show();
            var i = new Intent(this, typeof (ItemListActivity));
            i.AddFlags(ActivityFlags.ClearTop);
            StartActivity(i);
        }

        public void _btnAdd_Click(object sender, EventArgs e)
        {
            var itemViewModel = new ItemViewModel();

            var item = new Item();
            item.Name = _txtName.Text;

            item.RetailPrice = double.Parse(_txtRetail.Text);
            item.AskingPrice = double.Parse(_txtPrice.Text);

            item.Description = _txtDescription.Text;
            item.CreatedDate = DateTime.Now;
            item.UserId = SettingsPresenter.Current.UserId;
            itemViewModel.Item = item;
            itemViewModel.AddPhoto(_bitmapData);

            RBAListPresenter.Current.AddItemAsync(itemViewModel, SaveComplete);
        }

        public static Bitmap scaleDown(Bitmap realImage, float maxImageSize, bool filter)
        {
            float ratio = Math.Min(maxImageSize/realImage.Width, maxImageSize/realImage.Height);
            double width = Math.Round(ratio*realImage.Width);
            double height = Math.Round(ratio*realImage.Height);

            Bitmap newBitmap = Bitmap.CreateScaledBitmap(realImage, (int) width, (int) height, filter);
            return newBitmap;
        }

        #endregion


        #region MENU

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.ItemCreate_Menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.addList:
                    StartActivity(typeof (ItemCreateActivity));
                    return true;
                case Resource.Id.logout:
                    SettingsPresenter.Current.Logout(Redirect);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void Redirect()
        {
            var i = new Intent(this, typeof (SplashScreen));
            i.AddFlags(ActivityFlags.ClearTop);
            StartActivity(i);
        }

        #endregion
    }
}