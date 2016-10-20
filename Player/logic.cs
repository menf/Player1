using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore;
using CSCore.Codecs;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
namespace Player
{
   public class Logic : Component
    {
        private ISoundOut _soundout;
        private IWaveSource _wavesource;

        public event EventHandler<PlaybackStoppedEventArgs> PlaybackStopped;

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


    }
}
