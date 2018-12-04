
using System.Windows.Documents;

namespace Nuntium
{
    public interface IHyperLinkFactory
    {
        Hyperlink CreateHyperLinkOnTopOfSelectedText(TextSelection selectedText, string linkUri);
    }
}
