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

using System.Collections.Generic;
using System.IO;

namespace Raindrop.Backend
{
    class RootTag : BlockTag<EOFTag>
    {
        /// <summary>
        /// The RootTag constructor.
        /// </summary>
        /// <param name="param">The parameter to use for the RootTag.</param>
        /// <param name="children">A List of child Tags for the RootTag to contain.</param>
        public RootTag(TagStream ts)
            : base(ts, false) { }
    }
}