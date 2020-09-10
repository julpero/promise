using System;
using System.Linq;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Bson;

namespace promise
{
    public class PlayerAI
    {
        public string AiName {get; set;}
        // AnalyzeDodgeable
        public int DodgeBase {get; set;}
        public int DodgeSure {get; set;}
        public int DodgeSmallestValuesInSuit {get; set;}
        public int DodgeSmallestValuesInSuitNOT {get; set;}
        public int DodgeCardCountAvgOtherPlayersCount1 {get; set;}
        public int DodgeBiggestValuesInSuit {get; set;}
        public int DodgeBiggestValuesInSuitNOT {get; set;}
        public int DodgeCardCountAvgOtherPlayersCount2 {get; set;}
        public double DodgeInChargeAverageCount {get; set;}

        // BigValuesInSuit, 2-14
        public int BigValuesInSuit {get; set;}

        // SmallValuesInSuit, 2-14
        public int SmallValuesInSuit {get; set;}

        // MakePromise
        public double PromiseMultiplierBase1 {get; set;}
        public double PromiseMultiplierBase2 {get; set;}
        public double PromiseMultiplierBase3 {get; set;}
        public double PromiseMultiplierBase4 {get; set;}
        public double PromiseMultiplierChange1A {get; set;}
        public double PromiseMultiplierChange1B {get; set;}
        public double PromiseMultiplierChange1C {get; set;}
        public double PromiseMultiplierChange2A {get; set;}
        public double PromiseMultiplierChange2B {get; set;}
        public double PromiseMultiplierChange2C {get; set;}
        public double PromiseMultiplierChange3A {get; set;}
        public double PromiseMultiplierChange3B {get; set;}
        public double PromiseMultiplierChange3C {get; set;}
        public double PromiseMultiplierChange4A {get; set;}
        public double PromiseMultiplierChange4B {get; set;}
        public double PromiseMultiplierChange4C {get; set;}
        public int MiniRisk {get; set;}

        private static Random randomAi = new Random();

        public PlayerAI()
        {
            AiName = "ee865a0c-a62d-49ec-98b8-1dc7a953c7f6";
            // AnalyzeDodgeable
            DodgeBase = 50;
            DodgeSure = 100;
            DodgeSmallestValuesInSuit = 95;
            DodgeSmallestValuesInSuitNOT = 85;
            DodgeCardCountAvgOtherPlayersCount1 = 3;
            DodgeBiggestValuesInSuit = 15;
            DodgeBiggestValuesInSuitNOT = 25;
            DodgeCardCountAvgOtherPlayersCount1 = 7;
            DodgeInChargeAverageCount = 0.8;
            
            // BigValuesInSuit
            BigValuesInSuit = 10;

            // SmallValuesInSuit
            SmallValuesInSuit = 6;

            // MakePromise
            PromiseMultiplierBase1 = 0.6;
            PromiseMultiplierBase2 = 0.2;
            PromiseMultiplierBase3 = 0.3;
            PromiseMultiplierBase4 = 0.25;
            PromiseMultiplierChange1A = 0.3;
            PromiseMultiplierChange1B = 0.15;
            PromiseMultiplierChange1C = 0.1;
            PromiseMultiplierChange2A = 0.1;
            PromiseMultiplierChange2B = 0.05;
            PromiseMultiplierChange2C = 0.1;
            PromiseMultiplierChange3A = 0.4;
            PromiseMultiplierChange3B = 0.25;
            PromiseMultiplierChange3C = 0.1;
            PromiseMultiplierChange4A = 0.1;
            PromiseMultiplierChange4B = 0.05;
            PromiseMultiplierChange4C = 0.1;
            MiniRisk = 5;
        }

        public static bool KeepAiValue()
        {
            if (randomAi.Next(0, 2) != 0)
            {
                return true;
            }
            return false;
        }

        public static double GetRandomNumber(double minimum, double maximum)
        {
            return randomAi.NextDouble() * (maximum - minimum) + minimum;
        }
        
