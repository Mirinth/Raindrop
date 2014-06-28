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
 * The exception thrown by Raindrop in various situations when
 * no system exceptions are appropriate.
 */
using System;
using System.Collections.Generic;

namespace Raindrop.Backend
{
    public class RaindropException : Exception
    {
        private Dictionary<string, object> extraData =
            new Dictionary<string,object>();

        /// <summary>
        /// Accesses extra data related to the exception.
        /// </summary>
        /// <param name="key">The key of the data to access.</param>
        /// <returns>The value associated with key.</returns>
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

        /// <summary>
        /// Gets a summary of the exception's extra data.
        /// </summary>
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

        /// <summary>
        /// The RaindropException constructor.
        /// </summary>
        /// <param name="msg">A message describing the error.</param>
        public RaindropException(string msg)
            : base(msg) { }
    }
}