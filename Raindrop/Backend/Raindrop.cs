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

using System;
using System.Collections.Generic;
using System.IO;
using Raindrop.Backend;
using Raindrop.Backend.Parser;
using Raindrop.Backend.Tags;

namespace Raindrop
{
    public class Raindrop
    {
        ITag template;

        /// <summary>
        /// The Raindrop constructor. Constructs a template using the
        /// given data source.
        /// </summary>
        /// <param name="templateSource">The data source to construct from.</param>
        /// <param name="templateName">
        /// The name of the data source. Used for error reporting.
        /// </param>
        public Raindrop(TextReader templateSource, string templateName)
        {
            if (templateSource == null)
            {
                throw new ArgumentNullException("templateSource");
            }
            if (string.IsNullOrEmpty(templateName))
            {
                throw new ArgumentException(
                    "A name for this template source is required for error reporting.",
                    "templateName");
            }
            TagStream ts = new TagStream(templateSource, templateName);
            template = new BlockTag<EOFTag>(templateName, ts);
        }

        /// <summary>
        /// Applies the Raindrop template to data, placing the
        /// output in output.
        /// </summary>
        /// <param name="data">The data source to use.</param>
        /// <param name="output">The location to write the output.</param>
        public void Apply(
            IDictionary<string, object> data,
            TextWriter output)
        {
            template.Apply(data, output);
        }
    }
}