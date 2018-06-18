using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.Helpers
{
    public static class Settings
    {
        private const string connectionStringSettingsKey = "connectionString";
        public static string ConnectionString
        {
            get
            {
                Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                if (localSettings.Values.ContainsKey(connectionStringSettingsKey))
                {
                    return (string)localSettings.Values[connectionStringSettingsKey];
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                if (localSettings.Values.ContainsKey(connectionStringSettingsKey))
                {
                    localSettings.Values[connectionStringSettingsKey] = value;
                }
                else
                {
                    localSettings.Values.Add(connectionStringSettingsKey, value);
                }
            }
        }
    }
}
