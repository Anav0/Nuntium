using Ninject;
using Nuntium.Core;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using static Nuntium.ButtonStateHelper;

namespace Nuntium
{
    public class FormattingSubmenuViewModel : BaseViewModel
    {

        #region Public properties

        public bool IsAdditionalOptionVisible { get; set; }
        public bool IsAdditionalAlignOptionVisible { get; set; }

        public bool IsMarkerColorPickerVisible { get; set; }
        public bool IsFontColorPickerVisible { get; set; }

        public ObservableCollection<double> FontSizes { get; set; }

        public ObservableCollection<FontFamily> FontFamilies { get; set; }

        public ObservableCollection<double> SpacingOptions { get; set; }

        private double _SelectedFontSize = 12;
        public double SelectedFontSize { get => _SelectedFontSize;
            set
            {
                if (_SelectedFontSize == value)
                    return;

                _SelectedFontSize = value;

                ApplyFontSize(IoC.Kernel.Get<CustomRichTextBox>());
            }
        }

        private FontFamily _SelectedFontFamily;
        public FontFamily SelectedFontFamily { get => _SelectedFontFamily;
            set
            {
                if (_SelectedFontFamily == value)
                    return;

                _SelectedFontFamily = value;

                ApplyFontFamily(IoC.Kernel.Get<CustomRichTextBox>());
            }
        }

        private double _SelectedSpacing;
        public double SelectedSpacing
        {
            get => _SelectedSpacing;
            set
            {
                if (_SelectedSpacing == value)
                    return;

                _SelectedSpacing = value;

                SelectedIndex = SpacingOptions.IndexOf(_SelectedSpacing);

                IoC.Kernel.Get<IEventAggregator>().GetEvent<LineSpacingChangedEvent>().Publish(_SelectedSpacing * SelectedFontSize);
            }
        }

        public int SelectedIndex { get; set; }

        public ColorPickerViewModel ColorPickerVM { get; set; }

        public Brush SelectedMarkerColor { get; set; } = new SolidColorBrush(Colors.Yellow);

        public Brush SelectedFontColor { get; set; } = new SolidColorBrush(Colors.Black);

        #endregion

        public FormattingSubmenuViewModel()
        {
            ShowMarkerColorPickerCommand = new RelayCommand(ShowMarkerColorPicker);
            ShowFontColorPickerCommand = new RelayCommand(ShowFontColorPicker);

            ShowAdditionalOptionsCommand = new RelayCommand(ShowAdditionalOptions);
            ShowAdditionalAlignOptionsCommand = new RelayCommand(ShowAdditionalAlignOptions);

            ApplyMarkerColorCommand = new RelayCommandWithParameter((param) => { ApplyMarkerColor((RichTextBox)(param)); });
            ApplyFontColorCommand = new RelayCommandWithParameter((param)=> { ApplyFontColor((RichTextBox)(param)); });

            FontSizes = new ObservableCollection<double>();
            FillFontSizesList();

            FontFamilies = new ObservableCollection<FontFamily>(Fonts.SystemFontFamilies.OrderBy(x => x.ToString()));
            SelectedFontFamily = FontFamilies.FirstOrDefault(x => x.Source.ToString() == "Segoe UI");

            SpacingOptions = new ObservableCollection<double>();
            FillSpacingOptions();

            ColorPickerVM = new ColorPickerViewModel();
            ColorPickerVM.OnColorSelectionChanged += ((s, e) =>
            {
                if (IsFontColorPickerVisible)
                {
                    SelectedFontColor = ColorPickerVM.SelectedStandardBrush;
                    ApplyFontColor(IoC.Kernel.Get<CustomRichTextBox>());
                }
                else
                {
                    SelectedMarkerColor = ColorPickerVM.SelectedStandardBrush;
                    ApplyMarkerColor(IoC.Kernel.Get<CustomRichTextBox>());
                }

            });
            
        }

        #region Public Command

        public ICommand ShowMarkerColorPickerCommand { get; set; }

        public ICommand ShowFontColorPickerCommand { get; set; }

        public ICommand ShowAdditionalOptionsCommand { get; set; }

        public ICommand ShowAdditionalAlignOptionsCommand { get; set; }

        public ICommand ApplyMarkerColorCommand { get; set; }

        public ICommand ApplyFontColorCommand { get; set; }

        #endregion

        #region Command methods

        private void ShowMarkerColorPicker() => IsMarkerColorPickerVisible ^= true;

        private void ShowFontColorPicker() => IsFontColorPickerVisible ^= true;

        private void ShowAdditionalOptions() => IsAdditionalOptionVisible ^= true;

        private void ShowAdditionalAlignOptions() => IsAdditionalAlignOptionVisible ^= true;

        private void ApplyFontColor(RichTextBox editor)
        {
            try
            {
                 editor.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, SelectedFontColor);
            }
            catch (Exception ex)
            {
                //TODO: log this exception
            }
        }

        private void ApplyMarkerColor(RichTextBox editor)
        {
            try
            {
                if (editor.Selection.GetPropertyValue(TextElement.BackgroundProperty) == SelectedMarkerColor)
                    editor.Selection.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(Colors.Transparent));
                else
                    editor.Selection.ApplyPropertyValue(TextElement.BackgroundProperty, SelectedMarkerColor);
            }
            catch (Exception ex)
            {
                //TODO: log this exception
            }
        }

        private void ApplyFontSize(RichTextBox editor)
        {
            editor.Selection?.ApplyPropertyValue(TextElement.FontSizeProperty, SelectedFontSize);
            editor.Selection?.Select(editor.Selection.Start, editor.Selection.End);
        }

        private void ApplyFontFamily(RichTextBox editor)
        {
            if (SelectedFontFamily == null)
                return;

            editor.Selection?.ApplyPropertyValue(TextElement.FontFamilyProperty, SelectedFontFamily);
            editor.Selection?.Select(editor.Selection.Start, editor.Selection.End);
        }

        #endregion

        #region Private methods

        //TODO: Find good way to update buttons state
        private void FillFontSizesList()
        {
            FontSizes.Add(8);
            FontSizes.Add(9);
            FontSizes.Add(10);
            FontSizes.Add(11);

            for (int i = 12; i < 28; i += 2)
            {
                FontSizes.Add(i);
            }

            FontSizes.Add(36);
            FontSizes.Add(48);
            FontSizes.Add(72);
        }

        private void FillSpacingOptions()
        {
            SpacingOptions.Add(0.5d);
            SpacingOptions.Add(1.0d);
            SpacingOptions.Add(1.15d);
            SpacingOptions.Add(1.50d);
            SpacingOptions.Add(2.0d);
            SpacingOptions.Add(2.5d);
            SpacingOptions.Add(3.0d);
        }

        #endregion
    }
}
