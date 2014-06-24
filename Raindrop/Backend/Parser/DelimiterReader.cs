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
 * The DelimiterReader reads from an InfoProvidingTextReader until a
 * given delimiter is found and assists with determining whether the
 * stream is at a given delimiter.
 */

using System;

namespace Raindrop.Backend.Parser
{
    public class DelimiterReader
    {
        const int delimiter_length = 2;

        /// <summary>
        /// Checks whether a string is a given length and throws
        /// an exception if not.
        /// </summary>
        /// <param name="s">The string to check.</param>
        /// <param name="length">The required length.</param>
        public static void EnforceLength(string s, int length)
        {
            if (s == null)
            {
                // A null string was passed to the calling function.
                throw new ArgumentNullException("s", "Parameter was null.");
            }
            if (s.Length != length)
            {
                // A string of invalid length was passed to the calling function.
                throw new ArgumentException("Parameter length was incorrect.");
            }
        }

        /// <summary>
        /// Determines whether the stream is at delimiter.
        /// </summary>
        /// <param name="reader">
        /// The InfoProvidingTextReader to check for a delimiter.
        /// </param>
        /// <param name="delimiter">
        /// The delimiter to check if the stream is at. Must be 2 characters
        /// in length.
        /// </param>
        /// <returns>
        /// True if delimiter was found at the start of the stream;
        /// false otherwise.
        /// </returns>
        public static bool IsAt(InfoProvidingTextReader reader, string delimiter)
        {
            EnforceLength(delimiter, delimiter_length);

            int peek = reader.Peek();
            int farPeek = reader.FarPeek();

            if (delimiter[0] == reader.Peek() &&
                delimiter[1] == reader.FarPeek())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Reads from the stream until delimiter is encountered or the stream
        /// is empty, and optionally includes the delimiter in the result.
        /// </summary>
        /// <param name="reader">
        /// The InfoProvidingTextReader to read from.
        /// </param>
        /// <param name="delimiter">
        /// The string to read until encountered. Must have length of 2.
        /// </param>
        /// <param name="includeDelimiter">
        /// True to read the delimiter and include it in the result;
        /// false to leave the delimiter in the stream for the next read.
        /// </param>
        /// <returns>
        /// A string containing all the available characters up to (and
        /// optionally including) the delimiter or the end of the stream.
        /// </returns>
        public static string ReadTo(
            InfoProvidingTextReader reader,
            string delimiter,
            bool includeDelimiter)
        {
            EnforceLength(delimiter, delimiter_length);

            string result = string.Empty;

            int peek = reader.Peek();
            int farPeek = reader.FarPeek();

            while (!reader.Empty &&
                (peek != delimiter[0] || farPeek != delimiter[1]))
            {
                // reader.Read() is either a valid char or -1,
                // and if it was -1 then reader.Empty would be
                // true and the loop would have ended already.
                // So reader.Read() is a valid char.
                result += (char)reader.Read();

                // peek and farPeek are both int, so there's no need to
                // cast them here.
                peek = reader.Peek();
                farPeek = reader.FarPeek();
            }

            if (includeDelimiter)
            {
                for (int i = 0; i < delimiter_length; i++)
                {
                    if (!reader.Empty)
                    {
                        // Read() guaranteed to be valid char if !reader.Empty
                        result += (char)reader.Read();
                    }
                }
            }

            return result;
        }
    }
}