        public PlayerAI(string guidStr)
        {
            AiName = guidStr;

            DodgeBase = randomAi.Next(100) + 1; // 50
            DodgeSure = 100; // this is a fact
            DodgeSmallestValuesInSuit = randomAi.Next(100); // 95
            DodgeSmallestValuesInSuitNOT = randomAi.Next(100); // 85
            DodgeCardCountAvgOtherPlayersCount1 = randomAi.Next(100); // 3
            DodgeBiggestValuesInSuit = randomAi.Next(100); // 15;
            DodgeBiggestValuesInSuitNOT = randomAi.Next(100); // 25;
            DodgeCardCountAvgOtherPlayersCount1 = randomAi.Next(100); // 7;
            DodgeInChargeAverageCount = randomAi.NextDouble(); // 0.8;
            
            // BigValuesInSuit
            BigValuesInSuit = randomAi.Next(1, 14) + 1; // 10;

            // SmallValuesInSuit
            SmallValuesInSuit = randomAi.Next(1, 14) + 1; // 6;

            // MakePromise
            PromiseMultiplierBase1 = randomAi.NextDouble(); // 0.6;
            PromiseMultiplierBase2 = randomAi.NextDouble(); // 0.2;
            PromiseMultiplierBase3 = randomAi.NextDouble(); // 0.3;
            PromiseMultiplierBase4 = randomAi.NextDouble(); // 0.25;
            PromiseMultiplierChange1A = randomAi.NextDouble(); // 0.3;
            PromiseMultiplierChange1B = randomAi.NextDouble(); // 0.15;
            PromiseMultiplierChange1C = randomAi.NextDouble(); // 0.1;
            PromiseMultiplierChange2A = randomAi.NextDouble(); // 0.1;
            PromiseMultiplierChange2B = randomAi.NextDouble(); // 0.05;
            PromiseMultiplierChange2C = randomAi.NextDouble(); // 0.1;
            PromiseMultiplierChange3A = randomAi.NextDouble(); // 0.4;
            PromiseMultiplierChange3B = randomAi.NextDouble(); // 0.25;
            PromiseMultiplierChange3C = randomAi.NextDouble(); // 0.1;
            PromiseMultiplierChange4A = randomAi.NextDouble(); // 0.1;
            PromiseMultiplierChange4B = randomAi.NextDouble(); // 0.05;
            PromiseMultiplierChange4C = randomAi.NextDouble(); // 0.1;
            MiniRisk = randomAi.Next(100); // 5;
        }

        public PlayerAI(string guidStr, PlayerAI goodAi)
        {
            AiName = Guid.NewGuid().ToString();

            DodgeBase = KeepAiValue() ? goodAi.DodgeBase : randomAi.Next(100) + 1; // 50
            DodgeSure = 100; // this is a fact
            DodgeSmallestValuesInSuit = KeepAiValue() ? goodAi.DodgeSmallestValuesInSuit : randomAi.Next(100); // 95
            DodgeSmallestValuesInSuitNOT = KeepAiValue() ? goodAi.DodgeSmallestValuesInSuitNOT : randomAi.Next(100); // 85
            DodgeCardCountAvgOtherPlayersCount1 = KeepAiValue() ? goodAi.DodgeCardCountAvgOtherPlayersCount1 : randomAi.Next(100); // 3
            DodgeBiggestValuesInSuit = KeepAiValue() ? goodAi.DodgeBiggestValuesInSuit : randomAi.Next(100); // 15;
            DodgeBiggestValuesInSuitNOT = KeepAiValue() ? goodAi.DodgeBiggestValuesInSuitNOT : randomAi.Next(100); // 25;
            DodgeCardCountAvgOtherPlayersCount1 = KeepAiValue() ? goodAi.DodgeCardCountAvgOtherPlayersCount1 : randomAi.Next(100); // 7;
            DodgeInChargeAverageCount = KeepAiValue() ? goodAi.DodgeInChargeAverageCount : randomAi.NextDouble(); // 0.8;
            
            // BigValuesInSuit
            BigValuesInSuit = KeepAiValue() ? goodAi.BigValuesInSuit : randomAi.Next(1, 14) + 1; // 10;

            // SmallValuesInSuit
            SmallValuesInSuit = KeepAiValue() ? goodAi.SmallValuesInSuit : randomAi.Next(1, 14) + 1; // 6;

            // MakePromise
            PromiseMultiplierBase1 = KeepAiValue() ? goodAi.PromiseMultiplierBase1 : randomAi.NextDouble(); // 0.6;
            PromiseMultiplierBase2 = KeepAiValue() ? goodAi.PromiseMultiplierBase2 : randomAi.NextDouble(); // 0.2;
            PromiseMultiplierBase3 = KeepAiValue() ? goodAi.PromiseMultiplierBase3 : randomAi.NextDouble(); // 0.3;
            PromiseMultiplierBase4 = KeepAiValue() ? goodAi.PromiseMultiplierBase4 : randomAi.NextDouble(); // 0.25;
            PromiseMultiplierChange1A = KeepAiValue() ? goodAi.PromiseMultiplierChange1A : randomAi.NextDouble(); // 0.3;
            PromiseMultiplierChange1B = KeepAiValue() ? goodAi.PromiseMultiplierChange1B : randomAi.NextDouble(); // 0.15;
            PromiseMultiplierChange1C = KeepAiValue() ? goodAi.PromiseMultiplierChange1C : randomAi.NextDouble(); // 0.1;
            PromiseMultiplierChange2A = KeepAiValue() ? goodAi.PromiseMultiplierChange2A : randomAi.NextDouble(); // 0.1;
            PromiseMultiplierChange2B = KeepAiValue() ? goodAi.PromiseMultiplierChange2B : randomAi.NextDouble(); // 0.05;
            PromiseMultiplierChange2C = KeepAiValue() ? goodAi.PromiseMultiplierChange2C : randomAi.NextDouble(); // 0.1;
            PromiseMultiplierChange3A = KeepAiValue() ? goodAi.PromiseMultiplierChange3A : randomAi.NextDouble(); // 0.4;
            PromiseMultiplierChange3B = KeepAiValue() ? goodAi.PromiseMultiplierChange3B : randomAi.NextDouble(); // 0.25;
            PromiseMultiplierChange3C = KeepAiValue() ? goodAi.PromiseMultiplierChange3C : randomAi.NextDouble(); // 0.1;
            PromiseMultiplierChange4A = KeepAiValue() ? goodAi.PromiseMultiplierChange4A : randomAi.NextDouble(); // 0.1;
            PromiseMultiplierChange4B = KeepAiValue() ? goodAi.PromiseMultiplierChange4B : randomAi.NextDouble(); // 0.05;
            PromiseMultiplierChange4C = KeepAiValue() ? goodAi.PromiseMultiplierChange4C : randomAi.NextDouble(); // 0.1;
            MiniRisk = KeepAiValue() ? goodAi.MiniRisk : randomAi.Next(100); // 5;
        }

