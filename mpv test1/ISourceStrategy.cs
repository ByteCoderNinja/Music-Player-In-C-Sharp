/**************************************************************************
 *                                                                        *
 *  Description: Strategy DP Interface                                    *
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

namespace MpvPlayerUI
{
    interface ISourceStrategy
    {
        List<string> LoadMusic();
        bool SaveMusic(string musicPath);
    }
}
