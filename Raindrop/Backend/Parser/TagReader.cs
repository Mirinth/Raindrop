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

using System;
using System.IO;

namespace Raindrop.Backend.Parser
{
    class TagReader
    {
        const int end_of_file = -1;
        const int delimiter_length = 2;

        bool disposed = false;
        private FarPeekTextReader reader;

        /// <summary>
        /// The TagReader constructor
        /// </summary>
        /// <param name="tr">
        /// A TextReader to use as the TagReader's backing data source.
        /// The TagReader will take responsibility for disposing of the
        /// TextReader.
        /// </param>
        public TagReader(FarPeekTextReader fptr)
        {
            reader = fptr;
        }

        /// <summary>
        /// Gets whether the TagReader is at the end of its stream.
        /// </summary>
        public bool EOF
        {
            get
            {
                if (disposed) { throw new ObjectDisposedException("TagReader"); }
                return reader.EOF;
            }
        }

        /// <summary>
        /// Gets the current index that the TagReader is into its
        /// stream.
        /// </summary>
        public int Index
        {
            get
            {
                if (disposed) { throw new ObjectDisposedException("TagReader"); }
                return reader.Index;
            }
        }

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
        /// <param name="delimiter">
        /// The delimiter to check if the stream is at. Must be 2 characters
        /// in length.
        /// </param>
        /// <returns>
        /// True if delimiter was found at the start of the stream;
        /// false otherwise.
        /// </returns>
        public bool IsAt(string delimiter)
        {
            if (disposed) { throw new ObjectDisposedException("TagReader"); }

            EnforceLength(delimiter, delimiter_length);

            int peek = reader.Peek();
            int farPeek = reader.FarPeek();

            if (peek == end_of_file || farPeek == end_of_file ||
                peek != delimiter[0] || farPeek != delimiter[1])
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Reads from the stream until delimiter is encountered and optionally
        /// includes the delimiter in the result.
        /// </summary>
        /// <param name="delimiter">
        /// The string to read until encountered. Must have length of 2.
        /// </param>
        /// <param name="includeDelimiter">
        /// True to read the delimiter and include it in the result;
        /// false to leave the delimiter in the stream for the next read.
        /// </param>
        /// <returns></returns>
        public string ReadTo(string delimiter, bool includeDelimiter)
        {
            if (disposed) { throw new ObjectDisposedException("TagReader"); }

            EnforceLength(delimiter, delimiter_length);

            string result = string.Empty;

            int peek = reader.Peek();
            int farPeek = reader.FarPeek();

            while (peek != end_of_file &&
                (peek != delimiter[0] || farPeek != delimiter[1]))
            {
                // reader.Read() is either a valid char or end_of_file,
                // and if it was end_of_file then peek would be
                // end_of_file and the loop would have ended already.
                // So reader.Read() is a valid char.
                result += (char)reader.Read();

                // peek and farPeek are both int, so there's no need to
                // cast them.
                peek = reader.Peek();
                farPeek = reader.FarPeek();
            }

            if (includeDelimiter)
            {
                for (int i = 0; i < delimiter_length; i++)
                {
                    int read = reader.Read();
                    if (read != end_of_file)
                    {
                        result += (char)read;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Releases the unmanaged resources used by the FarPeekTextReader
        /// and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources; false to
        /// release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing) { reader.Dispose(); }
                disposed = true;
            }
        }

        /// <summary>
        /// Releases all resources used by the FarPeekTextReader object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
