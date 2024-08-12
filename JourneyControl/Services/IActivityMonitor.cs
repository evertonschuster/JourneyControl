using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyControl.Services
{
    internal interface IActivityMonitor
    {
        TimeOnly GetLastActivity();
    }
}
