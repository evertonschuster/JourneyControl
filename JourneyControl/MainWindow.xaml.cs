using JourneyControl.Application.Services;
using JourneyControl.Infra.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Windows;
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

        public MainWindow(IActivityService activityService, JourneyControlContext context, ILogger<MainWindow> logger) 
        {
            try
            {
                ClockTimer = new DispatcherTimer();
                ActivityService = activityService;
                
                context.Database.Migrate();

                InitializeComponents();
                InitializeComponent();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());

                logger.LogError(e, "Erro ao inicializar a aplicação");
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