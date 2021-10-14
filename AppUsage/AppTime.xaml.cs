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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Threading;
using System.Diagnostics;

namespace AppUsage
{
    /// <summary>
    /// Interaction logic for AppTime.xaml
    /// </summary>
    public partial class AppTime : Page
    {
        /// Getting JsonFile Path And JsonDir Path, Creating The DispTimer
        private static string jsonDirName = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AppUsage_Json");
        private static string jsonFileName = System.IO.Path.Combine(jsonDirName, "appinfo.json");
        private static DispatcherTimer checkingTimer = new DispatcherTimer();
        private static string json = "";

        public AppTime()
        {
            InitializeComponent();
        }

        /// Function To Build The Time String
        private string TimeString(AppUsageTime time)
        {
            string minutes = "";
            string hours = "";
            string days = "";

            //Minutes
            if (time.minutes < 10)
            {
                minutes = $"0{time.minutes}";
            }
            else
            {
                minutes = time.minutes.ToString();
            }

            //Hours
            if (time.hours < 10)
            {
                hours = $"0{time.hours}";
            }
            else
            {
                hours = time.hours.ToString();
            }

            //Days
            if (time.days < 10)
            {
                days = $"0{time.days}";
            }
            else
            {
                days = time.days.ToString();
            }

            return $"{days}d:{hours}h:{minutes}m";
        }

        /// Resets The Children Objects In The StackPanel To Display New Information
        private void ResetStackPanel()
        {
            AppStacker.Children.Clear();
            DisplayAppsInfo();
        }

        /// DispTimer Tick Add/Remove Functions
        private void AddEvent(EventHandler a)
        {
            checkingTimer.Tick += a;
        }

        private void RemoveEvent(EventHandler a)
        {
            checkingTimer.Tick += a;
        }

        /// DispTime Tick Function (Adds The Time And Saves It All To The Json File)
        private void AppInfoTick(object sender, EventArgs e)
        {
            if (File.Exists(jsonFileName))
            {
                List<AppInfo> items = JsonConvert.DeserializeObject<List<AppInfo>>(json); // Coverts All Json Items Into Objects
                int counter = 0;

                // Gets Current Running Processes And Checks If The Processes In Json Are Amongst The Running Ones
                foreach (var item in items)
                {
                    Process[] processes = Process.GetProcessesByName(item.Name);

                    if (processes.Length != 0)
                    {
                        item.Time.addSeconds(1);
                        item.IsRunning = true;
                    }
                    else
                    {
                        item.IsRunning = false;
                    }

                    counter++;
                }

                json = JsonConvert.SerializeObject(items, Formatting.Indented); // Converts All Objects Into Json
                File.WriteAllText(jsonFileName, json); // Saves To Json
                ResetStackPanel(); // Resets Stack Panel
            }
        }

        /// Creates TextBox Object Which Is Then Added To The StackPanel
        private TextBox AppInfo(int id, string name, AppUsageTime time, bool isRunning)
        {
            int width = 420;
            int fontSize = 18;

            if (isRunning)
            {
                TextBox textBox = new TextBox
                {
                    Text = $"Id: {id} || Name: {name} || Time Used: {TimeString(time)} || App Is Running!",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Width = width,
                    FontSize = fontSize,
                    TextWrapping = TextWrapping.Wrap,
                    FontWeight = FontWeights.Bold
                };

                return textBox;
            }
            else
            {
                TextBox textBox = new TextBox
                {
                    Text = $"Id: {id} || Name: {name} || Time Used: {TimeString(time)} || App Is Not Running!",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Width = width,
                    FontSize = fontSize,
                    TextWrapping = TextWrapping.Wrap,
                    FontWeight = FontWeights.Bold,
                    BorderBrush = Brushes.Orange,
                    BorderThickness = new Thickness(5)
                };

                return textBox;
            }
        }

        /// Gets All The Objects From Json File And Adds Them In The StackPanel
        private void DisplayAppsInfo()
        {
            if (File.Exists(jsonFileName))
            {
                using (StreamReader r = new StreamReader(jsonFileName))
                {
                    json = r.ReadToEnd();
                }

                List<AppInfo> appInfo = JsonConvert.DeserializeObject<List<AppInfo>>(json);

                foreach (var app in appInfo)
                {
                    AppStacker.Children.Add(AppInfo(app.Id, app.Name, app.Time, app.IsRunning));
                }
            }
        }

        /// ADD Button Styling
        private void AddStyling(bool a)
        {
            switch (a)
            {
                case true:
                    ADD.Background = ADD.Background = new SolidColorBrush(Color.FromRgb(249, 170, 51));
                    ADD.Foreground = Brushes.White;
                    break;

                case false:
                    ADD.Background = Brushes.White;
                    ADD.Foreground = Brushes.Black;
                    break;

                default:
                    break;
            }
        }

        /// Remove Button Styling
        private void RemoveStyling(bool a)
        {
            switch (a)
            {
                case true:
                    REMOVE.Background = REMOVE.Background = new SolidColorBrush(Color.FromRgb(249, 170, 51));
                    REMOVE.Foreground = Brushes.White;
                    break;

                case false:
                    REMOVE.Background = Brushes.White;
                    REMOVE.Foreground = Brushes.Black;
                    break;

                default:
                    break;
            }
        }

        /// If Add Button Is Clicked Open The AppTimeAdd Window Where You Add A New Object In Json
        private void ADD_Click(object sender, RoutedEventArgs e)
        {
            AddStyling(true);

            AppTimeAdd appTimeAdd = new AppTimeAdd();
            appTimeAdd.ShowDialog();
            ResetStackPanel();

            AddStyling(false);
        }

        /// If Remove Button Is Clicked Open The AppTimeRemove Window Where You Remove An Existing Object From Json 
        private void REMOVE_Click(object sender, RoutedEventArgs e)
        {
            RemoveStyling(true);

            AppTimeRemove appTimeRemove = new AppTimeRemove();
            appTimeRemove.ShowDialog();
            ResetStackPanel();

            RemoveStyling(false);
        }

        /// On Page Load, Start The DispTimer
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            checkingTimer.Interval = new TimeSpan(0, 0, 1);
            checkingTimer.IsEnabled = true;
            checkingTimer.Start();
            AddEvent(AppInfoTick);
            DisplayAppsInfo();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
