
using Nuntium.Core;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Nuntium
{
    public static class RichTextBoxExtentions
    {
        public static System.Windows.Point GetEditorPointToScreen(this RichTextBox editor)
        {
            var rect = editor.CaretPosition.GetCharacterRect(LogicalDirection.Backward);

            if (rect.IsEmpty)
                return new System.Windows.Point();

            return editor.PointToScreen(rect.BottomRight);
        }

        public static SpellcheckResult SpellCheck(this RichTextBox editor, int startingPoint = 0)
        {
            editor.SelectAll();

            SpellcheckResult output = new SpellcheckResult();

            for (int i = startingPoint; i < editor.Selection.Text.Length; i++)
            {
                //Get starting insertion point
                TextPointer start = editor.Document.ContentStart.GetNextInsertionPosition(LogicalDirection.Forward).GetPositionAtOffset(i, LogicalDirection.Forward);

                //Check for errors at start
                SpellingError spellingError = editor.GetSpellingError(start);

                //if there is misspelling
                if (spellingError != null)
                {
                    //get range of error
                    int errRange = editor.GetSpellingErrorRange(start).Text.Length;
                    TextPointer end = editor.Document.ContentStart.GetNextInsertionPosition(LogicalDirection.Forward).GetPositionAtOffset(i + errRange, LogicalDirection.Forward);

                    //focus editor
                    editor.Focus();

                    //select text containing error
                    editor.Selection.Select(start, end);

                    output.endPosition = editor.Document.ContentStart.GetOffsetToPosition(end);
                    output.SpellingErrors = spellingError.Suggestions;

                    return output;

                }
            }

            return output;

        }
    }
}
