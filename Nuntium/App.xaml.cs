using Ninject;
using Nuntium.Core;
using Prism.Events;
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

            Current.MainWindow = new MainWindow();

            //TODO: this is just a quick hack for fixing the issue width not binding TextEditor's editor at startup
            //IoC.Kernel.Get<TextEditorViewModel>().SelectedMenu = MenuCategories.Options;
            //IoC.Kernel.Get<TextEditorViewModel>().SelectedMenu = MenuCategories.Format;

            Current.MainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            IoC.Kernel.Dispose();
            base.OnExit(e);
        }

        private void SetupIoC()
        {
            IoC.Kernel.Bind<IEmailService>().To<MockedEmailService>();

            IoC.Kernel.Bind<IEventAggregator>().To<EventAggregator>();

            IoC.Kernel.Bind<AddressSectionViewModel>().To<AddressSectionViewModel>();

            IoC.Kernel.Bind<AttachmentsSectionViewModel>().To<AttachmentsSectionViewModel>();

            IoC.Kernel.Bind<SearchSectionViewModel>().To<SearchSectionViewModel>();

            IoC.Kernel.Bind<TextEditorViewModel>().To<TextEditorViewModel>();

            IoC.Kernel.Bind<FormattingSubmenuViewModel>().ToConstant(new FormattingSubmenuViewModel());

            IoC.Kernel.Bind<ApplicationViewModel>().ToConstant(new ApplicationViewModel());

            IoC.Kernel.Bind<SideMenuViewModel>().ToConstant(new SideMenuViewModel());

        }
    }
}
