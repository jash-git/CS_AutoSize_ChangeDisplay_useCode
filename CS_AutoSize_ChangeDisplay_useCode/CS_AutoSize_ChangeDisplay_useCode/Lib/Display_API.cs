using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//--
//step_01
using System.Runtime.InteropServices;
using System.Drawing;
//-
/*Auto Size in responce to the Display Settings*/
namespace CS_AutoSize_ChangeDisplay_useCode
{
    public class Display_API
    {
        //--
        //step_02
        public enum DMDO
        {
            DEFAULT = 0,
            D90 = 1,
            D180 = 2,
            D270 = 3
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        struct DEVMODE
        {
            public const int DM_DISPLAYFREQUENCY = 0x400000;
            public const int DM_PELSWIDTH = 0x80000;
            public const int DM_PELSHEIGHT = 0x100000;
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
            public DMDO dmDisplayOrientation;
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
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int ChangeDisplaySettings([In] ref DEVMODE lpDevMode, int dwFlags);
        //--

        //--
        //step_03
        private void changeDisplaySetting(int intWidth, int intHeight, int intFrequency)
        {
            long RetVal = 0;
            DEVMODE dm = new DEVMODE();
            dm.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
            dm.dmPelsWidth = intWidth;
            dm.dmPelsHeight = intHeight;
            dm.dmDisplayFrequency = intFrequency;
            dm.dmFields = DEVMODE.DM_PELSWIDTH | DEVMODE.DM_PELSHEIGHT | DEVMODE.DM_DISPLAYFREQUENCY;
            RetVal = ChangeDisplaySettings(ref dm, 0);
        }
        public int m_intWidth = -1;
        public int m_intHeight = -1;
        public float m_fltScreenScalingFactor_H=0.0f;
        public float m_fltScreenScalingFactor_W = 0.0f;
        //--

        //--
        //step_04
        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
        public enum DeviceCap
        {
            HORZRES = 8,
            VERTRES = 10,
            DESKTOPVERTRES = 117,
            DESKTOPHORZRES = 118,

            // http://pinvoke.net/default.aspx/gdi32/GetDeviceCaps.html
        }

        public void getDisplaySetting()
        {
            m_intWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            m_intHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            getScalingFactor();

        }
        public void getScalingFactor()//放大比例
        {
            Graphics g = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr desktop = g.GetHdc();
            int LogicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.VERTRES);
            int PhysicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES);

            float ScreenScalingFactor = (float)PhysicalScreenHeight / (float)LogicalScreenHeight;
            m_fltScreenScalingFactor_H = ScreenScalingFactor; // 1.25 = 125%

            int LogicalScreenWidth = GetDeviceCaps(desktop, (int)DeviceCap.HORZRES);
            int PhysicalScreenWidth = GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPHORZRES);
            ScreenScalingFactor = (float)PhysicalScreenWidth / (float)LogicalScreenWidth;
            m_fltScreenScalingFactor_W = ScreenScalingFactor; // 1.25 = 125%
        }
        //--
    }
}
