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
        private Dictionary<string, object> extraData =
            new Dictionary<string,object>();

        public object this[string key]
        {
            get
            {
                return extraData[key];
            }
            set
            {
                if (extraData.ContainsKey(key))
                {
                    extraData[key] = value;
                }
                else
                {
                    extraData.Add(key, value);
                }
            }
        }

        public string Details
        {
            get
            {
                string result = string.Empty;
                foreach (KeyValuePair<string, object> kvp in extraData)
                {
                    result += kvp.Key + "=" + kvp.Value.ToString() + ";\n";
                }
                return result;
            }
        }

        public string Name { get; set; }
        //new public string Message { get; set; }

        public RaindropException(string msg)
            : base(msg) { }
    }
}