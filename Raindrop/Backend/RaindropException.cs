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

namespace Raindrop.Backend
{
    public class RaindropException : Exception
    {
        public string Name { get; set; }

        public RaindropException(string message, string name)
            : base(message)
        {
            Name = name;
        }
    }

    public class ParserException : RaindropException
    {
        public int Location
        {
            get;
            private set;
        }
        public ParserException(string message, string name, int index)
            : base(message, name)
        {
            Location = index;
        }
    }

    public class TemplatingException : RaindropException
    {
        public string DataPath
        {
            get;
            private set;
        }

        public TemplatingException(string message, string name, string key)
            : base(message, name)
        {
            DataPath = key;
        }

        public TemplatingException(string message, string key)
            : base(message, "<unknown template source>")
        {
            DataPath = key;
        }

        public TemplatingException(
            string message,
            string name,
            string key,
            TemplatingException innerException)
            : base (message, name)
        {
            DataPath = '[' + key + ']' + innerException.DataPath;
        }

        public TemplatingException(
            string message,
            string key,
            TemplatingException innerException)
            : base(message, "<unknown template source>")
        {
            DataPath = '[' + key + ']' + innerException.DataPath;
        }
    }
}