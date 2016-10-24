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
using System.Windows.Forms;
using System.IO;
//using CursesSharp;

namespace Player
{
    class Program
    {

        static void Main(string[] args)
        {
            
            tui userInterface = new tui();
            userInterface.loadInterface();
            
            /// testy dzialania
            

            Console.ReadLine();
        }
    }
}

