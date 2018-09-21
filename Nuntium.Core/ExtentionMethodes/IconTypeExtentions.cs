using Nuntium;
using Nuntium.Core;

namespace Nuntium.Core
{
    public static class IconTypeExtentions
    {

        public static string ToFontAwsome(this IconType type)
        {
            switch(type)
            {
                case IconType.Bell:
                    return "\uf0f3";

                case IconType.Comments:
                    return "\uf27a";

                default:
                    return null;
            }
        }
    }
}
