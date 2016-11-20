using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
using System.Windows.Forms;
namespace Player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Logic _musicPlayer = new Logic();
        private bool _stopSliderUpdate;
        private readonly ObservableCollection<MMDevice> _devices = new ObservableCollection<MMDevice>();

        public MainWindow()
        {
            InitializeComponent();
        }


        private void Exit_Click(object sender, RoutedEventArgs e)
        {

            System.Windows.Application.Current.Shutdown();

        }





    }
}
