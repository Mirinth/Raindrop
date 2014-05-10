/*
 * Tag.cs
 * By Mirinth (mirinth@gmail.com)
 * 
 * The Tag file contains the Tag class, which represents a
 * generic tag that contains no children.
 */

using System.Collections.Generic;
using System.IO;
using System;

namespace Raindrop
{
    partial class Raindrop
    {
        private abstract class Tag : ITag
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
                        ts.FilePath,
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
                Dictionary<string, object> data,
                TextWriter output);

            /// <summary>
            /// Strips the endcaps off of a tag string.
            /// </summary>
            /// <param name="tag">The tag to strip endcaps from.</param>
            /// <returns>The input with the endcaps stripped.</returns>
            private string StripCaps(string tagString)
            {
                string result = tagString.Substring(
                    Settings.LeftCap.Length,
                    tagString.Length - Settings.LeftCap.Length - Settings.RightCap.Length);
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
                    new char[] { Settings.TagSplitter },
                    max_pieces);

                if (pieces.Length == param_included_length)
                {
                    return pieces[1];
                }
                else
                {
                    return null;
                }
            }
        }
    }
}