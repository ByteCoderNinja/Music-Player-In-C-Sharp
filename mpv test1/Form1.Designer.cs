namespace MpvPlayerUI
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.ComboBox comboSpeed;
        private System.Windows.Forms.Button btnAddSong;
        private System.Windows.Forms.ListBox listBoxSongs;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.comboSpeed = new System.Windows.Forms.ComboBox();
            this.btnAddSong = new System.Windows.Forms.Button();
            this.listBoxSongs = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.volumeTrackBar = new System.Windows.Forms.TrackBar();
            this.volumeLabel = new System.Windows.Forms.Label();
            this.trackBarSong = new System.Windows.Forms.TrackBar();
            this.timeLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.volumeTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSong)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(784, 100);
            this.panel1.TabIndex = 0;
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(12, 361);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(90, 30);
            this.btnPause.TabIndex = 7;
            this.btnPause.Text = "Pause";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(210, 361);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(90, 30);
            this.btnNext.TabIndex = 5;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrevious
            // 
            this.btnPrevious.Location = new System.Drawing.Point(110, 361);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(90, 30);
            this.btnPrevious.TabIndex = 6;
            this.btnPrevious.Text = "Previous";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // comboSpeed
            // 
            this.comboSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSpeed.FormattingEnabled = true;
            this.comboSpeed.Items.AddRange(new object[] {
            "0.5x",
            "1.0x",
            "1.5x",
            "2.0x"});
            this.comboSpeed.Location = new System.Drawing.Point(320, 365);
            this.comboSpeed.Name = "comboSpeed";
            this.comboSpeed.Size = new System.Drawing.Size(90, 24);
            this.comboSpeed.TabIndex = 4;
            this.comboSpeed.SelectedIndexChanged += new System.EventHandler(this.comboSpeed_SelectedIndexChanged);
            // 
            // btnAddSong
            // 
            this.btnAddSong.Location = new System.Drawing.Point(430, 361);
            this.btnAddSong.Name = "btnAddSong";
            this.btnAddSong.Size = new System.Drawing.Size(90, 30);
            this.btnAddSong.TabIndex = 0;
            this.btnAddSong.Text = "Add Song";
            this.btnAddSong.UseVisualStyleBackColor = true;
            this.btnAddSong.Click += new System.EventHandler(this.btnAddSong_Click);
            // 
            // listBoxSongs
            // 
            this.listBoxSongs.FormattingEnabled = true;
            this.listBoxSongs.ItemHeight = 16;
            this.listBoxSongs.Location = new System.Drawing.Point(12, 120);
            this.listBoxSongs.Name = "listBoxSongs";
            this.listBoxSongs.Size = new System.Drawing.Size(784, 132);
            this.listBoxSongs.TabIndex = 6;
            this.listBoxSongs.SelectedIndexChanged += new System.EventHandler(this.listBoxSongs_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(706, 359);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(90, 30);
            this.button1.TabIndex = 8;
            this.button1.Text = "Help";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // timer
            // 
            this.timer.Interval = 500;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // volumeTrackBar
            // 
            this.volumeTrackBar.Location = new System.Drawing.Point(548, 345);
            this.volumeTrackBar.Name = "volumeTrackBar";
            this.volumeTrackBar.Size = new System.Drawing.Size(104, 56);
            this.volumeTrackBar.TabIndex = 9;
            this.volumeTrackBar.Value = 10;
            this.volumeTrackBar.Scroll += new System.EventHandler(this.volumeTrackBar_Scroll);
            // 
            // volumeLabel
            // 
            this.volumeLabel.AutoSize = true;
            this.volumeLabel.Location = new System.Drawing.Point(568, 385);
            this.volumeLabel.Name = "volumeLabel";
            this.volumeLabel.Size = new System.Drawing.Size(84, 16);
            this.volumeLabel.TabIndex = 10;
            this.volumeLabel.Text = "Volum: 100%";
            // 
            // trackBarSong
            // 
            this.trackBarSong.Location = new System.Drawing.Point(12, 283);
            this.trackBarSong.Maximum = 1000;
            this.trackBarSong.Name = "trackBarSong";
            this.trackBarSong.Size = new System.Drawing.Size(784, 56);
            this.trackBarSong.TabIndex = 11;
            this.trackBarSong.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarSong.Scroll += new System.EventHandler(this.trackBarSong_Scroll);
            // 
            // timeLabel
            // 
            this.timeLabel.AutoSize = true;
            this.timeLabel.Location = new System.Drawing.Point(32, 322);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(113, 16);
            this.timeLabel.TabIndex = 12;
            this.timeLabel.Text = "00:00:00 / 00:00:00";
            this.timeLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.timeLabel.UseMnemonic = false;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(808, 410);
            this.Controls.Add(this.timeLabel);
            this.Controls.Add(this.trackBarSong);
            this.Controls.Add(this.volumeLabel);
            this.Controls.Add(this.volumeTrackBar);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnAddSong);
            this.Controls.Add(this.comboSpeed);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrevious);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.listBoxSongs);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Mpv Music Player";
            ((System.ComponentModel.ISupportInitialize)(this.volumeTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSong)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.TrackBar volumeTrackBar;
        private System.Windows.Forms.Label volumeLabel;
        private System.Windows.Forms.TrackBar trackBarSong;
        private System.Windows.Forms.Label timeLabel;
    }
}
