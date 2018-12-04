
using System;
using System.Windows.Controls;

namespace Nuntium
{
    public class ControlEventArgs : EventArgs
    {
        public Control control;

        public ControlEventArgs(Control control)
        {
            this.control = control;
        }
    }
}
