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
 * The CondTag class represents an optional block of the template.
 * An NCondTag's children are only processed if its parameter
 * represents a "false" value in the data dictionary. See
 * Helpers.Truth() for the rules of truth.
 * 
 * The NCondEndTag represents the end of an NCondTag block.
 */

using System.Collections.Generic;
using System.IO;
using Raindrop.Backend.Parser;
using System;

namespace Raindrop.Backend.Tags
{
    [TagBuilder("ncond")]
    public class NCondTag
    {
        /// <summary>
        /// Builds an NCondTag.
        /// </summary>
        /// <param name="tag">
        /// The TagStruct to put information in.
        /// </param>
        /// <param name="reader">
        /// The InfoProvidingTextReader to read additional tags from.
        /// </param>
        public static void BuildTag(ref TagStruct tag, InfoProvidingTextReader reader)
        {
            Helpers.RequireParameter(tag.Param, reader);
            tag.ApplyMethod = ApplyTag;
            tag.Children = Helpers.GetChildren(reader, EndTagPredicate);
        }

        public static bool EndTagPredicate(TagStruct endTag)
        {
            if (endTag.Name == "/ncond") { return true; }
            else { return false; }
        }

        /// <summary>
        /// Applies the CondTag to the given data and outputs the result.
        /// </summary>
        /// <param name="tag">The TagStruct to apply.</param>
        /// <param name="output">The place to put the output.</param>
        /// <param name="data">The data to be applied to.</param>
        public static void ApplyTag(
            TagStruct tag,
            TextWriter output,
            IDictionary<string, object> data)
        {
            int index = 0;
            Helpers.RequireKey(tag.Param, data);
            if (!Helpers.Truth(tag.Param, data))
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
                index++;
            }
        }
    }

    [TagBuilder("/ncond")]
    public class NCondEndTag
    {
        /// <summary>
        /// Builds an NCondEndTag.
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
        /// Applies the CondEndTag to the given data and outputs the result.
        /// </summary>
        /// <param name="tag">The TagStruct to apply.</param>
        /// <param name="output">The place to put the output.</param>
        /// <param name="data">The data to be applied to.</param>
        public static void ApplyTag(
            TagStruct tag,
            TextWriter output,
            IDictionary<string, object> data)
        {
            throw new NotImplementedException(
                "NCondEndTag does not support being applied.");
        }
    }
}