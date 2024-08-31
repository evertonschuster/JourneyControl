using JourneyControl.Application.Services;
using System.Runtime.InteropServices;

namespace JourneyControl.Infra.Windows.Services
{
    internal class ActivityMonitor : IActivityMonitor
    {
        public TimeOnly GetLastActivity()
        {
            int idleTime = GetIdleTime();

            int hours = idleTime / (1000 * 60 * 60) % 24;
            int minutes = idleTime / (1000 * 60) % 60;
            int seconds = idleTime / 1000 % 60;
            int milliseconds = idleTime % 1000;

            return new TimeOnly(hours, minutes, seconds, milliseconds);
        }

        static int GetIdleTime()
        {
            LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);
            if (!GetLastInputInfo(ref lastInputInfo))
            {
                return 0;
            }
            return Environment.TickCount - (int)lastInputInfo.dwTime;
        }


        [DllImport("user32.dll")]
        static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        [StructLayout(LayoutKind.Sequential)]
        struct LASTINPUTINFO
        {
            public uint cbSize;
            public uint dwTime;
        }
    }
}
