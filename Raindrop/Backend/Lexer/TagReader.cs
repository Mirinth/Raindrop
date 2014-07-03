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
 * Wraps a TextReader to allow reading of TagData structs from it.
 */

using System;
using System.IO;

namespace Raindrop.Backend.Lexer
{
    public struct TagData
    {
        public int Line;
        public int Offset;
        public string ID;
        public string Param;
    }

    public class TagReader
    {
        private static readonly string leftCap = "<:";
        private static readonly string rightCap = ":>";
        private static readonly char[] tagSplitter = { ' ' };
        private static readonly char[] trimChars = { ' ', '/' };

        const bool include_delimiter = true;
        const bool exclude_delimiter = false;

        private TagData peek;
        private bool peekSet;
        private InfoProvidingTextReader reader;

        /// <summary>
        /// Gets whether the TagReader is empty
        /// (has no more tags to read).
        /// </summary>
        public bool Empty
        {
            get
            {
                if (peekSet && peek.ID == "eof") { return true; }
                else if (peekSet) { return false; }
                else { return reader.Empty; }
            }
        }

        /// <summary>
        /// The offset into the stream the TagReader is currently at.
        /// </summary>
        public int Offset
        {
            get { return reader.Offset; }
        }

        /// <summary>
        /// Gets the line the TagReader's source is
        /// currently reading from.
        /// </summary>
        public int Line
        {
            get { return reader.Line; }
        }

        /// <summary>
        /// Initializes the TagReader.
        /// </summary>
        /// <param name="tr">The TextReader to use as the data source.</param>
        public TagReader(TextReader tr)
        {
            reader = new InfoProvidingTextReader(tr);
            peek = new TagData();
            peekSet = false;
        }

        /// <summary>
        /// Gets the next tag (or text) in the TagReader.
        /// </summary>
        /// <returns>The next tag (or text) in the TagReader.</returns>
        public TagData Read()
        {
            if (peekSet)
            {
                peekSet = false;
                return peek;
            }

            if (reader.Empty)
            {
                return new TagData()
                {
                    ID = "eof",
                    Line = -1, 
                    Param = "eof",
                    Offset = -1
                };
            }

            if (DelimiterReader.IsAt(reader, leftCap)) { return ReadTag(); }
            else { return ReadText(); }
        }

        /// <summary>
        /// Gets the next tag (or text) in the TagReader without advancing the stream.
        /// </summary>
        /// <returns>The next tag (or text) in the TagReader.</returns>
        public TagData Peek()
        {
            if (!peekSet)
            {
                peek = Read();
                peekSet = true;
            }

            return peek;
        }

        /// <summary>
        /// Strips the endcaps off of a tag string.
        /// </summary>
        /// <param name="tagString">The tag string to strip endcaps from.</param>
        /// <returns>The input with the endcaps stripped.</returns>
        public static string StripCaps(string tagString)
        {
            string result = tagString.Substring(
                leftCap.Length,
                tagString.Length - leftCap.Length - rightCap.Length);
            return result;
        }

        /// <summary>
        /// Reads from the stream when it is at text.
        /// </summary>
        /// <returns>
        /// A TagData representing the read tag.
        /// </returns>
        public TagData ReadText()
        {
            if (reader.Empty)
            {
                RaindropException exc = new RaindropException(
                    "End-of-file encountered when text was expected");
                exc["raindrop.start-offset"] = reader.Offset;
                exc["raindrop.start-line"] = reader.Line;
                throw exc;
            }

            if (DelimiterReader.IsAt(reader, leftCap))
            {
                RaindropException exc = new RaindropException(
                    "Tag encountered when text was expected");
                exc["raindrop.start-offset"] = reader.Offset;
                exc["raindrop.start-line"] = reader.Line;
                throw exc;
            }

            return new TagData()
            {
                ID = "",
                Line = reader.Line,
                Offset = reader.Offset,
                Param = DelimiterReader.ReadTo(reader, leftCap, exclude_delimiter)
            };
        }

        /// <summary>
        /// Reads from the stream when it is at a tag.
        /// </summary>
        /// <returns>
        /// A TagData representing the read tag.
        /// </returns>
        public TagData ReadTag()
        {
            if (reader.Empty)
            {
                RaindropException exc = new RaindropException(
                    "End-of-file found when tag was expected");
                exc["raindrop.start-offset"] = reader.Offset;
                exc["raindrop.start-line"] = reader.Line;
                throw exc;
            }

            if (!DelimiterReader.IsAt(reader, leftCap))
            {
                RaindropException exc = new RaindropException(
                    "Plain-text found when tag was expected");
                exc["raindrop.start-offset"] = reader.Offset;
                exc["raindrop.start-line"] = reader.Line;
                throw exc;
            }

            TagData tag = new TagData()
            {
                Line = reader.Line,
                Offset = reader.Offset
            };

            string tagString = DelimiterReader.ReadTo(reader, rightCap, include_delimiter);

            if (!tagString.EndsWith(rightCap))
            {
                RaindropException exc = new RaindropException(
                    "Ending tag delimiter not found before end-of-file");
                exc["raindrop.expected-delimiter"] = rightCap;
                exc["raindrop.start-offset"] = tag.Offset;
                exc["raindrop.start-line"] = tag.Line;
                exc["raindrop.end-offset"] = reader.Offset;
                exc["raindrop.end-line"] = reader.Line;
                throw exc;
            }

            tagString = StripCaps(tagString);
            tagString = tagString.TrimEnd(trimChars);

            string[] pieces = tagString.Split(tagSplitter, 2);

            tag.ID = pieces[0];

            if (pieces.Length > 1)
            {
                tag.Param = pieces[1];
            }
            else
            {
                tag.Param = string.Empty;
            }

            return tag;
        }
    }
}