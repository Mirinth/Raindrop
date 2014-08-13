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
 * TagData provides additional data to a tag during construction.
 */

using System;
using System.IO;

namespace Raindrop.Backend
{
    public struct TagData
    {
        Func<string, TextReader> PathMapper { get; private set; }
        public int Line { get; private set; }
        public string Name { get; private set; }
        public string Param { get; private set; }
        public Template Source { get; private set; }

        public TagData(
            int tagLine,
            string tagName,
            string tagParam,
            Template tagSource,
            Func<string, TextReader> tagPathMapper)
        {
            Line = tagLine;
            Name = tagName;
            Param = tagParam;
            Source = tagSource;
            PathMapper = tagPathMapper;
        }
    }
}