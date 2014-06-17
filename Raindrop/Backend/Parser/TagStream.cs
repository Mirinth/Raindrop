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

using System.IO;

namespace Raindrop.Backend.Parser
{
    public struct TagData
    {
        public string ID;
        public string Param;
    }

    public class TagStream
    {
        private static readonly string leftCap = "<:";
        private static readonly string rightCap = ":>";
        private static readonly char[] tagSplitter = { ' ' };
        private static readonly char[] trimChars = { ' ', '/' };

        const bool include_delimiter = true;
        const bool exclude_delimiter = false;

        private TagReader reader;

        /// <summary>
        /// Initializes the TagStream with template as its data source.
        /// </summary>
        /// <param name="template">The TextReader to initialize the TagStream with.</param>
        /// <param name="name">The name of the template. Used for error reporting.</param>
        public TagStream(TextReader template)
        {
            reader = new TagReader(new FarPeekTextReader(template));
        }

        /// <summary>
        /// Whether the end of the file has been reached.
        /// </summary>
        public bool EOF
        {
            get { return reader.EOF; }
        }

        /// <summary>
        /// The current index of the TagStream.
        /// </summary>
        public int Index
        {
            get { return reader.Index; }
        }

        /// <summary>
        /// Gets the next tag (or text) in the TagStream.
        /// </summary>
        /// <returns>The next tag (or text) in the TagStream.</returns>
        public TagData GetTag()
        {
            if (EOF)
            {
                return new TagData() { ID = "EOF", Param = "EOF" };
            }

            if (reader.IsAt(leftCap)) { return ReadTag(); }
            else { return ReadText(); }
        }

        /// <summary>
        /// Strips the endcaps off of a tag string.
        /// </summary>
        /// <param name="tag">The tag to strip endcaps from.</param>
        /// <returns>The input with the endcaps stripped.</returns>
        public string StripCaps(string tagString)
        {
            string result = tagString.Substring(
                TagStream.leftCap.Length,
                tagString.Length - TagStream.leftCap.Length - TagStream.rightCap.Length);
            return result;
        }

        /// <summary>
        /// Converts an escape sequence into plain text.
        /// </summary>
        /// <param name="sequence">The sequence to convert.</param>
        /// <returns>The unescaped sequence.</returns>
        public string Unescape(string sequence)
        {
            string result = string.Empty;
            string[] pieces = sequence.Split(tagSplitter);

            foreach (string piece in pieces)
            {
                switch (piece)
                {
                    case "lc":
                        result += leftCap;
                        break;
                    case "rc":
                        result += rightCap;
                        break;
                    default:
                        RaindropException exc = new RaindropException(
                            "Invalid escape sequence");
                        exc["raindrop.escape-sequence"] = sequence;
                        exc["raindrop.start-index"] = reader.Index;
                        throw exc;
                }
            }

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
            if (EOF)
            {
                RaindropException exc = new RaindropException(
                    "End-of-file encountered when text was expected");
                exc["raindrop.start-index"] = reader.Index;
                throw exc;
            }

            if (reader.IsAt(leftCap))
            {
                RaindropException exc = new RaindropException(
                    "Tag encountered when text was expected");
                exc["raindrop.start-index"] = reader.Index;
                throw exc;
            }

            return new TagData()
            {
                ID = "",
                Param = reader.ReadTo(leftCap, exclude_delimiter)
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
            if (EOF)
            {
                RaindropException exc = new RaindropException(
                    "End-of-file found when tag was expected");
                exc["raindrop.start-index"] = reader.Index;
                throw exc;
            }

            if (!reader.IsAt(leftCap))
            {
                RaindropException exc = new RaindropException(
                    "Plain-text found when tag was expected");
                exc["raindrop.start-index"] = reader.Index;
                throw exc;
            }

            int startIndex = reader.Index;

            string tagString = reader.ReadTo(rightCap, include_delimiter);

            if (!tagString.EndsWith(rightCap))
            {
                RaindropException exc = new RaindropException(
                    "Ending tag delimiter not found before end-of-file");
                exc["raindrop.expected-delimiter"] = rightCap;
                exc["raindrop.start-index"] = startIndex;
                exc["raindrop.end-index"] = reader.Index;
                throw exc;
            }

            tagString = StripCaps(tagString);
            tagString = tagString.TrimEnd(trimChars);

            string[] pieces = tagString.Split(tagSplitter, 2);

            TagData tag = new TagData();
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