using System;
using System.IO;

namespace TestHarness
{
    class TestMethods
    {
        public static void TestRaindropConstructorWithNullSource()
        {
            Raindrop.Raindrop rd = new Raindrop.Raindrop(null, "");
        }

        public static void TestRaindropConstructorWithNullName()
        {
            using (StringReader sr = new StringReader(""))
            {
                Raindrop.Raindrop rd = new Raindrop.Raindrop(sr, null);
            }
        }

        public static void TestRaindropConstructorWithEmptyName()
        {
            using (StringReader sr = new StringReader(""))
            {
                Raindrop.Raindrop rd = new Raindrop.Raindrop(sr, "");
            }
        }
    }
}
