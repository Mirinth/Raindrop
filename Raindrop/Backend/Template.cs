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
 * Contains the text of a template.
 */

using System.IO;

namespace Raindrop.Backend
{
    public class Template
    {
        private int nlCount = 1;
        private int crCount = 1;
        private int offset = 0;

        public string Text { get; set; }

        public int Index
        {
            get { return offset; }
            set
            {
                if (value >= offset) { RecalculateLine(value, offset); }
                else { RecalculateLine(value); }
                offset = value;
            }
        }

        public int Line
        {
            get
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
                else { return -1; }
            }
        }

        public Template(string source)
        {
            Text = source;
        }

        private void RecalculateLine(int offset)
        {
            RecalculateLine(offset, 0);
        }

        private void RecalculateLine(int offset, int oldOffset)
        {
            for (int i = oldOffset; i < offset && i < Text.Length; i++)
            {
                if (Text[i] == '\r') { crCount++; }
                if (Text[i] == '\n') { nlCount++; }
            }
        }
    }
}