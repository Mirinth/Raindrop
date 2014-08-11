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

using System;
using System.IO;

namespace Raindrop.Backend
{
    class Template
    {
        private string sourceText;

        public char this[int index]
        {
            get { return sourceText[index]; }
        }

        public Template(string source)
        {
            sourceText = source;
        }

        public Template(TextReader source)
        {
            sourceText = source.ReadToEnd();
        }
    }
}