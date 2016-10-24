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
        private string _lastDir;
        private int vol;
        private int shown;

        Dictionary<int, string> _directoryMenu;

        public tui()
        {
            this._menu = new Dictionary<int, string>();
            this._menuBar = new Dictionary<ConsoleKey, string>();
            this._menuStartRow = 8;
            this._musicPlayer = new Logic();
            this._drives = DriveInfo.GetDrives();
            this._directoryMenu = new Dictionary<int, string>();
            vol = 100;
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
            _menuBar.Add(ConsoleKey.F4, "Help (F4)");

        }

        public void loadInterface()
        {
            this.loadMenus();
            Console.SetWindowSize(50, 14);
            Console.SetBufferSize(50, 14);
            Console.Title = "Music Player";
            IntPtr handle = GetConsoleWindow();
            IntPtr sysMenu = GetSystemMenu(handle, false);

            if (handle != IntPtr.Zero)
            {
                DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);
            }
            Console.CursorVisible = true;

            selectDevice();
            this.mainMenu(); 
        }

        private void refreshMenuBar()
        {
            Console.SetCursorPosition(0, 0);
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.ForegroundColor = ConsoleColor.Black;
            foreach (var item in _menuBar)
            { 
                Console.Write(item.Value + "    ");
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.CursorLeft = Console.CursorLeft - 4;
            Console.Write("    ");
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

        

        private void upMenu(Dictionary<int, String> menu, int start)
        {
            if (Console.CursorTop > start)
            {
                Console.SetCursorPosition(0, (Console.CursorTop - 1));
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine(menu[Console.CursorTop - start]);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(menu[Console.CursorTop - start]);
                Console.SetCursorPosition(0, (Console.CursorTop - 2));
            }
        }

        private void upMenu(string[] menu, int start)
        {
            if (Console.CursorTop > start)
            {
                Console.SetCursorPosition(0, (Console.CursorTop - 1));
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine(menu[Console.CursorTop - start]);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(menu[Console.CursorTop - start]);
                Console.SetCursorPosition(0, (Console.CursorTop - 2));
            }
        }

        private void upFileMenu(Dictionary<int, String> menu, int start)
        {
            if (Console.CursorTop > start)
            {
                Console.SetCursorPosition(0, (Console.CursorTop - 1));
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine(Path.GetFileName(menu[Console.CursorTop - start]));
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(Path.GetFileName(menu[Console.CursorTop - start]));
                Console.SetCursorPosition(0, (Console.CursorTop - 2));
            }
            else if(shown > 5)
            {
                refreshDirs(0);
            }
        }



        private void downMenu(Dictionary<int, String> menu, int start)
        {
            if (Console.CursorTop < (start + menu.Count - 1))
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine(menu[Console.CursorTop - start]);
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.ForegroundColor = ConsoleColor.Black;

                Console.WriteLine(menu[Console.CursorTop - start]);
                Console.SetCursorPosition(0, (Console.CursorTop - 1));
            }
        }

        private void downMenu(string[] menu, int start)
        {
            if (Console.CursorTop < (start + menu.Length - 1))
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine(menu[Console.CursorTop - start]);
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.ForegroundColor = ConsoleColor.Black;

                Console.WriteLine(menu[Console.CursorTop - start]);
                Console.SetCursorPosition(0, (Console.CursorTop - 1));
            }
        }


        private void downFileMenu(Dictionary<int, String> menu, int start)
        {
            if (Console.CursorTop < (start + menu.Count - 1))
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine(Path.GetFileName(menu[Console.CursorTop - start]));
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.ForegroundColor = ConsoleColor.Black;

                Console.WriteLine(Path.GetFileName(menu[Console.CursorTop - start]));
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
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.ForegroundColor = ConsoleColor.Black;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }
            int current = Console.CursorTop;
            vol = _musicPlayer.Volume + v;
            _musicPlayer.Volume = vol;
            _menu[3] = "Volume: " + _musicPlayer.Volume + "%";
            Console.SetCursorPosition(0,(_menuStartRow + 3));
            clearLine();
            if (current == _menuStartRow + 3)
            {
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.ForegroundColor = ConsoleColor.Black;
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

                        timer = new Timer(Timer, null, 0, 1000);

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
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write("TYTUL PIOSENKI "+_musicPlayer.Name);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(String.Format(@"<{0:mm\:ss}/{1:mm\:ss}>", _musicPlayer.Position, _musicPlayer.Length));
                Console.ForegroundColor = ConsoleColor.White;
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
       
      


        private void clearDirectorySearch(int i)
        {
            int t = Console.CursorTop;
            for (int x = 0; x < i; x++)
            {
                Console.CursorTop = 4 + x;
                clearLine();
            }
            Console.SetCursorPosition(0, t);
        }


        private void selectDrive()
        {
            shown = 0;
            Console.SetCursorPosition(0, 5);

            string[] drives = new String[_drives.Length];
            ConsoleKey key;
            Console.WriteLine("Wybierz dysk:");
            for(int i = 0; i< _drives.Length; i++)
            {
                drives[i] = _drives[i].Name;
            }
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(drives[0]);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 1; i < drives.Length; i++)
            {
                Console.WriteLine(drives[i]);
            }

            Console.SetCursorPosition(0, 5);

            do
            {
                key = Console.ReadKey(true).Key;
                
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        upMenu(drives, 5);
                    break;
                    case ConsoleKey.DownArrow:
                        downMenu(drives, 5);
                    break;
                    case ConsoleKey.Enter:
                        Directory.SetCurrentDirectory(drives[Console.CursorTop - 5]);
                        clearDirectorySearch(drives.Length);
                    break;
                }

            } while (key != ConsoleKey.X);
        }

        private void refreshDirs(int x)
        {
            int top = Console.CursorTop;
            Console.SetCursorPosition(0, 6);

            if (_directoryMenu.Count - shown > 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    try
                    {
                        Console.WriteLine(Path.GetFileName(_directoryMenu[shown + i]));
                        shown++;
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
            

        }

        private String directorySearchThrough()
        {

            Console.SetCursorPosition(0, 4);
            clearLine();
            _lastDir = Directory.GetCurrentDirectory();
            _directoryMenu.Clear();
            _directoryMenu[0] = "../";
            Console.WriteLine(_lastDir);
            Console.WriteLine(_directoryMenu[0]);

            string path = null;
            string[] files = null;
            string[] dirs = null;
            string[] shownDirs = null;

                try
                {

                    files = Directory.GetFiles(_lastDir);
                    dirs = Directory.GetDirectories(_lastDir);
                    if (files.Length <= 0 && dirs.Length <= 0)
                    {
                        Console.WriteLine("Folder jest pusty.");
                    }
                    else
                    {
                        Console.SetBufferSize(80, 35);
                        foreach (string f in files)
                        {
                            //Console.WriteLine(Path.GetFileName(f));
                            _directoryMenu.Add(_directoryMenu.Count, f);
                        }

                        foreach (string d in dirs)
                        {
                        
                            //Console.WriteLine(Path.GetFileName(d));
                            _directoryMenu.Add(_directoryMenu.Count, d);
                        }
                    }
                //shown += shownDirs.Length;
                //    if(_directoryMenu.Count - shown > 0)
                //{
                //    for (int i = 0;i < 5; i++)
                //    {
                //        try
                //        {
                //            shownDirs[i] = _directoryMenu[shown + i];
                //        }catch(Exception e)
                //        {
                //            shownDirs[i] = null;
                //        }
                //    }
                //}
                    

                } 
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            

            ConsoleKey key;

            Console.SetCursorPosition(0, 5);
            do
            {
                key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        upFileMenu(_directoryMenu,5);
                    break;
                    case ConsoleKey.DownArrow:
                        downFileMenu(_directoryMenu, 5);
                    break;
                    case ConsoleKey.Enter:
                        clearDirectorySearch(_directoryMenu.Count()+1);
                        if (_directoryMenu[Console.CursorTop - 5] == "../")
                        {
                            try {
                                Directory.SetCurrentDirectory(Directory.GetParent(Directory.GetCurrentDirectory()).FullName);
                            }catch (NullReferenceException e )
                            {
                                selectDrive();
                            }
                            return directorySearchThrough();
                        }
                        else if (dirs.Contains(_directoryMenu[Console.CursorTop - 5]))
                        {
                            Directory.SetCurrentDirectory(_directoryMenu[Console.CursorTop - 5]);
                           return directorySearchThrough();
                        }
                        else
                        {
                            return _directoryMenu[Console.CursorTop - 5];
                            
                        }
                    break;
                }

            } while (key != ConsoleKey.X);

            return path;
        }




    }
}
