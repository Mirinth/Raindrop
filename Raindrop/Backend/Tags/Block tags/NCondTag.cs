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
 * An ncond tag represents an optional block of the template.
 * An ncond tag's children are only processed if its parameter
 * represents a "false" value in the data dictionary. See
 * Helpers.Truth() for the rules of truth.
 * 
 * The /ncond tag represents the end of an ncond tag block.
 */

using System;
using System.Collections.Generic;
using System.IO;
using Raindrop.Backend.Parser;
using Raindrop.Backend.Templater;

namespace Raindrop.Backend.Tags
{
    public class NCondTag : ITag
    {
        /// <summary>
        /// Builds an ncond tag.
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
            tag.Children = Helpers.GetChildren(reader, EndTagPredicate);
        }

        /// <summary>
        /// Determines whether a given TagStruct should be considered
        /// the end of the current block.
        /// </summary>
        /// <param name="tag">The TagStruct to test.</param>
        /// <returns>True if tag should end the block; else false.</returns>
        public static bool EndTagPredicate(TagStruct tag)
        {
            if (tag.Name == "/ncond") { return true; }
            else { return false; }
        }

        /// <summary>
        /// Applies the cond tag to the given data and outputs the result.
        /// </summary>
        /// <param name="tag">The tag to be applied.</param>
        /// <param name="output">The place to put the output.</param>
        /// <param name="data">The data to be applied to.</param>
        public void Apply(
            TagStruct tag,
            TextWriter output,
            IDictionary<string, object> data)
        {
            int index = 0;
            Helpers.RequireKey(tag.Param, data);
            if (!Helpers.Truth(tag.Param, data))
            {
                foreach (TagStruct child in tag.Children)
                {
                    TagApplyer.Apply(child, output, data);
                }
                index++;
            }
        }

        public string GetName() { return "ncond"; }
    }

    public class NCondEndTag : ITag
    {
        /// <summary>
        /// Builds a /cond tag.
        /// </summary>
        /// <param name="tag">
        /// The TagStruct to put information in.
        /// </param>
        /// <param name="reader">
        /// The InfoProvidingTextReader to read additional tags from.
        /// </param>
        public void Build(ref TagStruct tag, InfoProvidingTextReader reader)
        {
            // TagFactory already does everything.
        }

        /// <summary>
        /// Applies the /cond to the given data and outputs the result.
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
                "/ncond tag does not support being applied.");
        }

        public string GetName() { return "/ncond"; }
    }
}