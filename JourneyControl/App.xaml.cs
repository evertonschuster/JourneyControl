using JourneyControl.Application.Extension;
using JourneyControl.GUI.Extension;
using JourneyControl.Infra.DB.Extension;
using JourneyControl.Infra.Windows.Extension;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.IO;
using System.Windows;

namespace JourneyControl
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private ServiceProvider serviceProvider;
        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }
        private void ConfigureServices(ServiceCollection services)
        {
            services.ApplicationLogging();
            services.ApplicationConfiguration();
            services.AddApplicationServices();
            services.AddWindowsApplicationServices();
            services.AddInfraDbServices();

            services.AddSingleton<MainWindow>();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow?.Show();
        }
    }
}
