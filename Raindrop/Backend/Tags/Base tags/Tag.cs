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
        /// <param name="ts">A TagStream to construct the Tag from.</param>
        public Tag(TagStream ts)
        {
            string tagString = ts.ReadTag();
            Param = GetParam(tagString);
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
        /// Strips the endcaps off of a tag string.
        /// </summary>
        /// <param name="tag">The tag to strip endcaps from.</param>
        /// <returns>The input with the endcaps stripped.</returns>
        private string StripCaps(string tagString)
        {
            string result = tagString.Substring(
                TagStream.LeftCap.Length,
                tagString.Length - TagStream.LeftCap.Length - TagStream.RightCap.Length);
            return result;
        }

        /// <summary>
        /// Gets a tag string's parameter.
        /// </summary>
        /// <param name="tag">The tag string containing a parameter.</param>
        /// <returns>The parameter contained in the string, or null if none.</returns>
        public string GetParam(string tagString)
        {
            const int max_pieces = 2;
            tagString = StripCaps(tagString);
            string[] pieces = tagString.Split(
                new char[] { TagStream.TagSplitter },
                max_pieces);

            if (pieces.Length < param_included_length)
            {
                return null;
            }

            return pieces[1].TrimEnd(TagStream.TrimChars);
        }

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