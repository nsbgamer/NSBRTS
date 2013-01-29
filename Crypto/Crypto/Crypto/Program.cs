using System;

namespace Crypto
{
#if WINDOWS || XBOX
    static class Program
    {
        static String testStr1 = "abcdef";
        //String testStr2 = "";
        //String testStr3 = "";
        //String testStr4 = "";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            Encryption coder = new Encryption();
            string encodeTestStr1 = coder.Encrypt(testStr1, 1);
            string decodeTestStr1 = coder.Decrypt(encodeTestStr1, 1);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("FINAL RESULTS testStr1       = " + testStr1);
            Console.WriteLine("FINAL RESULTS encodeTestStr1 = " + encodeTestStr1);
            Console.Write("FINAL RESULTS decodeTestStr1 = ");
            Console.WriteLine(decodeTestStr1);

            using (Game1 game = new Game1())
            {
                game.Run();
            }
        }
    }
#endif
}

