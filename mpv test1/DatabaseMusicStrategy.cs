/**************************************************************************
 *                                                                        *
 *  Description: Database Starter Code                                    *
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


using MpvPlayerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mpv_test1
{
    class DatabaseMusicStrategy : ISourceStrategy
    {
        private string source;
        public DatabaseMusicStrategy(string path)
        {
            this.source = path;
        }
        public List<string> LoadMusic()
        {
            return new List<string>();
        }

        public bool SaveMusic(string path) { return false; }
    }
}
