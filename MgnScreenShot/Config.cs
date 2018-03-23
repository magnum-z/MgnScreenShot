using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MgnScreenShot
{
    class Config
    {
        public static string sDestinationFolder = "";
        public static string sPrefixFilename = "";
        public static string sFormatFilename = "";

        public static int cut_top = 0;
        public static int cut_bottom = 0;
        public static int cut_left = 0;
        public static int cut_right = 0;

        public static void ReadSettings()
        {
            sDestinationFolder = SettingsApp.ReadSetting("destinaion_folder");
            sPrefixFilename = SettingsApp.ReadSetting("prefix_filename");
            sFormatFilename = SettingsApp.ReadSetting("format_filename");
            cut_top = int.Parse("0" + SettingsApp.ReadSetting("cut_top"));
            cut_bottom = int.Parse("0" + SettingsApp.ReadSetting("cut_bottom"));
            cut_left = int.Parse("0" + SettingsApp.ReadSetting("cut_left"));
            cut_right = int.Parse("0" + SettingsApp.ReadSetting("cut_right"));
        }

        public static void SaveSettings()
        {
            SettingsApp.SaveSetting("destinaion_folder", sDestinationFolder);
            SettingsApp.SaveSetting("prefix_filename", sPrefixFilename);
            SettingsApp.SaveSetting("format_filename", sFormatFilename);
            SettingsApp.SaveSetting("cut_top", cut_top.ToString());
            SettingsApp.SaveSetting("cut_bottom", cut_bottom.ToString());
            SettingsApp.SaveSetting("cut_left", cut_left.ToString());
            SettingsApp.SaveSetting("cut_right", cut_right.ToString());
        }
    }
}
