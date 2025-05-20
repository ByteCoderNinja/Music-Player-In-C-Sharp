// File: MusicPlayerFacade.cs
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MpvAPI;

namespace MpvPlayerUI
{
    public class MusicPlayerFacade : IDisposable
    {
        private Mpv mpv;
        private List<string> playlist = new List<string>();
        private int currentIndex = 0;

        public MusicPlayerFacade(string dllPath)
        {
            mpv = new Mpv(dllPath);
            mpv.Initialize();
        }

        public void AddSong(string filePath)
        {
            playlist.Add(filePath);
        }

        public List<string> GetPlaylist()
        {
            return playlist;
        }

        public int GetCurrentIndex()
        {
            return currentIndex;
        }

        public void Play(IntPtr handle)
        {
            if (playlist.Count == 0) return;
            mpv.Load(playlist[currentIndex], handle);
        }

        public void PauseOrResume()
        {
            if (mpv == null || playlist.Count == 0)
            {
                return;
            }
            if (mpv.isPaused())
            {
                mpv.Resume();
            }
            else
            {
                mpv.Pause();
            }
        }

        public void Next(IntPtr handle)
        {
            if (playlist.Count == 0) return;
            currentIndex = (currentIndex + 1) % playlist.Count;
            Play(handle);
        }

        public void Previous(IntPtr handle)
        {
            if (playlist.Count == 0) return;
            currentIndex = (currentIndex - 1 + playlist.Count) % playlist.Count;
            Play(handle);
        }

        public void SelectTrack(int index, IntPtr handle)
        {
            if (index >= 0 && index < playlist.Count)
            {
                currentIndex = index;
                Play(handle);
            }
        }

        public void SetSpeed(float speed)
        {
            mpv.SetSpeed(speed);
        }

        public void Dispose()
        {
            mpv?.Dispose();
        }
    }
}
