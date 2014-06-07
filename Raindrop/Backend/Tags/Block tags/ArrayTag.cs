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
 * ArrayTag.cs
 * By Mirinth (mirinth@gmail.com)
 * 
 * The ArrayTag file contains the ArrayTag class, which represents a
 * repeated block of text in the template.
 */

using System.Collections.Generic;
using System.IO;
using Raindrop.Backend.Parser;

namespace Raindrop.Backend.Tags
{
    class ArrayTag : BlockTag<ArrayEndTag>
    {
        public static string ID = "array";

        /// <summary>
        /// The ArrayTag constructor.
        /// </summary>
        /// <param name="param">The tag's parameter.</param>
        /// <param name="ts">A TagStream to construct child tags from.</param>
        public ArrayTag(string param, TagStream ts)
            : base(param, ts)
        {
            RequireParameter(ts);
        }

        /// <summary>
        /// Applies the ArrayTag to the given data and outputs the result.
        /// </summary>
        /// <param name="data">The data to be applied to.</param>
        /// <param name="output">The place to put the output.</param>
        public override void Apply(
            IDictionary<string, object> data,
            TextWriter output)
        {
            if (!data.ContainsKey(Param))
            {
                KeyMissing();
                return;
            }

            IEnumerable<IDictionary<string, object>> items =
                (IEnumerable<IDictionary<string, object>>)data[Param];

            foreach (IDictionary<string, object> item in items)
            {
                base.Apply(item, output);
            }
        }
    }
}