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
 * TagStream reads structures representing a tag (ID and parameter)
 * out of an InfoProvidingTextStream.
 */

using Raindrop.Backend.Lexer;

namespace Raindrop.Backend.Parser
{
    public class TagStream
    {
        /// <summary>
        /// Converts an escape sequence into plain text.
        /// </summary>
        /// <param name="reader">The reader to use for error reporting.</param>
        /// <param name="sequence">The sequence to convert.</param>
        /// <returns>The unescaped sequence.</returns>
        public static string Unescape(TagReader reader, string sequence)
        {
            // TODO: Move syntax handling into the lexer.
            string result = string.Empty;
            string[] pieces = sequence.Split(' ');

            foreach (string piece in pieces)
            {
                switch (piece)
                {
                    case "lc":
                        result += "<:";
                        break;
                    case "rc":
                        result += ":>";
                        break;
                    default:
                        RaindropException exc = new RaindropException(
                            "Invalid escape sequence");
                        exc["raindrop.escape-sequence"] = sequence;
                        exc["raindrop.start-offset"] = reader.Offset;
                        exc["raindrop.start-line"] = reader.Line;
                        throw exc;
                }
            }

            return result;
        }
    }
}