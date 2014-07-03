﻿/*
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
 * TagApplyer applies tags given a TagStruct.
 */

using System.Collections.Generic;
using System.IO;
using Raindrop.Backend.Tags;

namespace Raindrop.Backend.Templater
{
    class TagApplyer
    {
        public static void Apply(
            TagStruct tag,
            TextWriter output,
            IDictionary<string, object> data)
        {
            TagFactory.ApplyITag(tag, output, data);
        }
    }
}