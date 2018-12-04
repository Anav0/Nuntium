
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;

namespace Nuntium
{
    public static class TextSelectionExtentions
    {
        public static bool CheckForStyle(this TextSelection selectedText, TextMarkerStyle expectedStyle)
        {

            if (selectedText == null)
                return false;

            var startParagraph = selectedText.Start.Paragraph;
            var endParagraph = selectedText.End.Paragraph;

            if (startParagraph != null && endParagraph != null && (startParagraph.Parent is ListItem) && (endParagraph.Parent is ListItem) && object.ReferenceEquals(((ListItem)startParagraph.Parent).List, ((ListItem)endParagraph.Parent).List))
            {
                TextMarkerStyle markerStyle = ((ListItem)startParagraph.Parent).List.MarkerStyle;

                if (markerStyle == expectedStyle)
                    return true;
                else return false;
            }
            else
            {
                return false;
            }
        }

        public static List<Hyperlink> GetHyperlinksFromSelection(this TextSelection pos)
        {
            if (pos?.Start.Paragraph == null) return null;

            return FindDependencyObjects<Hyperlink>(pos.Start, pos.End);
        }

        private static List<T> FindDependencyObjects<T>(TextPointer topPos, TextPointer bottomPos) 
            where T : DependencyObject
        {
            var results = new List<T>();

            while (topPos != null && topPos.CompareTo(bottomPos) <= 0)
            {
                DependencyObject depObj = topPos.GetAdjacentElement(LogicalDirection.Forward);

                if (depObj is T)
                {
                    if(!results.Contains((T)depObj))
                        results.Add((T)depObj);
                }

                topPos = topPos.GetNextContextPosition(LogicalDirection.Forward);
            }

            return results;
        }
    }
}
