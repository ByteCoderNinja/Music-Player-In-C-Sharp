/**************************************************************************
 *                                                                        *
 *  Description: Generic functions                                        *
 *  Website: https://github.com/SorinNoroc/MusicPlayer                    *
 *  Copyright: (c) 2025, Noroc Sorin                                      *
 *                                                                        *
 *  This code and information is provided "as is" without warranty of     *
 *  any kind, either expressed or implied, including but not limited      *
 *  to the implied warranties of merchantability or fitness for a         *
 *  particular purpose. You are free to use this source code in your      *
 *  applications as long as the original copyright notice is included.    *
 *                                                                        *
 **************************************************************************/


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
