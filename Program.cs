using System;

namespace promise
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.SetWindowSize(170, 40);

            int GameCount = 1;
            bool isBotMatch = false;
            if (args.Length > 0 && args[0] == "botmatch")
            {
                GameCount = 5;
                isBotMatch = true;
            }
            for (int i = 0; i < GameCount; i++)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                ScreenUtils.ClearScreen();
                Game promiseGame = new Game(isBotMatch);
            }
        }
    }
}
