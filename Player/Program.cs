using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore.Codecs;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
using CSCore.Streams;
using System.Collections.ObjectModel;

namespace Player
{
    class Program
    {

        static void Main(string[] args)
        {

            /// testy dzialania

            Logic _musicPlayer = new Logic();
            ObservableCollection<MMDevice> _devices = new ObservableCollection<MMDevice>();
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
                for(int i=0;i<_devices.Count;i++)
                {
                    Console.WriteLine(i + " " + _devices[i]);
                }
                Console.WriteLine("wybierz nr urządzenia");
                int option = Int32.Parse(Console.ReadLine());
                Console.WriteLine("Podaj sciezke do pliku");
                _musicPlayer.Open(Console.ReadLine(), _devices[option]);
                if (_musicPlayer.PlaybackState != PlaybackState.Playing)
                {
                    _musicPlayer.Play();
                }

                }

            Console.ReadLine();
        }
    }
}

