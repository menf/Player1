using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using CSCore.Codecs;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
using CSCore.Streams;
using Player;
using System.IO;
using System.Collections.Generic;

namespace Player
{
    public partial class PlayerWindow : Form
    {
        private readonly Logic _musicPlayer = new Logic();
        private bool _stopSliderUpdate;
        private readonly ObservableCollection<MMDevice> _devices = new ObservableCollection<MMDevice>();
        private List<string> _shownList = new List<string>();


        public PlayerWindow()
        {
            InitializeComponent();
            components = new Container();
            components.Add(_musicPlayer);
            _musicPlayer.PlaybackStopped += (s, args) =>
            {
                //WasapiOut uses SynchronizationContext.Post to raise the event
                //There might be already a new WasapiOut-instance in the background when the async Post method brings the PlaybackStopped-Event to us.
                if (_musicPlayer.PlaybackState != PlaybackState.Stopped)
                    btnPlay.Enabled = btnStop.Enabled = btnPause.Enabled = false;
            };
            this.playlistBox.DataSource = this.getPlaylistSongNames();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = CodecFactory.SupportedFilesFilterEn
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    
                    _musicPlayer.Open(openFileDialog.FileName, (MMDevice)comboBox1.SelectedItem);
                    _musicPlayer.Volume = trackbarVolume.Value;
                    _musicPlayer.Name = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                    btnPlay.Enabled = true;
                    btnPause.Enabled = btnStop.Enabled = false;
                    label2.Text =_musicPlayer.Name;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not open file: " + ex.Message);
                }
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (_musicPlayer.PlaybackState != PlaybackState.Playing)
            {
                _musicPlayer.Play();
                btnPlay.Enabled = false;
                btnPause.Enabled = btnStop.Enabled = true;
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (_musicPlayer.PlaybackState == PlaybackState.Playing)
            {
                _musicPlayer.Pause();
                btnPause.Enabled = false;
                btnPlay.Enabled = btnStop.Enabled = true;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (_musicPlayer.PlaybackState != PlaybackState.Stopped)
            {
                _musicPlayer.Stop();
                _musicPlayer.Position = TimeSpan.Zero;
                btnPlay.Enabled = true;
                btnStop.Enabled = btnPause.Enabled = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan position = _musicPlayer.Position;
            TimeSpan length = _musicPlayer.Length;
            if (position > length)
                length = position;

            lblPosition.Text = String.Format(@"{0:mm\:ss} / {1:mm\:ss}", position, length);

            if (!_stopSliderUpdate &&
                length != TimeSpan.Zero && position != TimeSpan.Zero)
            {
                double perc = position.TotalMilliseconds / length.TotalMilliseconds * trackBar1.Maximum;
                trackBar1.Value = (int)perc;
            }

        }

       


        private void trackBar1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _stopSliderUpdate = true;
            }
            if (!btnPlay.Enabled)
            {
                _musicPlayer.Pause();
            }
                
        }

        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _stopSliderUpdate = false;
            }
            if (!btnPlay.Enabled)
            {
                _musicPlayer.Play();
            }

        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (_stopSliderUpdate)
            {
                double perc = trackBar1.Value / (double)trackBar1.Maximum;
                TimeSpan position = TimeSpan.FromMilliseconds(_musicPlayer.Length.TotalMilliseconds * perc);
                _musicPlayer.Position = position;
            }
        }




        private List<string> getPlaylistSongNames()
        {
            List<string> names = new List<string>();
            Dictionary<string, string> pl = _musicPlayer.getPlaylist();
            foreach (KeyValuePair<string, string> entry in pl)
            {
                names.Add(entry.Key);
            }
            return names;
        }



        private void addToPlaylist_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = CodecFactory.SupportedFilesFilterEn,
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach(String file in openFileDialog.FileNames)
                {
                    _musicPlayer.addToPlaylist(Path.GetFileNameWithoutExtension(file), file);
                    
                }
                this.playlistBox.DataSource = this.getPlaylistSongNames();
            }
        }

        private void removeFromPlaylist_Click(object sender, EventArgs e)
        {
            string curItem = playlistBox.SelectedItem.ToString();
            int index = playlistBox.SelectedIndex;
            _musicPlayer.removeFromPlaylist(curItem);
            playlistBox.DataSource = getPlaylistSongNames();
            if (index < playlistBox.Items.Count)
            {
                playlistBox.SelectedIndex = index;
            }  
            else
            {
                playlistBox.SelectedIndex = index - 1;
            }
        }


        private void playlist_doubleClicked(object sender, EventArgs e)
        {
            // Get the currently selected item in the ListBox.
            string curItem = playlistBox.SelectedItem.ToString();
            PlaybackState state = _musicPlayer.PlaybackState;
           
                _musicPlayer.Stop();
                _musicPlayer.Position = TimeSpan.Zero;
                btnPlay.Enabled = true;
                btnStop.Enabled = btnPause.Enabled = false;
           
            try
            {
                _musicPlayer.Open(_musicPlayer.getPlaylist()[curItem], (MMDevice)comboBox1.SelectedItem);              
                _musicPlayer.Name = curItem;
                label2.Text = _musicPlayer.Name;
                btnPlay.Enabled = true;
                btnPause.Enabled = btnStop.Enabled = false;
                _musicPlayer.Volume = trackbarVolume.Value;

                if (state != PlaybackState.Playing)
                {
                   
                        _musicPlayer.Play();
                        btnPlay.Enabled = false;
                        btnPause.Enabled = btnStop.Enabled = true;

                }
               

            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not open file: " + ex.Message);
            }
            
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            using (var mmdeviceEnumerator = new MMDeviceEnumerator())
            {
                using (
                    var mmdeviceCollection = mmdeviceEnumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active))
                {
                    foreach (var device in mmdeviceCollection)
                    {
                        _devices.Add(device);
                    }
                }
            }

            comboBox1.DataSource = _devices;
            comboBox1.DisplayMember = "FriendlyName";
            comboBox1.ValueMember = "DeviceID";
        }

        private void trackbarVolume_ValueChanged(object sender, EventArgs e)
        {
            _musicPlayer.Volume = trackbarVolume.Value;
        }

        private void trackbarVolume_Scroll(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(trackbarVolume, (trackbarVolume.Value.ToString() + "%"));
        }


        private void savePlaylist_Click(object sender, EventArgs e)
        {
            _musicPlayer.savePlaylist();
        }

        private void nextSong()
        {
            if(_musicPlayer.Position >= _musicPlayer.Length)
            {

                playlistBox.SelectedIndex++;
                if (playlistBox.SelectedIndex >= playlistBox.Items.Count)
                {
                    playlistBox.SelectedIndex = 0;
                }

                string curItem = playlistBox.SelectedItem.ToString();

                try
                {
                    _musicPlayer.Open(_musicPlayer.getPlaylist()[curItem], (MMDevice)comboBox1.SelectedItem);
                    _musicPlayer.Name = curItem;
                    label2.Text = _musicPlayer.Name;
                    _musicPlayer.Volume = trackbarVolume.Value;
                    _musicPlayer.Play();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not open file: " + ex.Message);
                }
            }
        }


    }
}
