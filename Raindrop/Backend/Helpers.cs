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

using System.Collections.Generic;
using Raindrop.Backend.Parser;
using Raindrop.Backend.Tags;

namespace Raindrop.Backend
{
    public class Helpers
    {
        /// <summary>
        /// Populates the children List with child tags.
        /// </summary>
        /// <param name="ts">A TagStream to extract children from.</param>
        /// <param name="children">The List to put children into.</param>
        public static List<TagStruct> GetChildren(
            InfoProvidingTextReader ts,
            EndTagPredicate predicate)
        {
            int startIndex = ts.Index;

            List<TagStruct> children = new List<TagStruct>();
            TagStruct child = TagFactory.Parse(ts);

            while (!predicate(child) && !BlockTag.EndTagPredicate(child))
            {
                children.Add(child);
                child = TagFactory.Parse(ts);
            }

            if (!predicate(child))
            {
                RaindropException exc = new RaindropException("End tag didn't match start tag.");
                exc["raindrop.expected-type"] = child.Name;
                exc["raindrop.encountered-type"] = child.GetType().FullName;
                exc["raindrop.start-index"] = startIndex;
                exc["raindrop.end-index"] = ts.Index;
                throw exc;
            }

            return children;
        }

        /// <summary>
        /// Ensures that the givien dictionary contains the given key.
        /// If not, an exception is thrown with known information about
        /// the expected key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dict"></param>
        public static void RequireKey(string key, IDictionary<string, object> dict)
        {
            if (!dict.ContainsKey(key))
            {
                RaindropException exc = new RaindropException(
                    "A required key was missing from the data dictionary.");
                exc["raindrop.expected-key"] = key;
                exc["raindrop.data-dictionary"] = dict;
                throw exc;
            }
        }

        /// <summary>
        /// Ensures that this Tag has a parameter. If it doesn't,
        /// an excception is thrown.
        /// </summary>
        /// <param name="param">The parameter to test.</param>
        /// <param name="ts">
        /// A TagStream containing data to include in the
        /// error message if an exception is thrown.
        /// </param>
        public static void RequireParameter(string param, InfoProvidingTextReader ts)
        {
            if (string.IsNullOrEmpty(param))
            {
                RaindropException exc = new RaindropException(
                    "Tag must have a parameter.");
                exc["raindrop.start-index"] = ts.Index;
                throw exc;
            }
        }

        /// <summary>
        /// Determines truth for a conditional tag.
        /// </summary>
        /// <param name="data">
        /// The data dictionary to use for determining truth.
        /// </param>
        /// <param name="param">
        /// The parameter to use for determining truth.
        /// </param>
        /// <returns>
        /// True if the given parameter and dictionary result in a
        /// true value; otherwise, false.
        /// </returns>
        public static bool Truth(
            string param,
            IDictionary<string, object> data)
        {
            // If the element is a bool, it's equal to itself.
            if (data[param] is bool)
            {
                return (bool)data[param];
            }

            // If the element is an IEnumerable with contents, it's true.
            else if (data[param] is IEnumerable<IDictionary<string, object>>)
            {
                // IEnumerable doesn't expose a Count variable, so test if it's
                // populated by trying to enumerate it and return true on
                // the first element.
                IEnumerable<IDictionary<string, object>> dummy =
                    (IEnumerable<IDictionary<string, object>>)data[param];

                // Loop will be skipped if dummy is empty.
                foreach (IDictionary<string, object> vdd in dummy)
                {
                    return true;
                }

                // The IEnumerable was empty.
                return false;
            }

            // In all other cases, assume false.
            return false;
        }
    }
}