using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
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
    /// Interaction logic for CpuGpu.xaml
    /// </summary>

    public partial class CpuGpu : Page
    {
        System.Windows.Threading.DispatcherTimer dispTimer = new System.Windows.Threading.DispatcherTimer();

        public CpuGpu()
        {
            InitializeComponent();
        }

        /// DispTimer Add/Remove Tick
        private void AddEvent(EventHandler a)
        {
            dispTimer.Tick += a;
        }

        private void RemoveEvent(EventHandler a)
        {
            dispTimer.Tick -= a;
        }

        private void CpuVisability(bool a)
        {
            switch (a)
            {
                //Getting CPU usage in percentage
                case true:
                    PerformanceCounter cpuUsage = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");

                    // DispTimer Tick, Gets New CPU Usage Info Every Tick
                    void CpuTick(object sender, EventArgs e)
                    {
                        float pCPU = cpuUsage.NextValue();
                        CpuProgress.Value = (int)pCPU;
                        CpuPercentage.Content = string.Format("{0}%", (int)pCPU);
                    }
                    AddEvent(CpuTick);

                    CPU.Background = new SolidColorBrush(Color.FromRgb(249, 170, 51));
                    CPU.Foreground = Brushes.White;
                    CpuProgress.Visibility = Visibility.Visible;
                    CpuPercentage.Visibility = Visibility.Visible;
                    break;

                case false:

                    CPU.Background = Brushes.White;
                    CPU.Foreground = Brushes.Black;
                    CpuProgress.Visibility = Visibility.Hidden;
                    CpuPercentage.Visibility = Visibility.Hidden;
                    break;

                default:
                    break;
            }
        }

        private void RamVisability(bool a)
        {
            switch (a)
            {
                //Getting Available RAM In MBytes Substracting Them With Total RAM, Multiplying With 100 To Get Total Usage In Percents
                case true:
                    PerformanceCounter ramUsage = new PerformanceCounter("Memory", "Available MBytes");
                    ObjectQuery wql = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher(wql);
                    ManagementObjectCollection results = searcher.Get();

                    double ramKB;
                    float ramMB = 0;

                    foreach (ManagementObject result in results)
                    {
                        ramKB = Convert.ToDouble(result["TotalVisibleMemorySize"]);
                        ramMB = (float)ramKB / 1024;
                    }

                    // DispTimer Tick, Gets New RAM Usage Info Every Tick
                    void RamTick(object sender, EventArgs e)
                    {
                        float pRAM = ramUsage.NextValue();
                        float ram = ((ramMB - pRAM) / ramMB) * 100;
                        RamAvailable.Content = string.Format("Available: {0} MB", (int)pRAM);
                        RamProgress.Value = (int)ram;
                        RamPercentage.Content = string.Format("{0}%", (int)ram);
                    }
                    AddEvent(RamTick);

                    RAM.Background = new SolidColorBrush(Color.FromRgb(249, 170, 51));
                    RAM.Foreground = Brushes.White;
                    RamAvailable.Visibility = Visibility.Visible;
                    RamProgress.Visibility = Visibility.Visible;
                    RamPercentage.Visibility = Visibility.Visible;
                    break;

                case false:
                    RAM.Background = Brushes.White;
                    RAM.Foreground = Brushes.Black;
                    RamAvailable.Visibility = Visibility.Hidden;
                    RamProgress.Visibility = Visibility.Hidden;
                    RamPercentage.Visibility = Visibility.Hidden;
                    break;

                default:
                    break;
            }
        }

        private void GpuVisability(bool a)
        {
            switch (a)
            {
                //Getting GPU Usage In Percentage
                case true:
                    List<PerformanceCounter> gpuCounters = new List<PerformanceCounter>();
                    PerformanceCounterCategory category = new PerformanceCounterCategory("GPU Engine");
                    var gpuNames = category.GetInstanceNames();

                    foreach (var name in gpuNames)
                    {
                        if (name.EndsWith("engtype_3D"))
                        {
                            foreach (PerformanceCounter gpuCounter in category.GetCounters(name))
                            {
                                if (gpuCounter.CounterName == "Utilization Percentage")
                                {
                                    gpuCounters.Add(gpuCounter);
                                }
                            }
                        }
                    }

                    // DispTimer Tick, Gets New GPU Usage Info Every Tick
                    void GpuTick(object sender, EventArgs e)
                    {
                        var gpu = 0f;

                        gpuCounters.ForEach(x =>
                        {
                            gpu += x.NextValue();
                        });

                        GpuProgress.Value = (int)gpu;
                        GpuPercentage.Content = string.Format("{0}%", (int)gpu);
                    }
                    AddEvent(GpuTick);

                    GPU.Background = new SolidColorBrush(Color.FromRgb(249, 170, 51));
                    GPU.Foreground = Brushes.White;
                    GpuProgress.Visibility = Visibility.Visible;
                    GpuPercentage.Visibility = Visibility.Visible;
                    break;

                case false:
                    GPU.Background = Brushes.White;
                    GPU.Foreground = Brushes.Black;
                    GpuProgress.Visibility = Visibility.Hidden;
                    GpuPercentage.Visibility = Visibility.Hidden;
                    break;

                default:
                    break;
            }
        }

        /// When Page Loads, Toggle CpuVisability To True So The Counters Can Start
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CpuVisability(true);
            dispTimer.Interval = new TimeSpan(0, 0, 1);
            dispTimer.IsEnabled = true;
            dispTimer.Start();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            dispTimer.IsEnabled = false;
            dispTimer.Stop();
        }

        /// Once Buttons Are Clicked Set Visabilites And Activate Counters
        private void CPU_Click(object sender, RoutedEventArgs e)
        {
            CpuVisability(true);
            RamVisability(false);
            GpuVisability(false);
        }

        private void RAM_Click(object sender, RoutedEventArgs e)
        {
            CpuVisability(false);
            RamVisability(true);
            GpuVisability(false);
        }

        private void GPU_Click(object sender, RoutedEventArgs e)
        {
            CpuVisability(false);
            RamVisability(false);
            GpuVisability(true);
        }
    }
}
