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
 * Provides several objects used for implementing a tag.
 * - ApplyTagDelegate
 * - EndTagPredicate
 * - TagBuilderAttribute
 */

using System;
using System.Collections.Generic;
using System.IO;
using Raindrop.Backend.Parser;

namespace Raindrop.Backend.Tags
{
    /// <summary>
    /// A method that can be used to apply a tag.
    /// </summary>
    /// <param name="tag">The tag to be applied to.</param>
    /// <param name="output">The place to put the output.</param>
    /// <param name="data">The data source to use.</param>
    public delegate void ApplyTagDelegate(
        TagStruct tag,
        TextWriter output,
        IDictionary<string, object> data);

    /// <summary>
    /// A method that can be used to build a tag.
    /// </summary>
    /// <param name="tag">The tag to build.</param>
    /// <param name="reader">
    /// An InfoProvidingTextReader to retrieve child tags from.
    /// </param>
    public delegate void BuildTagDelegate(
        out TagStruct tag,
        InfoProvidingTextReader reader);

    /// <summary>
    /// Determines whether the given TagStruct represents the
    /// end of the current block tag.
    /// </summary>
    /// <param name="endTag">The TagStruct to check.</param>
    /// <returns>
    /// True if endTag represents the end of the current block;
    /// otherwise false.
    /// </returns>
    public delegate bool EndTagPredicate(TagStruct endTag);

    /// <summary>
    /// Denotes a class with a public static BuildTag method
    /// matching TagBuilderDelegate that can build a tag.
    /// </summary>
    class TagBuilderAttribute : Attribute
    {
        public string Name;
        public TagBuilderAttribute(string tagName)
        {
            Name = tagName;
        }
    }
}
