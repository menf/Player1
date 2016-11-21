using CSCore;
using CSCore.Codecs;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Resources;

namespace Player
{
    public class Logic : Component
    {

        private ISoundOut _soundout;
        private IWaveSource _wavesource;
        private Dictionary<String,String> _playlist;
        private string name;
        
        public event EventHandler<PlaybackStoppedEventArgs> PlaybackStopped;



        public Logic()
        {
            _playlist = new Dictionary<String,String>();
            string line;
            StreamReader file = new StreamReader(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/Resources/playlist.cyk");
            while ((line = file.ReadLine()) != null)
            {
                _playlist.Add(Path.GetFileNameWithoutExtension(line), line);
            }
            file.Close();

        }

        public PlaybackState PlaybackState
        {
            get
            {
                if (_soundout != null)
                    return _soundout.PlaybackState;
                return PlaybackState.Stopped;
            }
        }

        public TimeSpan Length
        {
            get
            {
                if (_wavesource != null)
                    return _wavesource.GetLength();
                return TimeSpan.Zero;
            }
        }

        public string Name
        {
            get
            {
                if (name != null)
                    return name;
                return "";
            }
            set
            {
                name = value;
            }
        }

        public TimeSpan Position
        {
            get
            {
                if (_wavesource != null)
                    return _wavesource.GetPosition();
                return TimeSpan.Zero;
            }
            set
            {
                if (_wavesource != null)
                    _wavesource.SetPosition(value);
            }
        }

        public int Volume
        {
            get
            {
                if (_soundout != null)
                    return Math.Min(100, Math.Max((int)(_soundout.Volume * 100), 0));
                return 100;
            }
            set
            {
                if (_soundout != null)
                {
                    _soundout.Volume = Math.Min(1.0f, Math.Max(value / 100f, 0f));
                }
            }
        }

        public void Play()
        {
            if (_soundout != null)
                _soundout.Play();  
        }

        public void Pause()
        {
            if (_soundout != null)
                _soundout.Pause();
        }

        public void Stop()
        {
            if (_soundout != null)
                _soundout.Stop();
        }


        public void Open(string filename, MMDevice device)
        {
            CleanupPlayback();
            
            _wavesource =
                CodecFactory.Instance.GetCodec(filename)
                    .ToSampleSource()
                    .ToMono()
                    .ToWaveSource();
            _soundout = new WasapiOut() { Latency = 100, Device = device };
            _soundout.Initialize(_wavesource);
            if (PlaybackStopped != null) _soundout.Stopped += PlaybackStopped;

            

        }

        private void CleanupPlayback()
        {
            if (_soundout != null)
            {
                _soundout.Dispose();
                _soundout = null;
            }
            if (_wavesource != null)
            {
                _wavesource.Dispose();
                _wavesource = null;
            }
        }



        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            CleanupPlayback();
        }



        public Dictionary<String,String> getPlaylist()
        {
            return this._playlist;
        }

        //dodawanie do playlisty
        public bool addToPlaylist(string newSong, string filepath)
        {
            if (_playlist.ContainsKey(newSong))
            {
                return false;
            }
            _playlist.Add(newSong,filepath);
            return true;
            
        }

        //usuwanie z playlisty
        public bool removeFromPlaylist(string removeSong)
        {
            if (!_playlist.ContainsKey(removeSong))
            {
                return false;
            }
            _playlist.Remove(removeSong);
            return true;
        }

    }
}
