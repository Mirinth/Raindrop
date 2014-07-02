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
 * The array tag represents a repeated block of tags in the template.
 * 
 * The /array tag represents the end of an array block.
 */

using System.Collections.Generic;
using System.IO;
using Raindrop.Backend.Parser;
using System;
using Raindrop.Backend.Templater;

namespace Raindrop.Backend.Tags
{
    public class ArrayTag : ITag
    {
        /// <summary>
        /// Builds an array tag.
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
        public static bool EndTagPredicate(TagStruct endTag)
        {
            if (endTag.Name == "/array") { return true; }
            else { return false; }
        }

        /// <summary>
        /// Applies the array tag to the given data and outputs the result.
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
                            TagApplyer.Apply(child, output, item);
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

        public string GetName() { return "array"; }
    }

    public class ArrayEndTag : ITag
    {
        /// <summary>
        /// Builds a /array tag.
        /// </summary>
        /// <param name="tag">
        /// The TagStruct to put information in.
        /// </param>
        /// <param name="reader">
        /// The InfoProvidingTextReader to read additional tags from.
        /// </param>
        public void Build(ref TagStruct tag, InfoProvidingTextReader reader)
        {
            // TagFactory already does all the work.
        }

        /// <summary>
        /// Applies the /array tag to the given data and outputs the result.
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
                "ArrayEndTag does not support being applied.");
        }

        public string GetName() { return "/array"; }
    }
}