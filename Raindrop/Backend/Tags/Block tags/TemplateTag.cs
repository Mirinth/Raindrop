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
 * The template tag represents a template. It's like the <html> root tag,
 * but for Raindrop templates. Plus, it doesn't need to be at the start
 * of the file (it's constructed internally).
 * 
 * The eof tag represents the end of the template file.
 */

using System;
using System.Collections.Generic;
using System.IO;

namespace Raindrop.Backend.Tags
{
    public class TemplateTag : ITagBuilder
    {
        /// <summary>
        /// Gets the name of the tag.
        /// </summary>
        public string Name
        {
            get { return StaticName; }
        }

        /// <summary>
        /// Gets the name of the tag without an instance.
        /// </summary>
        public static string StaticName
        {
            get { return "template"; }
        }

        /// <summary>
        /// Gets whether a blank line before this tag should
        /// be removed.
        /// </summary>
        public bool RemoveBlankLine { get { return true; } }

        /// <summary>
        /// Builds a template tag.
        /// </summary>
        /// <param name="data">Information about the tag to build.</param>
        public TagStruct Build(TagData data)
        {
            TextReader templateReader = data.PathMapper(data.Param);
            Template templateSource = new Template(templateReader.ReadToEnd());

            data = new TagData(
                templateSource.Line,
                data.Name,
                data.Param,
                templateSource,
                data.PathMapper);

            List<TagStruct> childTags;

            try
            {
                childTags = Helpers.GetChildren(data.Source, EndTagPredicate);
            }
            catch (RaindropException exc)
            {
                exc["raindrop.template-name"] = data.Param;
                throw;
            }

            return Helpers.BuildTag(Apply, childTags, data);
        }

        /// <summary>
        /// Determines whether a given TagStruct should be considered
        /// the end of the template.
        /// </summary>
        /// <param name="tag">The TagStruct to test.</param>
        /// <returns>True if tag should end the template; else false.</returns>
        public static bool EndTagPredicate(TagStruct endTag)
        {
            if (endTag.Name == EofTag.StaticName) { return true; }
            else { return false; }
        }

        /// <summary>
        /// Applies the template tag to the given data and outputs the result.
        /// </summary>
        /// <param name="tag">The tag to be applied.</param>
        /// <param name="output">The place to put the output.</param>
        /// <param name="data">The data to be applied to.</param>
        public void Apply(
            TagStruct tag,
            TextWriter output,
            IDictionary<string, object> data)
        {
            try
            {
                foreach (TagStruct child in tag.Children)
                {
                    child.Apply(output, data);
                }
            }
            catch (RaindropException exc)
            {
                exc["raindrop.template-name"] = tag.Param;
                throw;
            }
        }
    }

    public class EofTag : ITagBuilder
    {
        /// <summary>
        /// Gets the name of the tag.
        /// </summary>
        public string Name
        {
            get { return StaticName; }
        }

        /// <summary>
        /// Gets whether a blank line before this tag should
        /// be removed.
        /// </summary>
        public bool RemoveBlankLine { get { return true; } }

        /// <summary>
        /// Gets the name of the tag without an instance.
        /// </summary>
        public static string StaticName
        {
            get { return "eof"; }
        }

        /// <summary>
        /// Builds an eof tag.
        /// </summary>
        /// <param name="data">Information about the tag to build.</param>
        public TagStruct Build(TagData data)
        {
            // eof tag intentionally doesn't access its TagData.
            // Several places need to construct eof tags without
            // having read a TagData from the TagReader, and
            // ignoring the TagData in the eof tag makes it
            // simpler for those builders without any significant
            // penalties to the eof tag.
            return new TagStruct()
            {
                ApplyMethod = Apply,
                Children = null,
                Name = StaticName,
                Param = StaticName
            };
        }

        /// <summary>
        /// Applies the eof tag to the given data and outputs the result.
        /// </summary>
        /// <param name="tag">The tag to be applied.</param>
        /// <param name="output">The place to put the output.</param>
        /// <param name="data">The data to be applied to.</param>
        public void Apply(
            TagStruct tag,
            TextWriter output,
            IDictionary<string, object> data)
        {
            throw new NotImplementedException(
                "eof does not support being applied.");
        }
    }
}