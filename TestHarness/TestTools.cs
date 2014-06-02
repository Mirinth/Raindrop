using System;

namespace TestHarness
{
    class TestTools
    {
        public static void TestThrows<T>(Action action, string failMsg)
            where T : Exception
        {
            bool pass = false;
            try
            {
                action.Invoke();
            }
            catch (T)
            {
                Console.WriteLine("Passed!");
                pass = true;
            }

            if (!pass)
            {
                Console.WriteLine();
                Console.WriteLine(failMsg);
            }
        }
    }
}
