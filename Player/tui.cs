using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player
{
    class tui 
    {
        private Dictionary<int,String> _menu;
        private int _menuStartRow;

        public tui()
        {
            this._menu = new Dictionary<int, string>();
            this._menuStartRow = 8;
        }

        private void loadMenu()
        {
            _menu.Add(0,"Playlist");
            _menu.Add(1,"Volume");
            _menu.Add(2,"Loop");
            _menu.Add(3, "Menu Item 4");
        }


        private void mainMenu()
        {
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
            ConsoleKey key;
            do
            {
                key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        this.upMenu();
                        break;

                    case ConsoleKey.DownArrow:
                        this.downMenu();
                        break;


                }
            } while (key != ConsoleKey.X);
        }

        public void loadInterface()
        {
            this.loadMenu();
            Console.SetWindowSize(40, 45);
            Console.SetBufferSize(40, 45);
            Console.CursorVisible = false;
            this.mainMenu();
           
        }

        private void upMenu()
        {
            if (Console.CursorTop > _menuStartRow)
            {
                Console.SetCursorPosition(0, (Console.CursorTop - 1));
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine(_menu[Console.CursorTop - _menuStartRow]);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(_menu[Console.CursorTop - _menuStartRow]);
                Console.SetCursorPosition(0, (Console.CursorTop - 2));
            }
        }
      
        private void downMenu()
        {
            if (Console.CursorTop < (_menuStartRow + _menu.Count - 1))
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine(_menu[Console.CursorTop - _menuStartRow]);
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.ForegroundColor = ConsoleColor.Black;

                Console.WriteLine(_menu[Console.CursorTop - _menuStartRow]);
                Console.SetCursorPosition(0, (Console.CursorTop - 1));
            }
        }


    }
}
