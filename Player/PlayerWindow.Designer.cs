namespace Player
{
    partial class PlayerWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnAddToPlaylist = new System.Windows.Forms.Button();
            this.btnDeleteFromPlaylist = new System.Windows.Forms.Button();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblPosition = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.trackbarVolume = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.playlistBox = new System.Windows.Forms.ListBox();
   
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackbarVolume)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(12, 39);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "Open";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.Enabled = false;
            this.btnPlay.Location = new System.Drawing.Point(93, 39);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(Properties.Resources.PlayButton.Width, Properties.Resources.PlayButton.Width);
            this.btnPlay.TabIndex = 1;
            this.btnPlay.Image = Properties.Resources.PlayButton;
            this.btnPlay.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.btnPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlay.FlatAppearance.BorderSize = 0;
            this.btnPlay.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPlay.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;

            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnPause
            // 
            this.btnPause.Enabled = false;
            this.btnPause.Location = new System.Drawing.Point(132, 39);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(Properties.Resources.PauseButton.Width, Properties.Resources.PauseButton.Width);
            this.btnPause.TabIndex = 2;
            this.btnPause.Image = Properties.Resources.PauseButton;
            this.btnPause.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.btnPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPause.FlatAppearance.BorderSize = 0;
            this.btnPause.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPause.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;

            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(171, 40);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(Properties.Resources.StopButton.Width, Properties.Resources.StopButton.Height);
            this.btnStop.TabIndex = 3;
            this.btnStop.Image = Properties.Resources.StopButton;
            this.btnStop.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.FlatAppearance.BorderSize = 0;
            this.btnStop.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnStop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;

            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // trackBar1
            // 
            this.trackBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar1.AutoSize = false;
            this.trackBar1.Location = new System.Drawing.Point(12, 69);
            this.trackBar1.Maximum = 1000;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(545, 20);
            this.trackBar1.TabIndex = 4;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            this.trackBar1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trackBar1_MouseDown);
            this.trackBar1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trackBar1_MouseUp);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblPosition
            // 
            this.lblPosition.Location = new System.Drawing.Point(12, 96);
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size(100, 23);
            this.lblPosition.TabIndex = 5;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(93, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(237, 21);
            this.comboBox1.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Device:";
            // 
            // trackbarVolume
            // 
            this.trackbarVolume.Location = new System.Drawing.Point(337, 16);
            this.trackbarVolume.Maximum = 100;
            this.trackbarVolume.Name = "trackbarVolume";
            this.trackbarVolume.Size = new System.Drawing.Size(220, 45);
            this.trackbarVolume.TabIndex = 8;
            this.trackbarVolume.TickFrequency = 10;
            
            this.trackbarVolume.ValueChanged += new System.EventHandler(this.trackbarVolume_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(103, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 9;
            //
            //AddToPlaylistButton
            //
            this.btnAddToPlaylist.Enabled = true;
            this.btnAddToPlaylist.Location = new System.Drawing.Point(380, 100);
            this.btnAddToPlaylist.Name = "btnAdd";
            this.btnAddToPlaylist.Size = new System.Drawing.Size(Properties.Resources.AddButton.Width+10, Properties.Resources.AddButton.Height+10);
            this.btnAddToPlaylist.TabIndex = 10;
            this.btnAddToPlaylist.Image = Properties.Resources.AddButton;
            this.btnAddToPlaylist.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.btnAddToPlaylist.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddToPlaylist.FlatAppearance.BorderSize = 0;
            this.btnAddToPlaylist.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAddToPlaylist.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;

            this.btnAddToPlaylist.UseVisualStyleBackColor = true;
            this.btnAddToPlaylist.Click += new System.EventHandler(this.btnStop_Click); //dodac handler

            //
            //RemoveFromPlaylistButton
            //
            this.btnDeleteFromPlaylist.Enabled = true;
            this.btnDeleteFromPlaylist.Location = new System.Drawing.Point(380, 135);
            this.btnDeleteFromPlaylist.Name = "btnRemove";
            this.btnDeleteFromPlaylist.Size = new System.Drawing.Size(Properties.Resources.RemoveButton.Width+10, Properties.Resources.RemoveButton.Height+10);
            this.btnDeleteFromPlaylist.TabIndex = 11;
            this.btnDeleteFromPlaylist.Image = Properties.Resources.RemoveButton;
            this.btnDeleteFromPlaylist.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.btnDeleteFromPlaylist.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteFromPlaylist.FlatAppearance.BorderSize = 0;
            this.btnDeleteFromPlaylist.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnDeleteFromPlaylist.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;

            this.btnDeleteFromPlaylist.UseVisualStyleBackColor = true;
            this.btnDeleteFromPlaylist.Click += new System.EventHandler(this.btnStop_Click); //dodac handler


            //
            // playlistBox
            //
            // this.playlistBox.DataSource = this._musicPlayer.getPlaylist();
            this.playlistBox.Location = new System.Drawing.Point(420, 100);
            this.playlistBox.Name = "playlistBox";
            this.playlistBox.Width = 150;
            this.playlistBox.Height = 150;
            this.playlistBox.TabIndex = 12;
            this.playlistBox.DataSource = getPlaylistSongNames();
           


            // 
            // PlayerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 250);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.trackbarVolume);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.lblPosition);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.playlistBox);
            this.Controls.Add(this.btnAddToPlaylist);
            this.Controls.Add(this.btnDeleteFromPlaylist);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PlayerWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Music Player";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackbarVolume)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();




        }

        #endregion

        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label lblPosition;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar trackbarVolume;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox playlistBox;
        private System.Windows.Forms.Button btnAddToPlaylist;
        private System.Windows.Forms.Button btnDeleteFromPlaylist;
    }
}

