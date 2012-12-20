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
using Xamarin.Media;
using System.Threading.Tasks;
using Android.Graphics.Drawables;
using System.Globalization;
using System.IO;
using Android.Graphics;

namespace RBAList.Core
{
    public class MediaFileHelper
    {
        public Task<MediaFile> GetPhoto(bool takeNew, Context context)
        {
            var picker = new MediaPicker(context);

            return takeNew ? picker.TakePhotoAsync(new StoreCameraMediaOptions()) : picker.PickPhotoAsync();
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bitmapDrawable = (BitmapDrawable)value;
            using (var ms = new MemoryStream())
            {
                bitmapDrawable.Bitmap.Compress(Bitmap.CompressFormat.Jpeg, 70, ms);

                return System.Convert.ToBase64String(ms.ToArray());
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var base64 = (string)value;

            var bytes = System.Convert.FromBase64String(base64);

            var drawable = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length);

            return new BitmapDrawable(drawable);
        }
    }
}