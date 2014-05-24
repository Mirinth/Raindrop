/*
 * ArrayTag.cs
 * By Mirinth (mirinth@gmail.com)
 * 
 * The ArrayTag file contains the ArrayTag class, which represents a
 * repeated block of text in the template.
 */

using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace Raindrop
{
    partial class Raindrop
    {
        private class ArrayTag : BlockTag<ArrayEndTag>
        {
            public static string ID = "<:array";

            /// <summary>
            /// The ArrayTag constructor.
            /// </summary>
            /// <param name="ts">A TagStream to construct the ArrayTag from.</param>
            public ArrayTag(TagStream ts)
                : base(ts)
            {
                RequireParameter(ts);
            }

            /// <summary>
            /// Applies the ArrayTag to the given data and outputs the result.
            /// </summary>
            /// <param name="data">The data to be applied to.</param>
            /// <param name="output">The place to put the output.</param>
            public override void Apply(
                ViewDataDictionary data,
                TextWriter output)
            {
                // If there's no data for the array, then skip it.
                if (!data.ContainsKey(Param))
                {
                    KeyMissing();
                    return;
                }

                // Convert data[Param] to an IEnumerable
                // TODO: Should probably be IEnumerable<ViewDataDictionary>
                IEnumerable<object> items = (IEnumerable<object>)data[Param];

                // Repeat the child nodes for each item in the IEnumerable
                foreach (object item in items)
                {
                    ViewDataDictionary newData = (ViewDataDictionary)item;
                    base.Apply(newData, output);
                }
            }
        }
    }
}