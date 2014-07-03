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
 * The block tag represents a generic tag that contains children.
 * 
 * The eof tag represents the end of the template file.
 */

using System;
using System.Collections.Generic;
using System.IO;
using Raindrop.Backend.Lexer;

namespace Raindrop.Backend.Tags
{
    public class BlockTag : ITag
    {
        /// <summary>
        /// Builds a block tag.
        /// </summary>
        /// <param name="tag">
        /// The TagStruct to put information in.
        /// </param>
        /// <param name="reader">
        /// The TagReader to read additional tags from.
        /// </param>
        public void Build(ref TagStruct tag, TagReader reader)
        {
            Helpers.RequireParameter(tag.Param, reader);
            tag.ApplyMethod = Apply;

            try
            {
                tag.Children = Helpers.GetChildren(reader, EndTagPredicate);
            }
            catch (RaindropException exc)
            {
                exc["raindrop.template-name"] = tag.Param;
                throw;
            }
        }

        /// <summary>
        /// Determines whether a given TagStruct should be considered
        /// the end of the current block.
        /// </summary>
        /// <param name="tag">The TagStruct to test.</param>
        /// <returns>True if tag should end the block; else false.</returns>
        public static bool EndTagPredicate(TagStruct endTag)
        {
            if (endTag.Name == "eof") { return true; }
            else { return false; }
        }

        /// <summary>
        /// Applies the block tag to the given data and outputs the result.
        /// </summary>
        /// <param name="tag">The tag to be applied.</param>
        /// <param name="output">The place to put the output.</param>
        /// <param name="data">The data to be applied to.</param>
        public void Apply(
            TagStruct tag,
            TextWriter output,
            IDictionary<string, object> data)
        {
            try
            {
                foreach (TagStruct child in tag.Children)
                {
                    child.Apply(output, data);
                }
            }
            catch (RaindropException exc)
            {
                exc["raindrop.template-name"] = tag.Param;
                throw;
            }
        }

        public string GetName() { return "block"; }
    }

    public class EofTag : ITag
    {
        /// <summary>
        /// Builds an eof tag.
        /// </summary>
        /// <param name="tag">
        /// The TagStruct to put information in.
        /// </param>
        /// <param name="reader">
        /// The TagReader to read additional tags from.
        /// </param>
        public void Build(ref TagStruct tag, TagReader reader)
        {
            tag.ApplyMethod = Apply;
        }

        /// <summary>
        /// Applies the eof tag to the given data and outputs the result.
        /// </summary>
        /// <param name="tag">The tag to be applied.</param>
        /// <param name="output">The place to put the output.</param>
        /// <param name="data">The data to be applied to.</param>
        public void Apply(
            TagStruct tag,
            TextWriter output,
            IDictionary<string, object> data)
        {
            throw new NotImplementedException(
                "EofTag does not support being applied.");
        }

        public string GetName() { return "eof"; }
    }
}