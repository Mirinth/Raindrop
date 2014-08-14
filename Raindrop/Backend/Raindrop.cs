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
 * The Raindrop file contains public functions for using the
 * Raindrop class. These functions handle converting a file
 * name into a template, and applying a template to a data
 * dictionary.
 */

using System;
using System.Collections.Generic;
using System.IO;
using Raindrop.Backend;
using Raindrop.Backend.Tags;

namespace Raindrop
{
    public class Raindrop
    {
        Tag template;

        /// <summary>
        /// The Raindrop constructor. Constructs a template using the
        /// given data source and mapper.
        /// </summary>
        /// <param name="templateName">
        /// The name of the template.
        /// </param>
        /// <param name="mapper">
        /// Maps templateName to a file on disk.
        /// </param>
        public Raindrop(string templateName, Func<string, TextReader> mapper)
        {
            // A bootstrap template is used instead of modifying the
            // real template because it's invisible.
            string bootstrap =
                Punctuation.LeftCap +
                TemplateTag.StaticName +
                Punctuation.Divider +
                templateName +
                Punctuation.RightCap;

            Template seedTemplate = new Template(bootstrap, mapper);

            template = Factory.Build(seedTemplate);
        }

        /// <summary>
        /// Applies the Raindrop template to data, placing the
        /// output in output.
        /// </summary>
        /// <param name="data">The data source to use.</param>
        /// <param name="output">The location to write the output.</param>
        public void Apply(
            TextWriter output,
            IDictionary<string, object> data)
        {
            template.Apply(output, data);
        }
    }
}