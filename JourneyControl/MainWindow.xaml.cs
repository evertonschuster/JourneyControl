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

namespace JourneyControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IActivityService? ActivityService { get; }

        public MainWindow()
        {
            try
            {
                InitializeComponent();

                var context = new JourneyControlContext();
                context.Database.Migrate();

                ActivityService = new ActivityService(new ActivityRepository(context), new ActivityMonitor());
                ActivityService.StartMonitoring();

                WorkTime.Content = "00:00:01";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}