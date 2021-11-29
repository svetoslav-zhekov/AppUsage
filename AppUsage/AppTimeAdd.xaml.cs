using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace AppUsage
{
    /// <summary>
    /// Interaction logic for AppTimeAdd.xaml
    /// </summary>
    public partial class AppTimeAdd : Window
    {
        /// Gets JsonFile Path And JsonDir Path
        private static string jsonDirName = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AppUsage_Json");
        private static string jsonFileName = System.IO.Path.Combine(jsonDirName, "appinfo.json");
        private bool exitWindow = true;

        public AppTimeAdd()
        {
            InitializeComponent();
        }

        /// Serialize Object To Json Format And Write In The Json File
        /// Checking If Duplicate Objects Already Exist In The File
        private void WriteToJson(int id, string name, AppUsageTime time, bool isRunning)
        {
            DirectoryInfo dir = Directory.CreateDirectory(jsonDirName);

            AppInfo appObj = new AppInfo
            {
                Id = id,
                Name = name,
                Time = time,
                IsRunning = isRunning
            };

            if (name.Length == 0)
            {
                MessageBox.Show("You dont have application name set!"); //If No Name Is Entered And Add Button Is Clicked
                exitWindow = false;
            }
            else
            {
                exitWindow = true;
                if (File.Exists(jsonFileName))
                {
                    bool duplicates = false;
                    string json = "";

                    using (StreamReader r = new StreamReader(jsonFileName))
                    {
                        json = r.ReadToEnd();
                    }

                    List<AppInfo> appInfo = JsonConvert.DeserializeObject<List<AppInfo>>(json);

                    foreach (var app in appInfo)
                    {
                        // Checks If Json With Entered Name Already Exists
                        if (appObj.Name == app.Name)
                        {
                            MessageBox.Show("You have entered a name that already exists in the time tracker!");
                            duplicates = true;
                            exitWindow = false;
                        }

                        if (appObj.Id == app.Id)
                        {
                            appObj.Id++;
                        }
                    }

                    if (duplicates != true)
                    {
                        appInfo.Add(appObj);

                        json = JsonConvert.SerializeObject(appInfo, Formatting.Indented);
                        File.WriteAllText(jsonFileName, json);
                    }
                }
                else
                {
                    List<AppInfo> appInfo = new List<AppInfo>
                    {
                        appObj
                    };

                    string json = JsonConvert.SerializeObject(appInfo, Formatting.Indented);
                    File.WriteAllText(jsonFileName, json);
                }
            }
        }

        /// Button Style Change On Click
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

        /// When Mouse Click Over TopBar, Make It Draggable
        private void TopBarGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }


        /// ADD Button Click Logic
        private void ADD_Click(object sender, RoutedEventArgs e)
        {
            AddStyling(true);
            string appName = GetName.Text;

            WriteToJson(1, appName, new AppUsageTime(0, 0, 0, 0), false); // Writes In The Json File, Adds New Object

            if (exitWindow == true)
            {
                this.Close();
            }

            AddStyling(false);
        }

        /// Exit Button Closes Window
        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