        public static int AverageInt(int i, int j)
        {
            return (i + j) / 2;
        }
        public static double AverageDouble(double i, double j)
        {
            return (i + j) / 2;
        }

        public PlayerAI(string guidStr, PlayerAI bestAi, PlayerAI goodAi, bool average = false)
        {
            AiName = Guid.NewGuid().ToString();

            if (average)
            {
                DodgeBase = AverageInt(bestAi.DodgeBase, goodAi.DodgeBase); // 50
                DodgeSure = 100; // this is a fact
                DodgeSmallestValuesInSuit = AverageInt(bestAi.DodgeBase, goodAi.DodgeBase); // 95
                DodgeSmallestValuesInSuitNOT = AverageInt(bestAi.DodgeBase, goodAi.DodgeBase); // 85
                DodgeCardCountAvgOtherPlayersCount1 = AverageInt(bestAi.DodgeBase, goodAi.DodgeBase); // 3
                DodgeBiggestValuesInSuit = AverageInt(bestAi.DodgeBase, goodAi.DodgeBase); // 15;
                DodgeBiggestValuesInSuitNOT = AverageInt(bestAi.DodgeBase, goodAi.DodgeBase); // 25;
                DodgeCardCountAvgOtherPlayersCount1 = AverageInt(bestAi.DodgeBase, goodAi.DodgeBase); // 7;
                DodgeInChargeAverageCount = AverageInt(bestAi.DodgeBase, goodAi.DodgeBase); // 0.8;
                
                // BigValuesInSuit
                BigValuesInSuit = AverageInt(bestAi.DodgeBase, goodAi.DodgeBase); // 10;

                // SmallValuesInSuit
                SmallValuesInSuit = AverageInt(bestAi.DodgeBase, goodAi.DodgeBase); // 6;

                // MakePromise
                PromiseMultiplierBase1 = AverageDouble(bestAi.DodgeBase, goodAi.DodgeBase); // 0.6;
                PromiseMultiplierBase2 = AverageDouble(bestAi.DodgeBase, goodAi.DodgeBase); // 0.2;
                PromiseMultiplierBase3 = AverageDouble(bestAi.DodgeBase, goodAi.DodgeBase); // 0.3;
                PromiseMultiplierBase4 = AverageDouble(bestAi.DodgeBase, goodAi.DodgeBase); // 0.25;
                PromiseMultiplierChange1A = AverageDouble(bestAi.DodgeBase, goodAi.DodgeBase); // 0.3;
                PromiseMultiplierChange1B = AverageDouble(bestAi.DodgeBase, goodAi.DodgeBase); // 0.15;
                PromiseMultiplierChange1C = AverageDouble(bestAi.DodgeBase, goodAi.DodgeBase); // 0.1;
                PromiseMultiplierChange2A = AverageDouble(bestAi.DodgeBase, goodAi.DodgeBase); // 0.1;
                PromiseMultiplierChange2B = AverageDouble(bestAi.DodgeBase, goodAi.DodgeBase); // 0.05;
                PromiseMultiplierChange2C = AverageDouble(bestAi.DodgeBase, goodAi.DodgeBase); // 0.1;
                PromiseMultiplierChange3A = AverageDouble(bestAi.DodgeBase, goodAi.DodgeBase); // 0.4;
                PromiseMultiplierChange3B = AverageDouble(bestAi.DodgeBase, goodAi.DodgeBase); // 0.25;
                PromiseMultiplierChange3C = AverageDouble(bestAi.DodgeBase, goodAi.DodgeBase); // 0.1;
                PromiseMultiplierChange4A = AverageDouble(bestAi.DodgeBase, goodAi.DodgeBase); // 0.1;
                PromiseMultiplierChange4B = AverageDouble(bestAi.DodgeBase, goodAi.DodgeBase); // 0.05;
                PromiseMultiplierChange4C = AverageDouble(bestAi.DodgeBase, goodAi.DodgeBase); // 0.1;
                MiniRisk = AverageInt(bestAi.DodgeBase, goodAi.DodgeBase); // 5;
            }
            else
            {
                DodgeBase = KeepAiValue() ? bestAi.DodgeBase : goodAi.DodgeBase; // 50
                DodgeSure = 100; // this is a fact
                DodgeSmallestValuesInSuit = KeepAiValue() ? bestAi.DodgeBase : goodAi.DodgeBase; // 95
                DodgeSmallestValuesInSuitNOT = KeepAiValue() ? bestAi.DodgeBase : goodAi.DodgeBase; // 85
                DodgeCardCountAvgOtherPlayersCount1 = KeepAiValue() ? bestAi.DodgeBase : goodAi.DodgeBase; // 3
                DodgeBiggestValuesInSuit = KeepAiValue() ? bestAi.DodgeBase : goodAi.DodgeBase; // 15;
                DodgeBiggestValuesInSuitNOT = KeepAiValue() ? bestAi.DodgeBase : goodAi.DodgeBase; // 25;
                DodgeCardCountAvgOtherPlayersCount1 = KeepAiValue() ? bestAi.DodgeBase : goodAi.DodgeBase; // 7;
                DodgeInChargeAverageCount = KeepAiValue() ? bestAi.DodgeBase : goodAi.DodgeBase; // 0.8;
                
                // BigValuesInSuit
                BigValuesInSuit = KeepAiValue() ? bestAi.DodgeBase : goodAi.DodgeBase; // 10;

                // SmallValuesInSuit
                SmallValuesInSuit = KeepAiValue() ? bestAi.DodgeBase : goodAi.DodgeBase; // 6;

                // MakePromise
                PromiseMultiplierBase1 = KeepAiValue() ? bestAi.DodgeBase : goodAi.DodgeBase; // 0.6;
                PromiseMultiplierBase2 = KeepAiValue() ? bestAi.DodgeBase : goodAi.DodgeBase; // 0.2;
                PromiseMultiplierBase3 = KeepAiValue() ? bestAi.DodgeBase : goodAi.DodgeBase; // 0.3;
                PromiseMultiplierBase4 = KeepAiValue() ? bestAi.DodgeBase : goodAi.DodgeBase; // 0.25;
                PromiseMultiplierChange1A = KeepAiValue() ? bestAi.DodgeBase : goodAi.DodgeBase; // 0.3;
                PromiseMultiplierChange1B = KeepAiValue() ? bestAi.DodgeBase : goodAi.DodgeBase; // 0.15;
                PromiseMultiplierChange1C = KeepAiValue() ? bestAi.DodgeBase : goodAi.DodgeBase; // 0.1;
                PromiseMultiplierChange2A = KeepAiValue() ? bestAi.DodgeBase : goodAi.DodgeBase; // 0.1;
                PromiseMultiplierChange2B = KeepAiValue() ? bestAi.DodgeBase : goodAi.DodgeBase; // 0.05;
                PromiseMultiplierChange2C = KeepAiValue() ? bestAi.DodgeBase : goodAi.DodgeBase; // 0.1;
                PromiseMultiplierChange3A = KeepAiValue() ? bestAi.DodgeBase : goodAi.DodgeBase; // 0.4;
                PromiseMultiplierChange3B = KeepAiValue() ? bestAi.DodgeBase : goodAi.DodgeBase; // 0.25;
                PromiseMultiplierChange3C = KeepAiValue() ? bestAi.DodgeBase : goodAi.DodgeBase; // 0.1;
                PromiseMultiplierChange4A = KeepAiValue() ? bestAi.DodgeBase : goodAi.DodgeBase; // 0.1;
                PromiseMultiplierChange4B = KeepAiValue() ? bestAi.DodgeBase : goodAi.DodgeBase; // 0.05;
                PromiseMultiplierChange4C = KeepAiValue() ? bestAi.DodgeBase : goodAi.DodgeBase; // 0.1;
                MiniRisk = KeepAiValue() ? bestAi.DodgeBase : goodAi.DodgeBase; // 5;
            }



            // mutation
            if (randomAi.NextDouble() > 0.98) DodgeBase = randomAi.Next(100) + 1; // 50
            DodgeSure = 100; // this is a fact
            if (randomAi.NextDouble() > 0.98) DodgeSmallestValuesInSuit = randomAi.Next(100); // 95
            if (randomAi.NextDouble() > 0.98) DodgeSmallestValuesInSuitNOT = randomAi.Next(100); // 85
            if (randomAi.NextDouble() > 0.98) DodgeCardCountAvgOtherPlayersCount1 = randomAi.Next(100); // 3
            if (randomAi.NextDouble() > 0.98) DodgeBiggestValuesInSuit = randomAi.Next(100); // 15;
            if (randomAi.NextDouble() > 0.98) DodgeBiggestValuesInSuitNOT = randomAi.Next(100); // 25;
            if (randomAi.NextDouble() > 0.98) DodgeCardCountAvgOtherPlayersCount1 = randomAi.Next(100); // 7;
            if (randomAi.NextDouble() > 0.98) DodgeInChargeAverageCount = randomAi.NextDouble(); // 0.8;
            
            // BigValuesInSuit
            if (randomAi.NextDouble() > 0.98) BigValuesInSuit = randomAi.Next(1, 14) + 1; // 10;

            // SmallValuesInSuit
            if (randomAi.NextDouble() > 0.98) SmallValuesInSuit = randomAi.Next(1, 14) + 1; // 6;

            // MakePromise
            if (randomAi.NextDouble() > 0.98) PromiseMultiplierBase1 = randomAi.NextDouble(); // 0.6;
            if (randomAi.NextDouble() > 0.98) PromiseMultiplierBase2 = randomAi.NextDouble(); // 0.2;
            if (randomAi.NextDouble() > 0.98) PromiseMultiplierBase3 = randomAi.NextDouble(); // 0.3;
            if (randomAi.NextDouble() > 0.98) PromiseMultiplierBase4 = randomAi.NextDouble(); // 0.25;
            if (randomAi.NextDouble() > 0.98) PromiseMultiplierChange1A = randomAi.NextDouble(); // 0.3;
            if (randomAi.NextDouble() > 0.98) PromiseMultiplierChange1B = randomAi.NextDouble(); // 0.15;
            if (randomAi.NextDouble() > 0.98) PromiseMultiplierChange1C = randomAi.NextDouble(); // 0.1;
            if (randomAi.NextDouble() > 0.98) PromiseMultiplierChange2A = randomAi.NextDouble(); // 0.1;
            if (randomAi.NextDouble() > 0.98) PromiseMultiplierChange2B = randomAi.NextDouble(); // 0.05;
            if (randomAi.NextDouble() > 0.98) PromiseMultiplierChange2C = randomAi.NextDouble(); // 0.1;
            if (randomAi.NextDouble() > 0.98) PromiseMultiplierChange3A = randomAi.NextDouble(); // 0.4;
            if (randomAi.NextDouble() > 0.98) PromiseMultiplierChange3B = randomAi.NextDouble(); // 0.25;
            if (randomAi.NextDouble() > 0.98) PromiseMultiplierChange3C = randomAi.NextDouble(); // 0.1;
            if (randomAi.NextDouble() > 0.98) PromiseMultiplierChange4A = randomAi.NextDouble(); // 0.1;
            if (randomAi.NextDouble() > 0.98) PromiseMultiplierChange4B = randomAi.NextDouble(); // 0.05;
            if (randomAi.NextDouble() > 0.98) PromiseMultiplierChange4C = randomAi.NextDouble(); // 0.1;
            if (randomAi.NextDouble() > 0.98) MiniRisk = randomAi.Next(100); // 5;
        }
    }

