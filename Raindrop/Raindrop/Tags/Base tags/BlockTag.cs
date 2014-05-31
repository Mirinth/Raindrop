﻿/*
 * Copyright 2014
 * 
 * This file is part of the Raindrop Templating Library.
 * 
 * The Raindrop Templating Library is free software: you can redistribute
 * it and/or modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation, either version 3
 * of the License, or (at your option) any later version.
 * 
 * The Raindrop Templating Library is distributed in the hope that it will
 * be useful, but WITHOUT ANY WARRANTY; without even the implied warranty
 * of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with the Raindrop Templating Library. If not, see
 * <http://www.gnu.org/licenses/>. 
 */

/*
* BlockTag.cs
* By Mirinth (mirinth@gmail.com)
* 
* The BlockTag file contains the BlockTag class, which represents a
* generic tag that contains children.
*/

using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

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

                while (!(child is T) && !(child is EndTag))
                {
                    children.Add(child);
                    child = TagFactory.Parse(ts);
                }

                if (!(child is T))
                {
                    string msg = string.Format(
                        "Tag mismatch. Expected '{0}', found '{1}'",
                        typeof(T),
                        child.GetType());

                    throw new RaindropException(
                        msg,
                        ts.FilePath,
                        ts.Index,
                        ErrorCode.EndTagMismatch);
                }
            }

            /// <summary>
            /// Applies the Tag to the given data and outputs the result.
            /// </summary>
            /// <param name="data">The data to be applied to.</param>
            /// <param name="output">The place to put the output.</param>
            public override void Apply(
                ViewDataDictionary data,
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