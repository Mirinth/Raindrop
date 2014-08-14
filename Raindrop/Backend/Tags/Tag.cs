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
 * The TagStruct represents a tag. It contains data such as
 * the tag's parameter, children, name and apply method.
 */

using System.Collections.Generic;
using System.IO;

namespace Raindrop.Backend.Tags
{
    public delegate void TagApplyDelegate(
        Tag tag,
        TextWriter output,
        IDictionary<string, object> data);

    public struct Tag
    {
        private readonly string _name;
        private readonly string _param;
        private readonly List<Tag> _children;
        private readonly TagApplyDelegate _apply;

        public string Name { get { return _name; } }
        public string Param { get { return _param; } }
        public List<Tag> Children { get { return _children; } }
        public TagApplyDelegate ApplyMethod { get { return _apply; } }

        public Tag(
            string name,
            string param,
            List<Tag> children,
            TagApplyDelegate applyMethod)
        {
            _name = name;
            _param = param;
            _children = children;
            _apply = applyMethod;
        }

        public void Apply(
            TextWriter output,
            IDictionary<string, object> data)
        {
            ApplyMethod(this, output, data);
        }
    }
}