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
    struct TagData
    {
        public string ID;
        public string Param;
    }

    class TagStream
    {
        private static string leftCap = "<:";
        private static string rightCap = ":>";
        private static char tagSplitter = ' ';
        private static char[] trimChars = { ' ', '/' };

        private string contents;
        private string templateName;
        private int index;

        /// <summary>
        /// Initializes the TagStream with template as its data source.
        /// </summary>
        /// <param name="template">The TextReader to initialize the TagStream with.</param>
        /// <param name="name">The name of the template. Used for error reporting.</param>
        public TagStream(TextReader template, string name)
        {
            contents = template.ReadToEnd();
            templateName = name;
            index = 0;
        }

        /// <summary>
        /// Whether the end of the file has been reached.
        /// </summary>
        public bool EOF
        {
            // The last index is contents.Length - 1,
            // so contents.Length means the end of the file.
            get { return (index >= contents.Length); }
        }

        /// <summary>
        /// The current index of the TagStream.
        /// </summary>
        public int Index
        {
            get { return index; }
        }

        /// <summary>
        /// The name of the source the TagStream is reading from.
        /// </summary>
        public string Name
        {
            get { return templateName; }
        }

        /// <summary>
        /// Checks if the TagStream is at a tag.
        /// </summary>
        /// <returns>Whether the TagStream is currently at a tag.</returns>
        public bool AtTag()
        {
            return (contents.IndexOf(leftCap, index) == index);
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
            if (AtTag())
            {
                return ReadTag();
            }
            else
            {
                return ReadText();
            }
        }

        /// <summary>
        /// Strips the endcaps off of a tag string.
        /// </summary>
        /// <param name="tag">The tag to strip endcaps from.</param>
        /// <returns>The input with the endcaps stripped.</returns>
        private string StripCaps(string tagString)
        {
            string result = tagString.Substring(
                TagStream.leftCap.Length,
                tagString.Length - TagStream.leftCap.Length - TagStream.rightCap.Length);
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
                throw new RaindropException(
                    "No more data available in the TagStream.",
                    templateName,
                    index,
                    ErrorCode.TagStreamEmpty);
            }

            int endIndex = contents.IndexOf(leftCap, index);

            if (endIndex == index)
            {
                throw new RaindropException(
                    "Tried to read text when the TagStream is at a Tag.",
                    templateName,
                    index,
                    ErrorCode.TagStreamAtTag);
            }

            if (endIndex == -1)
            {
                // If no leftCap found, read to end.
                endIndex = contents.Length;
            }

            string result = contents.Substring(index, endIndex - index);
            index = endIndex;

            return new TagData() { ID = "", Param = result };
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
                throw new RaindropException(
                    "No more data available in the TagStream.",
                    templateName,
                    index,
                    ErrorCode.TagStreamEmpty);
            }

            if (contents.IndexOf(leftCap, index) != index)
            {
                throw new RaindropException(
                    "Tried to read a Tag when the TagStream is at text.",
                    templateName,
                    index,
                    ErrorCode.TagStreamAtText);
            }

            int endIndex = contents.IndexOf(rightCap, index);

            if (endIndex == -1)
            {
                string msg = string.Format(
                    "'{0}' not found before end of file.",
                    rightCap);

                throw new RaindropException(
                    msg,
                    templateName,
                    index,
                    ErrorCode.TemplateFormat);
            }

            /*
             * Want to include rightCap in result, so increase endIndex
             * to include it. Already know the string is long enough
             * to do this because rightCap was found.
             */
            endIndex += rightCap.Length;

            string tagString = contents.Substring(index, endIndex - index);

            index = endIndex;

            tagString = StripCaps(tagString);
            tagString = tagString.TrimEnd(trimChars);

            string[] pieces = tagString.Split(new char[] { tagSplitter }, 2);

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