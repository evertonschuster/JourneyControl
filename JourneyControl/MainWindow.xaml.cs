using JourneyControl.Models;
using JourneyControl.Repositories;
using JourneyControl.Services;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Timers;
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

namespace JourneyControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IActivityService ActivityService { get; }
        private DispatcherTimer ClockTimer { get; } 

        public MainWindow()
        {
            try
            {
                ClockTimer = new DispatcherTimer();
                var context = new JourneyControlContext();


                InitializeComponents();
                InitializeComponent();

                context.Database.Migrate();

                ActivityService = new ActivityService(new ActivityRepository(context), new ActivityMonitor());
                ActivityService.StartMonitoring();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void InitializeComponents()
        {
            ClockTimer.Interval = TimeSpan.FromSeconds(1);
            ClockTimer.Tick += UpdateClock;
            ClockTimer.Start();
        }

        public void UpdateClock(object? sender, EventArgs e)
        {
            var today = ActivityService.GetTodayActivity();
            TodayTotal.Text = today.time.ToString("HH:mm:ss");

            WorkingImage.Visibility = today.isActive ? Visibility.Visible : Visibility.Collapsed;
            SleepingImage.Visibility = today.isActive ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}