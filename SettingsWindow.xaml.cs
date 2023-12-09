using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Windows.Forms;

namespace Aottg2Launcher
{
    /// <summary>
    /// Interaction logic for ErrorWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public static bool UninstallSuccess;
        public static bool UninstallCancelled;

        public SettingsWindow()
        {
            InitializeComponent();
            UninstallSuccess = false;
            UninstallCancelled = false;
        }

        private void UninstallButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                UninstallSuccess = Updater.UninstallGame();
            }
            catch
            {
                UninstallSuccess = false;
            }
            Close();
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            UninstallCancelled = true;
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
