/*
 * ArrayTag.cs
 * By Mirinth (mirinth@gmail.com)
 * 
 * The ArrayTag file contains the ArrayTag class, which represents a
 * repeated block of text in the template.
 */

using System.Collections.Generic;
using System.IO;

namespace Raindrop
{
    partial class Raindrop
    {
        private class ArrayTag : BlockTag<EndTag>
        {
            public static string ID = "<:array";

            /// <summary>
            /// The ArrayTag constructor.
            /// </summary>
            /// <param name="ts">A TagStream to construct the ArrayTag from.</param>
            public ArrayTag(TagStream ts)
                : base(ts)
            {
                if (Param == null)
                {
                    throw new RaindropException(
                        "ArrayTag has no parameter.",
                        ts.FilePath,
                        ts.Index,
                        ErrorCode.ParameterMissing);
                }
            }

            /// <summary>
            /// Applies the ArrayTag to the given data and outputs the result.
            /// </summary>
            /// <param name="data">The data to be applied to.</param>
            /// <param name="output">The place to put the output.</param>
            public override void Apply(
                Dictionary<string, object> data,
                TextWriter output)
            {
                // If there's no data for the array, then skip it.
                if (!data.ContainsKey(Param))
                {
                    return;
                }

                // Convert data[Param] to an IEnumerable
                IEnumerable<object> items = (IEnumerable<object>)data[Param];

                // Repeat the child nodes for each item in the IEnumerable
                foreach (object item in items)
                {
                    Dictionary<string, object> newData = (Dictionary<string, object>)item;
                    base.Apply(newData, output);
                }
            }
        }
    }
}