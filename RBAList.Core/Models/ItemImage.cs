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

namespace RBAList.Core.Models
{
    public class ItemImage
    {

        public ItemImage()
		{			
			ImageBase64 = string.Empty;
		}

        public int Id { get; set; }
        public string ImageBase64 { get; set; }

    }
}