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
 * Extracts tags from a stream.
 */

using Raindrop.Backend.Tags;

namespace Raindrop.Backend
{
    public static class Parser
    {
        /// <summary>
        /// Reads the next TagData out of a template.
        /// </summary>
        /// <param name="source">The template to read from.</param>
        /// <returns>The next TagData in source.</returns>
        public static TagData Read(Template source)
        {
            Symbol typeTest = Lexer.Peek(source);

            if (typeTest.Text == null)
            {
                return ReadEof(source);
            }
            else if (typeTest.Text == Punctuation.LeftCap)
            {
                return ReadTag(source);
            }
            else
            {
                return ReadText(source);
            }
        }

        /// <summary>
        /// Reads the next TagData out of a template without advancing the
        /// index of the template.
        /// </summary>
        /// <param name="source">The template to read from.</param>
        /// <returns>The next TagData in source.</returns>
        public static TagData Peek(Template source)
        {
            int oldIndex = source.Index;

            TagData tag = Read(source);
            source.Index = oldIndex;

            return tag;
        }

        /// <summary>
        /// Reads from a template when it is at end-of-file.
        /// </summary>
        /// <param name="source">The template to read from.</param>
        /// <returns>An eof tag.</returns>
        private static TagData ReadEof(Template source)
        {
            return new TagData()
            {
                Name = EofTag.StaticName,
                Line = -1,
                Param = EofTag.StaticName,
                Source = source
            };
        }

        /// <summary>
        /// Reads from a template when it is at a tag.
        /// </summary>
        /// <param name="source">The template to read from.</param>
        /// <returns>The non-text, non-eof tag in the template.</returns>
        private static TagData ReadTag(Template source)
        {
            TagData tag = new TagData();
            tag.Source = source;
            tag.Param = "";

            Symbol leftCap = Lexer.Read(source);
            tag.Line = leftCap.Line;

            Symbol name = Lexer.Read(source);
            tag.Name = name.Text;

            Symbol paramPart = Lexer.Peek(source);
            while (paramPart.Text != null &&
                    paramPart.Text != Punctuation.RightCap)
            {
                tag.Param += paramPart.Text + Punctuation.Divider;
                Lexer.Commit(source, paramPart);
                paramPart = Lexer.Peek(source);
            }

            if (paramPart.Text == null)
            {
                RaindropException exc = new RaindropException(
                    "Ending tag delimiter not found before end-of-file");
                exc["raindrop.expected-delimiter"] = Punctuation.RightCap;
                exc["raindrop.start-line"] = tag.Line;
                throw exc;
            }

            // Discard trailing cap
            Lexer.Commit(source, paramPart);

            tag.Name = tag.Name.Trim(Punctuation.NameTrim);
            tag.Param = tag.Param.Trim(Punctuation.ParamTrim);

            return tag;
        }

        /// <summary>
        /// Reads from a template when it is at text.
        /// </summary>
        /// <param name="source">The template to read from.</param>
        /// <returns>The text tag in the template.</returns>
        private static TagData ReadText(Template source)
        {
            TagData tag = new TagData()
            {
                Source = source,
                Param = "",
                Name = TextTag.StaticName,
            };

            Symbol textPart = Lexer.Peek(source);
            tag.Line = textPart.Line;

            while (textPart.Text != null &&
                    textPart.Text != Punctuation.LeftCap)
            {
                if (tag.Param.Length > 0 && textPart.Text.Length > 0)
                {
                    tag.Param += Punctuation.Divider;
                }

                tag.Param += textPart.Text;
                Lexer.Commit(source, textPart);
                textPart = Lexer.Peek(source);
            }

            return tag;
        }
    }
}