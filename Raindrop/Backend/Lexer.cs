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

    class Lexer
    {
        private string sourceText;
        private int nlCount;
        private int crCount;
        private int index;

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
            if (index == sourceText.Length)
            {
                return new Symbol() { Line = -1, Text = null };
            }

            Symbol s = new Symbol();

            s.Text = GetSymbolText();
            s.Line = CurrentLine();

            UpdateState(s.Text);

            return s;
        }

        /// <summary>
        /// Identifies the index immediately after the current symbol.
        /// </summary>
        /// <returns>The index immediately after the current symbol.</returns>
        private int FindSymbolEnd()
        {
            // Handle when we're already at punctuation
            if (sourceText[index] == '<' && sourceText[index + 1] == ':')
            {
                return index + 2;
            }
            if (sourceText[index] == ':' && sourceText[index + 1] == '>')
            {
                return index + 2;
            }

            // Else find the next punctuation.
            for (int i = index; i < sourceText.Length - 1; i++)
            {
                if (sourceText[i] == '<' && sourceText[i + 1] == ':')
                {
                    return i;
                }
                if (sourceText[i] == ':' && sourceText[i + 1] == '>')
                {
                    return i;
                }
            }

            // Else the rest of the string should be used.
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
            // \r\n system between lines
            else if (crCount - nlCount == 1) { return crCount; }
            // \n\r system between lines
            else if (nlCount - crCount == 1) { return nlCount; }
            // Probably inconsistent line endings.
            else { return -1; } // 
        }

        /// <summary>
        /// Updates the line and index to reflect that the
        /// given symbol has been read.
        /// </summary>
        /// <param name="symbol">The symbol which has been read.</param>
        private void UpdateState(string symbol)
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