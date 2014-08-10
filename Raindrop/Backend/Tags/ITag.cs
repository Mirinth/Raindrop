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
 * ITagBuilder describes the interface to be used by tag builders.
 */

using Raindrop.Backend.LexerNS;

namespace Raindrop.Backend.Tags
{
    public interface ITagBuilder
    {
        /// <summary>
        /// Builds a TagStruct.
        /// </summary>
        /// <param name="td">Information about the tag to build.</param>
        TagStruct Build(TagData td);

        /// <summary>
        /// Gets the name of the tag.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets whether a blank line before this tag should
        /// be removed.
        /// </summary>
        bool RemoveBlankLine { get; }
    }
}