using System;
using System.IO;
using System.Linq; 
using System.Collections.Generic;

namespace promise
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.SetWindowSize(170, 42);

            int GameCount = 1;
            bool isBotMatch = false;
            bool showCards = true;
            bool randomizedBots = false;

            List<PlayerAI> playerAIs = new List<PlayerAI>();

            if (args.Any(x => x.ToLower() == "botmatch"))
            {
                GameCount = 5;
                isBotMatch = true;
            }
            if (args.Any(x => x.ToLower() == "hidecards"))
            {
                showCards = false;
            }
            if (args.Any(x => x.ToLower() == "randombots"))
            {
                randomizedBots = true;
            }
            if (args.Any(x => x.ToLower() == "totaltest"))
            {
                // totalTest = true;
            }
            if (args.Any(x => x.ToLower() == "usedb"))
            {

            }

            for (int i = 0; i < 5; i++)
            {
                playerAIs.Add(new PlayerAI());
            }

            for (int i = 0; i < GameCount; i++)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                ScreenUtils.ClearScreen();
                Game promiseGame = new Game(isBotMatch, showCards, randomizedBots, playerAIs);
            }
        }
    }
}
