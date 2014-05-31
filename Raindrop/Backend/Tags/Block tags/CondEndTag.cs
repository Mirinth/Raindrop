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
 * The CondEndTag file contains the CondEndTag class, which represents
 * the end of a CondTag block.
 */

using System.IO;
using System.Web.Mvc;

namespace Raindrop.Backend
{
    class CondEndTag : EndTag
    {
        public static string ID = "<:/cond";

        /// <summary>
        /// The ArrayEndTag constructor.
        /// </summary>
        /// <param name="ts">A TagStream to construct the EndTag from.</param>
        public CondEndTag(TagStream ts)
            : base(ts) { }
    }
}