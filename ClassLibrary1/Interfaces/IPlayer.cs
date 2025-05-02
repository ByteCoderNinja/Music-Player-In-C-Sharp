using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpvAPI.Interfaces
{
    internal interface IPlayer
    {
         Mpv mpvAPI { get; }
        string libPath { get; }
        string screenshotPath {  get; }
        bool playing {  get; }
        int Volume { get; }
        int playbackSpeed { get; }
        TimeSpan position { get; }
        TimeSpan progress { get; }

         void Load(string path, bool force = false);

         void Resume();

         void Pause();

         void Stop();
        void frameSkipFore();
        void frameSkipBack();
    }
}
