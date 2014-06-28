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
 * A text tag represents a block of plain text in the template.
 */

using System.Collections.Generic;
using System.IO;
using Raindrop.Backend.Parser;

namespace Raindrop.Backend.Tags
{
    [TagBuilder("")]
    public class TextTag
    {
        /// <summary>
        /// Builds a text tag.
        /// </summary>
        /// <param name="tag">
        /// The TagStruct to put information in.
        /// </param>
        /// <param name="reader">
        /// The InfoProvidingTextReader to read additional tags from.
        /// </param>
        public static void BuildTag(ref TagStruct tag, InfoProvidingTextReader reader)
        {
            tag.ApplyMethod = ApplyTag;
        }

        /// <summary>
        /// Applies a text tag to the given data and outputs the result.
        /// </summary>
        /// <param name="tag">The tag to be applied.</param>
        /// <param name="output">The place to put the output.</param>
        /// <param name="data">The data to be applied to.</param>
        public static void ApplyTag(
            TagStruct tag,
            TextWriter output,
            IDictionary<string, object> data)
        {
            output.Write(tag.Param);
        }
    }
}