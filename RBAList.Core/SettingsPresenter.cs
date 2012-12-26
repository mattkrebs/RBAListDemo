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
using System.IO.IsolatedStorage;
using System.IO;
using Newtonsoft.Json;

namespace RBAList.Core
{
    public class SettingsPresenter : ISettingsProvider
    {

        private static SettingsPresenter _presenter;
        public static SettingsPresenter Current
        {
            get { return _presenter ?? (_presenter = new SettingsPresenter()); }
        }

        public SettingsPresenter()
        {
            UserId = string.Empty;
            AuthenticationProvider = -1;
        }

        public string UserId { get; set; }
        public int AuthenticationProvider { get; set; }

        public void Load()
        {
            try
            {
                var json = string.Empty;

                using (var isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                using (var file = new IsolatedStorageFileStream("Settings.json", FileMode.Open, isolatedStorage))
                using (var sw = new StreamReader(file))
                {
                    json = sw.ReadToEnd();
                }

                var settings = JsonConvert.DeserializeObject<SettingsPresenter>(json);

                UserId = settings.UserId;
                AuthenticationProvider = settings.AuthenticationProvider;
            }
            catch (Exception)
            {
            }
        }

        public void Save()
        {
            try
            {
                var json = JsonConvert.SerializeObject(this);

                using (var isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                using (var file = new IsolatedStorageFileStream("Settings.json", FileMode.Create, isolatedStorage))
                using (var sw = new StreamWriter(file))
                {
                    sw.Write(json);
                    sw.Flush();
                }
            }
            catch (Exception)
            {
            }
        }
    }
}