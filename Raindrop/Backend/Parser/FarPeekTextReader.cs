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

using System;
using System.IO;

namespace Raindrop.Backend.Parser
{
    /// <summary>
    /// Wraps a regular TextReader with a small buffer to
    /// allow peeking at the next two characters instead
    /// of just one.
    /// </summary>
    public class FarPeekTextReader : IDisposable
    {
        const int end_of_file = -1;
        private TextReader reader;
        private int peek;
        private int offset;
        private bool peekSet = false;
        private bool disposed = false;

        /// <summary>
        /// The FarPeekTextReader constructor
        /// </summary>
        /// <param name="tr">
        /// The TextReader to wrap. This object will take
        /// responsibility for disposing the TextReader.</param>
        public FarPeekTextReader(TextReader tr)
        {
            if (tr == null) { throw new ArgumentNullException("tr"); }

            reader = tr;
            offset = 0;
        }

        /// <summary>
        /// Gets whether the FarPeekTextReader is at the end of its
        /// stream.
        /// </summary>
        public bool EOF
        {
            get
            {
                // Note: A FarPeek with no characters left will result in
                // peekSet=true, which will trip up the else-if block unless
                // it also ensures that peek!=end_of_file.
                if (disposed) { throw new ObjectDisposedException("FarPeekTextReader"); }
                else if (peekSet && peek != end_of_file) { return false; }
                else { return (reader.Peek() < 0); }
            }
        }

        /// <summary>
        /// The index into the stream the FarPeekTextReader is currently at.
        /// </summary>
        public int Index
        {
            get
            {
                if (disposed) { throw new ObjectDisposedException("FarPeekTextReader"); }
                else { return offset; }
            }
        }

        /// <summary>
        /// Reads the next character without changing the state of the reader or the
        /// character source. Returns the next available character without actually
        /// reading it from the input stream.
        /// </summary>
        /// <returns>
        /// An integer representing the next character to be read, or -1 if no more
        /// characters are available or the stream does not support seeking.
        /// </returns>
        public int Peek()
        {
            if (disposed) { throw new ObjectDisposedException("FarPeekTextReader"); }
            if (peekSet) { return peek; }
            else { return reader.Peek(); }
        }

        /// <summary>
        /// Reads the second available character without changing the state
        /// of the reader or the character source. Returns the second available
        /// character without actually reading it from the input stream.
        /// </summary>
        /// <returns>
        /// An integer representing the second character to be read, or -1 if too
        /// few characters are available or the stream does not support seeking.
        /// </returns>
        public int FarPeek()
        {
            if (disposed) { throw new ObjectDisposedException("FarPeekTextReader"); }
            
            if (!peekSet)
            {
                peek = reader.Read();
                peekSet = true;
            }
            
            return reader.Peek();
        }

        /// <summary>
        /// Reads the next character from the input stream and advances the
        /// character position by one character.
        /// </summary>
        /// <returns>
        /// The next character from the input stream, or -1 if no more characters
        /// are available. The default implementation returns -1.
        /// </returns>
        public int Read()
        {
            if (disposed) { throw new ObjectDisposedException("FarPeekTextReader"); }

            offset++;

            if (peekSet)
            {
                peekSet = false;
                return peek;
            }
            else
            {
                return reader.Read();
            }
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