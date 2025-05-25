using MpvPlayerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        public bool SaveMusic(string musicPath)
        {
            try
            {
                File.Copy(musicPath, Path.Combine(Path.GetFullPath(source), Path.GetFileName(musicPath)));
                return true;
            } catch(IOException ex)
            {
                MessageBox.Show("Duplicate music files not allowed!", ex.Message);
            } catch (Exception ex)
            {
                MessageBox.Show("Error saving song. ", ex.Message);
            }
            return false;
        }
    }
}
