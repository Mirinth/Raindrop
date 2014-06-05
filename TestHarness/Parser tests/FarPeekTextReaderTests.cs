using System;
using System.IO;
using Raindrop.Backend.Parser;

namespace TestHarness.Parser_tests
{
    class FarPeekTextReaderTests
    {
        static string testString = "Test string";

        public static FarPeekTextReader GetReader()
        {
            TextReader tr = new StringReader(testString);
            return new FarPeekTextReader(tr);
        }

        public static void RunTests()
        {
            TestFirstPeekIsValidChar();
            TestFirstFarPeekIsValidChar();
            TestFirstPeekIsExpectedChar();
            TestFirstFarPeekIsExpectedChar();
            TestPeekMakesNoChanges();
            TestFarPeekMakesNoChanges();
        }

        public static void TestFarPeekMakesNoChanges()
        {
            using (var reader = GetReader())
            {
                TestMakesNoChanges(reader.FarPeek, "FarPeekTextReader.FarPeek()");
            }
        }

        public static void TestPeekMakesNoChanges()
        {
            using (var reader = GetReader())
            {
                TestMakesNoChanges(GetReader().Peek, "FarPeekTextReader.Peek()");
            }
        }

        public static void TestFirstFarPeekIsExpectedChar()
        {
            using (var reader = GetReader())
            {
                TestIsExpectedChar(GetReader().FarPeek(), testString[1], "FarPeekTextReader.FarPeek()");
            }
        }

        public static void TestFirstPeekIsExpectedChar()
        {
            using (var reader = GetReader())
            {
                TestIsExpectedChar(GetReader().Peek(), testString[0], "FarPeekTextReader.Peek()");
            }
        }

        public static void TestFirstFarPeekIsValidChar()
        {
            using (var reader = GetReader())
            {
                TestIsValidChar(GetReader().FarPeek(), "FarPeekTextReader.Peek()");
            }
        }

        public static void TestFirstPeekIsValidChar()
        {
            using (var reader = GetReader())
            {
                TestIsValidChar(GetReader().Peek(), "FarPeekTextReader.Peek()");
            }
        }



        public static void TestMakesNoChanges(Func<int> method, string methodName)
        {
            int test = method();

            for (int i = 0; i < 100; i++)
            {
                int result = method();
                if (result != test)
                {
                    throw new TestException(
                        "{0} returned a different value after multiple consecutive calls. " +
                        " Expected {0}, got {1} on call {2}",
                        methodName,
                        test,
                        result,
                        i);
                }
            }

            Console.WriteLine("Pass");
        }

        public static void TestIsExpectedChar(int test, char expected, string method)
        {
            if ((char)test != expected)
            {
                throw new TestException(
                    "{0} returned {1}, expected {2}",
                    method,
                    test,
                    expected);
            }

            Console.WriteLine("Pass");
        }

        public static void TestIsValidChar(int test, string method)
        {
            if (test < char.MinValue || test > char.MaxValue)
            {
                throw new TestException(
                    "{0} returned {1}, expected a valid char.",
                    method,
                    test);
            }

            Console.WriteLine("Pass");
        }
    }
}
