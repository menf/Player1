using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace Player
{
    class Program
    {

        #region Do WPF'a

        public static Application WinApp { get; private set; }
        public static Window MainWindow { get; private set; }


        internal static void InitializeWindows()
        {
            WinApp = new Application();
            WinApp.Run(MainWindow = new MainWindow()); // note: blocking call
        }

        #endregion

        #region Do wyswietlania konsoli
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool AllocConsole(); // Create console window

        [DllImport("kernel32.dll")]
        internal static extern IntPtr GetConsoleWindow(); // Get console window handle

        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        internal const int SW_HIDE = 0;
        internal const int SW_SHOW = 5;


        static void ShowConsole()
        {
            var handle = GetConsoleWindow();
            if (handle == IntPtr.Zero)
                AllocConsole();
            else
                ShowWindow(handle, SW_SHOW);
        }

        static void HideConsole()
        {
            var handle = GetConsoleWindow();
            if (handle != null)
                ShowWindow(handle, SW_HIDE);
        }
        #endregion

        [STAThread]
        static void Main(string[] args)
        {
            InitializeWindows();
           // tui userInterface = new tui();
           // userInterface.loadInterface();
            
            /// testy dzialania
            

            Console.ReadLine();
        }
    }
}

