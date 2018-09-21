using Ninject;
using Nuntium.Core;

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

    }
}
