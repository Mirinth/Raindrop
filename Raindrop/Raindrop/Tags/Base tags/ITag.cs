﻿/*
 * ITag.cs
 * By Mirinth (mirinth@gmail.com)
 * 
 * ITag is the generic tag interface.
 */

using System.IO;
using System.Web.Mvc;

namespace Raindrop
{
    partial class Raindrop
    {
        interface ITag
        {
            /// <summary>
            /// Applies the Tag to the given data and outputs the result.
            /// </summary>
            /// <param name="data">The data to be applied to.</param>
            /// <param name="output">The place to put the output.</param>
            void Apply(
                ViewDataDictionary data,
                TextWriter output);
        }
    }
}