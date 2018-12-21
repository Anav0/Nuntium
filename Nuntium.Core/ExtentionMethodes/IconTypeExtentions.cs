using Nuntium;
using Nuntium.Core;

namespace Nuntium.Core
{
    public static class IconTypeExtentions
    {

        public static string ToFontAwsome(this IconType type)
        {
            switch (type)
            {
                case IconType.Envelope:
                    return "\uf0e0";

                case IconType.Suitcase:
                    return "\uf0f2";

                case IconType.CaretLeft:
                    return "\uf104";

                case IconType.CaretRight:
                    return "\uf105";

                case IconType.Search:
                    return "\uf002";

                case IconType.Save:
                    return "\uf0c7";

                case IconType.Print:
                    return "\uf02f";

                case IconType.Inbox:
                    return "\uf01c";

                case IconType.File:
                    return "\uf15b";

                case IconType.PaperPlane:
                    return "\uf15b";

                case IconType.Ban:
                    return "\uf05e";

                case IconType.Star:
                    return "\uf005";

                case IconType.Archive:
                    return "\uf187";

                case IconType.Bin:
                    return "\uf1f8";
                default:
                    return null;
            }
        }
    }
}
