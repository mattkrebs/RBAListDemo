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
using Microsoft.WindowsAzure.MobileServices;

namespace RBAList.Core.Models
{
    public class LoginPlatform
    {
        public string Name { get; set; }
        public MobileServiceAuthenticationProvider Provider { get; set; }
    }
}