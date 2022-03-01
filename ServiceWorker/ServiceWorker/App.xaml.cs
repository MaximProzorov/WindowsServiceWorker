using Microsoft.Extensions.DependencyInjection;
using ServiceWorker.Views;
using System.Windows;
using ServiceWorker.Infrastructure.Service;
using ServiceWorker.ViewModels;
using ServiceWorker.Infrastructure;

namespace ServiceWorker
{
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;
        public App()
        {
            ServiceCollection services = new ServiceCollection();
            configureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        private void configureServices(IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<IServiceHelper, ServiceHelper>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow.DataContext = serviceProvider.GetService<MainViewModel>();
            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}

