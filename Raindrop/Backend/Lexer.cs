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
 * Breaks an input file into symbols.
 */

namespace Raindrop.Backend
{
    public struct Symbol
    {
        public string Text;
        public int Line;
    }

    static class StartsWithExtension
    {
        /// <summary>
        /// Determines if a string contains a substring at the given location.
        /// </summary>
        /// <param name="str">The string to search.</param>
        /// <param name="value">The substring to check for.</param>
        /// <param name="startIndex">The index to begin the search at.</param>
        /// <returns>
        /// True if the given substring is present at the given
        /// location; else false.
        /// </returns>
        public static bool SubstringIsAt(this string str, string value, int startIndex)
        {
            if (value.Length + startIndex > str.Length) { return false; }

            int findResult = str.IndexOf(value, startIndex, value.Length);

            return (findResult == startIndex);
        }
    }

    public static class Lexer
    {
        /// <summary>
        /// Reads a symbol from a template.
        /// </summary>
        /// <param name="source">The Template to read a symbol from.</param>
        /// <returns>
        /// The next symbol in the template if available;
        /// an empty symbol (Text = null, Line = -1) otherwise.
        /// </returns>
        public static Symbol Read(Template source)
        {
            Symbol s = Peek(source);
            Commit(source, s);
            return s;
        }

        /// <summary>
        /// Looks at the next symbol in the template, but doesn't read it.
        /// </summary>
        /// <param name="source">The template to read from.</param>
        /// <returns>
        /// The next symbol in the template if available;
        /// an empty symbol (Text = null, Line = -1) otherwise.
        /// </returns>
        public static Symbol Peek(Template source)
        {
            if (source.Index == source.Text.Length)
            {
                return new Symbol() { Line = -1, Text = null };
            }

            Symbol s = new Symbol();
            s.Text = GetSymbolText(source);
            s.Line = source.Line;

            return s;
        }

        /// <summary>
        /// Commits a symbol as read.
        /// </summary>
        /// <param name="source">The template the symbol was read from.</param>
        /// <param name="s">The symbol that was read.</param>
        public static void Commit(Template source, Symbol s)
        {
            source.Index += s.Text.Length;
        }

        /// <summary>
        /// Identifies the index immediately after the current symbol.
        /// </summary>
        /// <param name="source">The template to search.</param>
        /// <param name="index">The index to begin searching at.</param>
        /// <returns>The index immediately after the current symbol.</returns>
        private static int FindSymbolEnd(Template source, int index)
        {
            // Are we already at punctuation?
            if (source.Text.SubstringIsAt(Punctuation.LeftCap, index))
            {
                return index + Punctuation.LeftCap.Length;
            }
            if (source.Text.SubstringIsAt(Punctuation.RightCap, index))
            {
                return index + Punctuation.RightCap.Length;
            }
            if (source.Text.SubstringIsAt(Punctuation.Divider, index))
            {
                return index + Punctuation.Divider.Length;
            }

            // Where's the next punctuation?
            for (int i = index; i < source.Text.Length - Punctuation.Longest; i++)
            {
                if (source.Text.SubstringIsAt(Punctuation.LeftCap, i))
                {
                    return i;
                }
                if (source.Text.SubstringIsAt(Punctuation.RightCap, i))
                {
                    return i;
                }
                if (source.Text.SubstringIsAt(Punctuation.Divider, i))
                {
                    return i;
                }
            }

            // Just use the rest of the string.
            return source.Text.Length;
        }

        /// <summary>
        /// Gets the text of the next symbol.
        /// </summary>
        /// <param name="source">The template to read from.</param>
        /// <returns>The text of the next symbol.</returns>
        private static string GetSymbolText(Template source)
        {
            int symbolEnd = FindSymbolEnd(source, source.Index);
            return source.Text.Substring(source.Index, symbolEnd - source.Index);
        }
    }
}