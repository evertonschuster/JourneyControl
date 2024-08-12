﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JourneyControl.Services
{
    internal class ActivityMonitor : IActivityMonitor
    {
        public TimeOnly GetLastActivity()
        {
            int idleTime = GetIdleTime();
            Console.WriteLine($"Usuário inativo por {idleTime / 1000} segundos.");

            int hours = (idleTime / (1000 * 60 * 60)) % 24;
            int minutes = (idleTime / (1000 * 60)) % 60;
            int seconds = (idleTime / 1000) % 60;
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
