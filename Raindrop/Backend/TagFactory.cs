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

using Raindrop.Backend.Parser;
using Raindrop.Backend.Tags;

namespace Raindrop.Backend
{
    class TagFactory
    {
        public static ITag Parse(TagStream ts)
        {
            if (ts.EOF)
            {
                return new EOFTag();
            }

            TagData td = ts.GetTag();

            if (Handles(td.ID, TextTag.ID))
            {
                return new TextTag(td.Param, ts);
            }
            else if (Handles(td.ID, ArrayTag.ID))
            {
                return new ArrayTag(td.Param, ts);
            }
            else if (Handles(td.ID, ArrayEndTag.ID))
            {
                return new ArrayEndTag(td.Param, ts);
            }
            else if (Handles(td.ID, CondTag.ID))
            {
                return new CondTag(td.Param, ts);
            }
            else if (Handles(td.ID, CondEndTag.ID))
            {
                return new CondEndTag(td.Param, ts);
            }
            else if (Handles(td.ID, DataTag.ID))
            {
                return new DataTag(td.Param, ts);
            }
            else if (Handles(td.ID, NCondTag.ID))
            {
                return new NCondTag(td.Param, ts);
            }
            else if (Handles(td.ID, NCondEndTag.ID))
            {
                return new NCondEndTag(td.Param, ts);
            }
            else
            {
                string msg = string.Format(
                    "The tag '{0}' is not supported.",
                    td.ID);
                throw new ParserException(
                    msg,
                    ts.Name,
                    ts.Index);
            }
        }

        /// <summary>
        /// Tests whether the given testId is handled by the tag that
        /// has the given tagId.
        /// </summary>
        /// <param name="testId">The ID to be tested.</param>
        /// <param name="tagId">The ID of the tag that may handle the testIDd.</param>
        /// <returns>
        /// Whether the given testId is handled by the tag that has
        /// the given tagId.
        /// </returns>
        private static bool Handles(string testId, string tagId)
        {
            return (testId == tagId);
        }
    }
}