/*
 * TextTag.cs
 * By Mirinth (mirinth@gmail.com)
 * 
 * The TextTag file contains the TextTag class, which represents a
 * tag that is just plain text (nothing fancy).
 */

using System.IO;
using System.Web.Mvc;

namespace Raindrop
{
    partial class Raindrop
    {
        private class TextTag : ITag
        {
            public static string ID = Settings.LeftCap;

            public string Param
            {
                get;
                protected set;
            }

            /// <summary>
            /// The TextTag constructor.
            /// </summary>
            /// <param name="data">The template to construct this Tag from.</param>
            public TextTag(TagStream template)
            {
                Param = template.ReadText();
            }

            /// <summary>
            /// Applies the TextTag and outputs the result.
            /// </summary>
            /// <param name="data">The data to apply the TextTag to.</param>
            /// <param name="output">The place to put the output.</param>
            public void Apply(
                ViewDataDictionary data,
                TextWriter output)
            {
                output.Write(Param);
            }
        }
    }
}