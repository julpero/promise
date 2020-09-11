using System;
using System.IO;
using System.Linq; 
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Bson;

namespace promise
{
    public class MongoAI
    {
        public ObjectId Id {get; set;}
        public string AiName {get; set;}
        public PlayerAI PlayerAI {get; set;}
        public int Points {get; set;}
        public int PromisesKept {get; set;}
        public int Evolution {get; set;}
        public DateTime Created {get; set;}

        public MongoAI(string guid, PlayerAI playerAI, int points, int promisesKept, int evolution = 0)
        {
            AiName = guid;
            PlayerAI = playerAI;
            Points = points;
            PromisesKept = promisesKept;
            Created = DateTime.Now;
            Evolution = evolution + 1;
        }

        public MongoAI(string guid, PlayerAI playerAI, int evolution = 0)
        {
            AiName = guid;
            PlayerAI = playerAI;
            Points = 0;
            PromisesKept = 0;
            Created = DateTime.Now;
            Evolution = evolution + 1;
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.SetWindowSize(170, 42);

            int GameCount = 1;
            int RandomizeLimit = 1;
            bool isBotMatch = false;
            bool showCards = true;
            bool randomizedBots = false;
            bool useDb = false;
            bool totalTest = false;

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
                totalTest = true;
            }
            if (args.Any(x => x.ToLower() == "usedb"))
            {
                useDb = true;
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

                    // get ai's from mongo

                    // or use now some random
                    for (int i = 0; i < 5; i++)
                    {
                        mongoAIs.Add(new MongoAI(Guid.NewGuid().ToString(), new PlayerAI("")));
                    }
                }
                catch
                {
                    throw;
                }
            }
            totalTest = totalTest && randomizedBots && useDb;

            if (totalTest)
            {
                GameCount = 20;
                RandomizeLimit = 20000;
                ScreenUtils.ClearScreen();
                Console.SetCursorPosition(0, 0);
            }

            
            for (int randomize = 0; randomize < RandomizeLimit; randomize++)
            {
                if (totalTest)
                {
                    var goodOnes = collection.AsQueryable()
                                            .GroupBy(x => new {AiName = x.AiName})
                                            .Select(x => new {AiName = x.Key.AiName
                                                            , AvgPoints = x.Average(y => y.Points)
                                                            , AvgKeeps = x.Average(y => y.PromisesKept)
                                                            }).OrderByDescending(z => z.AvgPoints).Take(2).ToList();
                    
                    MongoAI mongoAI1 = collection.Find(x => x.AiName == goodOnes.First().AiName).First();
                    MongoAI mongoAI2 = collection.Find(x => x.AiName == goodOnes.Last().AiName).First();

                    Logger.Log($"paras: {mongoAI1.AiName}", "parhaat");
                    
                    mongoAIs = new List<MongoAI>();

                    string guidStr = Guid.NewGuid().ToString();
                    mongoAIs.Add(new MongoAI(guidStr, new PlayerAI(guidStr, mongoAI1.PlayerAI, mongoAI2.PlayerAI), Math.Max(mongoAI1.Evolution, mongoAI2.Evolution)));
                    guidStr = Guid.NewGuid().ToString();
                    mongoAIs.Add(new MongoAI(guidStr, new PlayerAI(guidStr, mongoAI1.PlayerAI), mongoAI1.Evolution));
                    guidStr = Guid.NewGuid().ToString();
                    mongoAIs.Add(new MongoAI(guidStr, new PlayerAI(guidStr, mongoAI1.PlayerAI, mongoAI2.PlayerAI, true), Math.Max(mongoAI1.Evolution, mongoAI2.Evolution)));

                    for (int i = 0; i < 2; i++)
                    {
                        guidStr = Guid.NewGuid().ToString();
                        mongoAIs.Add(new MongoAI(guidStr, new PlayerAI(guidStr)));
                    }
                    Console.Write($"{randomize + 1}: ");
                }

                for (int i = 0; i < GameCount; i++)
                {
                    if (totalTest)
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("*");
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                        ScreenUtils.ClearScreen();
                    }
                    Game promiseGame = new Game(isBotMatch, showCards, randomizedBots, useDb, collection, mongoAIs, totalTest);
                }
                if (totalTest) Console.WriteLine();
            }
        }
    }
}
