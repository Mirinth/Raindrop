/*
 * ArrayEndTag.cs
 * By Mirinth (mirinth@gmail.com)
 * 
 * The NCondEndTag file contains the NCondEndTag class, which represents
 * the end of an NCondTag block.
 */

using System.IO;
using System.Web.Mvc;

namespace Raindrop
{
    partial class Raindrop
    {
        private class NCondEndTag : EndTag
        {
            public static string ID = "<:/ncond";

            /// <summary>
            /// The ArrayEndTag constructor.
            /// </summary>
            /// <param name="ts">A TagStream to construct the EndTag from.</param>
            public NCondEndTag(TagStream ts)
                : base(ts) { }
        }
    }
}