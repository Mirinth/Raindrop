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
 * A data tag inserts a piece of server-provided data into the
 * output stream.
 */

using System.Collections.Generic;
using System.IO;
using Raindrop.Backend.Parser;

namespace Raindrop.Backend.Tags
{
    public class DataTag : ITag
    {
        /// <summary>
        /// Builds a data tag.
        /// </summary>
        /// <param name="tag">
        /// The TagStruct to put information in.
        /// </param>
        /// <param name="reader">
        /// The InfoProvidingTextReader to read additional tags from.
        /// </param>
        public void Build(ref TagStruct tag, InfoProvidingTextReader reader)
        {
            Helpers.RequireParameter(tag.Param, reader);
            tag.ApplyMethod = Apply;
        }

        /// <summary>
        /// Applies the data tag to the given data and outputs the result.
        /// </summary>
        /// <param name="tag">The tag to be applied.</param>
        /// <param name="output">The place to put the output.</param>
        /// <param name="data">The data to be applied to.</param>
        public void Apply(
            TagStruct tag,
            TextWriter output,
            IDictionary<string, object> data)
        {
            Helpers.RequireKey(tag.Param, data);
            output.Write(data[tag.Param]);
        }

        public string GetName() { return "data"; }
    }
}