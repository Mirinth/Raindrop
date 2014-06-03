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
 * CondTag.cs
 * By Mirinth (mirinth@gmail.com)
 * 
 * The CondTag file contains the CondTag class, which represents an
 * optional tag. A CondTag tag is only processed if its parameter
 * represents a "true" value in the data dictionary.
 */

using System.Collections.Generic;
using System.IO;

namespace Raindrop.Backend
{
    class CondTag : BlockTag<CondEndTag>
    {
        public static string ID = "<:cond";

        /// <summary>
        /// The CondTag constructor.
        /// </summary>
        /// <param name="ts">A TagStream to construct the CondTag from.</param>
        public CondTag(TagStream ts)
            : base(ts)
        {
            RequireParameter(ts);
        }

        /// <summary>
        /// Applies the CondTag to the given data and outputs the result
        /// if the CondTag is "true", else does nothing. The CondTag is
        /// "true" if the data dictionary contains a value at the key
        /// matching the CondTag's Param which is either a Boolean
        /// representing true, or an IEnumerable with at least one
        /// value in it. The CondTag is false in all but these two
        /// conditions.
        /// </summary>
        /// <param name="data">The data to be applied to.</param>
        /// <param name="output">The place to put the output.</param>
        public override void Apply(
            IDictionary<string, object> data,
            TextWriter output)
        {
            if (Helpers.Pass(data, Param))
            {
                base.Apply(data, output);
            }
        }
    }
}