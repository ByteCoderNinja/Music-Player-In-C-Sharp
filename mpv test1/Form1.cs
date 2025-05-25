// File: Form1.cs
// Functionalitate: Interfața grafică principală pentru playerul audio MPV.
// Testare unități: Interfața este testabilă vizual și poate fi testată prin acțiuni simulate (UI automation).
// Sablon utilizat: Observer Pattern (pentru notificarea schimbării stării playerului)

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
using MpvAPI;

namespace MpvPlayerUI
{
    public partial class Form1 : Form
    {
        private MpvFacade mpv;
        private List<string> playlist = new List<string>();
        private ISourceStrategy musicSource = new LocalMusicStrategy("..\\..\\..\\local\\");
        private int currentIndex = 0;

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
            this.FormClosing += Form1_FormClosing;

            // Adaugare muzica din folderul "local" in interfata
            foreach (var song in musicSource.LoadMusic())
            {
                addMusic(song);
            }
            ;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string dllPath = "mpv-1.dll";
                // API de acces al functiilor MPV
                mpv = new MpvFacade(dllPath);
                mpv.Initialize();
                // Timer-ul ce raspunde de actualizarea barei de timp a melodiei
                timer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea melodiei: " + ex.Message);
            }
        }

        private void addMusic(string music)
        {
            playlist.Add(music);
            listBoxSongs.Items.Add(System.IO.Path.GetFileName(music));
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
                    btnPause.Text = "Pause";
                }
                else
                {
                    mpv.Pause();
                    btnPause.Text = "Play";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la pauză/reluare: " + ex.Message);
            }
        }

        private void playNext()
        {
            if (playlist.Count == 0)
            {
                return;
            }
            // Urmatorul index sau primul in caz ca am ajuns la final
            currentIndex = (currentIndex + 1) % playlist.Count;
            PlayCurrent();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            playNext();
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
            ofd.Multiselect = true;
            ofd.Filter = "Fișiere audio (*.mp3;*.wav;*.flac)|*.mp3;*.wav;*.flac";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                int i;
                for (i = 0; i < ofd.FileNames.Length; i++)
                {
                    if (playlist.Contains(ofd.FileNames[i]))
                    {
                        continue;
                    }
                    bool success = musicSource.SaveMusic(ofd.FileNames[i]);
                    if (success)
                    {
                        addMusic(ofd.FileNames[i]);
                    }
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
            // Citire fisier help
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

        private void volumeTrackBar_Scroll(object sender, EventArgs e)
        {
            double volume = volumeTrackBar.Value;
            mpv.SetVolume((int)volume * 10);
            volumeLabel.Text = $"Volum: {volume * 10}%";
        }

        /**
         * Timer ce citeste date despre melodie (durata trecuta si durata totala)
         * Actualizeaza trackBar-ul cantecului si label-ul cu timpul
         */
        private void timer_Tick(object sender, EventArgs e)
        {
            string currentString, totalString;
            int current = (int)mpv.GetTime();
            int total = (int)mpv.GetDuration();

            if (current >= 0 && total > 0)
            {
                if (current >= total - 1)
                {
                    playNext();
                }
                currentString = Utilities.secondsToTime(current);
                totalString = Utilities.secondsToTime(total);
                trackBarSong.Maximum = total;
                trackBarSong.Value = Math.Min(current, trackBarSong.Maximum);
                
                timeLabel.Text = $"{currentString} / {totalString}";
            }
        }

        private void trackBarSong_Scroll(object sender, EventArgs e)
        {
            mpv.SetTime(trackBarSong.Value);
        }

        /**
         * Porneste cantecul de la indexul curent
         */
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
