using Nuntium.Core;
using System.Windows;

namespace Nuntium
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            SetupIoC();

            Logger.DeleteLogFile();

            Current.MainWindow = new MainWindow();
            Current.MainWindow.Show();
        }

        private void SetupIoC()
        {
            IoC.Kernel.Bind<ApplicationViewModel>().ToConstant(new ApplicationViewModel());

            IoC.Kernel.Bind<SideMenuViewModel>().ToConstant(new SideMenuViewModel());
        }
    }
}
