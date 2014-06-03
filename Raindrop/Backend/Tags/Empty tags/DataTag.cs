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
 * Tag.cs
 * By Mirinth (mirinth@gmail.com)
 * 
 * The Tag file contains the Tag class, which represents a
 * generic tag.
 */

using System.Collections.Generic;
using System.IO;

namespace Raindrop.Backend
{
    class DataTag : Tag
    {
        public static string ID = "<:data";

        /// <summary>
        /// The DataTag constructor.
        /// </summary>
        /// <param name="ts">A TagStream to construct the DataTag from.</param>
        public DataTag(TagStream ts)
            : base(ts)
        {
            if (Param == null)
            {
                throw new RaindropException(
                    "DataTag has no parameter.",
                    ts.Name,
                    ts.Index,
                    ErrorCode.ParameterMissing);
            }
        }

        /// <summary>
        /// Applies the DataTag to the given data and outputs the result.
        /// </summary>
        /// <param name="data">The data to be applied to.</param>
        /// <param name="output">The place to put the output.</param>
        public override void Apply(
            IDictionary<string, object> data,
            TextWriter output)
        {
            if (data.ContainsKey(Param))
            {
                output.Write(data[Param]);
            }
        }
    }
}