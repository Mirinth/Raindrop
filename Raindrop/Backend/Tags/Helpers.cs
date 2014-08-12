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
 * Various helper functions for the tags.
 */

using System;
using System.Collections.Generic;

namespace Raindrop.Backend.Tags
{
    public class Helpers
    {
        /// <summary>
        /// Initializes a TagStruct.
        /// </summary>
        /// <param name="apply">The tag's apply method</param>
        /// <param name="childTags">The tag's children</param>
        /// <param name="td">Additional information about the tag</param>
        /// <returns>The initialized TagStruct.</returns>
        public static TagStruct BuildTag(
            TagApplyDelegate apply,
            List<TagStruct> childTags,
            TagData td)
        {
            return new TagStruct()
            {
                ApplyMethod = apply,
                Children = childTags,
                Name = td.Name,
                Param = td.Param
            };
        }

        /// <summary>
        /// Gets a list of child tags.
        /// </summary>
        /// <param name="source">A template to extract children from.</param>
        /// <param name="predicate">
        /// The predicate to use when testing whether a given child tag should end
        /// the current template block.
        /// </param>
        /// <returns>
        /// A list of child tags up to (but not including) the first tag that
        /// matches the predicate.
        /// </returns>
        public static List<TagStruct> GetChildren(
            Template source,
            Predicate<TagStruct> predicate)
        {
            int startLine = source.Line;

            List<TagStruct> children = new List<TagStruct>();
            TagStruct child = TagFactory.Parse(source);

            while (!predicate(child) && !BlockTag.EndTagPredicate(child))
            {
                children.Add(child);
                child = TagFactory.Parse(source);
            }

            if (!predicate(child))
            {
                RaindropException exc = new RaindropException("End tag didn't match start tag.");
                exc["raindrop.expected-type"] = child.Name;
                exc["raindrop.encountered-type"] = child.GetType().FullName;
                exc["raindrop.start-Line"] = startLine;
                exc["raindrop.end-line"] = source.Line;
                throw exc;
            }

            return children;
        }

        /// <summary>
        /// Ensures that the givien dictionary contains the given key.
        /// If not, an exception is thrown with known information about
        /// the expected key.
        /// </summary>
        /// <param name="key">The key to verify the presence of.</param>
        /// <param name="dict">The dictionary to check for the key.</param>
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
        /// Ensures that this tag has a parameter. If it doesn't,
        /// an excception is thrown.
        /// </summary>
        /// <param name="param">The parameter to test.</param>
        /// <param name="source">
        /// A template to extract data from for
        /// error reporting if an exception is thrown.
        /// </param>
        public static void RequireParameter(string param, Template source)
        {
            if (string.IsNullOrEmpty(param))
            {
                RaindropException exc = new RaindropException(
                    "Tag must have a parameter.");
                exc["raindrop.start-line"] = source.Line;
                throw exc;
            }
        }

        /// <summary>
        /// Determines truth for a conditional tag.
        /// </summary>
        /// <param name="data">
        /// The data dictionary to use for determining truth.
        /// </param>
        /// <param name="key">
        /// The key to use for determining truth.
        /// </param>
        /// <returns>
        /// True if the given parameter and dictionary result in a
        /// true value; otherwise, false.
        /// </returns>
        public static bool Truth(
            string key,
            IDictionary<string, object> data)
        {
            // If the element is a bool, it's equal to itself.
            if (data[key] is bool)
            {
                return (bool)data[key];
            }

            // If the element is an IEnumerable with contents, it's true.
            else if (data[key] is IEnumerable<IDictionary<string, object>>)
            {
                // IEnumerable doesn't expose a Count variable, so test if it's
                // populated by trying to enumerate it and return true on
                // the first element.
                IEnumerable<IDictionary<string, object>> dummy =
                    (IEnumerable<IDictionary<string, object>>)data[key];

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