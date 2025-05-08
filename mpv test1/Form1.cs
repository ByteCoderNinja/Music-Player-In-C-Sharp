// File: Form1.cs
// Functionalitate: Interfața grafică principală pentru playerul audio MPV.
// Testare unități: Interfața este testabilă vizual și poate fi testată prin acțiuni simulate (UI automation).
// Sablon utilizat: Observer Pattern (pentru notificarea schimbării stării playerului)

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MpvAPI;
using System.Configuration;

namespace MpvPlayerUI
{
    public partial class Form1 : Form
    {
        private DatabaseManager dbManager = new DatabaseManager(ConfigurationManager.ConnectionStrings["MusicDBConnectionString"].ConnectionString);
        private Mpv mpv;
        private List<string> playlist = new List<string>();
        private int currentIndex = 0;

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
            this.FormClosing += Form1_FormClosing;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string dllPath = "mpv-1.dll";
                mpv = new Mpv(dllPath);
                mpv.Initialize();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea melodiei: " + ex.Message);
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
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

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (playlist.Count == 0)
            {
                return;
            }
            currentIndex = (currentIndex + 1) % playlist.Count;
            PlayCurrent();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (playlist.Count == 0)
            {
                return;
            }
            currentIndex = (currentIndex - 1 + playlist.Count) % playlist.Count;
            PlayCurrent();
        }

        private void comboSpeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mpv == null)
            {
                return;
            }
            string selected = comboSpeed.SelectedItem.ToString();
            selected = selected.Replace("x", "");
            if (float.TryParse(selected, out float speed))
            {
                mpv.SetSpeed(speed);
            }
        }

        private void btnAddSong_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "MP3 files (*.mp3)|*.mp3";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string selectedFile = ofd.FileName;
                string fileName = System.IO.Path.GetFileName(selectedFile);

                if (playlist.Contains(selectedFile))
                {
                    MessageBox.Show("Melodia este deja în playlist.");
                    return;
                }

                playlist.Add(selectedFile);
                listBoxSongs.Items.Add(fileName);

                if (playlist.Count == 1)
                {
                    PlayCurrent();
                }

                mpv.Load(selectedFile, this.Handle);

                dbManager.AddSong(fileName, selectedFile);
            }
        }

        private void listBoxSongs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxSongs.SelectedIndex >= 0)
            {
                currentIndex = listBoxSongs.SelectedIndex;
                PlayCurrent();
            }
        }

        private void PlayCurrent()
        {
            if (playlist.Count == 0 || mpv == null)
            {
                return;
            }
            string songPath = playlist[currentIndex];
            mpv.Load(songPath, panel1.Handle);
            listBoxSongs.SelectedIndex = currentIndex;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            mpv?.Dispose();
        }
    }
}
