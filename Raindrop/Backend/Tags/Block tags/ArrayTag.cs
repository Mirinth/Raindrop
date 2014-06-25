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
 * The ArrayTag class represents a repeated block of tags in the template.
 * 
 * The ArrayEndTag class represents the end of an ArrayTag block.
 */

using System.Collections.Generic;
using System.IO;
using Raindrop.Backend.Parser;
using System;

namespace Raindrop.Backend.Tags
{
    [TagBuilder("array")]
    public class ArrayTag
    {
        /// <summary>
        /// Builds an ArrayTag.
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
            if (endTag.Name == "/array") { return true; }
            else { return false; }
        }

        /// <summary>
        /// Applies the ArrayTag to the given data and outputs the result.
        /// </summary>
        /// <param name="tag">The TagStruct to apply.</param>
        /// <param name="output">The place to put the output.</param>
        /// <param name="data">The data to be applied to.</param>
        public static void ApplyTag(
            TagStruct tag,
            TextWriter output,
            IDictionary<string, object> data)
        {
            Helpers.RequireKey(tag.Param, data);

            IEnumerable<IDictionary<string, object>> items =
                (IEnumerable<IDictionary<string, object>>)data[tag.Param];
            int index = 0;

            // TODO: Clean up nested try blocks
            try
            {
                foreach (IDictionary<string, object> item in items)
                {
                    try
                    {
                        foreach (TagStruct child in tag.Children)
                        {
                            child.Apply(output, item);
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
            catch (RaindropException exc)
            {
                string key = (string)exc["raindrop.key-path"];
                key = "[" + tag.Param + "][" + index.ToString() + "]";
                exc["raindrop.key-path"] = key;
                throw;
            }
        }
    }

    [TagBuilder("/array")]
    public class ArrayEndTag
    {
        /// <summary>
        /// Builds an ArrayEndTag.
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
        /// Applies the ArrayEndTag to the given data and outputs the result.
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
                "ArrayEndTag does not support being applied.");
        }
    }
}