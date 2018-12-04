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

            Logger.DeleteLogFile();

            Current.MainWindow = new MainWindow();

            //TODO: this is just a quick hack for fixing the issue width not binding TextEditor's editor at startup
            IoC.Kernel.Get<TextEditorViewModel>().SelectedMenu = MenuCategories.Options;
            IoC.Kernel.Get<TextEditorViewModel>().SelectedMenu = MenuCategories.Format;

            Current.MainWindow.Show();
        }

        private void SetupIoC()
        {
            IoC.Kernel.Bind<ApplicationViewModel>().ToConstant(new ApplicationViewModel());

            IoC.Kernel.Bind<SideMenuViewModel>().ToConstant(new SideMenuViewModel());

            IoC.Kernel.Bind<IEventAggregator>().ToConstant(new EventAggregator());

            IoC.Kernel.Bind<TextEditorViewModel>().ToConstant(new TextEditorViewModel());

            IoC.Kernel.Bind<AddressSectionViewModel>().ToConstant(new AddressSectionViewModel());

            IoC.Kernel.Bind<AttachmentsSectionViewModel>().ToConstant(new AttachmentsSectionViewModel());

            IoC.Kernel.Bind<SearchSectionViewModel>().ToConstant(new SearchSectionViewModel());

            IoC.Kernel.Bind<FormattingSubmenuViewModel>().ToConstant(new FormattingSubmenuViewModel());
        }
    }
}