    class Game
    {
        const bool DEBUGMODE = false;
        const int MAXPLAYERS = 5;
        const int SCOREBOARDSTART = 130;
        const int PROMISEBOARDX = 10;
        const int PROMISEBOARDY = 33;

        public Player[] Players {get; set;}
        public List<PlayerAI> PlayerAIs {get; set;}

        public int StartRound {get; set;}
        public int TurnRound {get; set;}
        public int EndRound {get; set;}

        public Round[] Rounds {get; set;}
        
        public int[] TotalPoints {get; set;}
        public int[] PromisesKept {get; set;}

        public bool IsBotMatch {get; set;}
        public bool IsDbInUse {get; set;}
        public bool ShowCards {get; set;}
        public bool TotalTest {get; set;}

        public Game(bool isBotMatch = false
                    , bool showCards = true
                    , bool randomizedBots = false
                    , bool isDbInUse = false
                    , IMongoCollection<MongoAI> collection = null
                    , List<MongoAI> mongoAIs = null
                    , bool totalTest = false
                    )
        {
            this.IsBotMatch = isBotMatch;
            this.IsDbInUse = isDbInUse;
            this.ShowCards = showCards;
            this.TotalTest = totalTest;

            this.PlayerAIs = new List<PlayerAI>();

            if (mongoAIs != null && mongoAIs.Count() == 5)
            {
                this.PlayerAIs.Add(mongoAIs.First().PlayerAI);
                this.PlayerAIs.Add(mongoAIs.Skip(1).First().PlayerAI);
                this.PlayerAIs.Add(mongoAIs.Skip(2).First().PlayerAI);
                this.PlayerAIs.Add(mongoAIs.Skip(3).First().PlayerAI);
                this.PlayerAIs.Add(mongoAIs.Skip(4).First().PlayerAI);
            }
            else if (randomizedBots)
            {
                this.PlayerAIs.Add(new PlayerAI(""));
                this.PlayerAIs.Add(new PlayerAI(""));
                this.PlayerAIs.Add(new PlayerAI(""));
                this.PlayerAIs.Add(new PlayerAI(""));
                this.PlayerAIs.Add(new PlayerAI(""));
            }
            else
            {
                this.PlayerAIs.Add(new PlayerAI());
                this.PlayerAIs.Add(new PlayerAI());
                this.PlayerAIs.Add(new PlayerAI());
                this.PlayerAIs.Add(new PlayerAI());
                this.PlayerAIs.Add(new PlayerAI());
            }

            GetPlayers(this.PlayerAIs);
            this.Players = ShufflePlayers();

            this.TotalPoints = new int[this.Players.Count()];
            this.PromisesKept = new int[this.Players.Count()];

            GetGameRules();
            if (!totalTest) ScreenUtils.ClearScreen();
            InitRounds();
            PlayPromise();

            if (isDbInUse)
            {
                for (int i = 0; i < this.Players.Count(); i++)
                {
                    MongoAI newPlayerAi = new MongoAI(this.Players[i].AI.AiName, this.Players[i].AI, this.TotalPoints[i], this.PromisesKept[i]);
                    collection.InsertOne(newPlayerAi);
                }
            }
        }

