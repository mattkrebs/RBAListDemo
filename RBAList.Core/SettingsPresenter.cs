using System;
using System.IO;
using System.IO.IsolatedStorage;
using Newtonsoft.Json;

namespace RBAList.Core
{
    public class SettingsPresenter : ISettingsProvider
    {
        #region Variables

        private static SettingsPresenter _presenter;

        #endregion


        #region Properties

        public static SettingsPresenter Current
        {
            get { return _presenter ?? (_presenter = new SettingsPresenter()); }
        }

        #endregion


        #region Constructors

        private SettingsPresenter()
        {
            UserId = string.Empty;
            AuthenticationProvider = -1;
        }

        #endregion


        #region ISettingsProvider Members

        public string UserId { get; set; }
        public int AuthenticationProvider { get; set; }

        public void Load()
        {
            try
            {
                string json;

                using (var isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (var file = new IsolatedStorageFileStream("Settings.json", FileMode.Open, isolatedStorage))
                    {
                        using (var sw = new StreamReader(file))
                        {
                            json = sw.ReadToEnd();
                        }
                    }
                }

                var settings = JsonConvert.DeserializeObject<SettingsPresenter>(json);

                UserId = settings.UserId;
                AuthenticationProvider = settings.AuthenticationProvider;
            }
            catch (Exception)
            {}
        }

        public void Save()
        {
            try
            {
                var json = JsonConvert.SerializeObject(this);

                using (var isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (var file = new IsolatedStorageFileStream("Settings.json", FileMode.Create, isolatedStorage))
                    {
                        using (var sw = new StreamWriter(file))
                        {
                            sw.Write(json);
                            sw.Flush();
                        }
                    }
                }
            }
            catch (Exception)
            {}
        }

        #endregion


        #region Methods

        public void Logout(Action callback)
        {
            var settings = this;

            settings.UserId = string.Empty;
            settings.AuthenticationProvider = -1;
            settings.Save();

            callback();
        }

        #endregion
    }
}