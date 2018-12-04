
using Ninject;
using Nuntium.Core;
using System;
using System.Globalization;

namespace Nuntium
{
    public class MenuCategoriesToControlConverter : BaseValueConverter<MenuCategoriesToControlConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is MenuCategories category))
                return null;

            switch (category)
            {
                case MenuCategories.Format:
                    {
                        var output = new FormattingSubmenu();
                        output.DataContext = IoC.Kernel.Get<FormattingSubmenuViewModel>();
                        
                        //TODO: Try other method than action target
                        output.ActionTarget = IoC.Kernel.Get<CustomRichTextBox>();
                        return output;
                    }

                case MenuCategories.Insert:
                    {
                        var output = new InsertSubmenu();
                        var editor = IoC.Kernel.Get<CustomRichTextBox>();
                        var document = editor.Document;
                        var point = editor.GetEditorPointToScreen();
                        var selectedText = editor.Selection;

                        output.DataContext = new InsertSubmenuViewModel(document, point, selectedText);
                        return output;
                    }
                case MenuCategories.Options:
                    {
                        var output = new OptionsSubmenu();
                        output.DataContext = new OptionsSubmenuViewModel();
                        return output;
                    }
            }

            return null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
