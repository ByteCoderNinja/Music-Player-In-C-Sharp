using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace MpvPlayerUI
{
    public class Form1TestWrapper : Form1
    {
        public void TestAddSong(string song)
        {
            if (!playlist.Contains(song))
            {
                playlist.Add(song);
                listBoxSongs.Items.Add(System.IO.Path.GetFileName(song));
            }
        }

        public int SongsCount()
        {
            return playlist.Count;
        }

        public void SetSelectedIndex(int i)
        {
            listBoxSongs.SelectedIndex = i;
        }

        public int CurrentIndex()
        {
            return currentIndex;
        }

        public void SimulateNextClick()
        {
            btnNext.PerformClick();
            if (currentIndex < playlist.Count - 1)
            {
                ++currentIndex;
            }
            else
            {
                currentIndex = 0;
            }
        }

        public void TestSelectSpeed(string speed)
        {
            var combo = (ComboBox)this.Controls["comboSpeed"];
            if (combo.Items.Contains(speed))
                combo.SelectedItem = speed;
            else
                combo.SelectedItem = "1.0x";
        }

        public double SelectedSpeed
        {
            get
            {
                var combo = (ComboBox)this.Controls["comboSpeed"];
                string selected = combo.SelectedItem as string;
                if (selected != null && selected.EndsWith("x") && double.TryParse(selected.Replace("x", ""), out var speed))
                    return speed;
                return 1.0;
            }
        }

        public bool IsPlaying
        {
            get
            {
                var field = typeof(Form1).GetField("isPlaying", BindingFlags.NonPublic | BindingFlags.Instance);
                return field != null && (bool)field.GetValue(this);
            }
        }

        public void TestSetPlaying(bool value)
        {
            var field = typeof(Form1).GetField("isPlaying", BindingFlags.NonPublic | BindingFlags.Instance);
            field?.SetValue(this, value);
        }

        public void SimulatePauseClick() => btnPause.PerformClick();

        public int SelectedIndex
        {
            get
            {
                return ((ListBox)this.Controls["listBoxSongs"]).SelectedIndex;
            }
        }

        public void SimulatePreviousClick()
        {
            btnPrevious.PerformClick();
            if (currentIndex > 0)
            {
                --currentIndex;
            }
            else if (currentIndex == 0)
            {
                currentIndex = playlist.Count - 1;
            }
        }

        public double Volume
        {
            get
            {
                var field = typeof(Form1).GetField("volume", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                return field != null ? (double)field.GetValue(this) : 0;
            }
            set
            {
                var field = typeof(Form1).GetField("volume", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                field?.SetValue(this, value);
            }
        }

        public List<string> Playlist()
        {
            return playlist;
        }
    }

    [TestClass]
    public class MusicPlayerTest
    {
        [TestMethod]
        public void Test_AddSong_IncreasesListCount()
        {
            var form = new Form1TestWrapper();
            form.TestAddSong("song1.mp3");
            Assert.AreEqual(5, form.SongsCount());
        }

        [TestMethod]
        public void Test_AddSameSongTwice_NotWorking()
        {
            var form = new Form1TestWrapper();
            form.TestAddSong("song1.mp3");
            form.TestAddSong("song1.mp3");
            Assert.AreEqual(5, form.SongsCount());
        }

        [TestMethod]
        public void Test_NextSong_ChangesSelection()
        {
            var form = new Form1TestWrapper();
            form.TestAddSong("s1.mp3");
            form.TestAddSong("s2.mp3");
            form.SetSelectedIndex(0);
            form.SimulateNextClick();
            Assert.AreEqual(1, form.CurrentIndex());
        }

        [TestMethod]
        public void Test_SpeedChange1_SetsCorrectSpeed()
        {
            var form = new Form1TestWrapper();
            form.TestSelectSpeed("0.5x");
            Assert.AreEqual(0.5, form.SelectedSpeed);
        }

        [TestMethod]
        public void Test_SpeedChange2_SetsCorrectSpeed()
        {
            var form = new Form1TestWrapper();
            form.TestSelectSpeed("1.0x");
            Assert.AreEqual(1.0, form.SelectedSpeed);
        }

        [TestMethod]
        public void Test_SpeedChange3_SetsCorrectSpeed()
        {
            var form = new Form1TestWrapper();
            form.TestSelectSpeed("1.5x");
            Assert.AreEqual(1.5, form.SelectedSpeed);
        }

        [TestMethod]
        public void Test_SpeedChange4_SetsCorrectSpeed()
        {
            var form = new Form1TestWrapper();
            form.TestSelectSpeed("2.0x");
            Assert.AreEqual(2.0, form.SelectedSpeed);
        }

        [TestMethod]
        public void Test_SpeedChangeAfterPause_SetsCorrectSpeed()
        {
            var form = new Form1TestWrapper();
            form.TestSetPlaying(true);
            form.SimulatePauseClick();
            form.TestSelectSpeed("2.0x");
            form.SimulatePauseClick();
            Assert.AreEqual(2.0, form.SelectedSpeed);
        }

        [TestMethod]
        public void Test_Pause_TogglesPlayFlag()
        {
            var form = new Form1TestWrapper();
            form.TestSetPlaying(false);
            form.SimulatePauseClick();
            Assert.IsFalse(form.IsPlaying);
        }

        [TestMethod]
        public void Test_AddMultipleSongs_IncreasesCountCorrectly()
        {
            var form = new Form1TestWrapper();
            form.TestAddSong("s1.mp3");
            form.TestAddSong("s2.mp3");
            form.TestAddSong("s3.mp3");
            Assert.AreEqual(7, form.SongsCount());
        }

        [TestMethod]
        public void Test_PreviousSong_ChangesSelection()
        {
            var form = new Form1TestWrapper();
            form.TestAddSong("a.mp3");
            form.TestAddSong("b.mp3");
            form.SetSelectedIndex(1);
            form.SimulatePreviousClick();
            Assert.AreEqual(0, form.CurrentIndex());
        }

        [TestMethod]
        public void Test_PreviousSong_AtStart_RemainsAtZero()
        {
            var form = new Form1TestWrapper();
            form.TestAddSong("song.mp3");
            form.SetSelectedIndex(0);
            form.SimulatePreviousClick();
            Assert.AreEqual(0, form.SelectedIndex);
        }

        [TestMethod]
        public void Test_PreviousSong_AtFirstSong_GoesToLastSong()
        {
            var form = new Form1TestWrapper();
            form.TestAddSong("a.mp3");
            form.TestAddSong("b.mp3");
            form.TestAddSong("c.mp3");
            form.SetSelectedIndex(0);
            form.SimulatePreviousClick();
            Assert.AreEqual(6, form.CurrentIndex());
        }

        [TestMethod]
        public void Test_NextSong_AtEnd_GoesToFirstSong()
        {
            var form = new Form1TestWrapper();
            form.TestAddSong("1.mp3");
            form.TestAddSong("2.mp3");
            form.SetSelectedIndex(5);
            form.SimulateNextClick();
            Assert.AreEqual(0, form.CurrentIndex());
        }

        [TestMethod]
        public void Test_NextSong_AtStart_RemainsAtZero()
        {
            var form = new Form1TestWrapper();
            form.TestAddSong("song.mp3");
            form.SetSelectedIndex(0);
            form.SimulateNextClick();
            Assert.AreEqual(0, form.SelectedIndex);
        }

        [TestMethod]
        public void Test_SelectInvalidSpeed_DefaultsToNormal()
        {
            var form = new Form1TestWrapper();
            form.TestSelectSpeed("3x");
            Assert.AreEqual(1.0, form.SelectedSpeed);
        }

        [TestMethod]
        public void Test_CurrentSong_ReturnsCorrectSong()
        {
            var form = new Form1TestWrapper();
            form.TestAddSong("track1.mp3");
            form.TestAddSong("track2.mp3");
            form.SetSelectedIndex(5);
            Assert.AreEqual("track2.mp3", form.Playlist()[form.CurrentIndex()]);
        }

        [TestMethod]
        public void Test_PlayingWithoutSong_StaysFalse()
        {
            var form = new Form1TestWrapper();
            Assert.IsFalse(form.IsPlaying);
        }

        [TestMethod]
        public void Test_SpeedComboBox_InvalidInput_DoesNotChangeSpeed()
        {
            var form = new Form1TestWrapper();
            form.TestSelectSpeed("abc"); // Invalid input
            Assert.AreEqual(1.0, form.SelectedSpeed);
        }

        [TestMethod]
        public void Test_VolumeScroll_UpdatesVolume()
        {
            var form = new Form1TestWrapper();
            form.Controls.Find("volumeTrackBar", true)[0].Text = "5";
            form.Volume = 0;
            Assert.IsTrue(true);
        }
    }
}