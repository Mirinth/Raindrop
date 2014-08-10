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
    }
}