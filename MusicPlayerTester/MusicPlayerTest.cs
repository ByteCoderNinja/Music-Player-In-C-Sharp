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
            Assert.AreEqual(1, form.SongsCount);
        }
        [TestMethod]
        public void Test_NextSong_ChangesSelection()
        {
            var form = new Form1();
            form.TestAddSong("s1.mp3");
            form.TestAddSong("s2.mp3");
            form.SetSelectedIndex(0);

            form.SimulateNextClick();
            Assert.AreEqual(1, form.SelectedIndex);
        }
        [TestMethod]
        public void Test_SpeedChange_SetsCorrectSpeed()
        {
            var form = new Form1();
            form.TestSelectSpeed("1.5x");
            Assert.AreEqual(1.5, form.SelectedSpeed);
        }
        [TestMethod]
        public void Test_Pause_TogglesPlayFlag()
        {
            var form = new Form1();
            form.TestSetPlaying(true);
            form.SimulatePauseClick();
            Assert.IsTrue(form.IsPlaying);
        }

    }
}
