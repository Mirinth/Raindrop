/*
 * NCondTag.cs
 * By Mirinth (mirinth@gmail.com)
 * 
 * The NCondTag file contains the NCondTag class, which represents an
 * optional tag. An NCond tag is only processed if its parameter
 * represents a "false" value in the data dictionary.
 */

using System.Collections.Generic;
using System.IO;

namespace Raindrop
{
    partial class Raindrop
    {
        private class NCondTag : BlockTag<EndTag>
        {
            public static string ID = "<:ncond";

            /// <summary>
            /// The NCondTag constructor.
            /// </summary>
            /// <param name="ts">A TagStream to construct the CondTag from.</param>
            public NCondTag(TagStream ts)
                : base(ts)
            {
                RequireParameter(ts);
            }

            /// <summary>
            /// Applies the CondTag to the given data and outputs the result
            /// if the NCondTag is "true", else does nothing. The NCondTag is
            /// "true" if the data dictionary contains a value at the key
            /// matching the NCondTag's Param which is either a Boolean
            /// representing true, or an IEnumerable with at least one
            /// value in it. The NCondTag is false in all but these two
            /// conditions.
            /// </summary>
            /// <param name="data">The data to be applied to.</param>
            /// <param name="output">The place to put the output.</param>
            public override void Apply(
                Dictionary<string, object> data,
                TextWriter output)
            {
                if (!Helpers.Pass(data, Param))
                {
                    base.Apply(data, output);
                }
            }
        }
    }
}