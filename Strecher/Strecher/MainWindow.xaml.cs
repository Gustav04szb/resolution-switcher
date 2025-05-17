using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.UI.Xaml;
using Microsoft.UI.Dispatching;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Strecher
{
    /// <summary>
    /// A window for changing screen resolution between 1440x1080 and max resolution
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        // Store the original resolution to restore it later
        private int originalWidth;
        private int originalHeight;
        private bool resolutionDetected = false;
        private DispatcherQueueTimer refreshTimer;

        [DllImport("user32.dll")]
        public static extern bool EnumDisplaySettings(
            string deviceName, int modeNum, ref DEVMODE devMode);

        [DllImport("user32.dll")]
        public static extern int ChangeDisplaySettings(
            ref DEVMODE devMode, int flags);

        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        
        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [StructLayout(LayoutKind.Sequential)]
        public struct DEVMODE
        {
            private const int CCHDEVICENAME = 32;
            private const int CCHFORMNAME = 32;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public int dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHFORMNAME)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        }

        // Constants for ChangeDisplaySettings
        private const int ENUM_CURRENT_SETTINGS = -1;
        private const int DISP_CHANGE_SUCCESSFUL = 0;
        private const int CDS_UPDATEREGISTRY = 0x01;
        private const int CDS_TEST = 0x02;
        private const int CDS_FULLSCREEN = 0x04;
        private const int DM_PELSWIDTH = 0x80000;
        private const int DM_PELSHEIGHT = 0x100000;

        // Windows API for window styles
        private const int GWL_STYLE = -16;
        private const int WS_THICKFRAME = 0x00040000;
        private const int WS_MAXIMIZEBOX = 0x00010000;

        public MainWindow()
        {
            this.InitializeComponent();
            
            // Aktiviere die benutzerdefinierte Titelleiste
            this.ExtendsContentIntoTitleBar = true;
            this.SetTitleBar(AppTitleBar);
            
            // Set window size to be small
            var windowNative = WindowNative.GetWindowHandle(this);
            var windowId = Win32Interop.GetWindowIdFromWindow(windowNative);
            var appWindow = AppWindow.GetFromWindowId(windowId);
            
            // Set a smaller window size (width: 400, height: 300)
            appWindow.Resize(new Windows.Graphics.SizeInt32(400, 300));
            
            // Konfiguriere die Titelleiste
            if (AppWindowTitleBar.IsCustomizationSupported())
            {
                var titleBar = appWindow.TitleBar;
                
                // Transparente Titelleiste für ein modernes Aussehen
                titleBar.BackgroundColor = Microsoft.UI.Colors.Transparent;
                titleBar.ButtonBackgroundColor = Microsoft.UI.Colors.Transparent;
                titleBar.ButtonInactiveBackgroundColor = Microsoft.UI.Colors.Transparent;
                
                // Farben für die Titelleistenbuttons
                titleBar.ButtonForegroundColor = Windows.UI.Color.FromArgb(255, 167, 139, 250); // #a78bfa
                titleBar.ButtonHoverForegroundColor = Microsoft.UI.Colors.White;
                titleBar.ButtonHoverBackgroundColor = Windows.UI.Color.FromArgb(70, 167, 139, 250);
                titleBar.ButtonPressedBackgroundColor = Windows.UI.Color.FromArgb(100, 167, 139, 250);
            }
            
            // Fenster nicht skalierbar machen
            IntPtr hWnd = windowNative;
            var style = GetWindowLong(hWnd, GWL_STYLE);
            SetWindowLong(hWnd, GWL_STYLE, style & ~(WS_THICKFRAME | WS_MAXIMIZEBOX));
            
            // Get the current resolution and display it
            DetectCurrentResolution();
            
            // Set up a timer to refresh the current resolution text
            refreshTimer = DispatcherQueue.CreateTimer();
            refreshTimer.Interval = TimeSpan.FromSeconds(2);
            refreshTimer.Tick += RefreshTimer_Tick;
            refreshTimer.Start();
        }

        private void RefreshTimer_Tick(DispatcherQueueTimer sender, object args)
        {
            if (!resolutionDetected)
            {
                DetectCurrentResolution();
            }
            else
            {
                refreshTimer.Stop();
            }
        }

        private void SetupWindowAppearance(AppWindow appWindow)
        {
            // No longer needed as we're using custom title bar
            // Kept for compatibility
            if (appWindow != null)
            {
                appWindow.Title = "Resolution Switcher";
            }
        }

        private void DetectCurrentResolution()
        {
            DEVMODE devMode = new DEVMODE();
            devMode.dmSize = (short)Marshal.SizeOf(devMode);
            
            #pragma warning disable CS8625 // Ein NULL-Literal kann nicht in einen Non-Nullable-Verweistyp konvertiert werden.
            if (EnumDisplaySettings(null, ENUM_CURRENT_SETTINGS, ref devMode))
            #pragma warning restore CS8625
            {
                originalWidth = devMode.dmPelsWidth;
                originalHeight = devMode.dmPelsHeight;
                
                UpdateResolutionText(originalWidth, originalHeight);
                resolutionDetected = true;
            }
            else
            {
                CurrentResolutionText.Text = "COULD NOT DETECT RESOLUTION";
            }
        }

        private void UpdateResolutionText(int width, int height)
        {
            // In der UI-Thread ausführen
            DispatcherQueue.TryEnqueue(() => 
            {
                CurrentResolutionText.Text = $"CURRENT: {width} × {height}";
            });
        }

        private void Set1440Button_Click(object sender, RoutedEventArgs e)
        {
            bool success = ChangeResolution(1440, 1080);
            if (!success)
            {
                CurrentResolutionText.Text = "FAILED TO SET 1440 × 1080";
            }
        }

        private void RestoreMaxButton_Click(object sender, RoutedEventArgs e)
        {
            // Try to find the highest available resolution for the display
            DEVMODE highestMode = new DEVMODE();
            highestMode.dmSize = (short)Marshal.SizeOf(highestMode);
            
            int highestWidth = originalWidth;  // Start with original as default
            int highestHeight = originalHeight;
            
            // Enumerate all possible display settings
            int modeNum = 0;
            DEVMODE tempMode = new DEVMODE();
            tempMode.dmSize = (short)Marshal.SizeOf(tempMode);
            
            #pragma warning disable CS8625 // Ein NULL-Literal kann nicht in einen Non-Nullable-Verweistyp konvertiert werden.
            while (EnumDisplaySettings(null, modeNum, ref tempMode))
            #pragma warning restore CS8625
            {
                // Check if this resolution is higher than what we have
                if (tempMode.dmPelsWidth > highestWidth || 
                    (tempMode.dmPelsWidth == highestWidth && tempMode.dmPelsHeight > highestHeight))
                {
                    highestWidth = tempMode.dmPelsWidth;
                    highestHeight = tempMode.dmPelsHeight;
                }
                
                modeNum++;
                tempMode.dmSize = (short)Marshal.SizeOf(tempMode);
            }
            
            // Set to the highest resolution found
            ChangeResolution(highestWidth, highestHeight);
        }

        private bool ChangeResolution(int width, int height)
        {
            DEVMODE devMode = new DEVMODE();
            devMode.dmSize = (short)Marshal.SizeOf(devMode);
            
            #pragma warning disable CS8625 // Ein NULL-Literal kann nicht in einen Non-Nullable-Verweistyp konvertiert werden.
            if (EnumDisplaySettings(null, ENUM_CURRENT_SETTINGS, ref devMode))
            #pragma warning restore CS8625
            {
                devMode.dmPelsWidth = width;
                devMode.dmPelsHeight = height;
                devMode.dmFields = DM_PELSWIDTH | DM_PELSHEIGHT;
                
                int result = ChangeDisplaySettings(ref devMode, CDS_UPDATEREGISTRY);
                
                if (result == DISP_CHANGE_SUCCESSFUL)
                {
                    UpdateResolutionText(width, height);
                    return true;
                }
                else
                {
                    string errorMsg = result switch
                    {
                        1 => "RESTART REQUIRED",
                        -1 => "INVALID PARAMETERS",
                        -2 => "ACCESS DENIED",
                        -3 => "MODE NOT COMPATIBLE",
                        -4 => "INSUFFICIENT MEMORY",
                        _ => $"ERROR CODE: {result}"
                    };
                    
                    CurrentResolutionText.Text = $"FAILED: {errorMsg}";
                    return false;
                }
            }
            
            CurrentResolutionText.Text = "COULD NOT GET CURRENT SETTINGS";
            return false;
        }
    }
}
