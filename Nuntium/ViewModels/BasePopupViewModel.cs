using System;
using System.Windows;

namespace Nuntium
{
    public class BasePopupViewModel : BaseViewModel
    {

        #region Public Properties

        public HorizontalAlignment ArrowHorizontalAligment { get; set; } = HorizontalAlignment.Left;

        public VerticalAlignment ArrowVerticalAligment { get; set; } = VerticalAlignment.Bottom;

        #endregion
    }
}
        
            