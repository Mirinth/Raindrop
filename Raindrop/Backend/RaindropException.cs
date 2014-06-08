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
using System.Collections.Generic;

namespace Raindrop.Backend
{
    public class RaindropException : Exception
    {
        public string Name { get; set; }

        public RaindropException(string message, string name)
            : base(message + " See Name for the name of the " +
                    "template source the error occurred with.")
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
            : base(message + " See Location for the index into the stream " +
                    "where the error was first noticed.",
            name)
        {
            Location = index;
        }
    }

    public class KeyException : RaindropException
    {
        private string path;

        public IDictionary<string, object> Dictionary
        {
            get;
            private set;
        }
        public string KeyPath
        {
            get
            {
                return "Dictionary" + path;
            }
        }

        public KeyException(string key)
            : base("A key was missing from the data dictionary. " +
                    "See KeyPath for a path to the missing key " +
                    "and DataDictionary for the dictionary the missing key was missing from.",
                    "<unknown template source>")
        {
            path = string.Empty;
            AddKeyLevel(key);
        }

        public void AddKeyLevel(string key)
        {
            path = "[" + key + "]" + path;
        }
    }
}