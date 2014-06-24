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
 * The CondTag class represents an optional block of the template.
 * A CondTag's children are only processed if its parameter
 * represents a "true" value in the data dictionary. See
 * Helpers.Truth() for the rules of truth.
 * 
 * The CondEndTag represents the end of a CondTag block.
 */

using System.Collections.Generic;
using System.IO;
using Raindrop.Backend.Parser;

namespace Raindrop.Backend.Tags
{
    public class CondTag : BlockTag<CondEndTag>
    {
        public static string ID = "cond";

        /// <summary>
        /// The CondTag constructor.
        /// </summary>
        /// <param name="param">The tag's parameter.</param>
        /// <param name="ts">A TagStream to construct child tags from.</param>
        public CondTag(string param, InfoProvidingTextReader ts)
            : base(param, ts)
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
            RequireKey(Param, data);
            if (Helpers.Truth(data, Param))
            {
                base.Apply(data, output);
            }
        }
    }

    public class CondEndTag : EndTag
    {
        public static string ID = "/cond";

        /// <summary>
        /// The ArrayEndTag constructor.
        /// </summary>
        /// <param name="param">The tag's parameter.</param>
        /// <param name="ts">A TagStream to construct child tags from.</param>
        public CondEndTag(string param, InfoProvidingTextReader ts)
            : base(param, ts) { }
    }
}