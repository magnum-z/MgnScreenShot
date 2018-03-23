using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MgnScreenShot
{
    public partial class frmMain : Form
    {
        internal string[] ImgFormatExt = new string[] {
            "png", "jpg", "bmp", "gif"
        };

        internal ImageFormat[] ImgFormat = new ImageFormat[] {
            ImageFormat.Png,
            ImageFormat.Jpeg,
            ImageFormat.Bmp,
            ImageFormat.Gif
        };

        static internal ResourceManager LocM = new ResourceManager("MgnScreenShot.Resources.WinFormStrings", typeof(frmMain).Assembly);

        internal string spath = "";
        internal string lastfile = "";
        internal string lastlink = "";

        internal Bitmap lastres = null;

        internal string removeid = "";

        private StreamWriter logfile = null;

        private bool mainlock = false;

        internal bool Selection = false;

        private void UpdateControlsFromSettings()
        {
            tbDestinationFolder.Text = Config.sDestinationFolder;
            tbPrefixFilename.Text = Config.sPrefixFilename;
            tbFormatFilename.Text = Config.sFormatFilename;
        }

        private void UpdateSettingsFromControls()
        {
            Config.sDestinationFolder = tbDestinationFolder.Text;
            Config.sPrefixFilename = tbPrefixFilename.Text;
            Config.sFormatFilename = tbFormatFilename.Text;
        }

        internal void OnGrabScreen(Bitmap res, bool clipboard = false, int mode = 0)
        {
            /*
            if (lastres != null) lastres.Dispose();  // fix memory leak with clicking print screen
            lastres = res;
            lastfile = "";
            imgedit = false;
            button1.Enabled = true;
            button2.Enabled = true;
            upload.Enabled = true;
            button5.Enabled = true;
            preview.Enabled = true;
            TraySave.Enabled = true;
            TraySaveAs.Enabled = true;
            TrayUpload.Enabled = true;
            TrayEdit.Enabled = true;
            TrayUploadFrom.Enabled = true;
            TrayPreview.Enabled = true;
            history.Enabled = true;
            if (Properties.Settings.Default.autoshow && !clipboard)
            {
                TrayShowHide(true);
            }
            if (res == null)
            {
                MessageBox.Show(LocM.GetString("capture_err"), LocM.GetString("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                AddEvent(LocM.GetString("event_capture_err"));
            }
            if (clipboard)
            {
                AddEvent(LocM.GetString("event_clipboard"));
            }
            else
            {
                if (mode == 2) AddEvent(LocM.GetString("event_grabc"));
                else AddEvent(LocM.GetString("event_grab" + (mode == 1 ? "w" : "")));
            }
            if (Properties.Settings.Default.autosave && !clipboard)
            {
                SimpleSave();
            }
            */
            SimpleSave();
        }

        internal Image SaveFile(string FilePath, ImageFormat format, bool stm = false)
        {
            /* Image crop at file save, moved to Program.cs
            var width = lastres.Width;
            var height = lastres.Height;
            if (CutBorder())
            {
                width -= Properties.Settings.Default.cut_left + Properties.Settings.Default.cut_right;
                height -= Properties.Settings.Default.cut_top + Properties.Settings.Default.cut_bottom;
                if (width < 1) width = 1;
                if (height < 1) height = 1;
            }
            var TempImg = new Bitmap(width, height);
            var G = Graphics.FromImage(TempImg);
            if (CutBorder()) {
                G.DrawImage(lastres, -Properties.Settings.Default.cut_left, -Properties.Settings.Default.cut_top);
            } else {
                G.DrawImage(lastres, 0, 0);
            }*/
            var curimg = lastres;
            var err = false;
            if (format == ImageFormat.Jpeg)
            {
                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                ImageCodecInfo Encoder = GetEncoder(format);
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                myEncoderParameters.Param[0] = new EncoderParameter(myEncoder, Properties.Settings.Default.quality);

                if (stm)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        curimg.Save(stream, Encoder, myEncoderParameters);
                        return Image.FromStream(stream);
                    }
                }
                else
                {
                    try
                    {
                        curimg.Save(FilePath, Encoder, myEncoderParameters);
                    }
                    catch
                    {
                        MessageBox.Show(LocM.GetString("file_err"), LocM.GetString("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        err = true;
                    }
                }
            }
            else
            {
                if (stm)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        curimg.Save(stream, format);
                        return Image.FromStream(stream);
                    }
                }
                else
                {
                    try
                    {
                        curimg.Save(FilePath, format);
                    }
                    catch
                    {
                        MessageBox.Show(LocM.GetString("file_err"), LocM.GetString("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        err = true;
                    }
                }
            }
            if (err)
            {
                savelabel.Text = LocM.GetString("error");
            }
            else
            {
                lastfile = FilePath;
                savelabel.Text = Path.GetFileName(lastfile);
                savelabel.Enabled = true;
                TrayCopyFilePath.Enabled = true;
            }
            //lastres.Dispose();
            return null;
        }

        private bool SimpleSave(bool edit = false)
        {
            if (!Directory.Exists(spath))
            {
                AddEvent(LocM.GetString("event_save_err"));
                MessageBox.Show(LocM.GetString("path_nf"), LocM.GetString("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            var FileName = ConvertToBase(unixTimestamp, 36);
            var FileExt = ImgFormatExt[Properties.Settings.Default.format];
            if (!edit)
            {
                var i = 0;
                var fname = FileName;
                while (File.Exists(spath + fname + "." + FileExt))
                {
                    i++;
                    fname = FileName + ConvertToBase(i, 36);
                }
                FileName = fname;
            }
            SaveFile(spath + FileName + "." + FileExt, ImgFormat[Properties.Settings.Default.format]);
            if (savelabel.Text == LocM.GetString("error"))
            {
                AddEvent(LocM.GetString("event_save_err"));
                return false;
            }
            AddEvent(LocM.GetString("event_save"), lastfile, 1);
            if (!edit && Properties.Settings.Default.opendir)
            {
                try
                {
                    System.Diagnostics.Process.Start(spath);
                }
                catch { }
            }
            return true;
        }

        public frmMain()
        {
            InitializeComponent();
        }

        private void btnSelectDestination_Click(object sender, EventArgs e)
        {
            string initialFolder = tbDestinationFolder.Text;

            if (!Directory.Exists(initialFolder))
            {
                initialFolder = Application.StartupPath;
            }

            dlgSelectDestination.ShowNewFolderButton = false;
            dlgSelectDestination.SelectedPath = initialFolder;
            dlgSelectDestination.Description = "Select destination folder for screenshots";
            DialogResult dlgResult = dlgSelectDestination.ShowDialog();
            if (dlgResult == DialogResult.OK)
            {
                tbDestinationFolder.Text = dlgSelectDestination.SelectedPath;
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            Config.ReadSettings();
            UpdateControlsFromSettings();
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            UpdateSettingsFromControls();
            Config.SaveSettings();
        }
    }
}
