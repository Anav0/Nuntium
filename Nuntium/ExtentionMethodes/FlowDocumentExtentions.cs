
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace Nuntium
{
    public static class FlowDocumentExtentions
    {

        //public static List<Hyperlink> GetHyperLinksContainingSelection(this TextSelection pos)
        //{
        //    var results = new List<Hyperlink>();

        //    foreach (var inline in pos.Start.Paragraph.Inlines)
        //    {
        //        if (inline is Hyperlink link)
        //        {
        //            try
        //            {
        //                if (link.Inlines.Contains((Inline)pos.Start.Parent))
        //                    results.Add(link);
        //            }
        //            catch (InvalidCastException ex)
        //            {
        //                MessageBox.Show(ex.ToString());
        //            }
        //        }
        //    }

        //    return results;
        //}

        public static void InsertAdornedImage(this FlowDocument document, System.Windows.Controls.Image image)
        {
            image.LostFocus += OnLostFocus;
            image.MouseDown += OnMouseDown;
            image.Stretch = System.Windows.Media.Stretch.None;

            var imageContainer = new BlockUIContainer(image);
            document.Blocks.Add(imageContainer);
        }


       

        private static void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var image = (System.Windows.Controls.Image)sender;
            AdornerLayer.GetAdornerLayer(image).Add(new ResizingAdorner(image));
        }

        private static void OnLostFocus(object sender, RoutedEventArgs e)
        {
            var image = (System.Windows.Controls.Image)sender;
            var layer = AdornerLayer.GetAdornerLayer(image);
            var toRemove = layer.GetAdorners(image);

            if (toRemove != null)
            {
                foreach (var item in toRemove)
                {
                    layer.Remove(item);
                }
            }
        }
    }
}
