using System;
using System.Windows.Forms;
using System.Windows.Media;

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
            this.volumeLabel = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnSavePlaylist = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackbarVolume)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOpen
            // 
            this.btnOpen.BackColor = System.Drawing.Color.Transparent;
            this.btnOpen.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnOpen.Location = new System.Drawing.Point(12, 39);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "Open";
            this.btnOpen.UseVisualStyleBackColor = false;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.BackColor = System.Drawing.Color.Transparent;
            this.btnPlay.Enabled = false;
            this.btnPlay.FlatAppearance.BorderSize = 0;
            this.btnPlay.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPlay.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlay.Image = global::Player.Properties.Resources.PlayButton;
            this.btnPlay.Location = new System.Drawing.Point(93, 39);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(23, 23);
            this.btnPlay.TabIndex = 1;
            this.btnPlay.UseVisualStyleBackColor = false;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnPause
            // 
            this.btnPause.BackColor = System.Drawing.Color.Transparent;
            this.btnPause.Enabled = false;
            this.btnPause.FlatAppearance.BorderSize = 0;
            this.btnPause.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPause.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPause.Image = global::Player.Properties.Resources.PauseButton;
            this.btnPause.Location = new System.Drawing.Point(132, 39);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(23, 23);
            this.btnPause.TabIndex = 2;
            this.btnPause.UseVisualStyleBackColor = false;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.Transparent;
            this.btnStop.Enabled = false;
            this.btnStop.FlatAppearance.BorderSize = 0;
            this.btnStop.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnStop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Image = global::Player.Properties.Resources.StopButton;
            this.btnStop.Location = new System.Drawing.Point(171, 40);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(23, 22);
            this.btnStop.TabIndex = 3;
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnAddToPlaylist
            // 
            this.btnAddToPlaylist.BackColor = System.Drawing.Color.Transparent;
            this.btnAddToPlaylist.FlatAppearance.BorderSize = 0;
            this.btnAddToPlaylist.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAddToPlaylist.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAddToPlaylist.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddToPlaylist.Image = global::Player.Properties.Resources.AddButton;
            this.btnAddToPlaylist.Location = new System.Drawing.Point(325, 105);
            this.btnAddToPlaylist.Name = "btnAddToPlaylist";
            this.btnAddToPlaylist.Size = new System.Drawing.Size(35, 35);
            this.btnAddToPlaylist.TabIndex = 10;
            this.btnAddToPlaylist.UseVisualStyleBackColor = false;
            this.btnAddToPlaylist.Click += new System.EventHandler(this.addToPlaylist_Click);
            // 
            // btnDeleteFromPlaylist
            // 
            this.btnDeleteFromPlaylist.BackColor = System.Drawing.Color.Transparent;
            this.btnDeleteFromPlaylist.FlatAppearance.BorderSize = 0;
            this.btnDeleteFromPlaylist.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnDeleteFromPlaylist.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnDeleteFromPlaylist.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteFromPlaylist.Image = global::Player.Properties.Resources.RemoveButton;
            this.btnDeleteFromPlaylist.Location = new System.Drawing.Point(325, 140);
            this.btnDeleteFromPlaylist.Name = "btnDeleteFromPlaylist";
            this.btnDeleteFromPlaylist.Size = new System.Drawing.Size(35, 35);
            this.btnDeleteFromPlaylist.TabIndex = 11;
            this.btnDeleteFromPlaylist.UseVisualStyleBackColor = false;
            this.btnDeleteFromPlaylist.Click += new System.EventHandler(this.removeFromPlaylist_Click);
            // 
            // trackBar1
            // 
            this.trackBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar1.BackColor = System.Drawing.ColorTranslator.FromHtml("#F3393A");
            this.trackBar1.AutoSize = false;
            this.trackBar1.Location = new System.Drawing.Point(12, 68);
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
            this.lblPosition.BackColor = System.Drawing.Color.Transparent;
            this.lblPosition.Location = new System.Drawing.Point(3, 92);
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size(100, 23);
            this.lblPosition.TabIndex = 5;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(60, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(237, 21);
            this.comboBox1.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Device:";
            // 
            // trackbarVolume
            // 
            
            this.trackbarVolume.BackColor = System.Drawing.ColorTranslator.FromHtml("#F3393A");
            this.trackbarVolume.Location = new System.Drawing.Point(345, 13);
            this.trackbarVolume.Maximum = 100;
            this.trackbarVolume.Name = "trackbarVolume";
            this.trackbarVolume.Size = new System.Drawing.Size(220, 20);
            this.trackbarVolume.TickStyle = TickStyle.Both;
            this.trackbarVolume.TabIndex = 8;
            this.trackbarVolume.TickFrequency = 10;
            this.trackbarVolume.Scroll += new System.EventHandler(this.trackbarVolume_Scroll);
            this.trackbarVolume.ValueChanged += new System.EventHandler(this.trackbarVolume_ValueChanged);
            
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(95, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 9;
            // 
            // playlistBox
            // 
            this.playlistBox.BackColor = System.Drawing.ColorTranslator.FromHtml("#F3393A");
            this.playlistBox.Location = new System.Drawing.Point(365, 110);
            this.playlistBox.Name = "playlistBox";
            this.playlistBox.Size = new System.Drawing.Size(200, 134);
            this.playlistBox.TabIndex = 12;
            this.playlistBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.playlist_doubleClicked);
           
            // 
            // volumeLabel
            // 
            this.volumeLabel.AutoSize = true;
            this.volumeLabel.BackColor = System.Drawing.Color.Transparent;
            this.volumeLabel.Location = new System.Drawing.Point(300, 26);
            this.volumeLabel.Name = "volumeLabel";
            this.volumeLabel.Size = new System.Drawing.Size(45, 13);
            this.volumeLabel.TabIndex = 13;
            this.volumeLabel.Text = "Volume:";
            // 
            // btnSavePlaylist
            // 
            this.btnSavePlaylist.BackColor = System.Drawing.Color.Transparent;
            this.btnSavePlaylist.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSavePlaylist.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSavePlaylist.Location = new System.Drawing.Point(280, 221);
            this.btnSavePlaylist.Name = "btnSavePlaylist";
            this.btnSavePlaylist.Size = new System.Drawing.Size(75, 23);
            this.btnSavePlaylist.TabIndex = 14;
            this.btnSavePlaylist.Text = "Save Playlist";
            this.btnSavePlaylist.UseVisualStyleBackColor = false;
            this.btnSavePlaylist.Click += new System.EventHandler(this.savePlaylist_Click);
            // 
            // PlayerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Player.Properties.Resources.html_color_codes_color_tutorials_hero_00e10b1f;
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
            this.Controls.Add(this.volumeLabel);
            this.Controls.Add(this.btnSavePlaylist);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "PlayerWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Music Player";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackbarVolume)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void TrackbarVolume_Scroll(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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
        private System.Windows.Forms.Label volumeLabel;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnSavePlaylist;
    }
}

