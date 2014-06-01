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
 * Raindrop.cs
 * By Mirinth (mirinth@gmail.com)
 * 
 * The Raindrop file contains public functions for using the
 * Raindrop class. These functions handle converting a file
 * name into a template, and applying a template to a data
 * dictionary.
 */

using System.IO;
using System.Web.Mvc;
using Raindrop.Backend;

namespace Raindrop
{
    class Raindrop
    {
        ITag template;

        /// <summary>
        /// The Raindrop constructor. Constructs a template using the
        /// given file name.
        /// </summary>
        /// <param name="fileName">
        /// The path to a file containing the template.
        /// </param>
        public Raindrop(TextReader data, string fileName)
        {
            TagStream ts = new TagStream(data, fileName);
            template = new RootTag(ts);
        }

        /// <summary>
        /// Applies the Raindrop template to data, placing the
        /// output in output.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="output"></param>
        public void Apply(
            ViewDataDictionary data,
            TextWriter output)
        {
            template.Apply(data, output);
        }
    }
}