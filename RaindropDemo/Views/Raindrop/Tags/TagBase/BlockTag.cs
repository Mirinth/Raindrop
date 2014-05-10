/*
 * BlockTag.cs
 * By Mirinth (mirinth@gmail.com)
 * 
 * The BlockTag file contains the BlockTag class, which represents a
 * generic tag that contains children.
 */

using System.IO;
using System.Collections.Generic;
using System;

namespace Raindrop
{
    partial class Raindrop
    {
        private abstract class BlockTag<T> : Tag where T : ITag
        {
            private List<ITag> children;

            /// <summary>
            /// The BlockTag constructor.
            /// </summary>
            /// <param name="ts">A TagStream to construct the BlockTag from.</param>
            public BlockTag(TagStream ts)
                : base(ts)
            {
                GetChildren(ts);
            }

            public BlockTag(TagStream ts, bool dummy)
            {
                GetChildren(ts);
            }

            /// <summary>
            /// Populates the given List with child tags.
            /// </summary>
            /// <param name="ts">A TagStream to extract children from.</param>
            /// <param name="children">The List to put children into.</param>
            protected void GetChildren(TagStream ts)
            {
                children = new List<ITag>();
                ITag child = TagFactory.Parse(ts);

                while (!(child is T) && !(child is EOFTag))
                {
                    children.Add(child);
                    child = TagFactory.Parse(ts);
                }

                if (!(child is T))
                {
                    throw new RaindropException(
                        "Found EOF before closing tag",
                        ts.FilePath,
                        ts.Index,
                        ErrorCode.EndTagMissing);
                }
            }

            /// <summary>
            /// Applies the Tag to the given data and outputs the result.
            /// </summary>
            /// <param name="data">The data to be applied to.</param>
            /// <param name="output">The place to put the output.</param>
            public override void Apply(
                Dictionary<string, object> data,
                TextWriter output)
            {
                foreach (ITag child in children)
                {
                    child.Apply(data, output);
                }
            }
        }
    }
}