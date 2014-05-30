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

using System;

namespace Raindrop
{
    public partial class Raindrop
    {
        private class TagFactory
        {
            public static ITag Parse(TagStream ts)
            {
                if (ts.EOF)
                {
                    return new EOFTag();
                }

                string testId = ts.GetId();

                // Note: TextTag is an annoying special case
                // that has to be handled by a special function.
                if (TextHandles(testId, TextTag.ID))
                {
                    return new TextTag(ts);
                }
                else if (Handles(testId, ArrayTag.ID))
                {
                    return new ArrayTag(ts);
                }
                else if (Handles(testId, ArrayEndTag.ID))
                {
                    return new ArrayEndTag(ts);
                }
                else if (Handles(testId, CondTag.ID))
                {
                    return new CondTag(ts);
                }
                else if (Handles(testId, CondEndTag.ID))
                {
                    return new CondEndTag(ts);
                }
                else if (Handles(testId, DataTag.ID))
                {
                    return new DataTag(ts);
                }
                else if (Handles(testId, NCondTag.ID))
                {
                    return new NCondTag(ts);
                }
                else if (Handles(testId, NCondEndTag.ID))
                {
                    return new NCondEndTag(ts);
                }
                else
                {
                    string msg = string.Format(
                        "The tag '{0}' is not supported.",
                        testId);
                    throw new RaindropException(
                        msg,
                        ts.FilePath,
                        ts.Index,
                        ErrorCode.TagNotSupported);
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

            /// <summary>
            /// A special case Handles for the TextTag because it won't
            /// work with the regular Handles function.
            /// </summary>
            /// <param name="testId">The ID to be tested.</param>
            /// <param name="tagId">The ID of the tag that may handle the testIDd.</param>
            /// <returns>
            /// Whether the given testId is handled by the tag that has
            /// the given tagId.
            /// </returns>
            private static bool TextHandles(string testId, string tagId)
            {
                return !testId.StartsWith(tagId);
            }
        }
    }
}
