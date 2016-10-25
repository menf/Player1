using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Player
{


    class tui 
    {
      
        
        private const int MF_BYCOMMAND = 0x00000000;
        public const int SC_MAXIMIZE = 0xF030;
        public const int SC_SIZE = 0xF000;
        Timer timer;
        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();


        private Dictionary<int, String> _menu;
        private Dictionary<ConsoleKey, String> _menuBar;
        private int _menuStartRow;
        
        private MMDevice _playerDevice;
        private Logic _musicPlayer;
        private DriveInfo[] _drives;
        private int vol;
        private Dictionary<int, String> _settingsMenu;
        Dictionary<int, string> _directoryMenu;

        public tui()
        {
            this._menu = new Dictionary<int, string>();
            this._menuBar = new Dictionary<ConsoleKey, string>();
            this._menuStartRow = 8;
            this._musicPlayer = new Logic();
            this._drives = DriveInfo.GetDrives();
            this._directoryMenu = new Dictionary<int, string>();
            this._settingsMenu = new Dictionary<int, string>();
            this.vol = 100;
            timer = new Timer(Timer, null, 0, 1000);
        }



#region Menu

        private void loadMenus()
        {
            _menu.Add(0, "Play");
            _menu.Add(1, "Pause");
            _menu.Add(2, "Stop");
            _menu.Add(3, "Volume: " + _musicPlayer.Volume +"%");
            _menuBar.Add(ConsoleKey.F1, "File (F1)");
            _menuBar.Add(ConsoleKey.F2, "Device (F2)");
            _menuBar.Add(ConsoleKey.F3, "Menu (F3)");
            _menuBar.Add(ConsoleKey.F4, "Settings (F4)");

            _settingsMenu.Add(0, "Kolor Tła");
            _settingsMenu.Add(1, "Nieaktywny Kolor Czcionki");
            _settingsMenu.Add(2, "Kolor Podświetlenia");
            _settingsMenu.Add(3, "Aktywny Kolor Czcionki");
            _settingsMenu.Add(4, "Kolor Timera");
            _settingsMenu.Add(5, "Kolor Paska Menu");
            _settingsMenu.Add(6, "Kolor Czcionki Paska Menu");
            _settingsMenu.Add(7, "Kolor Czcionki Menu Głównego");
        }

        public void loadInterface()
        {
            this.loadMenus();
            Console.SetWindowSize(54, 16);
            Console.SetBufferSize(54, 16);
            Console.Title = "Music Player";
            IntPtr handle = GetConsoleWindow();
            IntPtr sysMenu = GetSystemMenu(handle, false);

            if (handle != IntPtr.Zero)
            {
                DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);
            }
            Console.CursorVisible = false;
     
            selectDevice();
            this.mainMenu(); 
        }
        
        private void refreshMenuBar()
        {
            Console.SetCursorPosition(0, 0);
            Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.barcolor;
            Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.barfontcolor;
            foreach (var item in _menuBar)
            {
                if (item.Key == ConsoleKey.F4)
                    Console.Write(item.Value);
                else
                    Console.Write(item.Value + "    ");
            }
            Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.backgroud;
            Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.foreground;

        }

        private void refreshMenu()
        {
 
            refreshMenuBar();


            Console.SetCursorPosition(0, _menuStartRow);
            Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.prompt;
            Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.promtfontcolor;


            foreach (var item in _menu)
            {

                Console.WriteLine(item.Value);
                Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.backgroud;
                Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.foreground;
            }
            Console.SetCursorPosition(0, _menuStartRow);
        }



        private void clearLine()
        {
            int c = Console.CursorTop;
            Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.backgroud;
            Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.foreground;
            Console.CursorLeft = 0;
            Console.Write(new String(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, c);
        }

        private void clearMenu(int i)
        {
            
            for (int x = 0; x < i; x++)
            {
                Console.CursorTop = _menuStartRow + x;
                clearLine();
            }
        }

        private void mainMenu()
        {

            refreshMenu();
            ConsoleKey key;
            do
            {
                key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        upMenu(_menu, _menuStartRow);
                        break;

                    case ConsoleKey.DownArrow:
                        downMenu(_menu, _menuStartRow);
                        break;
                    case ConsoleKey.F1:
                        clearMenu(_menu.Count);
                        playMusic(selectFile());
                        clearSelectFile();
                        refreshMenu();
                    break;
                    case ConsoleKey.F2:
                        clearMenu(_menu.Count);
                        selectDevice();
                        refreshMenu();
                    break;
                    case ConsoleKey.F4:
                        clearMenu(_menu.Count);
                        settingsMenu();
                        refreshMenu();
                        break;
                    case ConsoleKey.OemPlus:
                    case ConsoleKey.Add:
                        updateVolume(10);
                    break;
                    case ConsoleKey.OemMinus:
                    case ConsoleKey.Subtract:
                        updateVolume(-10);
                        
                        break;
                    case ConsoleKey.Enter:
                        switch (Console.CursorTop - _menuStartRow)
                        {
                            case 0:
                                if (_musicPlayer.PlaybackState != PlaybackState.Playing)
                                {
                                    _musicPlayer.Play();

                                }
                            break;
                            case 1:
                                if (_musicPlayer.PlaybackState != PlaybackState.Paused)
                                {
                                    _musicPlayer.Pause();

                                }

                            break;
                            case 2:
                                if (_musicPlayer.PlaybackState != PlaybackState.Stopped)
                                {
                                    _musicPlayer.Stop();
                                    _musicPlayer.Position = TimeSpan.Zero;
                                }
                                break;
                        }
                    break;
                    


                }
            } while (key != ConsoleKey.X);
        }

        private void clearSettingsMenu(int i)
        {
            for (int x = 0; x < i; x++)
            {
                Console.CursorTop = _menuStartRow - 3 + x;
                clearLine();
            }
        }


        private void refreshSettingsMenu()
        {
            Console.SetCursorPosition(0, _menuStartRow - 3);
            Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.prompt;
            Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.promtfontcolor;


            foreach (var item in _settingsMenu)
            {

                Console.WriteLine(item.Value);
                Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.backgroud;
                Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.foreground;
            }

            Console.SetCursorPosition(0, _menuStartRow - 3);
        }

        private void settingsMenu()
        {

            refreshSettingsMenu();
            ConsoleKey key;
            do
            {
                key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        upMenu(_settingsMenu, _menuStartRow-3);
                        break;

                    case ConsoleKey.DownArrow:
                        downMenu(_settingsMenu, _menuStartRow-3);
                        break;
                    case ConsoleKey.F1:
                        clearSettingsMenu(_settingsMenu.Count);
                        playMusic(selectFile());
                        clearSelectFile();
                        refreshSettingsMenu();
                        break;
                    case ConsoleKey.F2:
                        clearSettingsMenu(_settingsMenu.Count);
                        selectDevice();
                        refreshSettingsMenu();
                        break;
           
                    case ConsoleKey.Enter:
                        switch (Console.CursorTop - _menuStartRow)
                        {
                            case 0:
                               
                                break;
                            case 1:
                              

                                break;
                            case 2:
                               
                                break;
                            case 3:

                                break;
                            case 4:

                                break;
                            case 5:

                                break;
                            case 6:

                                break;
                            case 7:

                                break;
                        }
                        break;



                }
            } while (key != ConsoleKey.X);
        }


        private void upMenu(Dictionary<int, String> menu, int start)
        {
            if (Console.CursorTop > start)
            {
                Console.SetCursorPosition(0, (Console.CursorTop - 1));
                Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.prompt;
                Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.promtfontcolor;
                Console.WriteLine(menu[Console.CursorTop - start]);
                Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.backgroud;
                Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.foreground;
                Console.WriteLine(menu[Console.CursorTop - start]);
                Console.SetCursorPosition(0, (Console.CursorTop - 2));
            }
        }

        private void upMenu(string[] menu, int start)
        {
            if (Console.CursorTop > start)
            {
                Console.SetCursorPosition(0, (Console.CursorTop - 1));
                Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.prompt;
                Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.promtfontcolor;
                Console.WriteLine(menu[Console.CursorTop - start]);
                Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.backgroud;
                Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.foreground;
                Console.WriteLine(menu[Console.CursorTop - start]);
                Console.SetCursorPosition(0, (Console.CursorTop - 2));
            }
        }




        private void downMenu(Dictionary<int, String> menu, int start)
        {
            if (Console.CursorTop < (start + menu.Count - 1))
            {
                Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.backgroud;
                Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.foreground;

                Console.WriteLine(menu[Console.CursorTop - start]);
                Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.prompt;
                Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.promtfontcolor;

                Console.WriteLine(menu[Console.CursorTop - start]);
                Console.SetCursorPosition(0, (Console.CursorTop - 1));
            }
        }

        private void downMenu(string[] menu, int start)
        {
            if (Console.CursorTop < (start + menu.Length - 1))
            {
                Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.backgroud;
                Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.foreground;

                Console.WriteLine(menu[Console.CursorTop - start]);
                Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.prompt;
                Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.promtfontcolor;

                Console.WriteLine(menu[Console.CursorTop - start]);
                Console.SetCursorPosition(0, (Console.CursorTop - 1));
            }
        }








        #endregion

     



        private ConsoleKey selectDevice()
        {

            Console.SetCursorPosition(0, _menuStartRow);
            ObservableCollection<MMDevice> _devices = new ObservableCollection<MMDevice>();
            int nbr = 0;
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

                if (_playerDevice != null)
                {
                    Console.WriteLine("Aktualne urządzenie:");
                    Console.WriteLine(_playerDevice);
                    Console.Write(new String(' ',Console.BufferWidth));
                    nbr += 3;
                }

                for (int i = 0; i < _devices.Count; i++)
                {
                    Console.WriteLine(i + " " + _devices[i]);
                    nbr++;
                }
                

                Console.WriteLine("Wybierz nr urządzenia: ");
                ConsoleKeyInfo key;
                int option = -1;
                nbr = Console.CursorTop - _menuStartRow;

                do
                {
                    key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.F3)
                    {
                        clearMenu(nbr);
                        return key.Key;
                    }
                    if(key.Key == ConsoleKey.F1 && _playerDevice != null)
                    {
                        clearMenu(nbr);
                        this.selectFile();
                        clearSelectFile();
                        return ConsoleKey.F3;
                    }
                    try
                    {
                        option = Int32.Parse(Char.ToString(key.KeyChar));
                    }
                    catch (Exception e) { };
                    if (option >= 0 && option < _devices.Count)
                    {
                        _playerDevice = _devices[option];
                        clearMenu(nbr);
                        return ConsoleKey.F3;
                    }
                        


                } while (key.Key != ConsoleKey.X);
            }
            clearMenu(nbr);
            return ConsoleKey.X;
        }

 

        private void updateVolume(int v)
        {

            if (Console.CursorTop == _menuStartRow + 3)
            {
                Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.prompt;
                Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.promtfontcolor;
            }
            else
            {
                Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.backgroud;
                Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.foreground;
            }
            int current = Console.CursorTop;
            vol = _musicPlayer.Volume + v;
            _musicPlayer.Volume = vol;
            _menu[3] = "Volume: " + _musicPlayer.Volume + "%";
            Console.SetCursorPosition(0,(_menuStartRow + 3));
            clearLine();
            if (current == _menuStartRow + 3)
            {
                Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.prompt;
                Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.promtfontcolor;
            }
            Console.WriteLine(_menu[3]);
            Console.SetCursorPosition(0, current);
                
        }

        private void playMusic(String filePath)
        {

            if (_playerDevice != null)
            {

                try
                {
                    _musicPlayer.Open(filePath, _playerDevice);
                    if (_musicPlayer.PlaybackState != PlaybackState.Playing)
                    {

                        
                        _musicPlayer.Name = Path.GetFileNameWithoutExtension(filePath);
                        _musicPlayer.Play();

                    }
                }
                catch (Exception e)
                {
                    clearSelectFile();
                    Console.SetCursorPosition(0, 4);
                    Console.WriteLine("Nie można odnaleźć pliku");
                    Console.ReadKey(true);
                }
            }
            }

        private  void Timer(object state)
        {
            _musicPlayer.Volume = vol;
            if (_musicPlayer.PlaybackState == PlaybackState.Playing)
            {
                TimeSpan position = _musicPlayer.Position;
                TimeSpan length = _musicPlayer.Length;
                
                if (position > length)
                    length = position;
                int l = Console.CursorLeft;
                int t = Console.CursorTop;
                Console.SetCursorPosition(0, 2);
                Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.foreground;
                Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.backgroud;
                Console.Write(_musicPlayer.Name+"   ");
                Console.ForegroundColor =(ConsoleColor) Properties.Settings.Default.timercolor;
                Console.WriteLine(String.Format(@"<{0:mm\:ss}/{1:mm\:ss}>", _musicPlayer.Position, _musicPlayer.Length));
                Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.foreground;
                Console.CursorTop = t;
                Console.CursorLeft = l;

            }

        }

        private void clearSelectFile()
        {
            Console.SetCursorPosition(0, 4);
            clearLine();
            Console.CursorTop = 5;
            clearLine();
        }

        private String selectFile()
        {

            Console.SetCursorPosition(0, 4);
            Console.WriteLine("Podaj scieżkę do pliku:");
            return Console.ReadLine();
            

        }
       
      
    }
}
