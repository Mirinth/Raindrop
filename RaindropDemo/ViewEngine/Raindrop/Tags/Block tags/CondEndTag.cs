/*
 * ArrayEndTag.cs
 * By Mirinth (mirinth@gmail.com)
 * 
 * The CondEndTag file contains the CondEndTag class, which represents
 * the end of a CondTag block.
 */

using System.IO;
using System.Web.Mvc;

namespace Raindrop
{
    partial class Raindrop
    {
        private class CondEndTag : EndTag
        {
            public static string ID = "<:/cond";

            /// <summary>
            /// The ArrayEndTag constructor.
            /// </summary>
            /// <param name="ts">A TagStream to construct the EndTag from.</param>
            public CondEndTag(TagStream ts)
                : base(ts) { }
        }
    }
}