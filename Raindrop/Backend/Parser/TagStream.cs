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
        private string templateName;

        /// <summary>
        /// Initializes the TagStream with template as its data source.
        /// </summary>
        /// <param name="template">The TextReader to initialize the TagStream with.</param>
        /// <param name="name">The name of the template. Used for error reporting.</param>
        public TagStream(TextReader template, string name)
        {
            reader = new TagReader(new FarPeekTextReader(template));
            templateName = name;
        }

        /// <summary>
        /// Whether the end of the file has been reached.
        /// </summary>
        public bool EOF
        {
            // The last index is contents.Length - 1,
            // so contents.Length means the end of the file.
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
        /// The name of the source the TagStream is reading from.
        /// </summary>
        public string Name
        {
            get { return templateName; }
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

        public string Escape(string sequence)
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
                        string msg = string.Format(
                            "Unrecognized escape: {0}", piece);
                        throw new ParserException(
                            msg,
                            Name,
                            Index);

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
                throw new ParserException(
                    "End of file found when TagStream expected text.",
                    templateName,
                    reader.Index);
            }

            if (reader.IsAt(leftCap))
            {
                throw new ParserException(
                    "Tag found when TagStream expected text.",
                    templateName,
                    reader.Index);
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
                throw new ParserException(
                    "End of file found when TagStream expected tag.",
                    templateName,
                    reader.Index);
            }

            if (!reader.IsAt(leftCap))
            {
                throw new ParserException(
                    "Text found when TagStream expected tag.",
                    templateName,
                    reader.Index);
            }

            string tagString = reader.ReadTo(rightCap, include_delimiter);

            if (!tagString.EndsWith(rightCap))
            {
                string msg = string.Format(
                    "Ending tag delimiter '{0}' not found before end of file.",
                    rightCap);

                throw new ParserException(
                    msg,
                    templateName,
                    reader.Index);
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