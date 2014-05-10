using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raindrop
{
    partial class Raindrop
    {
        private class Helpers
        {
            public static bool Pass(
                Dictionary<string, object> data,
                string Param)
            {
                // If the element doesn't exist, it's false.
                if (!data.ContainsKey(Param))
                {
                    return false;
                }

                // If the element is a bool, it's equal to itself.
                else if (data[Param] is bool)
                {
                    return (bool)data[Param];
                }

                // If the element is an IEnumerable with contents, it's true.
                else if (data[Param] is IEnumerable<Tag>)
                {
                    // IEnumerable doesn't expose a Count variable, so test if it's
                    // populated by trying to enumerate it and return true on
                    // the first element.
                    IEnumerable<Tag> dummy = (IEnumerable<Tag>)data[Param];

                    // Loop will be skipped if dummy is empty.
                    foreach (Tag t in dummy)
                    {
                        return true;
                    }

                    // The IEnumerable was empty.
                    return false;
                }

                // In all other cases, assume false.
                return false;
            }
        }
    }
}
