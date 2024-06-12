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
using System.ServiceProcess;
using System.Diagnostics;

namespace _16_TASK_WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ServiceController myService;
        private string serviceName = "MonitorService";
        private string serviceExecutablePath = @"D:\С#\PRACTICE\16_TASK_SERV\bin\Debug\net8.0\16_TASK_SERV.exe";

        public MainWindow()
        {
            InitializeComponent();
            myService = new ServiceController(serviceName);
            UpdateServiceInfo();
        }

        private void UpdateServiceInfo()
        {
            try
            {
                Text_Info.Text = $"Service Status: {myService.Status}\n" +
                                 $"Can Stop: {myService.CanStop}\n" +
                                 $"Can Pause and Continue: {myService.CanPauseAndContinue}\n" +
                                 $"Can ShutDown: {myService.CanShutdown}\n" +
                                 $"Service Type: {myService.ServiceType}";
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void Button_Install_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Verb = "runas", // запуск с правами администратора
                Arguments = $"/C sc create {serviceName} binPath= \"{serviceExecutablePath}\" start= auto",
                WindowStyle = ProcessWindowStyle.Hidden
            });
            UpdateServiceInfo();
        }

        private void Button_Unistall_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Verb = "runas", // запуск с правами администратора
                Arguments = $"/C sc delete {serviceName}",
                WindowStyle = ProcessWindowStyle.Hidden
            });
            UpdateServiceInfo();
        }

        private void Button_Start_Serv_Click(object sender, RoutedEventArgs e)
        {
            myService.Start();
            myService.WaitForStatus(ServiceControllerStatus.Running);
            UpdateServiceInfo();
        }

        private void Button_Stop_Serv_Click(object sender, RoutedEventArgs e)
        {
            myService.Stop();
            myService.WaitForStatus(ServiceControllerStatus.Stopped);
            UpdateServiceInfo();
        }
    }
}
