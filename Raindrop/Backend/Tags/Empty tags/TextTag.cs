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
 * TextTag.cs
 * By Mirinth (mirinth@gmail.com)
 * 
 * The TextTag file contains the TextTag class, which represents a
 * tag that is just plain text (nothing fancy).
 */

using System.Collections.Generic;
using System.IO;
using Raindrop.Backend.Parser;

namespace Raindrop.Backend
{
    class TextTag : ITag
    {
        public static string ID = string.Empty;

        public string Param
        {
            get;
            protected set;
        }

        /// <summary>
        /// The TextTag constructor.
        /// </summary>
        /// <param name="param">The tag's parameter.</param>
        /// <param name="ts">A TagStream to construct child tags from.</param>
        public TextTag(string param, TagStream ts)
        {
            Param = param;
        }

        /// <summary>
        /// Applies the TextTag and outputs the result.
        /// </summary>
        /// <param name="data">The data to apply the TextTag to.</param>
        /// <param name="output">The place to put the output.</param>
        public void Apply(
            IDictionary<string, object> data,
            TextWriter output)
        {
            output.Write(Param);
        }
    }
}