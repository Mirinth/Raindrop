using System;
using System.Collections.Generic;
using System.Text;
using TestHarness.Parser_tests;

namespace TestHarness
{
    public class Program
    {
        static void Main(string[] args)
        {
            TestTools.TestThrows<ArgumentNullException>(
                TestMethods.TestRaindropConstructorWithNullSource,
                "Raindrop constructor failed to detect null source parameter.");

            TestTools.TestThrows<ArgumentException>(
                TestMethods.TestRaindropConstructorWithNullName,
                "Raindrop constructor failed to detect null name parameter.");

            TestTools.TestThrows<ArgumentException>(
                TestMethods.TestRaindropConstructorWithEmptyName,
                "Raindrop constructor failed to detect empty name parameter.");

            FarPeekTextReaderTests.RunTests();

            Console.ReadLine();
        }
    }
}
