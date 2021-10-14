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
using System.Windows.Threading;

namespace AppUsage
{
    /// <summary>
    /// Interaction logic for AppUsageProcesses.xaml
    /// </summary>
    public partial class AppUsageProcesses : Page
    {
        private static DispatcherTimer dispTimer = new DispatcherTimer();

        public AppUsageProcesses()
        {
            InitializeComponent();
        }

        /// DispTimer Add/Remove Tick Functions
        private void AddEvent(EventHandler a)
        {
            dispTimer.Tick += a;
        }

        private void RemoveEvent(EventHandler a)
        {
            dispTimer.Tick += a;
        }

        /// Get All Current Running Processes And Add The In The StackPanel (DispTimer Tick)
        private void AppInfoTick(object sender, EventArgs e)
        {
            Process[] processes = Process.GetProcesses();
            ResetStackPanel();

            foreach (Process process in processes)
            {
                AppStacker.Children.Add(AppInfo(process));
            }
        }

        /// On StartUp Of Page, Get All Currently Running Processes And Add Them In StackPanel 
        private void AppInfoStartUp()
        {
            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes)
            {
                AppStacker.Children.Add(AppInfo(process));
            }
        }

        /// Convert RAM Bytes To MBytes
        private double ConvertToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }

        /// Creates TextBox Object With The Required Items And Text
        private TextBox AppInfo(Process app)
        {
            TextBox textBox = new TextBox
            {
                Text = $"pId: {app.Id} || Name: {app.ProcessName} || RAM Usage: {(int)ConvertToMegabytes(app.WorkingSet64)} MB",
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = 420,
                FontSize = 18,
                TextWrapping = TextWrapping.Wrap,
                FontWeight = FontWeights.Bold
            };

            return textBox;
        }

        /// Resets The StackPanel Contents
        private void ResetStackPanel()
        {
            AppStacker.Children.Clear();
        }

        /// On Page Load, Start DispTimer
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AppInfoStartUp();
            dispTimer.Interval = new TimeSpan(0, 0, 10);
            dispTimer.IsEnabled = true;
            dispTimer.Start();
            AddEvent(AppInfoTick);
        }

        /// On Page Unload, Stop DispTimer
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            dispTimer.IsEnabled = false;
            dispTimer.Stop();
        }
    }
}
