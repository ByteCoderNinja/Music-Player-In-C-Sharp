using MpvPlayerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpvPlayerUI
{
    class LocalMusicStrategy: ISourceStrategy
    {
        private string source;
        public LocalMusicStrategy(string path)
        {
            this.source = path;
        }
        public List<string> LoadMusic()
        {
            List<string> musicFiles = new List<string>();
            musicFiles = Directory.GetFiles(Path.GetFullPath(source)).ToList();
            return musicFiles;
        }
    }
}
