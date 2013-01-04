using Microsoft.WindowsAzure.MobileServices;

namespace RBAList.Core.Models
{
    public class LoginPlatform
    {
        #region Properties

        public string Name { get; set; }
        public MobileServiceAuthenticationProvider Provider { get; set; }

        #endregion
    }
}