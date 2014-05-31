/*
 * Copyright 2014
 * 
 * This file is part of the Raindrop Templating Library.
 * 
 * The Raindrop Templating Library is free software: you can redistribute
 * it and/or modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation, either version 3
 * of the License, or (at your option) any later version.
 * 
 * The Raindrop Templating Library is distributed in the hope that it will
 * be useful, but WITHOUT ANY WARRANTY; without even the implied warranty
 * of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with the Raindrop Templating Library. If not, see
 * <http://www.gnu.org/licenses/>. 
 */

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
