/*
 * Tag.cs
 * By Mirinth (mirinth@gmail.com)
 * 
 * The Tag file contains the Tag class, which represents a
 * generic tag.
 */

using System.Collections.Generic;
using System.IO;

namespace Raindrop
{
    partial class Raindrop
    {
        private class DataTag : Tag
        {
            public static string ID = "<:data";

            /// <summary>
            /// The DataTag constructor.
            /// </summary>
            /// <param name="ts">A TagStream to construct the DataTag from.</param>
            public DataTag(TagStream ts)
                : base(ts)
            {
                if (Param == null)
                {
                    throw new RaindropException(
                        "DataTag has no parameter.",
                        ts.FilePath,
                        ts.Index,
                        ErrorCode.ParameterMissing);
                }
            }

            /// <summary>
            /// Applies the DataTag to the given data and outputs the result.
            /// </summary>
            /// <param name="data">The data to be applied to.</param>
            /// <param name="output">The place to put the output.</param>
            public override void Apply(
                Dictionary<string, object> data,
                TextWriter output)
            {
                if (data.ContainsKey(Param))
                {
                    output.Write(data[Param]);
                }
            }
        }
    }
}