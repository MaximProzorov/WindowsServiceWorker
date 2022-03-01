using ServiceWorker.Infrastructure;
using ServiceWorker.Infrastructure.Service;
using ServiceWorker.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace ServiceWorker.ViewModels
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        private IServiceHelper serviceHelper;
        private DispatcherTimer timer;
        public ICommand StartServiceCommand { get; protected set; }
        public ICommand StopServiceCommand { get; protected set; }

        public MainViewModel(IServiceHelper serviceHelper)
        {
            this.serviceHelper = serviceHelper;
            StartServiceCommand = new Command((_) => serviceHelper.StartService(SelectedService.Name));
            StopServiceCommand = new Command((_) => serviceHelper.StopService(SelectedService.Name));
            Services = new ObservableCollection<ServiceModel>();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += GetServices;
            timer.Start();
        }

        private void GetServices(object? sender, EventArgs e)
        {
            var updatedServices = serviceHelper.GetServices();
            var result = updatedServices.Except(services).ToList();
            if (result.Count != 0)
            {
                var updatedSelectedItem = result.FirstOrDefault(x => x == selectedService);
                if (updatedSelectedItem != null)
                    SelectedService = updatedSelectedItem;
                Services = new ObservableCollection<ServiceModel>(updatedServices);
            }
        }

        private ServiceModel selectedService;
        public ServiceModel SelectedService
        {
            get => selectedService;
            set
            {
                selectedService = value;
                switchButtonStatus(selectedService?.Status);
                OnPropertyChanged();
            }
        }

        private void switchButtonStatus(System.ServiceProcess.ServiceControllerStatus? status)
        {
            switch (status)
            {
                case System.ServiceProcess.ServiceControllerStatus.Stopped:
                case System.ServiceProcess.ServiceControllerStatus.Paused:
                    IsEnabledStartButton = true;
                    IsEnabledStopButton = false;
                    break;
                case System.ServiceProcess.ServiceControllerStatus.Running:
                    IsEnabledStartButton = false;
                    IsEnabledStopButton = true;
                    break;
                default:
                    IsEnabledStartButton = false;
                    IsEnabledStopButton = false;
                    break;
            }
        }
        private bool isEnabledStartButton;
        public bool IsEnabledStartButton
        {
            get => isEnabledStartButton;
            set
            {
                isEnabledStartButton = value;
                OnPropertyChanged();
            }
        }

        private bool isEnabledStopButton;
        public bool IsEnabledStopButton
        {
            get => isEnabledStopButton;
            set
            {
                isEnabledStopButton = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ServiceModel> services;
        public ObservableCollection<ServiceModel> Services
        {
            get => services;
            set
            {
                services = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
