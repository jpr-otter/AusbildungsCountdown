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
using System.Windows.Threading;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Threading;

namespace CountdownAusbildung
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer Timer { get; set; }
        System.Timers.Timer HideTimeTimer { get; set; } // dat is ne property
        DateTime TargetDate { get; set; }
        
        public MainWindow()
        {
            InitializeComponent();
            CreateStartupShortcut();

            /*
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;            
            System.Drawing.Rectangle workingArea = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
            this.Left = 0;
            this.Top = workingArea.Height - this.Height;
            */

            if (1==1)
            {
                // Load target date from file
                string terminationDate = "31.07.2026";
                TargetDate = DateTime.Parse(terminationDate);
                
            }            
            string displayMessage = "bis zum Ausbildungsende";

            Timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, (s, e) =>
            {
                var remainingTime = TargetDate - DateTime.Now;

                if (remainingTime.TotalSeconds <= 0)
                {
                    tbTime.Text = "Du bist nun Anwendungsentwickler.";
                    Timer?.Stop();
                }
                else
                {                    
                    tbTime.Text = $" {remainingTime.Days} Days\n {remainingTime.Hours}:{remainingTime.Minutes}:{remainingTime.Seconds}\n {displayMessage} ";
                }
            }, Application.Current.Dispatcher);

            Timer.Start();
        }

        private void CreateStartupShortcut()
        {
            dynamic shell = Activator.CreateInstance(Type.GetTypeFromProgID("WScript.Shell")!)!;
            dynamic link = shell!.CreateShortcut(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "Ausbildungs-Counter.lnk"));

            link.TargetPath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            link.WindowStyle = 1;
            link.Save();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;
            this.Left = 0;
            this.Top = 0;


        }

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            HideTimeTimer = new System.Timers.Timer(5000)
            {
                AutoReset = false,
                Enabled = true
            };
            HideTimeTimer.Elapsed += HideTimeTimer_Elapsed;
        }

        private void HideTimeTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                this.Visibility = Visibility.Visible;
            }));
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
        
        }



        /*  
        wenn die targetdate txt gelöscht wird schreibt diese funktion ne neue, nicht unbedingt notwendig

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Save target date to file when application is closing
            File.WriteAllText(filePath, _targetDate.ToString());
        }
        */
    }
}