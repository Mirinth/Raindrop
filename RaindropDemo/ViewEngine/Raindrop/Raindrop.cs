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

namespace Raindrop
{
    partial class Raindrop
    {
        ITag template;

        /// <summary>
        /// The Raindrop constructor. Constructs a template using the
        /// given file name.
        /// </summary>
        /// <param name="fileName">
        /// The path to a file containing the template.
        /// </param>
        public Raindrop(string fileName)
        {
            TagStream ts = new TagStream(fileName);
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
