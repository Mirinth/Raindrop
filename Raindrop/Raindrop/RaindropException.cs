using System;
using System.Text;

namespace Raindrop
{
    partial class Raindrop
    {
        public enum ErrorCode
        {
            TagStreamEmpty,
            TagStreamAtTag,
            TagStreamAtText,
            TemplateFormat,
            TagNotSupported,
            ParameterMissing,
            AppliedEOF,
            EndTagMismatch,
            MissingKey,
        }

        public class RaindropException : Exception
        {
            public ErrorCode Code { get; set; }
            public int Index { get; set; }
            public string FilePath { get; set; }

            public RaindropException(
                string message,
                string filePath,
                int templateIndex,
                ErrorCode code)
                : base(message)
            {
                Code = code;
                Index = templateIndex;
                FilePath = filePath;
            }
        }
    }
}
