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

using System.IO;

namespace Raindrop.Backend
{
    struct Symbol
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

    class Lexer
    {
        private string sourceText;
        private int nlCount = 0;
        private int crCount = 0;
        private int index = 0;

        private Symbol lookahead;
        private bool lookaheadSet = false;

        /// <summary>
        /// Initialize the lexer with a string.
        /// </summary>
        /// <param name="source">
        /// A string containing the text to lex.
        /// </param>
        public Lexer(string source)
        {
            sourceText = source;
        }

        /// <summary>
        /// Initialize the lexer with a string.
        /// </summary>
        /// <param name="source">
        /// A TextReader containing the text to lex.
        /// </param>
        public Lexer(TextReader source)
        {
            sourceText = source.ReadToEnd();
        }

        /// <summary>
        /// Reads a symbol from the lexer.
        /// </summary>
        /// <returns>
        /// The next symbol in the lexer if available;
        /// an empty symbol (Text = null, Line = -1) otherwise.
        /// </returns>
        public Symbol Read()
        {
            Symbol s = Peek();
            Commit(s.Text);
            return s;
        }

        /// <summary>
        /// Looks at the next symbol in the lexer, but doesn't read it.
        /// </summary>
        /// <returns>
        /// The next symbol in the lexer if available;
        /// an empty symbol (Text = null, Line = -1) otherwise.
        /// </returns>
        public Symbol Peek()
        {
            if (lookaheadSet) { return lookahead; }

            if (index == sourceText.Length)
            {
                lookahead = new Symbol() { Line = -1, Text = null };
                lookaheadSet = true;
                return lookahead;
            }

            lookahead = new Symbol();
            lookahead.Text = GetSymbolText();
            lookahead.Line = CurrentLine();

            return lookahead;
        }

        /// <summary>
        /// Commits the most recent Peek() as read.
        /// </summary>
        public void Commit()
        {
            if (lookaheadSet)
            {
                Commit(lookahead.Text);
                lookaheadSet = false;
            }
        }

        /// <summary>
        /// Identifies the index immediately after the current symbol.
        /// </summary>
        /// <returns>The index immediately after the current symbol.</returns>
        private int FindSymbolEnd()
        {
            // Are we already at punctuation?
            if (sourceText.SubstringIsAt(Punctuation.LeftCap, index))
            {
                return index + Punctuation.LeftCap.Length;
            }
            if (sourceText.SubstringIsAt(Punctuation.RightCap, index))
            {
                return index + Punctuation.RightCap.Length;
            }
            if (sourceText.SubstringIsAt(Punctuation.Divider, index))
            {
                return index + Punctuation.Divider.Length;
            }

            // Where's the next punctuation?
            for (int i = index; i < sourceText.Length - Punctuation.Longest; i++)
            {
                if (sourceText.SubstringIsAt(Punctuation.LeftCap, i))
                {
                    return i;
                }
                if (sourceText.SubstringIsAt(Punctuation.RightCap, i))
                {
                    return i;
                }
                if (sourceText.SubstringIsAt(Punctuation.Divider, i))
                {
                    return i;
                }
            }

            // Just use the rest of the string.
            return sourceText.Length;
        }

        /// <summary>
        /// Gets the text of the next symbol.
        /// </summary>
        /// <returns>The text of the next symbol.</returns>
        private string GetSymbolText()
        {
            int symbolEnd = FindSymbolEnd();
            return sourceText.Substring(index, symbolEnd - index);
        }

        /// <summary>
        /// Calculates the current line.
        /// </summary>
        /// <returns>The calculated current line.</returns>
        private int CurrentLine()
        {
            // Newline convention:

            // \r\n, \n\r, or first line
            if (nlCount == crCount) { return nlCount; }
            // \n system (or first line)
            else if (crCount == 1) { return nlCount; }
            // \r system (or first line)
            else if (nlCount == 1) { return crCount; }
            // \r\n system between lines
            else if (crCount - nlCount == 1) { return crCount; }
            // \n\r system between lines
            else if (nlCount - crCount == 1) { return nlCount; }
            // Probably inconsistent line endings.
            else { return -1; } // 
        }

        /// <summary>
        /// Commits a symbol as read.
        /// </summary>
        /// <param name="symbol">The symbol which has been read.</param>
        private void Commit(string symbol)
        {
            foreach (char c in symbol)
            {
                if (c == '\r') { crCount++; }
                if (c == '\n') { nlCount++; }
            }

            index += symbol.Length;
        }
    }
}