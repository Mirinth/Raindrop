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

namespace Raindrop
{
    partial class Raindrop
    {
        private class TagStream
        {
            private string leftCap;
            private string rightCap;
            private string contents;
            private string filePath;
            private int index;

            /// <summary>
            /// Initializes the TagStream with the contents of the
            /// file given by path.
            /// </summary>
            /// <param name="path">The file to extract template data from.</param>
            public TagStream(string path)
            {
                leftCap = Settings.LeftCap;
                rightCap = Settings.RightCap;
                contents = File.ReadAllText(path);
                filePath = path;
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
            /// The file path the TagStream is reading from.
            /// </summary>
            public string FilePath
            {
                get { return filePath; }
            }

            /// <summary>
            /// Gets the ID of the current tag.
            /// </summary>
            /// <returns>A string representing the ID of the current tag.</returns>
            public string GetId()
            {
                int spaceIndex = contents.IndexOf(Settings.TagSplitter, index);
                int capIndex = contents.IndexOf(Settings.RightCap, index);

                int endIndex = -1;

                if (capIndex < spaceIndex && capIndex != -1)
                {
                    endIndex = capIndex;
                }
                else
                {
                    endIndex = spaceIndex;
                }

                if (endIndex == -1)
                {
                    endIndex = contents.Length;
                }

                string result = contents.Substring(index, endIndex - index);

                return result;
            }

            /// <summary>
            /// Reads from the stream until a left cap is found.
            /// The left cap is not included in the result.
            /// </summary>
            /// <returns>
            /// The string of text from the current index to
            /// the next left cap.
            /// </returns>
            public string ReadText()
            {
                if (EOF)
                {
                    throw new RaindropException(
                        "No more data available in the TagStream.",
                        filePath,
                        index,
                        ErrorCode.TagStreamEmpty);
                }

                int endIndex = contents.IndexOf(leftCap, index);

                if (endIndex == index)
                {
                    throw new RaindropException(
                        "Tried to read text when the TagStream is at a Tag.",
                        filePath,
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

                return result;
            }

            /// <summary>
            /// Reads from the stream until a right cap is found.
            /// The caps are not included in the result.
            /// </summary>
            /// <returns>
            /// The string of text from the current index to
            /// the next right cap.
            /// </returns>
            public string ReadTag()
            {
                if (EOF)
                {
                    throw new RaindropException(
                        "No more data available in the TagStream.",
                        filePath,
                        index,
                        ErrorCode.TagStreamEmpty);
                }

                if (contents.IndexOf(leftCap, index) != index)
                {
                    throw new RaindropException(
                        "Tried to read a Tag when the TagStream is at text.",
                        filePath,
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
                        filePath,
                        index,
                        ErrorCode.TemplateFormat);
                }

                /*
                 * Want to include rightCap in result, so increase endIndex
                 * to include it. Already know the string is long enough
                 * to do this because rightCap was found.
                 */
                endIndex += rightCap.Length;

                string result = contents.Substring(index, endIndex - index);

                index = endIndex;

                return result;
            }
        }
    }
}