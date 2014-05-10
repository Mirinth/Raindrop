/*
 * ArrayEndTag.cs
 * By Mirinth (mirinth@gmail.com)
 * 
 * The ArrayEndTag file contains the ArrayEndTag class, which represents
 * the end of an ArrayTag block.
 */

using System.IO;
using System.Web.Mvc;

namespace Raindrop
{
    partial class Raindrop
    {
        private class ArrayEndTag : EndTag
        {
            public static string ID = "<:/array";

            /// <summary>
            /// The ArrayEndTag constructor.
            /// </summary>
            /// <param name="ts">A TagStream to construct the EndTag from.</param>
            public ArrayEndTag(TagStream ts)
                : base(ts) { }
        }
    }
}