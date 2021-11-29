using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AppUsage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Windows.Forms.NotifyIcon notifIcon = new System.Windows.Forms.NotifyIcon();

        public MainWindow()
        {
            InitializeComponent();
        }

        /// Button Clicks Logic (Changing Pages)
        private void MenuButtonClose_Click(object sender, RoutedEventArgs e)
        {
            MenuButtonOpen.Visibility = Visibility.Visible;
            MenuButtonClose.Visibility = Visibility.Hidden;
        }

        private void MenuButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            MenuButtonOpen.Visibility = Visibility.Hidden;
            MenuButtonClose.Visibility = Visibility.Visible;
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            Grid.SetRow(ClickRect, 1);
            ContentDisplay.Content = new Home();
        }

        private void AppUsage_Click(object sender, RoutedEventArgs e)
        {
            Grid.SetRow(ClickRect, 2);
            ContentDisplay.Content = new AppUsageProcesses();
        }

        private void CpuGpu_Click(object sender, RoutedEventArgs e)
        {
            Grid.SetRow(ClickRect, 3);
            ContentDisplay.Content = new CpuGpu();
        }

        private void AppTime_Click(object sender, RoutedEventArgs e)
        {
            Grid.SetRow(ClickRect, 4);
            ContentDisplay.Content = new AppTime();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            Grid.SetRow(ClickRect, 5);
            ContentDisplay.Content = new About();

        }

        /// Add Dragability To The Top Bar
        private void TopBarGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        /// Send To Tray Function
        private void SendApplicationToTray()
        {
            AppWindow.Visibility = Visibility.Hidden;
            AppWindow.WindowState = WindowState.Minimized;
            this.ShowInTaskbar = false;

            notifIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetEntryAssembly().ManifestModule.Name); // Gets Application Icon And Sets It As Tray Icon
            notifIcon.Visible = true;
            notifIcon.BalloonTipText = "The application has been minimized to tray.";
            notifIcon.BalloonTipTitle = "AppUsage";
            notifIcon.ShowBalloonTip(10);
            notifIcon.DoubleClick += nIcon_DoubleClick;

            System.Windows.Forms.ContextMenu contMenu = new System.Windows.Forms.ContextMenu();
            System.Windows.Forms.MenuItem menuItemExit = new System.Windows.Forms.MenuItem();
            contMenu.MenuItems.Add(menuItemExit);
            menuItemExit.Text = "Exit";
            menuItemExit.Click += MenuItemExit_Click;

            notifIcon.ContextMenu = contMenu;
        }

        /// On Click Send The Program To Tray
        private void ButtonToTray_Click(object sender, RoutedEventArgs e)
        {
            SendApplicationToTray();
        }

        /// When The Program Is To Tray, On Click Shutdown The Application
        private void MenuItemExit_Click(object sender, System.EventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// When The Program Is To Tray, Double Clicking It Opens It Back Up
        private void nIcon_DoubleClick(object sender, EventArgs e)
        {
            AppWindow.Visibility = Visibility.Visible;
            AppWindow.WindowState = WindowState.Normal;

            notifIcon.Visible = false;
            this.ShowInTaskbar = true;
        }

        /// Minimize The Application
        private void ButtonMimize_Click(object sender, RoutedEventArgs e)
        {
            AppWindow.WindowState = WindowState.Minimized;
        }

        /// On Click Shutdown The Application
        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// On MainWindow Load
        private void AppWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.CheckBoxWinStart == true)
            {
                SendApplicationToTray();
                Grid.SetRow(ClickRect, 4);
                ContentDisplay.Content = new AppTime();
            }
        }
    }
}
