/*
 * EndTag.cs
 * By Mirinth (mirinth@gmail.com)
 * 
 * The EndTag file contains the EndTag class, which represents
 * the end of a block. It currently "ends" the blocks that OptTag
 * and ArrayTag start.
 */

using System.Collections.Generic;
using System.IO;

namespace Raindrop
{
    partial class Raindrop
    {
        private class EndTag : Tag
        {
            public static string ID = "<:end";

            /// <summary>
            /// The EndTag constructor.
            /// </summary>
            /// <param name="ts">A TagStream to construct the EndTag from.</param>
            public EndTag(TagStream ts)
                : base(ts) { }

            /// <summary>
            /// Applies the EndTag to the given data and outputs the result.
            /// </summary>
            /// <param name="data">The data to be applied to.</param>
            /// <param name="output">The place to put the output.</param>
            public override void Apply(
                Dictionary<string, object> data,
                TextWriter output)
            {
                // Do nothing
                return;
            }
        }
    }
}