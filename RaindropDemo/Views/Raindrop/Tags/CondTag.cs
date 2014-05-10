/*
 * CondTag.cs
 * By Mirinth (mirinth@gmail.com)
 * 
 * The CondTag file contains the CondTag class, which represents an
 * optional tag. A CondTag tag is only processed if its parameter
 * represents a "true" value in the data dictionary.
 */

using System.Collections.Generic;
using System.IO;

namespace Raindrop
{
    partial class Raindrop
    {
        private class CondTag : BlockTag<EndTag>
        {
            public static string ID = "<:cond";

            /// <summary>
            /// The CondTag constructor.
            /// </summary>
            /// <param name="ts">A TagStream to construct the CondTag from.</param>
            public CondTag(TagStream ts)
                : base(ts)
            {
                RequireParameter(ts);
            }

            /// <summary>
            /// Applies the CondTag to the given data and outputs the result
            /// if the CondTag is "true", else does nothing. The CondTag is
            /// "true" if the data dictionary contains a value at the key
            /// matching the CondTag's Param which is either a Boolean
            /// representing true, or an IEnumerable with at least one
            /// value in it. The CondTag is false in all but these two
            /// conditions.
            /// </summary>
            /// <param name="data">The data to be applied to.</param>
            /// <param name="output">The place to put the output.</param>
            public override void Apply(
                Dictionary<string, object> data,
                TextWriter output)
            {
                if (Helpers.Pass(data, Param))
                {
                    base.Apply(data, output);
                }
            }
        }
    }
}