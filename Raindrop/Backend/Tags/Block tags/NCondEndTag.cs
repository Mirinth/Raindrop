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
 * ArrayEndTag.cs
 * By Mirinth (mirinth@gmail.com)
 * 
 * The NCondEndTag file contains the NCondEndTag class, which represents
 * the end of an NCondTag block.
 */

namespace Raindrop.Backend
{
    class NCondEndTag : EndTag
    {
        public static string ID = "/ncond";

        /// <summary>
        /// The ArrayEndTag constructor.
        /// </summary>
        /// <param name="param">The tag's parameter.</param>
        /// <param name="ts">A TagStream to construct child tags from.</param>
        public NCondEndTag(string param, TagStream ts)
            : base(param, ts) { }
    }
}