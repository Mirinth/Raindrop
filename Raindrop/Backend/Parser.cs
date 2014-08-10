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

    static class Parser
    {
        public static TagData Read(Lexer lex)
        {
            Symbol s = lex.Peek();

            if (s.Text == null)
            {
                return ReadEof(lex);
            }
            else if (s.Text == Punctuation.LeftCap)
            {
                return ReadTag(lex);
            }
            else
            {
                return ReadText(lex);
            }
        }

        private static TagData ReadEof(Lexer lex)
        {
            return new TagData()
            {
                Name = EofTag.StaticName,
                Line = -1,
                Param = EofTag.StaticName,
                Offset = -1,
                Source = lex
            };
        }

        private static TagData ReadTag(Lexer lex)
        {
            TagData td = new TagData();
            td.Source = lex;
            td.Param = "";

            Symbol leftCap = lex.Read();
            td.Line = leftCap.Line;

            Symbol name = lex.Read();
            td.Name = name.Text;

            Symbol paramPart = lex.Peek();
            while (paramPart.Text != Punctuation.RightCap)
            {
                td.Param += paramPart.Text + Punctuation.Divider;
                lex.Commit();
                paramPart = lex.Peek();
            }

            // Discard trailing cap
            lex.Commit();

            return td;
        }

        private static TagData ReadText(Lexer lex)
        {
            TagData td = new TagData();
            td.Source = lex;
            td.Param = "";
            td.Name = "";

            Symbol textPart = lex.Peek();
            td.Line = textPart.Line;
            while (textPart.Text != Punctuation.LeftCap)
            {
                td.Param += textPart.Text + Punctuation.Divider;
                lex.Commit();
                textPart = lex.Peek();
            }

            return td;
        }
    }
}