/*
 * RootTag.cs
 * By Mirinth (mirinth@gmail.com)
 * 
 * The RootTag file contains the RootTag class, which represents a
 * Tag containing other Tags. RootTag is just a generic container.
 */

using System.Collections.Generic;
using System.IO;

namespace Raindrop
{
    partial class Raindrop
    {
        private class RootTag : BlockTag<EOFTag>
        {
            /// <summary>
            /// The RootTag constructor.
            /// </summary>
            /// <param name="param">The parameter to use for the RootTag.</param>
            /// <param name="children">A List of child Tags for the RootTag to contain.</param>
            public RootTag(TagStream ts)
                : base(ts, false) { }
        }
    }
}