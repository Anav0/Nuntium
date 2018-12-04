using Ninject;
using Nuntium.Core;
using Prism.Events;

namespace Nuntium
{
    /// <summary>
    /// Shortcut for accesing various view model instances in xaml.
    /// </summary>
    public class ConstantViewModels
    {
        public static ConstantViewModels Instance { get; private set; } = new ConstantViewModels();

        public ApplicationViewModel ApplicationViewModelInstance { get; private set; } = IoC.Kernel.Get<ApplicationViewModel>();

        public NotificationIconViewModel NotificationIconViewModelInstance { get; private set; } = IoC.Kernel.Get<NotificationIconViewModel>();

        public SideMenuViewModel SideMenuViewModelInstance { get; private set; } = IoC.Kernel.Get<SideMenuViewModel>();

        public IEventAggregator EventAggregatorInstance { get; private set; } = IoC.Kernel.Get<IEventAggregator>();

        public TextEditorViewModel TextEditorVM { get; private set; } = IoC.Kernel.Get<TextEditorViewModel>();

        public AttachmentsSectionViewModel AttachmentsSectionVM { get; private set; } = IoC.Kernel.Get<AttachmentsSectionViewModel>();

        public AddressSectionViewModel AddressSectionVM { get; private set; } = IoC.Kernel.Get<AddressSectionViewModel>();

        public SearchSectionViewModel SearchSectionVM { get; private set; } = IoC.Kernel.Get<SearchSectionViewModel>();
    }
}
