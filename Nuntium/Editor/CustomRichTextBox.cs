
using Nuntium.Core;
using System;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Nuntium
{
    public class CustomRichTextBox : RichTextBox
    {
        public CustomRichTextBox()
        {
            DataObject.AddPastingHandler(this, OnPaste);
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            var data = Clipboard.GetImage();//e.SourceDataObject.GetData(DataFormats.Bitmap);

            if (data != null)
            {
                var image = new System.Windows.Controls.Image { Source = data, };

                Document.InsertAdornedImage(image);

                //Cancel so we can add image with hooked events instead
                e.CancelCommand();
            }
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            var changeList = e.Changes.ToList();
            if (changeList.Count > 0)
            {
                foreach (var change in changeList)
                {
                    TextPointer start = null;
                    TextPointer end = null;
                    if (change.AddedLength > 0)
                    {
                        start = Document.ContentStart.GetPositionAtOffset(change.Offset);
                        end = Document.ContentStart.GetPositionAtOffset(change.Offset + change.AddedLength);
                    }
                    else
                    {
                        int startOffset = Math.Max(change.Offset - change.RemovedLength, 0);
                        start = Document.ContentStart.GetPositionAtOffset(startOffset);
                        end = Document.ContentStart.GetPositionAtOffset(change.Offset);
                    }

                    if (start != null && end != null)
                    {
                        var range = new TextRange(start, end);
                        range.ApplyPropertyValue(LanguageProperty, Document.Language);
                    }
                }
            }
            base.OnTextChanged(e);
        }
    }
}

