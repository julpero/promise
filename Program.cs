using System;
using System.IO;
using System.Linq; 
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Bson;

namespace promise
{

    class Program
    {
        static void Main(string[] args)
        {
            const int CREATEDBOTS = 5;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.SetWindowSize(170, 42);

            int GameCount = 1;
            bool isBotMatch = false;
            bool showCards = true;
            bool randomizedBots = false;

            MongoClient mongoClient = null;
            IMongoDatabase database = null;
            IMongoCollection<MongoAI> collection = null;
            List<MongoAI> mongoAIs = new List<MongoAI>();

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
                try
                {
                    string[] config = File.ReadAllLines("mongo.config");
                    foreach (string line in config)
                    {
                        if (line.Length < 1) continue;
                        if (line.Substring(0, 2) == "//") continue;
                        string[] values = line.Split('=');
                        if (values.Count() == 2)
                        {
                            switch (values[0])
                            {
                                case "MongoClient": mongoClient = new MongoClient(values[1]); break;
                                case "database": database = mongoClient.GetDatabase(values[1]); break;
                                case "collection": collection = database.GetCollection<MongoAI>(values[1]); break;
                                default: break;
                            }
                        }
                    }
                }
                catch
                {
                    throw;
                }
            }

            mongoAIs.Add(new MongoAI("Fuison"));

            for (int i = 0; i < Math.Max(CREATEDBOTS, 5); i++)
            {
                if (randomizedBots)
                {
                    string name = Guid.NewGuid().ToString();
                    mongoAIs.Add(new MongoAI(name));
                }
                else
                {
                    mongoAIs.Add(new MongoAI(i));
                }
            }
            Random randomX = new Random();
            mongoAIs = mongoAIs.OrderBy(x => randomX.Next()).ToList();


            for (int i = 0; i < GameCount; i++)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                ScreenUtils.ClearScreen();
                Game promiseGame = new Game(isBotMatch, showCards, randomizedBots, mongoAIs, collection);
            }
        }
    }
}
