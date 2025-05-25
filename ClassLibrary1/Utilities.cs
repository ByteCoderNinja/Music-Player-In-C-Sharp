using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpvAPI
{
    public class Utilities
    {
        static public string secondsToTime(int seconds)
        {
            if (seconds == 0) return "00:00:00";
            
            string s = "";
            s = ":" + (seconds % 60).ToString("D2") + s;
            seconds = seconds / 60;
            s = ":" + (seconds % 60).ToString("D2") + s;
            seconds = seconds / 60;
            s = seconds.ToString("D2") + s;

            return s;
        }
    }
}
