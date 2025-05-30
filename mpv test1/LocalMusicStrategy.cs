/**************************************************************************
 *                                                                        *
 *  Description: Strategy Design Pattern for Local Storage                *
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
using System.Windows.Forms;

namespace MpvPlayerUI
{
    class LocalMusicStrategy: ISourceStrategy
    {
        private string _source;
        public LocalMusicStrategy(string path)
        {
            this._source = path;
        }
        public List<string> LoadMusic()
        {
            List<string> musicFiles = new List<string>();
            musicFiles = Directory.GetFiles(Path.GetFullPath(_source)).ToList();
            return musicFiles;
        }

        public bool SaveMusic(string musicPath)
        {
            try
            {
                File.Copy(musicPath, Path.Combine(Path.GetFullPath(_source), Path.GetFileName(musicPath)));
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
