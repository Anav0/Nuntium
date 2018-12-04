
using System.Collections.Generic;

namespace Nuntium.Core
{
    public class SpellcheckResult
    {
        public IEnumerable<string> SpellingErrors;

        public int endPosition;
    }
}
