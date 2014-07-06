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

using System.Collections.Generic;
using Raindrop.Backend.Lexer;

namespace Raindrop.Backend.Tags
{
    public class TagFactory
    {
        private static Dictionary<string, ITag> itags;

        /// <summary>
        /// The TagFactory constructor.
        /// </summary>
        static TagFactory()
        {
            itags = GetITags();
        }

        public static Dictionary<string, ITag> GetITags()
        {
            Dictionary<string, ITag> itags = new Dictionary<string, ITag>();

            ITag itag = new DataTag();
            itags.Add(itag.Name, itag);

            itag = new EscapeTag();
            itags.Add(itag.Name, itag);

            itag = new TextTag();
            itags.Add(itag.Name, itag);

            itag = new ArrayTag();
            itags.Add(itag.Name, itag);

            itag = new ArrayEndTag();
            itags.Add(itag.Name, itag);

            itag = new EofTag();
            itags.Add(itag.Name, itag);

            itag = new CondTag();
            itags.Add(itag.Name, itag);

            itag = new CondEndTag();
            itags.Add(itag.Name, itag);

            itag = new NCondTag();
            itags.Add(itag.Name, itag);

            itag = new NCondEndTag();
            itags.Add(itag.Name, itag);

            itag = new BlockTag();
            itags.Add(itag.Name, itag);

            return itags;
        }

        /// <summary>
        /// A temporary tag builder until the reflection version can be fixed.
        /// </summary>
        /// <param name="td">A TagData representing the tag to be built.</param>
        /// <returns>A TagStruct representing the next tag in the reader.</returns>
        public static TagStruct DevBuildTag(TagData td)
        {
            if (!itags.ContainsKey(td.Name))
            {
                RaindropException exc = new RaindropException("Tag is not supported.");
                exc["raindrop.encountered-tag-id"] = td.Name;
                exc["raindrop.start-offset"] = td.Reader.Offset;
                exc["raindrop.start-line"] = td.Reader.Line;
                throw exc;
            }
            
            return itags[td.Name].Build(td);
        }

        /// <summary>
        /// Parses a tag out of the given reader.
        /// </summary>
        /// <param name="reader">The TagReader to parse a tag from.</param>
        /// <returns>The tag parsed from the reader.</returns>
        public static TagStruct Parse(TagReader reader)
        {
            if (reader.Empty)
            {
                return itags[EofTag.StaticName].Build(new TagData());
            }

            TagData td = reader.Read();

            return DevBuildTag(td);

            //if (!itags.ContainsKey(td.ID))
            //{
            //    RaindropException exc = new RaindropException("Tag is not supported.");
            //    exc["raindrop.encountered-tag-id"] = td.ID;
            //    exc["raindrop.start-index"] = ts.Index;
            //    throw exc;
            //}

            //ConstructorInfo constructor = itags[td.ID];

            //// Invoke the constructor with the parameter and TagStream.
            //Tag tag = (Tag)constructor.Invoke(new object[] { td.Param, ts });

            //return tag;
        }

        /// <summary>
        /// Generates a Dictionary of Types that inherit from the Tag class,
        /// are not abstract, and have a public static field named "ID" which
        /// is a string. The  Dictionary is indexed by the value of the type's
        /// "ID" property, and all loaded assemblies are searched for objects.
        /// </summary>
        /// <returns>A Dictionary containing the identified types.</returns>
        //public static Dictionary<string, ConstructorInfo> GetTagTypes()
        //{
        //    const BindingFlags flags = BindingFlags.Public | BindingFlags.Static;
        //    Type tagBase = typeof(Tag);
        //    Dictionary<string, ConstructorInfo> types = new Dictionary<string, ConstructorInfo>();

        //    foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
        //    {
        //        foreach (Type t in asm.GetTypes())
        //        {
        //            if (!t.IsSubclassOf(tagBase)) { continue; }
        //            if (t.IsAbstract) { continue; }
        //            if (t.GetField("ID", flags) == null) { continue; }

        //            object key = t.GetField("ID").GetValue(null);

        //            // Tags with non-string ID fields can't be used, so
        //            // just skip them.
        //            if (!(key is string)) { continue; }

        //            ConstructorInfo constructor = t.GetConstructor(
        //                new Type[] { typeof(string), typeof(TagStream) });

        //            // If it doesn't implement the (string,TagStream) constructor,
        //            // then it can't be used, so skip it.
        //            if (constructor == null) { continue; }

        //            // Finally out of things to filter!
        //            // Key is a string; else it would have been filtered by the
        //            // check for (key is string) above. Casting is safe.
        //            types.Add((string)key, constructor);
        //        }
        //    }

        //    return types;
        //}
    }
}