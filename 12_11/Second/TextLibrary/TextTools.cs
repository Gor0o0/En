
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace TextLibrary
{
    public static class TextTools
    {
        public static bool IsPalindrome(string text)
        {
            string cleaned = new string(
                text.Where(char.IsLetterOrDigit)
                    .Select(char.ToLowerInvariant)
                    .ToArray()
            );

            string reversed = new string(cleaned.Reverse().ToArray());

            return cleaned == reversed;
        }

        public static int CountSentences(string text)
        {
            MatchCollection matches = Regex.Matches(
                text,
                @"[.!?]+(?=\s|$)"
            );

            return matches.Count;
        }

        public static string ReverseString(string text)
        {
            char[] characters = text.ToCharArray();

            Array.Reverse(characters);

            return new string(characters);
        }
    }
}
