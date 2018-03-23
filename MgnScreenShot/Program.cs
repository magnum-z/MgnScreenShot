using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

class ScreenCapturer
{
    public enum CaptureMode
    {
        Screen,
        Window
    }
 
    [DllImport(@"dwmapi.dll")]
    private static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwAttribute, out Rect pvAttribute, int cbAttribute);

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    private Bitmap lastres;

    public Bitmap Capture(CaptureMode screenCaptureMode = CaptureMode.Window, bool cutborder = true)
    {
        /*
        var Settings = MgnScreenShot.Properties.Settings.Default;
        if (Settings.clipboard)
        {
            if (Clipboard.ContainsImage())
            {
                var res = new Bitmap(Clipboard.GetImage());
                if (screenCaptureMode == CaptureMode.Window && cutborder)
                {
                    var cropRect = new Rectangle(
                        Settings.cut_top,
                        Settings.cut_left,
                        res.Width - Settings.cut_right,
                        res.Height - Settings.cut_bottom
                    );
                    var bmp = res.Clone(cropRect, res.PixelFormat);
                    res.Dispose();
                    res = bmp;
                }
                return res;
            }
            return null;
        }
        */

        Rectangle bounds;

        if (screenCaptureMode == CaptureMode.Screen)
        {
            bounds = Screen.GetBounds(Point.Empty);
            CursorPosition = Cursor.Position;
        }
        else
        {
            var handle = GetForegroundWindow();
            var rect = new Rect();
            if (Environment.OSVersion.Version.Major < 6)
            {
                GetWindowRect(handle, ref rect);
            }
            else
            {
                var res = -1;
                try
                {
                    res = DwmGetWindowAttribute(handle, 9, out rect, Marshal.SizeOf(typeof(Rect)));
                }
                catch { }
                if (res < 0) GetWindowRect(handle, ref rect);
            }

            var top = rect.Top;
            var left = rect.Left;
            var right = rect.Right - rect.Left;
            var bottom = rect.Bottom - rect.Top;
            if (cutborder)
            {
                top += MgnScreenShot.Config.cut_top;
                left += MgnScreenShot.Config.cut_left;
                right -= MgnScreenShot.Config.cut_left + MgnScreenShot.Config.cut_right;
                bottom -= MgnScreenShot.Config.cut_top + MgnScreenShot.Config.cut_bottom;
                if (right < 1) right = 1;
                if (bottom < 1) bottom = 1;
            }

            bounds = new Rectangle(left, top, right, bottom);
            //CursorPosition = new Point(Cursor.Position.X - rect.Left, Cursor.Position.Y - rect.Top);
        }

        var result = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format24bppRgb);

        using (var g = Graphics.FromImage(result))
        {
            g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
        }

        lastres = result;
        return result;
    }

    public Bitmap GetResult()
    {
        return lastres;
    }

    public Point CursorPosition
    {
        get;
        protected set;
    }
}

namespace MgnScreenShot
{
    static class Program
    {
        private static frmMain WinForm;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var attribute = (GuidAttribute)assembly.GetCustomAttributes(typeof(GuidAttribute), true)[0];
            var appGuid = attribute.Value;

            using (Mutex mutex = new Mutex(false, @"Global\" + appGuid))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                _hookID = SetHook(_proc);
                WinForm = new frmMain();
                if (!mutex.WaitOne(0, false))
                {
                    MessageBox.Show(frmMain.LocM.GetString("running"), frmMain.LocM.GetString("error"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                Application.Run(WinForm);
                UnhookWindowsHookEx(_hookID);
            }            
        }

        private const int WH_KEYBOARD_LL = 13;        
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;
        private const int WM_SYSKEYDOWN = 0x104;
        private const int VK_F1 = 0x70;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        private static bool pressed = false;

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static ScreenCapturer Scr = new ScreenCapturer();

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                Keys number = (Keys)Marshal.ReadInt32(lParam);
                //MessageBox.Show(number.ToString());
                if (number == Keys.PrintScreen)
                {
                    Debug.WriteLine(wParam.ToString());
                    if (pressed && wParam == (IntPtr)261 && Keys.Alt == Control.ModifierKeys && number == Keys.PrintScreen)
                    {
                        var res = Scr.Capture(ScreenCapturer.CaptureMode.Window, Properties.Settings.Default.cutborder);
                        WinForm.OnGrabScreen(res, false, 1);
                        pressed = false;
                    }
                    else if (pressed && wParam == (IntPtr)257 && Keys.Shift == Control.ModifierKeys && number == Keys.PrintScreen)
                    {
                        if (!WinForm.Selection)
                        {
                            var res = Scr.Capture(ScreenCapturer.CaptureMode.Screen);
                            var form = new SelectionFrame(WinForm, res);
                            form.ShowDialog();
                        }
                        pressed = false;
                    }
                    else if (pressed && wParam == (IntPtr)257 && number == Keys.PrintScreen)
                    {
                        var res = Scr.Capture(ScreenCapturer.CaptureMode.Screen);
                        WinForm.OnGrabScreen(res);
                        pressed = false;
                    }
                    else if (wParam == (IntPtr)256 || wParam == (IntPtr)260)
                    {
                        pressed = true; // fix for win xp double press
                    }
                }
            }

            return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);


















        /*


        private const int WH_KEYBOARD_LL = 13;        
        private const int VK_F1 = 0x70;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                Keys number = (Keys)Marshal.ReadInt32(lParam);
                //MessageBox.Show(number.ToString());
                if (number == Keys.PrintScreen)
                {
                    if (wParam == (IntPtr)261 && Keys.Alt == Control.ModifierKeys && number == Keys.PrintScreen)
                    {
                        // Alt+PrintScreen                        
                        MessageBox.Show(number.ToString() + " [" + Control.ModifierKeys + "]");
                    }
                    else if (wParam == (IntPtr)257 && number == Keys.PrintScreen)
                    {
                        // PrintScreen
                        MessageBox.Show(number.ToString() + " [" + Control.ModifierKeys + "]");
                    }
                }
            }

            return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
        */
    }
}