        private int PlayerPositionHelper(int i)
        {
            if (i < 0) return PlayerPositionHelper(i + this.Players.Count());
            if (i < this.Players.Count()) return i;
            return PlayerPositionHelper(i - this.Players.Count());
        }

        private int CountPoint(int round, Promise promise)
        {
            if (!promise.PromiseKept) return 0;
            if (promise.PromiseNumber > 0) return 10 + promise.PromiseNumber;
            return (round > 5) ? 15 : 5;
        }

        private void CalculateAndPrintScoreBoard()
        {
            int[] promiseSums = new int[this.Players.Count()];

            if (!this.TotalTest) Console.SetCursorPosition(SCOREBOARDSTART, 1);
            if (!this.TotalTest) Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < this.Players.Count(); i++)
            {
                if (!this.TotalTest) Console.Write("|");
                if (!this.TotalTest) Console.Write(this.Players[i].PlayerInitials);
                this.TotalPoints[i] = 0;
            }

            for (int i = 0; i < this.Rounds.Count(); i++)
            {
                if (!this.TotalTest) Console.SetCursorPosition(SCOREBOARDSTART - 3, 2 + i);
                string roundStr = $"{this.Rounds[i].CardsInRound}";
                if (!this.TotalTest) Console.Write(roundStr.PadLeft(2, ' '));
                if (!this.TotalTest) Console.Write(" |");
                
                if (!this.Rounds[i].RoundPlayed) continue;

                for (int j = 0; j < this.Players.Count(); j++)
                {
                    int promiseSum = CountPoint(this.Rounds[i].CardsInRound, this.Rounds[i].Promises[PlayerPositionHelper(j - i)]);
                    promiseSums[j]+= promiseSum;
                    this.TotalPoints[j]+= promiseSum;
                    string sumStr = (promiseSum > 0) ? $"{promiseSums[j]}" : "";
                    if (!this.TotalTest) Console.Write(sumStr.PadLeft(3, ' '));
                    if (!this.TotalTest) Console.Write(" ");
                }
            }
        }

