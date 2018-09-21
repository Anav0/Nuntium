using Ninject;
using Nuntium.Core;
using System.ComponentModel;
using System.Windows.Controls;

namespace Nuntium
{
    public class BasePage : UserControl
    {
        #region Private Member

        private object mViewModel;

        #endregion

        #region Public Properties

        public object ViewModelObject
        {
            get => mViewModel;
            set
            {
                // If nothing has changed, return
                if (mViewModel == value)
                    return;

                // Update the value
                mViewModel = value;

                // Fire the view model changed method
                OnViewModelChanged();

                // Set the data context for this page
                DataContext = mViewModel;
            }
        }

        #endregion

        #region Constructor

        public BasePage()
        {
            // Don't animate in design time
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
        }

        #endregion

        protected virtual void OnViewModelChanged()
        {

        }
    }

    public class BasePage<VM> : BasePage
        where VM : BaseViewModel, new()
    {
        #region Public Properties

        public VM ViewModel
        {
            get => (VM)ViewModelObject;
            set => ViewModelObject = value;
        }

        #endregion

        #region Constructor

        public BasePage() : base()
        {
            // If in design time mode...
            if (DesignerProperties.GetIsInDesignMode(this))
                // Just use a new instance of the VM
                ViewModel = new VM();
            else
                // Get vm from DI or create a new one
                ViewModel = IoC.Kernel.Get<VM>() ?? new VM();
        }

        public BasePage(VM specificViewModel = null) : base()
        {
            // Set specific view model
            if (specificViewModel != null)
                ViewModel = specificViewModel;
            else
            {
                // If in design time mode...
                if (DesignerProperties.GetIsInDesignMode(this))
                    // Just use a new instance of the VM
                    ViewModel = new VM();
                else
                {
                    // Get vm from DI or create a new one
                    ViewModel = IoC.Kernel.Get<VM>() ?? new VM();
                }
            }
        }

        #endregion
    }
}
