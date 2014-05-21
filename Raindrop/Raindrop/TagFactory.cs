using System;

namespace Raindrop
{
    partial class Raindrop
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
