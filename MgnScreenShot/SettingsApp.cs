using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace MgnScreenShot
{
    class SettingsApp
    {
        public static string ReadSetting(string key)
        {
            string result;
            try
            {
                result = ConfigurationManager.AppSettings[key];
            }
            catch (ConfigurationErrorsException)
            {
                result = "";
            }

            return result;
        }

        public static void SaveSetting(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);

            }
            catch (ConfigurationErrorsException)
            {
                //
            }
        }
    }
}
