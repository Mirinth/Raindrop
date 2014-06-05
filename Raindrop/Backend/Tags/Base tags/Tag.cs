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
 * Tag.cs
 * By Mirinth (mirinth@gmail.com)
 * 
 * The Tag file contains the Tag class, which represents a
 * generic tag that contains no children.
 */

using System.Collections.Generic;
using System.IO;
using Raindrop.Backend.Parser;

namespace Raindrop.Backend
{
    abstract class Tag : ITag
    {
        protected const int param_included_length = 2;

        public string Param
        {
            get;
            protected set;
        }

        /// <summary>
        /// The Tag constructor. Reads a Tag string from the TagStream,
        /// extracts a parameter, and stores the result.
        /// </summary>
        /// <param name="param">The tag's parameter.</param>
        /// <param name="ts">A TagStream to construct child tags from.</param>
        public Tag(string param, TagStream ts)
        {
            Param = param;
        }

        /// <summary>
        /// Ensures that this Tag has a parameter. If it doesn't,
        /// an excception is thrown.
        /// </summary>
        /// <param name="ts">
        /// A TagStream containing data to include in the
        /// error message if an exception is thrown.
        /// </param>
        public void RequireParameter(TagStream ts)
        {
            if (Param == null)
            {
                throw new RaindropException(
                    "CondTag has no parameter.",
                    ts.Name,
                    ts.Index,
                    ErrorCode.ParameterMissing);
            }
        }

        /// <summary>
        /// The Tag constructor. Initializes the parameter to null.
        /// </summary>
        public Tag()
        {
            Param = null;
        }

        /// <summary>
        /// Applies the Tag to the given data and outputs the result.
        /// </summary>
        /// <param name="data">The data to be applied to.</param>
        /// <param name="output">The place to put the output.</param>
        public abstract void Apply(
            IDictionary<string, object> data,
            TextWriter output);

        /// <summary>
        /// Handles the case where a tag that requires a key is missing it
        /// by either doing nothing or crashing (depending on value of
        /// Settings.MissingKeyFailMode).
        /// </summary>
        public void KeyMissing()
        {
            Helpers.KeyMissing(Param);
        }
    }
}