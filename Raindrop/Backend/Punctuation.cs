using System;

namespace Raindrop.Backend
{
    static class Punctuation
    {
        public static readonly string LeftCap = "<:";
        public static readonly string RightCap = ":>";
        public static readonly string Divider = " ";
        public static readonly char[] Trim = { ' ', '/' };

        public static readonly int Longest = Math.Max(Divider.Length,
                                                  Math.Max(LeftCap.Length,
                                                           RightCap.Length));

        public static bool StartsWith(this string str, string value, int index)
        {
            if (str.Length - value.Length < index)
            {
                return false;
            }

            for (int i = 0; i < value.Length && index + i < str.Length; i++)
            {
                if (str[index + i] != value[i]) { return false; }
            }

            return true;
        }
    }
}