        private void CalculateAndPrintPromiseBoard(int inGame = -1)
        {
            if (!this.TotalTest) Console.SetCursorPosition(PROMISEBOARDX + 15, PROMISEBOARDY);
            if (!this.TotalTest) Console.ForegroundColor = ConsoleColor.White;
            if (!this.TotalTest) Console.Write("|");
            for (int i = 0; i < this.Rounds.Length; i++)
            {
                string promiseBoardStr = this.Rounds[i].CardsInRound.ToString();
                if (!this.TotalTest) Console.Write(promiseBoardStr.PadLeft(2, ' '));
                if (!this.TotalTest) Console.Write("|");
            }

            for (int i = 0; i < this.Players.Count(); i++)
            {
                this.PromisesKept[i] = 0;
                if (!this.TotalTest) Console.SetCursorPosition(PROMISEBOARDX, PROMISEBOARDY + 1 + i);
                if (!this.TotalTest) Console.ForegroundColor = ConsoleColor.White;
                var nameStr = this.Players[i].PlayerName;
                if (nameStr.Length > 14) nameStr = nameStr.Substring(0, 14);
                if (!this.TotalTest) Console.Write(nameStr.PadLeft(14, ' '));
                if (!this.TotalTest) Console.Write(" |");
                for (int j = 0; j < this.Rounds.Count(); j++)
                {
                    if (!this.Rounds[j].RoundPlayed)
                    {
                        if (inGame != j) continue;
                    }

                    if (inGame == j)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    else if (this.Rounds[j].Promises[PlayerPositionHelper(i - j)].PromiseKept)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        this.PromisesKept[i]++;
                    }
                    else
                    {
                        Console.ForegroundColor = (this.Rounds[j].Promises[PlayerPositionHelper(i - j)].PromiseKeptType == PromiseKeptTypeEnum.UNDER)
                                                ? ConsoleColor.DarkGray
                                                : ConsoleColor.DarkRed;
                    }

                    string thisPromiseStr = this.Rounds[j].Promises[PlayerPositionHelper(i - j)].PromiseNumber.ToString();
                    if (!this.TotalTest) Console.Write(thisPromiseStr.PadLeft(2, ' '));
                    if (!this.TotalTest) Console.Write(" ");
                }
            }

