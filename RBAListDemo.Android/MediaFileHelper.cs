using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Xamarin.Media;

namespace RBAListDemo.Android
{
    public class MediaFileHelper
    {
        #region Methods

        public static object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var base64 = (string) value;

            var bytes = System.Convert.FromBase64String(base64);

            var drawable = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length);

            return new BitmapDrawable(drawable);
        }

        public static object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bitmapDrawable = (BitmapDrawable) value;
            using (var ms = new MemoryStream())
            {
                bitmapDrawable.Bitmap.Compress(Bitmap.CompressFormat.Jpeg, 70, ms);

                return System.Convert.ToBase64String(ms.ToArray());
            }
        }

        public Task<MediaFile> GetPhoto(bool takeNew, Context context)
        {
            var picker = new MediaPicker(context);

            return takeNew ? picker.TakePhotoAsync(new StoreCameraMediaOptions()) : picker.PickPhotoAsync();
        }

        #endregion
    }
}