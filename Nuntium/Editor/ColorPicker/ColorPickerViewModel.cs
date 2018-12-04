
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Nuntium
{
    public class ColorPickerViewModel : BaseViewModel
    {

        #region Public properties

        public ObservableCollection<SolidColorBrush> StandardColors { get; set; }

        public SolidColorBrush SelectedStandardBrush { get; set; }

        #endregion

        public ColorPickerViewModel()
        {
            StandardColorClickedCommand = new RelayCommandWithParameter((param) => ChangeSelectedColor(param));

            StandardColors = new ObservableCollection<SolidColorBrush>();

            FillStandardColorsList();
        }

        #region Public Commands

        public ICommand StandardColorClickedCommand { get; set; }

        #endregion

        #region Events

        public event EventHandler OnColorSelectionChanged;

        #endregion

        #region Event raisers

        private void RaiseOnColorSelectionChanged() => OnColorSelectionChanged?.Invoke(this, new EventArgs());

        #endregion

        #region Command methods

        private void ChangeSelectedColor(object param)
        {
            if (!(param is SolidColorBrush selectedBursh))
                return;

            if (SelectedStandardBrush == selectedBursh)
                return;

            SelectedStandardBrush = selectedBursh;
            RaiseOnColorSelectionChanged();
        }

        #endregion

        #region Private methods

        private void FillStandardColorsList()
        {
            StandardColors.Add(new SolidColorBrush(Colors.DarkRed));
            StandardColors.Add(new SolidColorBrush(Colors.Red));
            StandardColors.Add(new SolidColorBrush(Colors.Orange));
            StandardColors.Add(new SolidColorBrush(Colors.Yellow));
            StandardColors.Add(new SolidColorBrush(Colors.LightGreen));
            StandardColors.Add(new SolidColorBrush(Colors.ForestGreen));
            StandardColors.Add(new SolidColorBrush(Colors.LightSkyBlue));
            StandardColors.Add(new SolidColorBrush(Colors.RoyalBlue));
            StandardColors.Add(new SolidColorBrush(Colors.DarkSlateBlue));
            StandardColors.Add(new SolidColorBrush(Colors.BlueViolet));
        }

        #endregion
    }
}
