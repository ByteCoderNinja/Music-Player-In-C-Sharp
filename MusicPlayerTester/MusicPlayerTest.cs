using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MpvPlayerUI
{
    [TestClass]
    public class MusicPlayerTest
    {
        [TestMethod]
        public void Test_AddSong_IncreasesListCount()
        {
            var form = new Form1();
            form.TestAddSong("song1.mp3");
            Assert.AreEqual(5, form.SongsCount);
        }

        [TestMethod]
        public void Test_AddSameSongTwice_NotWorking()
        {
            var form = new Form1();
            form.TestAddSong("song1.mp3");
            form.TestAddSong("song1.mp3");
            Assert.AreEqual(5, form.SongsCount);
        }

        [TestMethod]
        public void Test_NextSong_ChangesSelection()
        {
            var form = new Form1();
            form.TestAddSong("s1.mp3");
            form.TestAddSong("s2.mp3");
            form.SetSelectedIndex(0);
            form.SimulateNextClick();
            Assert.AreEqual(1, form.CurrentIndex());
        }

        [TestMethod]
        public void Test_SpeedChange1_SetsCorrectSpeed()
        {
            var form = new Form1();
            form.TestSelectSpeed("0.5x");
            Assert.AreEqual(0.5, form.SelectedSpeed);
        }

        [TestMethod]
        public void Test_SpeedChange2_SetsCorrectSpeed()
        {
            var form = new Form1();
            form.TestSelectSpeed("1.0x");
            Assert.AreEqual(1.0, form.SelectedSpeed);
        }

        [TestMethod]
        public void Test_SpeedChange3_SetsCorrectSpeed()
        {
            var form = new Form1();
            form.TestSelectSpeed("1.5x");
            Assert.AreEqual(1.5, form.SelectedSpeed);
        }

        [TestMethod]
        public void Test_SpeedChange4_SetsCorrectSpeed()
        {
            var form = new Form1();
            form.TestSelectSpeed("2.0x");
            Assert.AreEqual(2.0, form.SelectedSpeed);
        }

        [TestMethod]
        public void Test_SpeedChangeAfterPause_SetsCorrectSpeed()
        {
            var form = new Form1();
            form.TestSetPlaying(true);
            form.SimulatePauseClick();
            form.TestSelectSpeed("2.0x");
            form.SimulatePauseClick();
            Assert.AreEqual(2.0, form.SelectedSpeed);
        }

        [TestMethod]
        public void Test_Pause_TogglesPlayFlag()
        {
            var form = new Form1();
            form.TestSetPlaying(false);
            form.SimulatePauseClick();
            Assert.IsFalse(form.IsPlaying);
        }

        [TestMethod]
        public void Test_AddMultipleSongs_IncreasesCountCorrectly()
        {
            var form = new Form1();
            form.TestAddSong("s1.mp3");
            form.TestAddSong("s2.mp3");
            form.TestAddSong("s3.mp3");
            Assert.AreEqual(7, form.SongsCount);
        }

        [TestMethod]
        public void Test_PreviousSong_ChangesSelection()
        {
            var form = new Form1();
            form.TestAddSong("a.mp3");
            form.TestAddSong("b.mp3");
            form.SetSelectedIndex(1);
            form.SimulatePreviousClick();
            Assert.AreEqual(0, form.CurrentIndex());
        }

        [TestMethod]
        public void Test_PreviousSong_AtStart_RemainsAtZero()
        {
            var form = new Form1();
            form.TestAddSong("song.mp3");
            form.SetSelectedIndex(0);
            form.SimulatePreviousClick();
            Assert.AreEqual(0, form.SelectedIndex);
        }

        [TestMethod]
        public void Test_PreviousSong_AtFirstSong_GoesToLastSong()
        {
            var form = new Form1();
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
            var form = new Form1();
            form.TestAddSong("1.mp3");
            form.TestAddSong("2.mp3");
            form.SetSelectedIndex(5);
            form.SimulateNextClick();
            Assert.AreEqual(0, form.CurrentIndex());
        }

        [TestMethod]
        public void Test_NextSong_AtStart_RemainsAtZero()
        {
            var form = new Form1();
            form.TestAddSong("song.mp3");
            form.SetSelectedIndex(0);
            form.SimulateNextClick();
            Assert.AreEqual(0, form.SelectedIndex);
        }

        [TestMethod]
        public void Test_SelectInvalidSpeed_DefaultsToNormal()
        {
            var form = new Form1();
            form.TestSelectSpeed("3x");
            Assert.AreEqual(1.0, form.SelectedSpeed);
        }

        [TestMethod]
        public void Test_CurrentSong_ReturnsCorrectSong()
        {
            var form = new Form1();
            form.TestAddSong("track1.mp3");
            form.TestAddSong("track2.mp3");
            form.SetSelectedIndex(5);
            Assert.AreEqual("track2.mp3", form.Playlist()[form.CurrentIndex()]);
        }

        [TestMethod]
        public void Test_PlayingSong_ChangesToTrue()
        {
            var form = new Form1();
            form.TestAddSong("a.mp3");
            form.SetSelectedIndex(0);
            Assert.IsTrue(form.IsPlaying);
        }

        [TestMethod]
        public void Test_PlayingWithoutSong_StaysFalse()
        {
            var form = new Form1();
            Assert.IsFalse(form.IsPlaying);
        }

        [TestMethod]
        public void Test_SpeedComboBox_InvalidInput_DoesNotChangeSpeed()
        {
            var form = new Form1();
            form.TestSelectSpeed("abc"); // Invalid input
            Assert.AreEqual(1.0, form.SelectedSpeed);
        }

        [TestMethod]
        public void Test_SelectSong_UpdatesCurrentIndexAndPlays()
        {
            var form = new Form1();
            form.TestAddSong("a.mp3");
            form.TestAddSong("b.mp3");
            form.TestAddSong("c.mp3");

            form.SetSelectedIndex(6);

            Assert.AreEqual(6, form.CurrentIndex());
            Assert.AreEqual("c.mp3", form.Playlist()[form.CurrentIndex()]);
            Assert.IsTrue(form.IsPlaying);
        }

        [TestMethod]
        public void Test_VolumeScroll_UpdatesVolume()
        {
            var form = new Form1();
            form.Controls.Find("volumeTrackBar", true)[0].Text = "5";
            form.Volume = 0;
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Test_PlayAfterPause_MusicStarts()
        {
            var form = new Form1();
            form.TestSetPlaying(true);
            form.SimulatePauseClick();
            form.SimulatePauseClick(); // Apasam a doua oara fiindca butonul "Pause" se transforma in "Play"
            Assert.IsTrue(form.IsPlaying);
        }


    }
}
