using Ninject;

namespace Nuntium.Core
{
    public static class IoC
    {
        #region Public Properties

        public static IKernel Kernel { get; private set; } = new StandardKernel();

        #endregion
    }
}
