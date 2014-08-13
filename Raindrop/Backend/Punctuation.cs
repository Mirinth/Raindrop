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

/*
 * Contains the various bits of Raindrop's punctuation in one
 * central place so changes to it automaticalaly update everything
 * that relies on punctuation.
 */

using System;

namespace Raindrop.Backend
{
    static class Punctuation
    {
        public static readonly string LeftCap = "<:";
        public static readonly string RightCap = ":>";
        public static readonly string Divider = " ";
        public static readonly char[] ParamTrim = { ' ', '\r', '\n', '\t', '/' };
        public static readonly char[] NameTrim = { ' ', '\r', '\n', '\t' };

        public static readonly int Longest = Math.Max(Divider.Length,
                                                  Math.Max(LeftCap.Length,
                                                           RightCap.Length));
    }
}