using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace AppUsage
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public RegistryKey regKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        public Home()
        {
            InitializeComponent();
        }

        /// StartWithWindows CheckBox Methods
        private void StartWithWindows_Checked(object sender, RoutedEventArgs e)
        {
            regKey.SetValue("AppUsage", Process.GetCurrentProcess().MainModule.FileName); // Add Application To Windows StartUp Programs       

            Properties.Settings.Default.CheckBoxWinStart = true;
            Properties.Settings.Default.Save();
        }

        private void StartWithWindows_Unchecked(object sender, RoutedEventArgs e)
        {
            regKey.DeleteValue("AppUsage", false); // Remove Application From Windows StartUp Programs

            Properties.Settings.Default.CheckBoxWinStart = false;
            Properties.Settings.Default.Save();
        }

        /// On Page Load, Get Saved Settings For CheckBox And If True, Check It, If False, Uncheck It
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.CheckBoxWinStart == true)
            {
                StartWithWindows.IsChecked = true;
            }
            else
            {
                StartWithWindows.IsChecked = false;
            }
        }
    }
}
