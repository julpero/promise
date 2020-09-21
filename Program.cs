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

            int GameCount = 1;
            int gameLoop = 1;
            bool knownPlayers = true;

            GameDebugSettings GameSettings = new GameDebugSettings{
                DebugPromise = false,
                IsBotMatch = false,
                ShowCards = true,
                IsTotalTest = false
            };

            MongoClient mongoClient = null;
            IMongoDatabase database = null;
            IMongoCollection<MongoAI> collection = null;
            List<MongoAI> mongoAIs = new List<MongoAI>();
            Random randomX = new Random();

            if (args.Any(x => x.ToLower() == "botmatch"))
            {
                GameCount = 5;
                GameSettings.IsBotMatch = true;
            }
            if (args.Any(x => x.ToLower() == "hidecards"))
            {
                GameSettings.ShowCards = false;
            }
            if (args.Any(x => x.ToLower() == "randombots"))
            {
                // do not use hard coded players
                knownPlayers = false;
            }
            if (args.Any(x => x.ToLower() == "debugpromise"))
            {
                // show bot cards so it is easier to debug how promise is made
                GameSettings.DebugPromise = true;
            }
            if (args.Any(x => x.ToLower() == "totaltest"))
            {
                GameSettings.IsTotalTest = true;
                GameCount = 20;
                gameLoop = 200000;
                ScreenUtils.ClearScreen();
            }
            else
            {
                Console.SetWindowSize(170, 42);
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
            
            string prevBestPlayerByPointsName = "";
            // string prevBestPlayerByPointsName2 = "";
            string prevBestPlayerByKeepsName = "";
            for (int loop = 0; loop < gameLoop; loop++)
            {
                mongoAIs.Clear();
                if (GameSettings.IsTotalTest && collection != null)
                {
                    string bestPlayerByPointsName = "";
                    // string bestPlayerByPointsName2 = "";
                    string bestPlayerByKeepsName = "";

                    Console.SetCursorPosition(0, loop);
                    Console.Write($"{loop+1}: ");
                    // get best ai players from mongo
                    var bestByPoints = collection.AsQueryable()
                                            .GroupBy(x => x.AiName)
                                            .Where(grp => grp.Count() >= 20)
                                            .Select(grp => new {AiName = grp.Key
                                                            , AvgPoints = grp.Average(y => y.Points)
                                                            , AvgKeeps = grp.Average(y => y.PromisesKept)
                                            })
                                            .OrderByDescending(z => z.AvgPoints)
                                            .ThenByDescending(z => z.AvgKeeps)
                                            .FirstOrDefault();
                    // var bestByPoints2 = collection.AsQueryable()
                    //                         .GroupBy(x => x.AiName)
                    //                         .Where(grp => grp.Count() >= 20)
                    //                         .Select(grp => new {AiName = grp.Key
                    //                                         , AvgPoints = grp.Average(y => y.Points)
                    //                                         , AvgKeeps = grp.Average(y => y.PromisesKept)
                    //                         })
                    //                         .OrderByDescending(z => z.AvgPoints)
                    //                         .ThenByDescending(z => z.AvgKeeps)
                    //                         .Skip(1)
                    //                         .FirstOrDefault();
                    var bestByKeeps = collection.AsQueryable()
                                            .GroupBy(x => x.AiName)
                                            .Where(grp => grp.Count() >= 20)
                                            .Select(grp => new {AiName = grp.Key
                                                            , AvgPoints = grp.Average(y => y.Points)
                                                            , AvgKeeps = grp.Average(y => y.PromisesKept)
                                            })
                                            .OrderByDescending(z => z.AvgKeeps)
                                            .ThenByDescending(z => z.AvgPoints)
                                            .FirstOrDefault();

                    MongoAI bestPlayerByPoints;
                    // MongoAI bestPlayerByPoints2;
                    MongoAI bestPlayerByKeeps;

                    bestPlayerByPoints = (bestByPoints != null)
                        ? collection.Find(x => x.AiName == bestByPoints.AiName).First() : null;
                    // bestPlayerByPoints2 = (bestByPoints2 != null)
                    //     ? collection.Find(x => x.AiName == bestByPoints2.AiName).First() : null;
                    bestPlayerByKeeps = (bestByKeeps != null)
                        ? collection.Find(x => x.AiName == bestByKeeps.AiName).First() : null;

                    if (bestPlayerByPoints != null)
                    {
                        bestPlayerByPointsName = bestPlayerByPoints.AiName;
                        if (bestPlayerByPointsName != prevBestPlayerByPointsName) Logger.Log($"points;loop:{loop};name:{bestPlayerByPointsName};date:{DateTime.Now}", "best");
                        prevBestPlayerByPointsName = bestPlayerByPointsName;
                        string guid = Guid.NewGuid().ToString();
                        mongoAIs.Add(new MongoAI(guid, bestPlayerByPoints.PlayerAI, new PlayerAI("random"), false, bestPlayerByPoints.Evolution));
                    }
                    // if (bestPlayerByPoints2 != null)
                    // {
                    //     bestPlayerByPointsName2 = bestPlayerByPoints2.AiName;
                    //     // if (bestPlayerByPointsName != prevBestPlayerByPointsName) Logger.Log($"points;loop:{loop};name:{bestPlayerByPointsName2};date:{DateTime.Now}", "best");
                    //     prevBestPlayerByPointsName = bestPlayerByPointsName;
                    //     string guid = Guid.NewGuid().ToString();
                    //     mongoAIs.Add(new MongoAI(guid, bestPlayerByPoints2.PlayerAI, new PlayerAI("random"), false, bestPlayerByPoints2.Evolution));
                    // }

                    if (bestPlayerByKeeps != null)
                    {
                        bestPlayerByKeepsName = bestPlayerByKeeps.AiName;
                        if (bestPlayerByKeepsName != prevBestPlayerByKeepsName) Logger.Log($"keeps;loop:{loop};name:{bestPlayerByKeepsName};date:{DateTime.Now}", "best");
                        prevBestPlayerByKeepsName = bestPlayerByKeepsName;
                        string guid = Guid.NewGuid().ToString();
                        mongoAIs.Add(new MongoAI(guid, bestPlayerByKeeps.PlayerAI, new PlayerAI("random"), false, bestPlayerByKeeps.Evolution));
                    }

                    if (bestPlayerByPoints != null && bestPlayerByKeeps != null)
                    {
                        string guid = Guid.NewGuid().ToString();
                        mongoAIs.Add(new MongoAI(guid, bestPlayerByKeeps.PlayerAI, bestPlayerByPoints.PlayerAI, false, Math.Max(bestPlayerByKeeps.Evolution, bestPlayerByPoints.Evolution)));
                    }

                    while (mongoAIs.Count < 5)
                    {
                        // add total random bot
                        string guid = Guid.NewGuid().ToString();
                        mongoAIs.Add(new MongoAI(guid, 0));
                    }
                }
                else if (!knownPlayers && collection != null)
                {
                    var bestByPoints = collection.AsQueryable()
                                            .GroupBy(x => x.AiName)
                                            .Where(grp => grp.Count() >= 20)
                                            .Select(grp => new {AiName = grp.Key
                                                            , AvgPoints = grp.Average(y => y.Points)
                                                            , AvgKeeps = grp.Average(y => y.PromisesKept)
                                            })
                                            .OrderByDescending(z => z.AvgPoints)
                                            .ThenByDescending(z => z.AvgKeeps)
                                            .FirstOrDefault();
                    var bestByKeeps = collection.AsQueryable()
                                            .GroupBy(x => x.AiName)
                                            .Where(grp => grp.Count() >= 20)
                                            .Select(grp => new {AiName = grp.Key
                                                            , AvgPoints = grp.Average(y => y.Points)
                                                            , AvgKeeps = grp.Average(y => y.PromisesKept)
                                            })
                                            .OrderByDescending(z => z.AvgKeeps)
                                            .ThenByDescending(z => z.AvgPoints)
                                            .FirstOrDefault();

                    MongoAI bestPlayerByPoints;
                    MongoAI bestPlayerByKeeps;

                    bestPlayerByPoints = (bestByPoints != null)
                        ? collection.Find(x => x.AiName == bestByPoints.AiName).First() : null;
                    bestPlayerByKeeps = (bestByKeeps != null)
                        ? collection.Find(x => x.AiName == bestByKeeps.AiName).First() : null;

                    if (bestPlayerByPoints != null)
                    {
                        string guid = Guid.NewGuid().ToString();
                        mongoAIs.Add(new MongoAI(bestPlayerByPoints.AiName, bestPlayerByPoints.PlayerAI, bestPlayerByPoints.Evolution));
                    }

                    if (bestPlayerByKeeps != null)
                    {
                        string guid = Guid.NewGuid().ToString();
                        mongoAIs.Add(new MongoAI(bestPlayerByKeeps.AiName, bestPlayerByKeeps.PlayerAI, bestPlayerByKeeps.Evolution));
                    }

                    if (bestPlayerByPoints != null && bestPlayerByKeeps != null)
                    {
                        string guid = Guid.NewGuid().ToString();
                        mongoAIs.Add(new MongoAI(guid, bestPlayerByKeeps.PlayerAI, bestPlayerByPoints.PlayerAI, true, Math.Max(bestPlayerByKeeps.Evolution, bestPlayerByPoints.Evolution)));
                    }

                    while (mongoAIs.Count < 5)
                    {
                        // add total random bot
                        string guid = Guid.NewGuid().ToString();
                        mongoAIs.Add(new MongoAI(guid, 0));
                    }

                }
                else
                {
                    mongoAIs.Add(new MongoAI("Fuison"));

                    for (int i = 0; i < Math.Max(CREATEDBOTS, 5); i++)
                    {
                        if (!knownPlayers)
                        {
                            string name = Guid.NewGuid().ToString();
                            mongoAIs.Add(new MongoAI(name));
                        }
                        else
                        {
                            mongoAIs.Add(new MongoAI(i));
                        }
                    }
                }
                mongoAIs = mongoAIs.OrderBy(x => randomX.Next()).ToList();

                for (int i = 0; i < GameCount; i++)
                {
                    if (!GameSettings.IsTotalTest)
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                        ScreenUtils.ClearScreen();
                    }
                    Game promiseGame = new Game(GameSettings, mongoAIs, collection);
                    if (GameSettings.IsTotalTest) Console.Write("*");
                }
                if (GameSettings.IsTotalTest) Console.WriteLine();
            }
        }
    }
}
