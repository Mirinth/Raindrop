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
 * The cond tag represents an optional block of the template.
 * A cond tag's children are only processed if its parameter
 * represents a "true" value in the data dictionary. See
 * Helpers.Truth() for the rules of truth.
 * 
 * The /cond tag represents the end of a cond tag block.
 */

using System;
using System.Collections.Generic;
using System.IO;

namespace Raindrop.Backend.Tags
{
    public class CondTag : ITag
    {
        /// <summary>
        /// Builds a cond tag.
        /// </summary>
        /// <param name="td">Information about the tag to build.</param>
        public TagStruct Build(TagData td)
        {
            Helpers.RequireParameter(td.Param, td.Reader);
            List<TagStruct> childTags = Helpers.GetChildren(td.Reader, EndTagPredicate);

            return new TagStruct()
            {
                ApplyMethod = Apply,
                Children = childTags,
                Name = td.Name,
                Param = td.Param
            };
        }

        /// <summary>
        /// Determines whether a given TagStruct should be considered
        /// the end of the current block.
        /// </summary>
        /// <param name="tag">The TagStruct to test.</param>
        /// <returns>True if tag should end the block; else false.</returns>
        public static bool EndTagPredicate(TagStruct tag)
        {
            if (tag.Name == "/cond") { return true; }
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
            if (Helpers.Truth(tag.Param, data))
            {
                foreach (TagStruct child in tag.Children)
                {
                    child.Apply(output, data);
                }
                index++;
            }
        }

        public string GetName() { return "cond"; }
    }

    public class CondEndTag : ITag
    {
        /// <summary>
        /// Builds a /cond tag.
        /// </summary>
        /// <param name="td">Information about the tag to build.</param>
        public TagStruct Build(TagData td)
        {
            return new TagStruct()
            {
                ApplyMethod = Apply,
                Children = null,
                Name = td.Name,
                Param = td.Param
            };
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
            // TODO: Fix reference to non-existing CondEndTag
            throw new NotImplementedException(
                "CondEndTag does not support being applied.");
        }

        public string GetName() { return "/cond"; }
    }
}