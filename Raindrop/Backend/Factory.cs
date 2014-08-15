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
 * The TagFactory is responsible for constructing tags.
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using Raindrop.Backend.Tags;

namespace Raindrop.Backend
{
    public static class Factory
    {
        private static Dictionary<string, ITagBuilder> builders;

        /// <summary>
        /// The Factory constructor.
        /// </summary>
        static Factory()
        {
            builders = GetITagBuilders();
        }

        /// <summary>
        /// Gets all the types in the current AppDomain that
        /// inherit the ITagBuilder interface.
        /// </summary>
        /// <returns>
        /// A List of Types inheriting ITagBuilder.
        /// </returns>
        private static List<Type> GetBuilderTypes()
        {
            Type builderType = typeof(ITagBuilder);
            List<Type> types = new List<Type>();

            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type t in asm.GetTypes())
                {
                    if (t.GetInterface(builderType.FullName) != null)
                    {
                        types.Add(t);
                    }
                }
            }

            return types;
        }

        /// <summary>
        /// Gets a dictionary of classes implementing the ITagBuilder interface
        /// keyed by the builder's Name property.
        /// </summary>
        /// <returns>A dictionary of ITagBuilders.</returns>
        private static Dictionary<string, ITagBuilder> GetITagBuilders()
        {
            List<Type> builderTypes = GetBuilderTypes();
            Dictionary<string, ITagBuilder> itags = new Dictionary<string, ITagBuilder>();

            foreach (Type t in builderTypes)
            {
                ConstructorInfo ci = t.GetConstructor(Type.EmptyTypes);

                if (ci == null)
                {
                    RaindropException exc = 
                        new RaindropException("Found ITagBuilder with no default constructor");
                    exc["raindrop.type-name"] = t.FullName;
                    throw exc;
                }

                object oBuilder = ci.Invoke(null);
                ITagBuilder builder = (ITagBuilder)oBuilder;

                if (itags.ContainsKey(builder.Name))
                {
                    RaindropException exc =
                        new RaindropException("Duplicate tags found.");
                    exc["raindrop.original-type"] = itags[builder.Name].GetType().FullName;
                    exc["raindrop.duplicate-type"] = builder.GetType().FullName;
                    exc["raindrop.duplicate-tag-name"] = builder.Name;
                    throw exc;
                }

                itags.Add(builder.Name, builder);
            }

            return itags;
        }

        /// <summary>
        /// Builds the next tag in a template.
        /// </summary>
        /// <param name="source">The template to build a tag from.</param>
        /// <returns>The tag built from the template.</returns>
        public static Tag Build(Template source)
        {
            TagData data = Parser.Read(source);

            if (!builders.ContainsKey(data.Name))
            {
                RaindropException exc = new RaindropException("Tag is not supported.");
                exc["raindrop.encountered-tag-id"] = data.Name;
                exc["raindrop.start-line"] = data.Source.Line;
                throw exc;
            }

            Tag tag = builders[data.Name].Build(data);
            return tag;
        }

        /// <summary>
        /// Determines whether the next tag requests that blank lines
        /// before it be removed.
        /// </summary>
        /// <param name="source">The template to get the next tag from.</param>
        /// <returns>Whether to remove a preceding blank line.</returns>
        public static bool RemovePrecedingBlankLine(Template source)
        {
            TagData tag = Parser.Peek(source);

            bool shouldRemove = builders[tag.Name].RemoveBlankLine;
            
            return shouldRemove;
        }
    }
}