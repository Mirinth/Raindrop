using System.Collections.Generic;
using System.IO;

namespace Raindrop.Backend.Tags
{
    public struct TagStruct
    {
        public string Name;
        public string Param;
        public List<TagStruct> Children;
        public ApplyTagDelegate ApplyMethod;

        public void Apply(
            TextWriter output,
            IDictionary<string, object> data)
        {
            ApplyMethod(this, output, data);
        }
    }
}
