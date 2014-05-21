using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raindrop
{
    partial class Raindrop
    {
        private class Settings
        {
            public enum MissingKeyFailMode
            {
                Crash,
                Ignore
            }

            public static string LeftCap = "<:";
            public static string RightCap = ":>";
            public static char TagSplitter = ' ';
            public static char[] TrimChars = { ' ', '/' };
            public static MissingKeyFailMode FailMode = MissingKeyFailMode.Ignore;
        }
    }
}
