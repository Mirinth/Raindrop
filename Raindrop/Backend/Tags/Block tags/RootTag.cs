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
 * RootTag.cs
 * By Mirinth (mirinth@gmail.com)
 * 
 * The RootTag file contains the RootTag class, which represents a
 * Tag containing other Tags. RootTag is just a generic container.
 */

using Raindrop.Backend.Parser;

namespace Raindrop.Backend.Tags
{
    class RootTag : BlockTag<EOFTag>
    {
        /// <summary>
        /// The RootTag constructor.
        /// </summary>
        /// <param name="param">The tag's parameter.</param>
        /// <param name="ts">A TagStream to construct child tags from.</param>
        public RootTag(string param, TagStream ts)
            : base(param, ts) { }
    }
}