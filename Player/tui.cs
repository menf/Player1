using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Player
{


    class tui 
    {
        
        private const int MF_BYCOMMAND = 0x00000000;
        public const int SC_MAXIMIZE = 0xF030;
        public const int SC_SIZE = 0xF000;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();


        private Dictionary<int, String> _menu;
        private Dictionary<int, String> _playlistMenu;
        private Dictionary<ConsoleKey, String> _menuBar;
        private int _menuStartRow;
        private MMDevice _playerDevice;
        private Logic _musicPlayer;

        public tui()
        {
            this._menu = new Dictionary<int, string>();
            this._menuBar = new Dictionary<ConsoleKey, string>();
            this._playlistMenu = new Dictionary<int, string>();
            this._menuStartRow = 8;
            this._musicPlayer = new Logic();
        }



#region Menu

        private void loadMenus()
        {
            _menu.Add(0, "Playlist");
            _menu.Add(1, "Play");
            _menu.Add(2, "Pause");
            _menu.Add(3, "Stop");
            _menu.Add(4, "Volume: " + _musicPlayer.Volume +"%");
            _menuBar.Add(ConsoleKey.F1, "File (F1)");
            _menuBar.Add(ConsoleKey.F2, "Device (F2)");
            _menuBar.Add(ConsoleKey.F3, "Menu (F3)");

            _playlistMenu.Add(0, "Dodaj do playlisty");
            _playlistMenu.Add(1, "Usuń z playlisty");
            _playlistMenu.Add(2, "Wybierz utwór");
        }

        public void loadInterface()
        {
            this.loadMenus();
            Console.SetWindowSize(50, 35);
            Console.SetBufferSize(50, 35);
            IntPtr handle = GetConsoleWindow();
            IntPtr sysMenu = GetSystemMenu(handle, false);

            if (handle != IntPtr.Zero)
            {
                DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);
            }
            Console.CursorVisible = true;
            this.mainMenu();

        }

        private void refreshMenuBar()
        {
            Console.SetCursorPosition(0, 0);
            foreach (var item in _menuBar)
            {
                Console.Write(item.Value + "    ");
            }
        }

        private void refreshMenu()
        {
 
            refreshMenuBar();


            Console.SetCursorPosition(0, _menuStartRow);
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.ForegroundColor = ConsoleColor.Black;


            foreach (var item in _menu)
            {

                Console.WriteLine(item.Value);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.SetCursorPosition(0, _menuStartRow);
        }



        private void clearLine()
        {
            int c = Console.CursorTop;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
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
                        upMenu(_menu);
                        break;

                    case ConsoleKey.DownArrow:
                        downMenu(_menu);
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
                                clearMenu(_menu.Count);
                                playlistMenu();
                                refreshMenu();
                            break;
                            case 1:
                                if (_musicPlayer.PlaybackState != PlaybackState.Playing)
                                {
                                    _musicPlayer.Play();

                                }
                            break;
                            case 2:
                                if (_musicPlayer.PlaybackState != PlaybackState.Paused)
                                {
                                    _musicPlayer.Pause();

                                }

                            break;
                            case 3:
                                if (_musicPlayer.PlaybackState != PlaybackState.Stopped)
                                {
                                    _musicPlayer.Stop();
                                }
                                break;
                        }
                    break;
                    


                }
            } while (key != ConsoleKey.X);
        }

        

        private void upMenu(Dictionary<int, String> menu)
        {
            if (Console.CursorTop > _menuStartRow)
            {
                Console.SetCursorPosition(0, (Console.CursorTop - 1));
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine(menu[Console.CursorTop - _menuStartRow]);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(menu[Console.CursorTop - _menuStartRow]);
                Console.SetCursorPosition(0, (Console.CursorTop - 2));
            }
        }
      
        private void downMenu(Dictionary<int, String> menu)
        {
            if (Console.CursorTop < (_menuStartRow + menu.Count - 1))
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine(menu[Console.CursorTop - _menuStartRow]);
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.ForegroundColor = ConsoleColor.Black;

                Console.WriteLine(menu[Console.CursorTop - _menuStartRow]);
                Console.SetCursorPosition(0, (Console.CursorTop - 1));
            }
        }
        #endregion

        private void addToPlaylist()
        {
            String file = selectFile();
            clearSelectFile();
            if (File.Exists(file))
            {
                _musicPlayer.addToPlaylist(Path.GetFileName(file), file);
            }

        }

        private void removeFromPlaylist()
        {

        }

        private void showPlaylist()
        {
            Console.SetCursorPosition(0, (_menuStartRow + _playlistMenu.Count + 1));
            SortedDictionary<String, String> playlist = _musicPlayer.getPlaylist();
            if (playlist == null)
                Console.WriteLine("Playlista jest pusta.");
            foreach (var item in playlist)
            {

                Console.WriteLine(item.Key.ToString());
            }
        }



        private void refreshPlaylistMenu()
        {
            Console.SetCursorPosition(0, _menuStartRow);
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.ForegroundColor = ConsoleColor.Black;

            foreach (var item in _playlistMenu)
            {
                Console.WriteLine(item.Value);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine(new String('-', Console.BufferWidth));

            showPlaylist();

            Console.SetCursorPosition(0, _menuStartRow);
        }

        private void playlistMenu()
        {
            refreshPlaylistMenu();

            ConsoleKey key;
            do
            {
                key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        upMenu(_playlistMenu);
                        break;

                    case ConsoleKey.DownArrow:
                        downMenu(_playlistMenu);
                        break;
                    case ConsoleKey.F1:
                        clearMenu(_playlistMenu.Count + 1 + _musicPlayer.getPlaylist().Count);
                        playMusic(selectFile());
                        clearSelectFile();
                        return;
                    case ConsoleKey.F2:
                        clearMenu(_playlistMenu.Count + 1 + _musicPlayer.getPlaylist().Count);
                        key = selectDevice();
                        break;
                    case ConsoleKey.Enter:
                        switch (Console.CursorTop - _menuStartRow)
                        {
                            case 0:
                                //dodaj do playlisty
                                clearMenu(_playlistMenu.Count + 1 + _musicPlayer.getPlaylist().Count);
                                addToPlaylist();
                                refreshPlaylistMenu();
                                break;
                            case 1:
                                //usun z playlisty
                                refreshPlaylistMenu();
                                break;
                            case 2:
                                //wybierz utwor
                                refreshPlaylistMenu();
                                break;

                        }
                        break;



                }
            } while (key != ConsoleKey.F3);
        }

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

            if (Console.CursorTop == _menuStartRow + 4)
            {
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.ForegroundColor = ConsoleColor.Black;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }
            int current = Console.CursorTop;
            _musicPlayer.Volume += v;
            _menu[4] = "Volume: " + _musicPlayer.Volume + "%";
            Console.SetCursorPosition(0,(_menuStartRow + 4));
            clearLine();
            if (current == _menuStartRow + 4)
            {
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.ForegroundColor = ConsoleColor.Black;
            }
            Console.WriteLine(_menu[4]);
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
                        _musicPlayer.Play();

                    }
                }
                catch (Exception e)
                {
                    Console.Clear();
                    Console.WriteLine("Nie można odnaleźć pliku");
                    Console.ReadKey(true);
                }
            }
        }

        private void clearSelectFile()
        {
            Console.SetCursorPosition(0, 2);
            for(int i = 0; i < 2; i++)
            {
                clearLine();
            }
        }

        private String selectFile()
        {

            Console.SetCursorPosition(0, 2);
            Console.WriteLine("Podaj scieżkę do pliku:");
            return Console.ReadLine();



        }


    }
}
