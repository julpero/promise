﻿using System;
using System.IO;
using System.Linq; 
using System.Collections.Generic;
using MongoDB.Driver;

namespace promise
{

    class Program
    {
        static void Main(string[] args)
        {
            const int CREATEDBOTS = 2;
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            int GameCount = 1;
            bool isBotMatch = false;
            bool showCards = true;
            bool randomizedBots = false;
            bool totalTest = false;
            int RandomizeLimit = 1;

            MongoClient mongoClient = null;
            IMongoDatabase database = null;
            IMongoCollection<MongoAI> collection = null;
            List<MongoAI> mongoAIs = new List<MongoAI>();

            if (args.Any(x => x.ToLower() == "botmatch"))
            {
                GameCount = 20;
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

                    // get ai's from mongo

                    // or use now some random
                    for (int i = 0; i < 5; i++)
                    {
                        string guidStr = Guid.NewGuid().ToString();
                        mongoAIs.Add(new MongoAI(guidStr, new PlayerAI(guidStr)));
                    }
                }
                catch
                {
                    throw;
                }
            }

            if (totalTest)
            {
                GameCount = 20;
                RandomizeLimit = 200000;
                ScreenUtils.ClearScreen();
                Console.SetCursorPosition(0, 0);
            }
            else
            {
                Console.SetWindowSize(170, 42);
            }

            var edellinenParas = "";
            for (int randomize = 0; randomize < RandomizeLimit; randomize++)
            {
                if (randomizedBots)
                {
                    var goodOnes = collection.AsQueryable()
                                            .GroupBy(x => x.AiName)
                                            .Where(grp => grp.Count() == 20)
                                            .Select(grp => new {AiName = grp.Key
                                                            , AvgPoints = grp.Average(y => y.Points)
                                                            , AvgKeeps = grp.Average(y => y.PromisesKept)
                                            })
                                            .OrderByDescending(z => z.AvgPoints)
                                            .ThenByDescending(z => z.AvgKeeps)
                                            .Take(2).ToList();
                    
                    // var goodOnes = collection.AsQueryable()
                    //                         .GroupBy(x => new {AiName = x.AiName})
                    //                         .Where(x => x.Key.AiName.Count() == 20)
                    //                         .Select(x => new {AiName = x.Key.AiName
                    //                                         , AvgPoints = x.Average(y => y.Points)
                    //                                         , AvgKeeps = x.Average(y => y.PromisesKept)
                    //                                         }).OrderByDescending(z => z.AvgPoints).Take(2).ToList();
                    
                    MongoAI mongoAI1 = collection.Find(x => x.AiName == goodOnes.First().AiName).First();
                    MongoAI mongoAI2 = collection.Find(x => x.AiName == goodOnes.Last().AiName).First();

                    var paras = mongoAI1.AiName;
                    if (paras != edellinenParas) Logger.Log($"paras: {paras} {DateTime.Now}", "parhaat");

                    edellinenParas = paras;
                    
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
                else
                {
                    for (int i = 0; i < 2; i++)
                    {
                        var guidStr = Guid.NewGuid().ToString();
                        mongoAIs.Add(new MongoAI(guidStr, new PlayerAI()));
                    }
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