            Console.ForegroundColor = ConsoleColor.White;
            if (!this.TotalTest) Console.SetCursorPosition(PROMISEBOARDX, PROMISEBOARDY + 1 + this.Players.Count());
            if (!this.TotalTest) Console.Write("".PadRight(16 + (this.Rounds.Count() * 3), '-'));
            if (!this.TotalTest) Console.SetCursorPosition(PROMISEBOARDX, PROMISEBOARDY + 1 + this.Players.Count()+1);
            if (!this.TotalTest) Console.Write("TOTAL".PadLeft(14, ' '));
            if (!this.TotalTest) Console.Write(" |");
            for (int j = 0; j < this.Rounds.Count(); j++)
            {
                if (!this.Rounds[j].RoundPlayed)
                {
                    if (inGame != j) continue;
                }

                int evenRound = this.Rounds[j].CardsInRound;
                int roundPromises = 0;
                for (int i = 0; i < this.Players.Count(); i++)
                {
                    roundPromises+= this.Rounds[j].Promises[i].PromiseNumber;
                }

                if (inGame == j)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else if (roundPromises == evenRound)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.ForegroundColor = (evenRound > roundPromises)
                                            ? ConsoleColor.DarkGray
                                            : ConsoleColor.DarkRed;
                }

                string thisPromiseStr = roundPromises.ToString();
                if (!this.TotalTest) Console.Write(thisPromiseStr.PadLeft(2, ' '));
                if (!this.TotalTest) Console.Write(" ");
            }
        }

        private void PlayRound(int roundNbr)
        {
            CalculateAndPrintScoreBoard();
            CalculateAndPrintPromiseBoard();
            var roundToPlay = this.Rounds[roundNbr];
            roundToPlay.MakePromises();
            CalculateAndPrintPromiseBoard(roundNbr);
            roundToPlay.PlayRound();
            CalculateAndPrintScoreBoard();
            CalculateAndPrintPromiseBoard();

            if (!this.IsBotMatch) Console.ReadKey();
        }

        private void PlayPromise()
        {
            for (int i = 0; i < this.Rounds.Count(); i++)
            {
                if (!this.TotalTest) ScreenUtils.ClearScreen();
                PlayRound(i);
            }

            if (!this.TotalTest) Console.ReadKey();
        }

        private int RoundsInThisGame()
        {
            return (this.StartRound - this.TurnRound + 1) + (this.EndRound - this.TurnRound);
        }

        private void InitRounds()
        {
            this.Rounds = new Round[RoundsInThisGame()];
            int round = 0;
            for (int i = this.StartRound; i >= this.TurnRound; i--)
            {
                this.Rounds[round] = new Round(i, round, this.Players, this.IsBotMatch || this.IsDbInUse, this.ShowCards, this.TotalTest);
                round++;
            }
            for (int i = this.TurnRound+1; i <= this.EndRound; i++)
            {
                this.Rounds[round] = new Round(i, round, this.Players, this.IsBotMatch || this.IsDbInUse, this.ShowCards, this.TotalTest);
                round++;
            }
        }

        private void GetPlayers(List<PlayerAI> playerAIs)
        {
            if (!this.TotalTest) ScreenUtils.ClearScreen();
            
            int lkm = 0;

            if (this.IsBotMatch || this.TotalTest)
            {
                lkm = 5;
            }
            else
            {
                Console.Write($"Pelaajien lukumäärä (2-{MAXPLAYERS}): ");
                ConsoleKeyInfo input = Console.ReadKey();
                while (!Int32.TryParse(input.KeyChar.ToString(), out lkm) || lkm > MAXPLAYERS || lkm < 2)
                {
                    ScreenUtils.ClearScreen();
                    Console.Write($"Pelaajien lukumäärä (2-{MAXPLAYERS}): ");
                    input = Console.ReadKey();
                }
                Console.WriteLine();
            }

            this.Players = new Player[lkm];
            for (int i = 0; i < this.Players.Count(); i++)
            {
                this.Players[i] = new Player(i+1, playerAIs.Skip(i).First(), this.IsBotMatch);
            }

            if (this.Players.Any(x => x.PlayerType == PlayerType.HUMAN)) this.IsBotMatch = false;

        }

        private int MaximumRounds()
        {
            if (this.Players.Count() <= 5) return 10;
            if (this.Players.Count() == 6) return 8;
            return 6;
        }

        private void GetGameRules()
        {
            if (!this.TotalTest) ScreenUtils.ClearScreen();
            
            string input = "";
            int lkm = 0;

            if (!this.TotalTest) Console.Write($"Mistä jaosta lähdetään (max {MaximumRounds()}): ");
            input = this.IsBotMatch || this.TotalTest ? "10" : DEBUGMODE ? "4" : Console.ReadLine();
            while (!Int32.TryParse(input, out lkm) || lkm > MaximumRounds())
            {
                Console.Write($"Mistä jaosta lähdetään (max {MaximumRounds()}): ");
                input = Console.ReadLine();
            }
            this.StartRound = lkm;

            if (!this.TotalTest) Console.Write("Missä jaossa käännytään: ");
            input = this.IsBotMatch || this.TotalTest ? "1" : DEBUGMODE ? "1" : Console.ReadLine();
            while (!Int32.TryParse(input, out lkm) || lkm < 1 || lkm > this.StartRound)
            {
                Console.Write("Missä jaossa käännytään: ");
                input = Console.ReadLine();
            }
            this.TurnRound = lkm;

            if (!this.TotalTest) Console.Write($"Mihin jakoon lopetetaan (max {MaximumRounds()}): ");
            input = this.IsBotMatch || this.TotalTest ? "10" : DEBUGMODE ? "4" : Console.ReadLine();
            while (!Int32.TryParse(input, out lkm) || lkm < this.TurnRound || lkm > MaximumRounds())
            {
                Console.Write($"Mihin jakoon lopetetaan (max {MaximumRounds()}): ");
                input = Console.ReadLine();
            }
            this.EndRound = lkm;
            if (!this.TotalTest) Console.WriteLine();

        }

        private Player[] ShufflePlayers(int shuffleTime = 0)
        {
            Random rnd = new Random();
            Player[] shuffledPlayers = this.Players.OrderBy(x => rnd.Next()).ToArray();
            return shuffledPlayers;
        }

    }
}
