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

namespace RBAList.Core
{
    public class MediaFileHelper
    {
#if MONOANDROID
        public Context context { get; set; }

        public MediaFileHelper(Context context)
        {
            this.context = context;
        }
#endif

        public Task<MediaFile> GetPhoto(bool takeNew)
        {
#if MONOANDROID
          
                var picker = new MediaPicker(context);
#else
            var picker = new MediaPicker();
#endif
            return takeNew ? picker.TakePhotoAsync(new StoreCameraMediaOptions()) : picker.PickPhotoAsync();
        }
    }
}