/*
 * EOFTag.cs
 * By Mirinth (mirinth@gmail.com)
 * 
 * The EOFTag file contains the EOFTag class, which represents
 * the end of the file.
 */

using System.Collections.Generic;
using System.IO;
using System;

namespace Raindrop
{
    partial class Raindrop
    {
        private class EOFTag : ITag
        {
            public EOFTag() { }

            /// <summary>
            /// Applies the Tag to the given data and outputs the result.
            /// </summary>
            /// <param name="data">The data to be applied to.</param>
            /// <param name="output">The place to put the output.</param>
            public void Apply(
                Dictionary<string, object> data,
                TextWriter output)
            {
                throw new RaindropException(
                    "Cannot apply EOFTag.",
                    null,
                    0,
                    ErrorCode.AppliedEOF);
            }
        }
    }
}