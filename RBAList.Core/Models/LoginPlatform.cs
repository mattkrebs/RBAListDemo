using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.MobileServices;

namespace RBAList.Core.Models
{
    public class LoginPlatform
    {
        public string Name { get; set; }
        public MobileServiceAuthenticationProvider Provider { get; set; }
    }
}