using System;

namespace TestHarness
{
    class TestException : Exception
    {
        public TestException(string msg) : base(msg) { }
        public TestException(string msg, params object[] p)
            : base(string.Format(msg, p)) { }
    }
}
