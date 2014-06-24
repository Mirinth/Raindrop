/*
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
 * The BlockTag represents a generic tag that contains children.
 * 
 * The EOFTag represents the end of the template file.
 */

using System;
using System.Collections.Generic;
using System.IO;
using Raindrop.Backend.Parser;

namespace Raindrop.Backend.Tags
{
    public class BlockTag<T> : Tag where T : EndTag
    {
        private List<Tag> children;

        /// <summary>
        /// The BlockTag constructor.
        /// </summary>
        /// <param name="param">The tag's parameter.</param>
        /// <param name="ts">A TagStream to construct child tags from.</param>
        public BlockTag(string param, InfoProvidingTextReader ts)
            : base(param, ts)
        {
            try
            {
                GetChildren(ts);
            }
            catch (RaindropException exc)
            {
                exc["raindrop.template-name"] = Param;
                throw;
            }
        }

        /// <summary>
        /// Populates the children List with child tags.
        /// </summary>
        /// <param name="ts">A TagStream to extract children from.</param>
        /// <param name="children">The List to put children into.</param>
        protected void GetChildren(InfoProvidingTextReader ts)
        {
            int startIndex = ts.Index;

            children = new List<Tag>();
            Tag child = TagFactory.Parse(ts);

            while (!(child is T) && !(child is EndTag))
            {
                children.Add(child);
                child = TagFactory.Parse(ts);
            }

            if (!(child is T))
            {
                RaindropException exc = new RaindropException("End tag didn't match start tag.");
                exc["raindrop.expected-type"] = typeof(T).FullName;
                exc["raindrop.encountered-type"] = child.GetType().FullName;
                exc["raindrop.start-index"] = startIndex;
                exc["raindrop.end-index"] = ts.Index;
                throw exc;
            }
        }

        /// <summary>
        /// Applies the Tag to the given data and outputs the result.
        /// </summary>
        /// <param name="data">The data to be applied to.</param>
        /// <param name="output">The place to put the output.</param>
        public override void Apply(
            IDictionary<string, object> data,
            TextWriter output)
        {
            try
            {
                foreach (Tag child in children)
                {
                    child.Apply(data, output);
                }
            }
            catch (RaindropException exc)
            {
                exc["raindrop.template-name"] = Param;
                throw;
            }
        }
    }

    public class EOFTag : EndTag
    {
        public EOFTag(InfoProvidingTextReader ts)
            : base("EOF", ts) { }

        /// <summary>
        /// Applies the Tag to the given data and outputs the result.
        /// </summary>
        /// <param name="data">The data to be applied to.</param>
        /// <param name="output">The place to put the output.</param>
        public override void Apply(
            IDictionary<string, object> data,
            TextWriter output)
        {
            throw new NotImplementedException(
                "EOFTag should not be applied. " +
                "An error in the templating logic has likely occurred.");
        }
    }
}