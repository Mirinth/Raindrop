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
 * The InfoProvidingTextReader wraps a FarPeekTextReader to provide
 * additional information about the current offset into the data
 * source, the number of lines read, and whether the source is 
 * empty or not.
 */

using System;
using System.IO;

namespace Raindrop.Backend.Parser
{
    public class InfoProvidingTextReader : FarPeekTextReader
    {
        const int end_of_file = -1;

        private int offset = 0;
        private int crCount = 1;
        private int nlCount = 1;
        private bool disposed = false;

        public InfoProvidingTextReader(TextReader source)
            : base(source) { }

        /// <summary>
        /// The index into the stream the InfoProvidingTextReader is currently at.
        /// </summary>
        public int Index
        {
            get
            {
                if (disposed) { throw new ObjectDisposedException("InfoProvidingTextReader"); }
                else { return offset; }
            }
        }

        /// <summary>
        /// Gets the line the InfoProvidingTextReader's source is
        /// currently reading from.
        /// </summary>
        public int Line
        {
            get
            {
                if (disposed) { throw new ObjectDisposedException("InfoProvidingTextReader"); }

                // \r\n, \n\r, or first line
                if (nlCount == crCount) { return nlCount; }
                // \n system (or first line)
                else if (crCount == 1) { return nlCount; }
                // \r\n system between lines
                else if (crCount - nlCount == 1) { return crCount; }
                // \n\r system between lines
                else if (nlCount - crCount == 1) { return nlCount; }
                // Probably inconsistent line endings.
                else { return -1; } // 
            }
        }

        /// <summary>
        /// Gets whether the InfoProvidingTextReader is empty
        /// (has no more data to read).
        /// </summary>
        public bool Empty
        {
            get
            {
                if (disposed) { throw new ObjectDisposedException("InfoProvidingTextReader"); }
                else if (Peek() == end_of_file) { return true; }
                else { return false; }
            }
        }

        /// <summary>
        /// Reads the next character from the input stream and advances the
        /// character position by one character.
        /// </summary>
        /// <returns>
        /// The next character from the input stream, or -1 if no more characters
        /// are available. The default implementation returns -1.
        /// </returns>
        public override int Read()
        {
            if (disposed) { throw new ObjectDisposedException("InfoProvidingTextReader"); }

            int read = base.Read();
            offset++;

            if (read == '\r') { crCount++; }
            if (read == '\n') { nlCount++; }

            return read;
        }
    }
}