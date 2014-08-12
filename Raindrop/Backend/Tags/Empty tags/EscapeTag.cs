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
 * An escape tag allows the template author to embed reserved
 * tokens directly in the template.
 */

using System.Collections.Generic;
using System.IO;

namespace Raindrop.Backend.Tags
{
    public class EscapeTag : ITagBuilder
    {
        /// <summary>
        /// Gets the name of the tag.
        /// </summary>
        public string Name
        {
            get { return "escape"; }
        }

        /// <summary>
        /// Gets whether a blank line before this tag should
        /// be removed.
        /// </summary>
        public bool RemoveBlankLine { get { return false; } }

        /// <summary>
        /// Builds an escape tag.
        /// </summary>
        /// <param name="td">Information about the tag to build.</param>
        public TagStruct Build(TagData td)
        {
            Helpers.RequireParameter(td.Param, td.Source);
            td.Param = TagReader.Unescape(td.Source, td.Param);

            return Helpers.BuildTag(Apply, null, td);
        }

        /// <summary>
        /// Applies the escape tag to the given data and outputs the result.
        /// </summary>
        /// <param name="tag">The tag to be applied.</param>
        /// <param name="output">The place to put the output.</param>
        /// <param name="data">The data to be applied to.</param>
        public void Apply(
            TagStruct tag,
            TextWriter output,
            IDictionary<string, object> data)
        {
            output.Write(tag.Param);
        }

        /// <summary>
        /// Converts an escape sequence into plain text.
        /// </summary>
        /// <param name="source">The template to use for error reporting.</param>
        /// <param name="sequence">The sequence to convert.</param>
        /// <returns>The unescaped sequence.</returns>
        private static string Unescape(Template source, string sequence)
        {
            const string left_cap_code = "lc";
            const string right_cap_code = "rc";
            const char divider = ' ';

            string result = string.Empty;
            string[] pieces = sequence.Split(divider);

            foreach (string piece in pieces)
            {
                switch (piece)
                {
                    case left_cap_code:
                        result += Punctuation.LeftCap;
                        break;
                    case right_cap_code:
                        result += Punctuation.RightCap;
                        break;
                    default:
                        RaindropException exc = new RaindropException(
                            "Invalid escape sequence");
                        exc["raindrop.escape-sequence"] = sequence;
                        exc["raindrop.start-line"] = source.Line;
                        throw exc;
                }
            }

            return result;
        }
    }
}