
using System.Windows.Documents;

namespace Nuntium
{
    public interface IMarkupConverter
    {
        string ConvertXamlToHtml(string xamlText);

        string ConvertHtmlToXaml(string htmlText);

        string ConvertRtfToHtml(FlowDocument doc);

        string ConvertHtmlToRtf(string htmlText);
    }
}
