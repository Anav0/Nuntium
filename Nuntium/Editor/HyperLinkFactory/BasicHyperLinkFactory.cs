
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using Nuntium.Core;

namespace Nuntium
{
    public class BasicHyperLinkFactory : IHyperLinkFactory
    {
        public Hyperlink CreateHyperLinkOnTopOfSelectedText(TextSelection selectedText, string linkUri)
        {
            var link = new Hyperlink(selectedText.Start, selectedText.End);
            link.IsEnabled = true;
            link.ToolTip = linkUri.RemoveWhitespace();

            try
            {
                var builder = new UriBuilder(linkUri);
                link.NavigateUri = builder.Uri;
            }
            catch (UriFormatException uriException)
            {
                MessageBox.Show(uriException.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }

            link.RequestNavigate += ((s, args) =>
            {
                try
                {
                    Process.Start(args.Uri.ToString());
                }
                catch (UriFormatException uriException)
                {
                    MessageBox.Show(uriException.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
            });


            return link;
        }
    }
}
