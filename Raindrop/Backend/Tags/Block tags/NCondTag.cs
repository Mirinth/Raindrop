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
 * NCondTag.cs
 * By Mirinth (mirinth@gmail.com)
 * 
 * The NCondTag file contains the NCondTag class, which represents an
 * optional tag. An NCond tag is only processed if its parameter
 * represents a "false" value in the data dictionary.
 */

using System.Collections.Generic;
using System.IO;
using Raindrop.Backend.Parser;

namespace Raindrop.Backend
{
    class NCondTag : BlockTag<NCondEndTag>
    {
        public static string ID = "ncond";

        /// <summary>
        /// The NCondTag constructor.
        /// </summary>
        /// <param name="param">The tag's parameter.</param>
        /// <param name="ts">A TagStream to construct child tags from.</param>
        public NCondTag(string param, TagStream ts)
            : base(param, ts)
        {
            RequireParameter(ts);
        }

        /// <summary>
        /// Applies the CondTag to the given data and outputs the result
        /// if the NCondTag is "true", else does nothing. The NCondTag is
        /// "true" if the data dictionary contains a value at the key
        /// matching the NCondTag's Param which is either a Boolean
        /// representing true, or an IEnumerable with at least one
        /// value in it. The NCondTag is false in all but these two
        /// conditions.
        /// </summary>
        /// <param name="data">The data to be applied to.</param>
        /// <param name="output">The place to put the output.</param>
        public override void Apply(
            IDictionary<string, object> data,
            TextWriter output)
        {
            if (!Helpers.Pass(data, Param))
            {
                base.Apply(data, output);
            }
        }
    }
}