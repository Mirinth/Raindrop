using System;
using Raindrop.Backend.Parser;
using System.IO;
using System.Collections.Generic;

namespace Raindrop.Backend.Tags
{
    public delegate void ApplyTagDelegate(
        TagStruct tag,
        TextWriter output,
        IDictionary<string, object> data);

    public delegate bool EndTagPredicate(TagStruct endTag);

    class TagBuilderAttribute : Attribute
    {
        public string Name;
        public TagBuilderAttribute(string tagName)
        {
            Name = tagName;
        }
    }
}
