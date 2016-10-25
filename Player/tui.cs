using Colorful;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
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
        private int vol;
        private Dictionary<int, String> _settingsMenu;

        public tui()
        {
            this._menu = new Dictionary<int, string>();
            this._menuBar = new Dictionary<ConsoleKey, string>();
            this._menuStartRow = 8;
            this._musicPlayer = new Logic();
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
            System.Console.SetWindowSize(54, 16);
            System.Console.SetBufferSize(54, 16);
            System.Console.Title = "Music Player";
            IntPtr handle = GetConsoleWindow();
            IntPtr sysMenu = GetSystemMenu(handle, false);

            if (handle != IntPtr.Zero)
            {
                DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);
            }
            System.Console.CursorVisible = false;
            greet();
            selectDevice();
            this.mainMenu(); 
        }
     private void   greet()
        {


        }
        private void refreshMenuBar()
        {
            System.Console.SetCursorPosition(0, 0);
            System.Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.barcolor;
            System.Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.barfontcolor;
            foreach (var item in _menuBar)
            {
                if (item.Key == ConsoleKey.F4)
                    System.Console.Write(item.Value);
                else
                    System.Console.Write(item.Value + "    ");
            }
            System.Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.backgroud;
            System.Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.foreground;

        }

        private void refreshMenu()
        {
 
            refreshMenuBar();


            System.Console.SetCursorPosition(0, _menuStartRow);
            System.Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.prompt;
            System.Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.promtfontcolor;


            foreach (var item in _menu)
            {

                System.Console.WriteLine(item.Value);
                System.Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.backgroud;
                System.Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.foreground;
            }
            System.Console.SetCursorPosition(0, _menuStartRow);
        }



        private void clearLine()
        {
            int c = System.Console.CursorTop;
            System.Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.backgroud;
            System.Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.foreground;
            System.Console.CursorLeft = 0;
            System.Console.Write(new String(' ', System.Console.BufferWidth));
            System.Console.SetCursorPosition(0, c);
        }

        private void clearMenu(int i)
        {
            
            for (int x = 0; x < i; x++)
            {
                System.Console.CursorTop = _menuStartRow + x;
                clearLine();
            }
        }

        private void mainMenu()
        {

            refreshMenu();
            ConsoleKey key;
            do
            {
                key = System.Console.ReadKey(true).Key;
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
                        switch (System.Console.CursorTop - _menuStartRow)
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
                System.Console.CursorTop = _menuStartRow - 3 + x;
                clearLine();
            }
        }


        private void refreshSettingsMenu()
        {
            System.Console.SetCursorPosition(0, _menuStartRow - 3);
            System.Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.prompt;
            System.Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.promtfontcolor;


            foreach (var item in _settingsMenu)
            {

                System.Console.WriteLine(item.Value);

                System.Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.backgroud;
                System.Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.foreground;
            }

            System.Console.SetCursorPosition(0, _menuStartRow - 3);
        }

        private void settingsMenu()
        {

            refreshSettingsMenu();
            ConsoleKey key;
            do
            {
                key = System.Console.ReadKey(true).Key;
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
           
                    case ConsoleKey.LeftArrow:
                        switch (System.Console.CursorTop - _menuStartRow)
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
            if (System.Console.CursorTop > start)
            {
                System.Console.SetCursorPosition(0, (System.Console.CursorTop - 1));
                System.Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.prompt;
                System.Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.promtfontcolor;
                System.Console.WriteLine(menu[System.Console.CursorTop - start]);
                System.Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.backgroud;
                System.Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.foreground;
                System.Console.WriteLine(menu[System.Console.CursorTop - start]);
                System.Console.SetCursorPosition(0, (System.Console.CursorTop - 2));
            }
        }

        private void upMenu(string[] menu, int start)
        {
            if (System.Console.CursorTop > start)
            {
                System.Console.SetCursorPosition(0, (System.Console.CursorTop - 1));
                System.Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.prompt;
                System.Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.promtfontcolor;
                System.Console.WriteLine(menu[System.Console.CursorTop - start]);
                System.Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.backgroud;
                System.Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.foreground;
                System.Console.WriteLine(menu[System.Console.CursorTop - start]);
                System.Console.SetCursorPosition(0, (System.Console.CursorTop - 2));
            }
        }




        private void downMenu(Dictionary<int, String> menu, int start)
        {
            if (System.Console.CursorTop < (start + menu.Count - 1))
            {
                System.Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.backgroud;
                System.Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.foreground;

                System.Console.WriteLine(menu[System.Console.CursorTop - start]);
                System.Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.prompt;
                System.Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.promtfontcolor;

                System.Console.WriteLine(menu[System.Console.CursorTop - start]);
                System.Console.SetCursorPosition(0, (System.Console.CursorTop - 1));
            }
        }

        private void downMenu(string[] menu, int start)
        {
            if (System.Console.CursorTop < (start + menu.Length - 1))
            {
                System.Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.backgroud;
                System.Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.foreground;

                System.Console.WriteLine(menu[System.Console.CursorTop - start]);
                System.Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.prompt;
                System.Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.promtfontcolor;

                System.Console.WriteLine(menu[System.Console.CursorTop - start]);
                System.Console.SetCursorPosition(0, (System.Console.CursorTop - 1));
            }
        }








        #endregion

     



        private ConsoleKey selectDevice()
        {

            System.Console.SetCursorPosition(0, _menuStartRow);
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
                    System.Console.WriteLine("Aktualne urządzenie:");
                    System.Console.WriteLine(_playerDevice);
                    System.Console.Write(new String(' ', System.Console.BufferWidth));
                    nbr += 3;
                }

                for (int i = 0; i < _devices.Count; i++)
                {
                    System.Console.WriteLine(i + " " + _devices[i]);
                    nbr++;
                }


                System.Console.WriteLine("Wybierz nr urządzenia: ");
                System.ConsoleKeyInfo key;
                int option = -1;
                nbr = System.Console.CursorTop - _menuStartRow;

                do
                {
                    key = System.Console.ReadKey(true);
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

            if (System.Console.CursorTop == _menuStartRow + 3)
            {
                System.Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.prompt;
                System.Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.promtfontcolor;
            }
            else
            {
                System.Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.backgroud;
                System.Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.foreground;
            }
            int current = System.Console.CursorTop;
            vol = _musicPlayer.Volume + v;
            _musicPlayer.Volume = vol;
            _menu[3] = "Volume: " + _musicPlayer.Volume + "%";
            System.Console.SetCursorPosition(0,(_menuStartRow + 3));
            clearLine();
            if (current == _menuStartRow + 3)
            {
                System.Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.prompt;
                System.Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.promtfontcolor;
            }
            System.Console.WriteLine(_menu[3]);
            System.Console.SetCursorPosition(0, current);
                
        }

        private void playMusic(String filePath)
        {

            if (_playerDevice != null)
            {
                if (File.Exists(filePath))
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
                        System.Console.SetCursorPosition(0, 4);
                        System.Console.WriteLine("Nie można odnaleźć pliku");
                        System.Console.ReadKey(true);
                    }
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
                int l = System.Console.CursorLeft;
                int t = System.Console.CursorTop;
                System.Console.SetCursorPosition(0, 2);
                System.Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.foreground;
                System.Console.BackgroundColor = (ConsoleColor)Properties.Settings.Default.backgroud;
                System.Console.Write(_musicPlayer.Name+"   ");
                System.Console.ForegroundColor =(ConsoleColor) Properties.Settings.Default.timercolor;
                System.Console.WriteLine(String.Format(@"<{0:mm\:ss}/{1:mm\:ss}>", _musicPlayer.Position, _musicPlayer.Length));
                System.Console.ForegroundColor = (ConsoleColor)Properties.Settings.Default.foreground;
                System.Console.CursorTop = t;
                System.Console.CursorLeft = l;

            }

        }

        private void clearSelectFile()
        {
            System.Console.SetCursorPosition(0, 4);
            clearLine();
            System.Console.CursorTop = 5;
            clearLine();
        }

        private String selectFile()
        {

            System.Console.SetCursorPosition(0, 4);
            System.Console.WriteLine("Podaj scieżkę do pliku:");
            return System.Console.ReadLine();
            

        }
       
      
    }
}
