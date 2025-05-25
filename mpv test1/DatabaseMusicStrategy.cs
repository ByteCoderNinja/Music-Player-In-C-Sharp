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
