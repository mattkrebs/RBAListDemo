using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RBAList.Core
{
    public interface ISettingsProvider
    {
        string UserId { get; set; }
        int AuthenticationProvider { get; set; }

        void Load();
        void Save();
    }
}
