/*
 * EndTag.cs
 * By Mirinth (mirinth@gmail.com)
 * 
 * The EndTag file contains the EndTag class, which represents
 * the a generic end of a block. Tags which end block tags should
 * derive from EndTag.
 */

using System.IO;
using System.Web.Mvc;

namespace Raindrop
{
    partial class Raindrop
    {
        private class EndTag : Tag
        {
            /// <summary>
            /// The EndTag constructor.
            /// </summary>
            /// <param name="ts">A TagStream to construct the EndTag from.</param>
            public EndTag(TagStream ts)
                : base(ts) { }

            public EndTag()
            { }

            /// <summary>
            /// Applies the EndTag to the given data and outputs the result.
            /// </summary>
            /// <param name="data">The data to be applied to.</param>
            /// <param name="output">The place to put the output.</param>
            public override void Apply(
                ViewDataDictionary data,
                TextWriter output)
            {
                // Do nothing
                return;
            }
        }
    }
}