using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace AppUsage
{
    /// <summary>
    /// Interaction logic for AppTimeRemove.xaml
    /// </summary>
    public partial class AppTimeRemove : Window
    {
        /// Gets JsonFile And JsonDir Path
        private static string jsonDirName = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AppUsage_Json");
        private static string jsonFileName = System.IO.Path.Combine(jsonDirName, "appinfo.json");
        private bool exitWindow = true;

        public AppTimeRemove()
        {
            InitializeComponent();
        }

        /// Removes The Selected Object From Json
        private void RemoveFromJson(string appName)
        {
            if (Directory.Exists(jsonDirName) && File.Exists(jsonFileName))
            {
                string json = "";
                using (StreamReader r = new StreamReader(jsonFileName)) // Reads Whole Json File And Converts It Into A String
                {
                    json = r.ReadToEnd();
                }

                List<AppInfo> appInfo = JsonConvert.DeserializeObject<List<AppInfo>>(json);

                var app = appInfo.SingleOrDefault(x => x.Name == appName); // Gets The Object From The List Of Json Objects If It Exists

                if (app != null) // If Json Object Exists, Remove It From The Json List
                {
                    appInfo.Remove(app);

                    exitWindow = true;
                    json = JsonConvert.SerializeObject(appInfo, Formatting.Indented);
                    File.WriteAllText(jsonFileName, json); // Write Json List Into Json File
                }
                else
                {
                    MessageBox.Show("The Application you entered was not found, check if name is correct or if it exists!");
                    exitWindow = false;
                }
            }
            else
            {
                MessageBox.Show("Your first have to add an Application to track before you can remove one!");
            }
        }

        /// Adds Dragability To The TopBar
        private void TopBarGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
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

        /// On Click Remove The Json If It Exists
        private void REMOVE_Click(object sender, RoutedEventArgs e)
        {
            RemoveStyling(true);
            string appName = GetName.Text;

            RemoveFromJson(appName);

            if (exitWindow == true)
            {
                this.Close();
            }

            RemoveStyling(false);
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
