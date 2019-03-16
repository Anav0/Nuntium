using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Nuntium.Core
{
    public static class StringExtentions
    {

        private static Random mRnd = new Random();

        public static string RemoveWhitespace(this string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !char.IsWhiteSpace(c))
                .ToArray());
        }

        public static string ToTitleCase(this string words)
        {
            if (string.IsNullOrEmpty(words))
                return words;

            return new CultureInfo("en-US").TextInfo.ToTitleCase(words.ToLower());
        }

        public static string GetInitials(this string words)
        {
            if (string.IsNullOrEmpty(words))
                return null;

            // first remove all: punctuation, separator chars, control chars, and numbers (unicode style regexes)
            string initials = Regex.Replace(words, @"[\p{P}\p{S}\p{C}\p{N}]+", "");

            // Replacing all possible whitespace/separator characters (unicode style), with a single, regular ascii space.
            initials = Regex.Replace(initials, @"\p{Z}+", " ");

            // Remove all Sr, Jr, I, II, III, IV, V, VI, VII, VIII, IX at the end of names
            initials = Regex.Replace(initials.Trim(), @"\s+(?:[JS]R|I{1,3}|I[VX]|VI{0,3})$", "", RegexOptions.IgnoreCase);

            // Extract up to 2 initials from the remaining cleaned name.
            initials = Regex.Replace(initials, @"^(\p{L})[^\s]*(?:\s+(?:\p{L}+\s+(?=\p{L}))?(?:(\p{L})\p{L}*)?)?$", "$1$2").Trim();

            if (initials.Length > 2)
            {
                // Worst case scenario, everything failed, just grab the first two letters of what we have left.
                initials = initials.Substring(0, 2);
            }

            return initials.ToUpperInvariant();
        }

        
    }
}
