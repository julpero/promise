using System;
using DSI.Deck;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

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
        // public double PromiseMultiplierBase2 {get; set;}
        // public double PromiseMultiplierBase3 {get; set;}
        // public double PromiseMultiplierBase4 {get; set;}
        public double PromiseMultiplierChange1A {get; set;}
        public double PromiseMultiplierChange1B {get; set;}
        public double PromiseMultiplierChange1C {get; set;}
        // public double PromiseMultiplierChange2A {get; set;}
        // public double PromiseMultiplierChange2B {get; set;}
        // public double PromiseMultiplierChange2C {get; set;}
        // public double PromiseMultiplierChange3A {get; set;}
        // public double PromiseMultiplierChange3B {get; set;}
        // public double PromiseMultiplierChange3C {get; set;}
        // public double PromiseMultiplierChange4A {get; set;}
        // public double PromiseMultiplierChange4B {get; set;}
        // public double PromiseMultiplierChange4C {get; set;}
        public int MiniRisk {get; set;}

        private static Random randomAi = new Random();
#region CreateAiRandoms
        public static double GetRandomNumber(double minimum, double maximum)
        {
            return randomAi.NextDouble() * (maximum - minimum) + minimum;
        }
        
        private static int PlayerRandInt(int mean, int variance = 2)
        {
            if (variance <= 0) return mean;
            int randInt = randomAi.Next(mean - variance, mean + variance + 1);
            if (randInt > 100) return 100;
            if (randInt < 0) return 0;
            return randInt;
        }

        private static double PlayerRandDouble(double mean, double variance = 0.05)
        {
            if (variance <= 0) return mean;
            double randDouble = GetRandomNumber(mean - variance, mean + variance);
            if (randDouble > 1) return 1.0;
            if (randDouble < 0) return 0.0;
            return randDouble;
        }

        public static bool KeepAiValue()
        {
            if (randomAi.Next(0, 2) != 0)
            {
                return true;
            }
            return false;
        }
        public static int AverageInt(int i, int j)
        {
            return (i + j) / 2;
        }
        public static double AverageDouble(double i, double j)
        {
            return (i + j) / 2;
        }

        public static double LesserDouble()
        {
            return Math.Pow(randomAi.NextDouble(), 2);
        }

        public PlayerAI(string guidStr, PlayerAI bestAi, PlayerAI goodAi, bool average = false)
        {
            AiName = guidStr;

            if (average)
            {
                DodgeBase = AverageInt(bestAi.DodgeBase, goodAi.DodgeBase); // 50
                DodgeSure = 100; // this is a fact
                DodgeSmallestValuesInSuit = AverageInt(bestAi.DodgeSmallestValuesInSuit, goodAi.DodgeSmallestValuesInSuit); // 95
                DodgeSmallestValuesInSuitNOT = AverageInt(bestAi.DodgeSmallestValuesInSuitNOT, goodAi.DodgeSmallestValuesInSuitNOT); // 85
                DodgeCardCountAvgOtherPlayersCount1 = AverageInt(bestAi.DodgeCardCountAvgOtherPlayersCount1, goodAi.DodgeCardCountAvgOtherPlayersCount1); // 3
                DodgeBiggestValuesInSuit = AverageInt(bestAi.DodgeBiggestValuesInSuit, goodAi.DodgeBiggestValuesInSuit); // 15;
                DodgeBiggestValuesInSuitNOT = AverageInt(bestAi.DodgeBiggestValuesInSuitNOT, goodAi.DodgeBiggestValuesInSuitNOT); // 25;
                DodgeCardCountAvgOtherPlayersCount2 = AverageInt(bestAi.DodgeCardCountAvgOtherPlayersCount2, goodAi.DodgeCardCountAvgOtherPlayersCount2); // 7;
                DodgeInChargeAverageCount = AverageDouble(bestAi.DodgeInChargeAverageCount, goodAi.DodgeInChargeAverageCount); // 0.8;
                
                // BigValuesInSuit
                BigValuesInSuit = AverageInt(bestAi.BigValuesInSuit, goodAi.BigValuesInSuit); // 10;

                // SmallValuesInSuit
                SmallValuesInSuit = AverageInt(bestAi.SmallValuesInSuit, goodAi.SmallValuesInSuit); // 6;

                // MakePromise
                PromiseMultiplierBase1 = AverageDouble(bestAi.PromiseMultiplierBase1, goodAi.PromiseMultiplierBase1); // 0.6;
                // PromiseMultiplierBase2 = AverageDouble(bestAi.PromiseMultiplierBase2, goodAi.PromiseMultiplierBase2); // 0.2;
                // PromiseMultiplierBase3 = AverageDouble(bestAi.PromiseMultiplierBase3, goodAi.PromiseMultiplierBase3); // 0.3;
                // PromiseMultiplierBase4 = AverageDouble(bestAi.PromiseMultiplierBase4, goodAi.PromiseMultiplierBase4); // 0.25;
                PromiseMultiplierChange1A = AverageDouble(bestAi.PromiseMultiplierChange1A, goodAi.PromiseMultiplierChange1A); // 0.3;
                PromiseMultiplierChange1B = AverageDouble(bestAi.PromiseMultiplierChange1B, goodAi.PromiseMultiplierChange1B); // 0.15;
                PromiseMultiplierChange1C = AverageDouble(bestAi.PromiseMultiplierChange1C, goodAi.PromiseMultiplierChange1C); // 0.1;
                // PromiseMultiplierChange2A = AverageDouble(bestAi.PromiseMultiplierChange2A, goodAi.PromiseMultiplierChange2A); // 0.1;
                // PromiseMultiplierChange2B = AverageDouble(bestAi.PromiseMultiplierChange2B, goodAi.PromiseMultiplierChange2B); // 0.05;
                // PromiseMultiplierChange2C = AverageDouble(bestAi.PromiseMultiplierChange2C, goodAi.PromiseMultiplierChange2C); // 0.1;
                // PromiseMultiplierChange3A = AverageDouble(bestAi.PromiseMultiplierChange3A, goodAi.PromiseMultiplierChange3A); // 0.4;
                // PromiseMultiplierChange3B = AverageDouble(bestAi.PromiseMultiplierChange3B, goodAi.PromiseMultiplierChange3B); // 0.25;
                // PromiseMultiplierChange3C = AverageDouble(bestAi.PromiseMultiplierChange3C, goodAi.PromiseMultiplierChange3C); // 0.1;
                // PromiseMultiplierChange4A = AverageDouble(bestAi.PromiseMultiplierChange4A, goodAi.PromiseMultiplierChange4A); // 0.1;
                // PromiseMultiplierChange4B = AverageDouble(bestAi.PromiseMultiplierChange4B, goodAi.PromiseMultiplierChange4B); // 0.05;
                // PromiseMultiplierChange4C = AverageDouble(bestAi.PromiseMultiplierChange4C, goodAi.PromiseMultiplierChange4C); // 0.1;
                MiniRisk = AverageInt(bestAi.MiniRisk, goodAi.MiniRisk); // 5;
            }
            else
            {
                DodgeBase = KeepAiValue() ? bestAi.DodgeBase : goodAi.DodgeBase; // 50
                DodgeSure = 100; // this is a fact
                DodgeSmallestValuesInSuit = KeepAiValue() ? bestAi.DodgeSmallestValuesInSuit : goodAi.DodgeSmallestValuesInSuit; // 95
                DodgeSmallestValuesInSuitNOT = KeepAiValue() ? bestAi.DodgeSmallestValuesInSuitNOT : goodAi.DodgeSmallestValuesInSuitNOT; // 85
                DodgeCardCountAvgOtherPlayersCount1 = KeepAiValue() ? bestAi.DodgeCardCountAvgOtherPlayersCount1 : goodAi.DodgeCardCountAvgOtherPlayersCount1; // 3
                DodgeBiggestValuesInSuit = KeepAiValue() ? bestAi.DodgeBiggestValuesInSuit : goodAi.DodgeBiggestValuesInSuit; // 15;
                DodgeBiggestValuesInSuitNOT = KeepAiValue() ? bestAi.DodgeBiggestValuesInSuitNOT : goodAi.DodgeBiggestValuesInSuitNOT; // 25;
                DodgeCardCountAvgOtherPlayersCount2 = KeepAiValue() ? bestAi.DodgeCardCountAvgOtherPlayersCount2 : goodAi.DodgeCardCountAvgOtherPlayersCount2; // 7;
                DodgeInChargeAverageCount = KeepAiValue() ? bestAi.DodgeInChargeAverageCount : goodAi.DodgeInChargeAverageCount; // 0.8;
                
                // BigValuesInSuit
                BigValuesInSuit = KeepAiValue() ? bestAi.BigValuesInSuit : goodAi.BigValuesInSuit; // 10;

                // SmallValuesInSuit
                SmallValuesInSuit = KeepAiValue() ? bestAi.SmallValuesInSuit : goodAi.SmallValuesInSuit; // 6;

                // MakePromise
                PromiseMultiplierBase1 = KeepAiValue() ? bestAi.PromiseMultiplierBase1 : goodAi.PromiseMultiplierBase1; // 0.6;
                // PromiseMultiplierBase2 = KeepAiValue() ? bestAi.PromiseMultiplierBase2 : goodAi.PromiseMultiplierBase2; // 0.2;
                // PromiseMultiplierBase3 = KeepAiValue() ? bestAi.PromiseMultiplierBase3 : goodAi.PromiseMultiplierBase3; // 0.3;
                // PromiseMultiplierBase4 = KeepAiValue() ? bestAi.PromiseMultiplierBase4 : goodAi.PromiseMultiplierBase4; // 0.25;
                PromiseMultiplierChange1A = KeepAiValue() ? bestAi.PromiseMultiplierChange1A : goodAi.PromiseMultiplierChange1A; // 0.3;
                PromiseMultiplierChange1B = KeepAiValue() ? bestAi.PromiseMultiplierChange1B : goodAi.PromiseMultiplierChange1B; // 0.15;
                PromiseMultiplierChange1C = KeepAiValue() ? bestAi.PromiseMultiplierChange1C : goodAi.PromiseMultiplierChange1C; // 0.1;
                // PromiseMultiplierChange2A = KeepAiValue() ? bestAi.PromiseMultiplierChange2A : goodAi.PromiseMultiplierChange2A; // 0.1;
                // PromiseMultiplierChange2B = KeepAiValue() ? bestAi.PromiseMultiplierChange2B : goodAi.PromiseMultiplierChange2B; // 0.05;
                // PromiseMultiplierChange2C = KeepAiValue() ? bestAi.PromiseMultiplierChange2C : goodAi.PromiseMultiplierChange2C; // 0.1;
                // PromiseMultiplierChange3A = KeepAiValue() ? bestAi.PromiseMultiplierChange3A : goodAi.PromiseMultiplierChange3A; // 0.4;
                // PromiseMultiplierChange3B = KeepAiValue() ? bestAi.PromiseMultiplierChange3B : goodAi.PromiseMultiplierChange3B; // 0.25;
                // PromiseMultiplierChange3C = KeepAiValue() ? bestAi.PromiseMultiplierChange3C : goodAi.PromiseMultiplierChange3C; // 0.1;
                // PromiseMultiplierChange4A = KeepAiValue() ? bestAi.PromiseMultiplierChange4A : goodAi.PromiseMultiplierChange4A; // 0.1;
                // PromiseMultiplierChange4B = KeepAiValue() ? bestAi.PromiseMultiplierChange4B : goodAi.PromiseMultiplierChange4B; // 0.05;
                // PromiseMultiplierChange4C = KeepAiValue() ? bestAi.PromiseMultiplierChange4C : goodAi.PromiseMultiplierChange4C; // 0.1;
                MiniRisk = KeepAiValue() ? bestAi.MiniRisk : goodAi.MiniRisk; // 5;
            }



            // mutation
            if (randomAi.NextDouble() > 0.98) DodgeBase = randomAi.Next(3, 90); // 50
            DodgeSure = 100; // this is a fact
            if (randomAi.NextDouble() > 0.98) DodgeSmallestValuesInSuit = randomAi.Next(30, 101); // 95
            if (randomAi.NextDouble() > 0.98) DodgeSmallestValuesInSuitNOT = randomAi.Next(0, 101); // 85
            if (randomAi.NextDouble() > 0.98) DodgeCardCountAvgOtherPlayersCount1 = randomAi.Next(0, 101); // 3
            if (randomAi.NextDouble() > 0.98) DodgeBiggestValuesInSuit = randomAi.Next(0, 101); // 15;
            if (randomAi.NextDouble() > 0.98) DodgeBiggestValuesInSuitNOT = randomAi.Next(0, 101); // 25;
            if (randomAi.NextDouble() > 0.98) DodgeCardCountAvgOtherPlayersCount2 = randomAi.Next(0, 101); // 7;
            if (randomAi.NextDouble() > 0.98) DodgeInChargeAverageCount = randomAi.NextDouble(); // 0.8;
            
            // BigValuesInSuit
            if (randomAi.NextDouble() > 0.98) BigValuesInSuit = randomAi.Next(9, 15); // 10;

            // SmallValuesInSuit
            if (randomAi.NextDouble() > 0.98) SmallValuesInSuit = randomAi.Next(2, 8); // 6;

            // MakePromise
            if (randomAi.NextDouble() > 0.98) PromiseMultiplierBase1 = randomAi.NextDouble(); // 0.6;
            // if (randomAi.NextDouble() > 0.98) PromiseMultiplierBase2 = randomAi.NextDouble(); // 0.2;
            // if (randomAi.NextDouble() > 0.98) PromiseMultiplierBase3 = randomAi.NextDouble(); // 0.3;
            // if (randomAi.NextDouble() > 0.98) PromiseMultiplierBase4 = randomAi.NextDouble(); // 0.25;
            if (randomAi.NextDouble() > 0.98) PromiseMultiplierChange1A = randomAi.NextDouble(); // 0.3;
            if (randomAi.NextDouble() > 0.98) PromiseMultiplierChange1B = randomAi.NextDouble(); // 0.15;
            if (randomAi.NextDouble() > 0.98) PromiseMultiplierChange1C = randomAi.NextDouble(); // 0.1;
            // if (randomAi.NextDouble() > 0.98) PromiseMultiplierChange2A = randomAi.NextDouble(); // 0.1;
            // if (randomAi.NextDouble() > 0.98) PromiseMultiplierChange2B = randomAi.NextDouble(); // 0.05;
            // if (randomAi.NextDouble() > 0.98) PromiseMultiplierChange2C = randomAi.NextDouble(); // 0.1;
            // if (randomAi.NextDouble() > 0.98) PromiseMultiplierChange3A = LesserDouble(); //randomAi.NextDouble(); // 0.4;
            // if (randomAi.NextDouble() > 0.98) PromiseMultiplierChange3B = LesserDouble(); //randomAi.NextDouble(); // 0.25;
            // if (randomAi.NextDouble() > 0.98) PromiseMultiplierChange3C = LesserDouble(); //randomAi.NextDouble(); // 0.1;
            // if (randomAi.NextDouble() > 0.98) PromiseMultiplierChange4A = randomAi.NextDouble(); // 0.1;
            // if (randomAi.NextDouble() > 0.98) PromiseMultiplierChange4B = randomAi.NextDouble(); // 0.05;
            // if (randomAi.NextDouble() > 0.98) PromiseMultiplierChange4C = randomAi.NextDouble(); // 0.1;
            if (randomAi.NextDouble() > 0.98) MiniRisk = randomAi.Next(0, 50); // 5;
        }

        public PlayerAI(string aiName)
        {
            if (aiName == "Fuison")
            {
                // this was my best guess when coding
                // now with very small random
                AiName = "Fuison";

                // AnalyzeDodgeable
                DodgeBase = PlayerRandInt(50);
                DodgeSure = 100;
                DodgeSmallestValuesInSuit = PlayerRandInt(95);
                DodgeSmallestValuesInSuitNOT = PlayerRandInt(85);
                DodgeCardCountAvgOtherPlayersCount1 = PlayerRandInt(3);
                DodgeBiggestValuesInSuit = PlayerRandInt(15);
                DodgeBiggestValuesInSuitNOT = PlayerRandInt(25);
                DodgeCardCountAvgOtherPlayersCount2 = PlayerRandInt(7);
                DodgeInChargeAverageCount = PlayerRandDouble(0.8);
                
                // BigValuesInSuit
                BigValuesInSuit = PlayerRandInt(11, 1);

                // SmallValuesInSuit
                SmallValuesInSuit = PlayerRandInt(6, 1);

                // MakePromise
                PromiseMultiplierBase1 = PlayerRandDouble(0.6);
                // PromiseMultiplierBase2 = PlayerRandDouble(0.2);
                // PromiseMultiplierBase3 = PlayerRandDouble(0.3);
                // PromiseMultiplierBase4 = PlayerRandDouble(0.25);
                PromiseMultiplierChange1A = PlayerRandDouble(0.3);
                PromiseMultiplierChange1B = PlayerRandDouble(0.15);
                PromiseMultiplierChange1C = PlayerRandDouble(0.1);
                // PromiseMultiplierChange2A = PlayerRandDouble(0.1);
                // PromiseMultiplierChange2B = PlayerRandDouble(0.06);
                // PromiseMultiplierChange2C = PlayerRandDouble(0.1);
                // PromiseMultiplierChange3A = PlayerRandDouble(0.4);
                // PromiseMultiplierChange3B = PlayerRandDouble(0.25);
                // PromiseMultiplierChange3C = PlayerRandDouble(0.1);
                // PromiseMultiplierChange4A = PlayerRandDouble(0.1);
                // PromiseMultiplierChange4B = PlayerRandDouble(0.06);
                // PromiseMultiplierChange4C = PlayerRandDouble(0.1);
                MiniRisk = PlayerRandInt(5);
            }
            else
            {
                // total random bot
                AiName = aiName;

                DodgeBase = randomAi.Next(0, 101); // 50
                DodgeSure = 100; // this is a fact
                DodgeSmallestValuesInSuit = randomAi.Next(0, 101); // 95
                DodgeSmallestValuesInSuitNOT = randomAi.Next(0, 101); // 85
                DodgeCardCountAvgOtherPlayersCount1 = randomAi.Next(0, 101); // 3
                DodgeBiggestValuesInSuit = randomAi.Next(0, 101); // 15;
                DodgeBiggestValuesInSuitNOT = randomAi.Next(0, 101); // 25;
                DodgeCardCountAvgOtherPlayersCount2 = randomAi.Next(0, 101); // 7;
                DodgeInChargeAverageCount = GetRandomNumber(0.0, 1.0); // 0.8;
                
                // BigValuesInSuit
                BigValuesInSuit = randomAi.Next(9, 15); // 10;

                // SmallValuesInSuit
                SmallValuesInSuit = randomAi.Next(2, 8); // 6;

                // MakePromise
                PromiseMultiplierBase1 = GetRandomNumber(0.0, 1.0); // 0.6;
                // PromiseMultiplierBase2 = GetRandomNumber(0.0, 1.0); // 0.2;
                // PromiseMultiplierBase3 = GetRandomNumber(0.0, 1.0); // 0.3;
                // PromiseMultiplierBase4 = GetRandomNumber(0.0, 1.0); // 0.25;
                PromiseMultiplierChange1A = GetRandomNumber(0.0, 1.0); // 0.3;
                PromiseMultiplierChange1B = GetRandomNumber(0.0, 1.0); // 0.15;
                PromiseMultiplierChange1C = GetRandomNumber(0.0, 1.0); // 0.1;
                // PromiseMultiplierChange2A = GetRandomNumber(0.0, 1.0); // 0.1;
                // PromiseMultiplierChange2B = GetRandomNumber(0.0, 1.0); // 0.05;
                // PromiseMultiplierChange2C = GetRandomNumber(0.0, 1.0); // 0.1;
                // PromiseMultiplierChange3A = LesserDouble(); //GetRandomNumber(0.0, 1.0); // 0.4;
                // PromiseMultiplierChange3B = LesserDouble(); //GetRandomNumber(0.0, 1.0); // 0.25;
                // PromiseMultiplierChange3C = LesserDouble(); //GetRandomNumber(0.0, 1.0); // 0.1;
                // PromiseMultiplierChange4A = GetRandomNumber(0.0, 1.0); // 0.1;
                // PromiseMultiplierChange4B = GetRandomNumber(0.0, 1.0); // 0.05;
                // PromiseMultiplierChange4C = GetRandomNumber(0.0, 1.0); // 0.1;
                MiniRisk = randomAi.Next(0, 101); // 5;            
            }
        }

        public PlayerAI(int playerId, bool mutate = false)
        {
            if (playerId == 0)
            {
                // b60821b5-e548-4407-9b4c-2c0d1e988b42
                AiName = "Jaska";

                DodgeBase = PlayerRandInt(57);
                DodgeSure = 100; // this is a fact
                DodgeSmallestValuesInSuit = PlayerRandInt(90);
                DodgeSmallestValuesInSuitNOT = PlayerRandInt(76);
                DodgeCardCountAvgOtherPlayersCount1 = PlayerRandInt(23);
                DodgeBiggestValuesInSuit = PlayerRandInt(14);
                DodgeBiggestValuesInSuitNOT = PlayerRandInt(15);
                DodgeCardCountAvgOtherPlayersCount2 = PlayerRandInt(19);
                DodgeInChargeAverageCount = PlayerRandDouble(0.94);
                
                // BigValuesInSuit
                BigValuesInSuit = PlayerRandInt(10, 1);

                // SmallValuesInSuit
                SmallValuesInSuit = PlayerRandInt(3, 1);

                // MakePromise
                PromiseMultiplierBase1 = PlayerRandDouble(0.54);
                PromiseMultiplierChange1A = PlayerRandDouble(0.01);
                PromiseMultiplierChange1B = PlayerRandDouble(0.03);
                PromiseMultiplierChange1C = PlayerRandDouble(0.21);
                MiniRisk = PlayerRandInt(53);

            }
            if (playerId == 1)
            {
                AiName = "Pera";
                // 113fd6f9-85a8-4b22-889d-7c94d1e23b7a

                DodgeBase = PlayerRandInt(94);
                DodgeSure = 100; // this is a fact
                DodgeSmallestValuesInSuit = PlayerRandInt(92);
                DodgeSmallestValuesInSuitNOT = PlayerRandInt(64);
                DodgeCardCountAvgOtherPlayersCount1 = PlayerRandInt(44);
                DodgeBiggestValuesInSuit = PlayerRandInt(36);
                DodgeBiggestValuesInSuitNOT = PlayerRandInt(79);
                DodgeCardCountAvgOtherPlayersCount2 = PlayerRandInt(83);
                DodgeInChargeAverageCount = PlayerRandDouble(0.97);
                
                // BigValuesInSuit
                BigValuesInSuit = PlayerRandInt(11, 1);

                // SmallValuesInSuit
                SmallValuesInSuit = PlayerRandInt(4, 1);

                // MakePromise
                PromiseMultiplierBase1 = PlayerRandDouble(0.17);
                PromiseMultiplierChange1A = PlayerRandDouble(0.81);
                PromiseMultiplierChange1B = PlayerRandDouble(0.48);
                PromiseMultiplierChange1C = PlayerRandDouble(0.46);
                MiniRisk = PlayerRandInt(32);
            }

            if (playerId == 2)
            {
                AiName = "Lissu";
                // a9b3534b-377b-4a35-98f9-e6ec9cb478df

                DodgeBase = PlayerRandInt(79);
                DodgeSure = 100; // this is a fact
                DodgeSmallestValuesInSuit = PlayerRandInt(17);
                DodgeSmallestValuesInSuitNOT = PlayerRandInt(2);
                DodgeCardCountAvgOtherPlayersCount1 = PlayerRandInt(28);
                DodgeBiggestValuesInSuit = PlayerRandInt(75);
                DodgeBiggestValuesInSuitNOT = PlayerRandInt(66);
                DodgeCardCountAvgOtherPlayersCount2 = PlayerRandInt(27);
                DodgeInChargeAverageCount = PlayerRandDouble(0.18);
                
                // BigValuesInSuit
                BigValuesInSuit = PlayerRandInt(12, 1);

                // SmallValuesInSuit
                SmallValuesInSuit = PlayerRandInt(6, 1);

                // MakePromise
                PromiseMultiplierBase1 = PlayerRandDouble(0.48);
                PromiseMultiplierChange1A = PlayerRandDouble(0.37);
                PromiseMultiplierChange1B = PlayerRandDouble(0.64);
                PromiseMultiplierChange1C = PlayerRandDouble(0.15);
                MiniRisk = PlayerRandInt(69);
            }

            if (playerId == 3)
            {
                AiName = "Repa";
                // ccb01d20-562c-4803-bd3f-c1e8449a3dda

                DodgeBase = PlayerRandInt(79);
                DodgeSure = 100; // this is a fact
                DodgeSmallestValuesInSuit = PlayerRandInt(17);
                DodgeSmallestValuesInSuitNOT = PlayerRandInt(42);
                DodgeCardCountAvgOtherPlayersCount1 = PlayerRandInt(28);
                DodgeBiggestValuesInSuit = PlayerRandInt(1);
                DodgeBiggestValuesInSuitNOT = PlayerRandInt(63);
                DodgeCardCountAvgOtherPlayersCount2 = PlayerRandInt(46);
                DodgeInChargeAverageCount = PlayerRandDouble(0.18);
                
                // BigValuesInSuit
                BigValuesInSuit = randomAi.Next(13, 15);

                // SmallValuesInSuit
                SmallValuesInSuit = PlayerRandInt(5, 1);

                // MakePromise
                PromiseMultiplierBase1 = PlayerRandDouble(0.48);
                PromiseMultiplierChange1A = PlayerRandDouble(0.37);
                PromiseMultiplierChange1B = PlayerRandDouble(0.64);
                PromiseMultiplierChange1C = PlayerRandDouble(0.15);
                MiniRisk = PlayerRandInt(69);
            }

            if (playerId == 4)
            {
                AiName = "Arska";
                // 2d4ef7b5-1cbe-4de3-9c6c-c6092352894e

                DodgeBase = PlayerRandInt(20);
                DodgeSure = 100; // this is a fact
                DodgeSmallestValuesInSuit = PlayerRandInt(9);
                DodgeSmallestValuesInSuitNOT = PlayerRandInt(2);
                DodgeCardCountAvgOtherPlayersCount1 = PlayerRandInt(61);
                DodgeBiggestValuesInSuit = PlayerRandInt(1);
                DodgeBiggestValuesInSuitNOT = PlayerRandInt(80);
                DodgeCardCountAvgOtherPlayersCount2 = PlayerRandInt(22);
                DodgeInChargeAverageCount = PlayerRandDouble(0.42);
                
                // BigValuesInSuit
                BigValuesInSuit = PlayerRandInt(12, 1);

                // SmallValuesInSuit
                SmallValuesInSuit = randomAi.Next(2, 4);

                // MakePromise
                PromiseMultiplierBase1 = PlayerRandDouble(0.64);
                PromiseMultiplierChange1A = PlayerRandDouble(0.08);
                PromiseMultiplierChange1B = PlayerRandDouble(0.01);
                PromiseMultiplierChange1C = PlayerRandDouble(0.01);
                MiniRisk = PlayerRandInt(75);
            }

            if (playerId == 5)
            {
                AiName = "Jossu";
                // 51180a82-4342-4b33-977a-a3ad71e1e04e

                DodgeBase = PlayerRandInt(69);
                DodgeSure = 100; // this is a fact
                DodgeSmallestValuesInSuit = PlayerRandInt(81);
                DodgeSmallestValuesInSuitNOT = PlayerRandInt(95);
                DodgeCardCountAvgOtherPlayersCount1 = PlayerRandInt(61);
                DodgeBiggestValuesInSuit = PlayerRandInt(94);
                DodgeBiggestValuesInSuitNOT = PlayerRandInt(57);
                DodgeCardCountAvgOtherPlayersCount2 = PlayerRandInt(85);
                DodgeInChargeAverageCount = PlayerRandDouble(0.42);
                
                // BigValuesInSuit
                BigValuesInSuit = PlayerRandInt(9, 1);

                // SmallValuesInSuit
                SmallValuesInSuit = PlayerRandInt(6, 1);

                // MakePromise
                PromiseMultiplierBase1 = PlayerRandDouble(0.10);
                PromiseMultiplierChange1A = PlayerRandDouble(0.14);
                PromiseMultiplierChange1B = PlayerRandDouble(0.41);
                PromiseMultiplierChange1C = PlayerRandDouble(0.05);
                MiniRisk = PlayerRandInt(97);
            }

            if (playerId == 6)
            {
                AiName = "Sussu";
                // 33347887-2f78-4caa-9acb-29f344b425d7

                DodgeBase = PlayerRandInt(82);
                DodgeSure = 100; // this is a fact
                DodgeSmallestValuesInSuit = PlayerRandInt(62);
                DodgeSmallestValuesInSuitNOT = PlayerRandInt(7);
                DodgeCardCountAvgOtherPlayersCount1 = PlayerRandInt(81);
                DodgeBiggestValuesInSuit = PlayerRandInt(11);
                DodgeBiggestValuesInSuitNOT = PlayerRandInt(37);
                DodgeCardCountAvgOtherPlayersCount2 = PlayerRandInt(82);
                DodgeInChargeAverageCount = PlayerRandDouble(0.23);
                
                // BigValuesInSuit
                BigValuesInSuit = PlayerRandInt(10, 1);

                // SmallValuesInSuit
                SmallValuesInSuit = PlayerRandInt(5, 1);

                // MakePromise
                PromiseMultiplierBase1 = PlayerRandDouble(0.31);
                PromiseMultiplierChange1A = PlayerRandDouble(0.08);
                PromiseMultiplierChange1B = PlayerRandDouble(0.05);
                PromiseMultiplierChange1C = PlayerRandDouble(0.13);
                MiniRisk = PlayerRandInt(78);
            }

            if (playerId == 7)
            {
                AiName = "Kake";
                // 64f323b4-0c22-46bd-80f3-779b219d6e8d

                DodgeBase = PlayerRandInt(17);
                DodgeSure = 100; // this is a fact
                DodgeSmallestValuesInSuit = PlayerRandInt(48);
                DodgeSmallestValuesInSuitNOT = PlayerRandInt(87);
                DodgeCardCountAvgOtherPlayersCount1 = PlayerRandInt(23);
                DodgeBiggestValuesInSuit = PlayerRandInt(57);
                DodgeBiggestValuesInSuitNOT = PlayerRandInt(43);
                DodgeCardCountAvgOtherPlayersCount2 = PlayerRandInt(6);
                DodgeInChargeAverageCount = PlayerRandDouble(0.28);
                
                // BigValuesInSuit
                BigValuesInSuit = PlayerRandInt(10, 1);

                // SmallValuesInSuit
                SmallValuesInSuit = PlayerRandInt(6, 1);

                // MakePromise
                PromiseMultiplierBase1 = PlayerRandDouble(0.09);
                PromiseMultiplierChange1A = PlayerRandDouble(0.85);
                PromiseMultiplierChange1B = PlayerRandDouble(0.80);
                PromiseMultiplierChange1C = PlayerRandDouble(0.08);
                MiniRisk = PlayerRandInt(2);
            }

            if (playerId == 8)
            {
                AiName = "Late";
                // 3e2d5eca-45b3-438e-8256-3d8a6c3ab2d1

                DodgeBase = PlayerRandInt(89);
                DodgeSure = 100; // this is a fact
                DodgeSmallestValuesInSuit = PlayerRandInt(34);
                DodgeSmallestValuesInSuitNOT = PlayerRandInt(58);
                DodgeCardCountAvgOtherPlayersCount1 = PlayerRandInt(65);
                DodgeBiggestValuesInSuit = PlayerRandInt(15);
                DodgeBiggestValuesInSuitNOT = PlayerRandInt(95);
                DodgeCardCountAvgOtherPlayersCount2 = PlayerRandInt(63);
                DodgeInChargeAverageCount = PlayerRandDouble(0.02);
                
                // BigValuesInSuit
                BigValuesInSuit = PlayerRandInt(13, 1);

                // SmallValuesInSuit
                SmallValuesInSuit = PlayerRandInt(7, 1);

                // MakePromise
                PromiseMultiplierBase1 = PlayerRandDouble(0.09);
                PromiseMultiplierChange1A = PlayerRandDouble(0.38);
                PromiseMultiplierChange1B = PlayerRandDouble(0.17);
                PromiseMultiplierChange1C = PlayerRandDouble(0.13);
                MiniRisk = PlayerRandInt(20);
            }


            if (mutate)
            {
                // mutation
                if (randomAi.NextDouble() > 0.98) DodgeBase = randomAi.Next(100) + 1; // 50
                DodgeSure = 100; // this is a fact
                if (randomAi.NextDouble() > 0.98) DodgeSmallestValuesInSuit = randomAi.Next(100); // 95
                if (randomAi.NextDouble() > 0.98) DodgeSmallestValuesInSuitNOT = randomAi.Next(100); // 85
                if (randomAi.NextDouble() > 0.98) DodgeCardCountAvgOtherPlayersCount1 = randomAi.Next(100); // 3
                if (randomAi.NextDouble() > 0.98) DodgeBiggestValuesInSuit = randomAi.Next(100); // 15;
                if (randomAi.NextDouble() > 0.98) DodgeBiggestValuesInSuitNOT = randomAi.Next(100); // 25;
                if (randomAi.NextDouble() > 0.98) DodgeCardCountAvgOtherPlayersCount2 = randomAi.Next(100); // 7;
                if (randomAi.NextDouble() > 0.98) DodgeInChargeAverageCount = randomAi.NextDouble(); // 0.8;
                
                // BigValuesInSuit
                if (randomAi.NextDouble() > 0.98) BigValuesInSuit = randomAi.Next(1, 14) + 1; // 10;

                // SmallValuesInSuit
                if (randomAi.NextDouble() > 0.98) SmallValuesInSuit = randomAi.Next(1, 14) + 1; // 6;

                // MakePromise
                if (randomAi.NextDouble() > 0.98) PromiseMultiplierBase1 = randomAi.NextDouble(); // 0.6;
                // if (randomAi.NextDouble() > 0.98) PromiseMultiplierBase2 = randomAi.NextDouble(); // 0.2;
                // if (randomAi.NextDouble() > 0.98) PromiseMultiplierBase3 = randomAi.NextDouble(); // 0.3;
                // if (randomAi.NextDouble() > 0.98) PromiseMultiplierBase4 = randomAi.NextDouble(); // 0.25;
                if (randomAi.NextDouble() > 0.98) PromiseMultiplierChange1A = randomAi.NextDouble(); // 0.3;
                if (randomAi.NextDouble() > 0.98) PromiseMultiplierChange1B = randomAi.NextDouble(); // 0.15;
                if (randomAi.NextDouble() > 0.98) PromiseMultiplierChange1C = randomAi.NextDouble(); // 0.1;
                // if (randomAi.NextDouble() > 0.98) PromiseMultiplierChange2A = randomAi.NextDouble(); // 0.1;
                // if (randomAi.NextDouble() > 0.98) PromiseMultiplierChange2B = randomAi.NextDouble(); // 0.05;
                // if (randomAi.NextDouble() > 0.98) PromiseMultiplierChange2C = randomAi.NextDouble(); // 0.1;
                // if (randomAi.NextDouble() > 0.98) PromiseMultiplierChange3A = randomAi.NextDouble(); // 0.4;
                // if (randomAi.NextDouble() > 0.98) PromiseMultiplierChange3B = randomAi.NextDouble(); // 0.25;
                // if (randomAi.NextDouble() > 0.98) PromiseMultiplierChange3C = randomAi.NextDouble(); // 0.1;
                // if (randomAi.NextDouble() > 0.98) PromiseMultiplierChange4A = randomAi.NextDouble(); // 0.1;
                // if (randomAi.NextDouble() > 0.98) PromiseMultiplierChange4B = randomAi.NextDouble(); // 0.05;
                // if (randomAi.NextDouble() > 0.98) PromiseMultiplierChange4C = randomAi.NextDouble(); // 0.1;
                if (randomAi.NextDouble() > 0.98) MiniRisk = randomAi.Next(100); // 5;
            }
        }
    }
    
#endregion

    public static class ComputerAI
    {
        private enum PlayingMethod
        {
            NOTSET,
            // player still have change to win by taking rounds
            TRYTOWIN,
            // player still have change to win by losing all rounds
            TRYTODODGE,
            // player has lost but makes harm to others by robbing
            TRYTOROB,
            // player has lost but makes harm to others by skipping
            TRYTOSKIP,
            // player has enough sure cards to keep promise so play other cards away
            SKIPSAFE,
            // player has to take all rest
            TAKEREST
        }

#region analyzing
        public class AnalyzedSuit
        {
            public bool IsTrump {get; set;}
            public CardSuit Suit {get; set;}
            public int BiggestValuesInSuit {get; set;}
            public int BigValuesInSuit {get; set;}
            public int SmallestValuesInSuit {get; set;}
            public int SmallValuesInSuit {get; set;}
            public int CardCount {get; set;}
            public int IsDodgeable {get; set;}
            public double AvgOtherPlayersCount {get; set;}

            private int AnalyzeDodgeable(PlayerAI ai, List<Card> playedCards, int playerCount, bool inCharge)
            {
                double retVal = ai.DodgeBase;
                if (this.CardCount == 0)
                {
                    // i don't have this suit at all so this is sure to dodge
                    return ai.DodgeSure;
                }
                else if (this.SmallValuesInSuit > this.BigValuesInSuit)
                {
                    // i have more smaller values than bigger values
                    if (this.SmallestValuesInSuit > 0)
                    {
                        // and i have some smallest values
                        retVal = ai.DodgeSmallestValuesInSuit;
                    }
                    else
                    {
                        // i don't have the smallest values
                        retVal = ai.DodgeSmallestValuesInSuitNOT;
                    }

                    if (CardCount < this.AvgOtherPlayersCount)
                    {
                        // i have less cards than average player with this suit
                        retVal+= ai.DodgeCardCountAvgOtherPlayersCount1;
                    }
                }
                else
                {
                    // more or equal of bigger values than smaller ones
                    if (this.BiggestValuesInSuit > 0)
                    {
                        // and i have at least one biggest in suit
                        retVal = ai.DodgeBiggestValuesInSuit;
                    }
                    else
                    {
                        // i don't have the biggest value in the suit
                        retVal = ai.DodgeBiggestValuesInSuitNOT;
                    }

                    if (CardCount > this.AvgOtherPlayersCount)
                    {
                        // i have more cards than average player with this suit
                        retVal=- ai.DodgeCardCountAvgOtherPlayersCount2;
                    }
                }

                if (inCharge && this.AvgOtherPlayersCount < 1)
                {
                    // i'm in charge and average players doesn't have this suit
                    retVal*= ai.DodgeInChargeAverageCount;
                }

                int dodgeable = (int)retVal;
                return dodgeable;
            }

            public AnalyzedSuit(PlayerAI ai, List<Card> cards, CardSuit suit, bool isTrump = false, List<Card> playedCards = null, int playerCount = 0, int cardsDrawn = 0, bool inCharge = false, PlayerInfo[] playerInfos = null)
            {
                CardCount = cards == null ? 0 : cards.Count();
                cardsDrawn++; // also trump is drawn
                List<Card> playedCardsInSuit = playedCards != null ? playedCards.Where(x => x.CardSuit == suit).ToList() : new List<Card>();
                PlayerInfo[] playerInfosInMethod = playerInfos != null ? playerInfos : new PlayerInfo[playerCount];
                if (playerInfos == null)
                {
                    for (int i = 0; i < playerCount; i++) playerInfosInMethod[i] = new PlayerInfo();
                }
                Suit = suit;
                IsTrump = isTrump;
                BiggestValuesInSuit = BiggestSuitsInHand(cards, playedCardsInSuit);
                BigValuesInSuit = BigValuesInSuit(ai, cards);
                SmallestValuesInSuit = SmallestValuesInSuit(cards, playedCardsInSuit);
                SmallValuesInSuit = SmallValuesInSuit(ai, cards);
                int cardsKnown = CardCount + playedCardsInSuit.Count();
                if (isTrump) cardsKnown++;
                AvgOtherPlayersCount = playerCount == 0 ? 0 : (13.0 - cardsKnown - ((52.0 - cardsDrawn) / 4.0)) / (playerCount - 1 - playerInfosInMethod.Count(x => !x.HasSuit(suit)));
                IsDodgeable = AnalyzeDodgeable(ai, playedCards, playerCount, inCharge);
            }
        }

        private static Random rand = new Random();

        private static bool CheckRandom(int checkValue)
        {
            if (checkValue < 0) checkValue = 0;
            if (checkValue > 100) checkValue = 100;
            int testInt = rand.Next(0, 101);
            return (testInt < checkValue);
        }

        private static int BiggestValuesInSuit(List<Card> cards, List<Card> playedCards = null)
        {
            if (cards == null || !cards.Any()) return 0;
            int retVal = 0;
            for (int i = 14; i > 1; i--)
            {
                if (cards.Any(x => (int)x.CardValue == i))
                {
                    retVal++;
                }
                else if (playedCards != null && playedCards.Any(x => (int)x.CardValue == i))
                {
                    continue;
                }
                else
                {
                    break;
                }
            }
            return retVal;
        }

        private static int BigValuesInSuit(PlayerAI ai, List<Card> cards)
        {
            if (cards == null || !cards.Any()) return 0;
            int retVal = 0;
            for (int i = 14; i >= ai.BigValuesInSuit; i--)
            {
                if (cards.Any(x => (int)x.CardValue == i))
                {
                    retVal++;
                }
            }
            return retVal;
        }

        private static int SmallestValuesInSuit(List<Card> cards, List<Card> playedCards = null)
        {
            if (cards == null || !cards.Any()) return 0;
            int retVal = 0;
            for (int i = 2; i <= 14; i++)
            {
                if (cards.Any(x => (int)x.CardValue == i))
                {
                    retVal++;
                }
                else if (playedCards != null && playedCards.Any(x => (int)x.CardValue == i))
                {
                    continue;
                }
                else
                {
                    break;
                }
            }
            return retVal;
        }

        private static int SmallValuesInSuit(PlayerAI ai, List<Card> cards)
        {
            if (cards == null || !cards.Any()) return 0;
            int retVal = 0;
            for (int i = 2; i <= ai.SmallValuesInSuit; i++)
            {
                if (cards.Any(x => (int)x.CardValue == i))
                {
                    retVal++;
                }
            }
            return retVal;
        }
#endregion

        private static int PlayersPromised(Promise[] promises)
        {
            int playersPromised = 0;
            for (int i = 0; i < promises.Count(); i++)
            {
                if (promises[i] == null) continue;
                playersPromised++;
            }
            return playersPromised;
        }

        private static int PromisesMade(Promise[] promises)
        {
            int promisesMade = 0;
            for (int i = 0; i < promises.Count(); i++)
            {
                if (promises[i] == null) continue;
                promisesMade+= promises[i].PromiseNumber;
            }
            return promisesMade;
        }

        private static bool IsBigPromise(int promiseNbr, double avgPoints)
        {
            // if some promise is over avg + 1 then it is big one
            return promiseNbr > avgPoints + 1;
        }

        private static int BigPromises(Promise[] promises, double avgPoints)
        {
            int bigPromises = 0;
            for (int i = 0; i < promises.Count(); i++)
            {
                if (promises[i] == null) continue;
                if (IsBigPromise(promises[i].PromiseNumber, avgPoints)) bigPromises++;
            }
            return bigPromises;
        }

        private static int ZeroPromises(Promise[] promises)
        {
            int zeroPromises = 0;
            for (int i = 0; i < promises.Count(); i++)
            {
                if (promises[i] == null) continue;
                if (promises[i].PromiseNumber == 0) zeroPromises++;
            }
            return zeroPromises;
        }

        private static int BiggestSuitsInHand(List<Card> myCardsInSuit, List<Card> playedCardsInSuit, Card trumpCard = null)
        {
            int retVal = 0;

            for (int i = 14; i > 1; i--)
            {
                if (myCardsInSuit.Any(x => (int)x.CardValue == i))
                {
                    retVal++;
                }
                else if ((trumpCard != null && (int)trumpCard.CardValue == i) || playedCardsInSuit.Any(x => (int)x.CardValue == i))
                {
                    continue;
                }
                else
                {
                    break;
                }
            }

            return retVal;
        }

        private static int KeepsAtLeastInSuit(List<Card> myCardsInSuit, List<Card> playedCardsInSuit, Card trumpCard = null)
        {
            // this method counts also if player has for example trum king and queen -> a least one keep

            // first take biggest trumps
            int retVal = BiggestSuitsInHand(myCardsInSuit, playedCardsInSuit, trumpCard);

            int counter = 14 - retVal;
            if (trumpCard.CardValue == CardValue.ace)
            {
                counter--;
            }

            int strike = 0;
            int gap = 0;

            // this loops starts from gap (or played card which leads to gap) because biggest trumps ends to gap
            for (int i = counter; i > 1; i--)
            {
                if (myCardsInSuit.Any(x => (int)x.CardValue == i))
                {
                    strike++;
                }
                else if ((trumpCard != null && (int)trumpCard.CardValue == i) || playedCardsInSuit.Any(x => (int)x.CardValue == i))
                {
                    continue;
                }
                else
                {
                    if (strike > gap)
                    {
                        retVal+= strike - gap;
                        strike = 0;
                        gap = 0;
                    }
                    gap++;
                }
            }
            if (strike > gap) retVal+= strike - gap;

            return retVal;
        }

        private static double NormalizeGoingOverOrUnder(double myPromise, double avgPoints, double goingOver, int playersLeft, bool debugPromise = false)
        {
            double changePromiseBy = 0.0;
            List<double> goingOverList = new List<double>();

            if (goingOver > 1.5 && myPromise > 1.5)
            {
                for (double i = 0; i < goingOver; i+= 0.5)
                {
                    for (double j = 0; j <= i; j+= 0.5)
                    {
                        goingOverList.Add(0);
                        goingOverList.Add(j);
                    }
                }
            }
            
            if (goingOver + (playersLeft * avgPoints) < -1.5 && myPromise > -1)
            {
                for (double i = 0; i > goingOver + (playersLeft * avgPoints); i-= 0.5)
                {
                    for (double j = 0; j >= i; j-= 0.5)
                    {
                        goingOverList.Add(0);
                        goingOverList.Add(j);
                    }
                }
            }
            
            if (myPromise > avgPoints + 2)
            {
                for (double i = 0; i < myPromise - avgPoints - 2; i+= 0.5)
                {
                    for (double j = 0; j <= i; j+= 0.5)
                    {
                        goingOverList.Add(0);
                        goingOverList.Add(j);
                    }
                }
            }

            if (goingOverList.Any()) changePromiseBy = goingOverList.OrderBy(x => rand.Next()).First() * -1;

            if (debugPromise)
            {
                Logger.Log($"myPromise: {String.Format("{0:0.00}", myPromise)}, avgPoints: {String.Format("{0:0.00}", avgPoints)}, goingOver: {String.Format("{0:0.00}", goingOver)}, playersLeft: {playersLeft}", "NormalizeGoingOverOrUnder");
                if (goingOverList.Any())
                {
                    var groupedList = goingOverList.GroupBy(x => x).Select(grp => new {muutos = grp.Key, lkm = grp.Count(), perc = (double)grp.Count()/(double)goingOverList.Count()}).ToList();
                    Logger.Log(JsonConvert.SerializeObject(groupedList) , "NormalizeGoingOverOrUnder");
                }
                Logger.Log($"changePromiseBy: {changePromiseBy}" , "NormalizeGoingOverOrUnder");
            }

            return changePromiseBy;
        }

        private static bool CountAsWinningCard(double probability)
        {
            if (probability <= 0) return false;
            if (probability >= 1) return true;
            if (rand.NextDouble() >= probability) return true;
            return false;
        }

        public class AnalyzedCard
        {
            public Card Card {get; set;}
            public bool CardIsTrump {get; set;}
            public double IsBiggest {get; set;}
            public double Skippable {get; set;}
            public double IsBiggestWithTrumps {get; set;}
            public bool CountAsWinningCard {get; set;}
            public bool CountAsSkippingCard {get; set;}
            public bool CountAsWinningCardWithTrumps {get; set;}

            public AnalyzedCard(PlayerAI ai, Card analyzeThisCard, List<Card> myHand, Card trumpCard, int playersInGame, List<Card> playedCards)
            {
                Card = analyzeThisCard;
                CardIsTrump = analyzeThisCard.CardSuit == trumpCard.CardSuit;

                int cardsInRound = myHand.Count();
                int cardsInDeck = 52 - 1 - (cardsInRound * playersInGame);
                int cardsInProbabilityDeck = 52 - 1 - cardsInRound;

                int otherPlayersCardCount = cardsInRound * (playersInGame - 1);

                double avgCardsEachSuitInHand = (double)cardsInRound / 4.0;

                /**/
                int biggerCards = 14 - (int)analyzeThisCard.CardValue;
                if (CardIsTrump && trumpCard.CardValue > analyzeThisCard.CardValue) biggerCards--;
                biggerCards-= myHand.Count(x => x.CardSuit == analyzeThisCard.CardSuit && x.CardValue > analyzeThisCard.CardValue);
                biggerCards-= playedCards.Count(x => x.CardSuit == analyzeThisCard.CardSuit && x.CardValue > analyzeThisCard.CardValue);

                IsBiggest = 1 - (double)biggerCards * ((double)cardsInDeck / (double)(cardsInDeck+otherPlayersCardCount));
                if (biggerCards > cardsInDeck)
                {
                    CountAsWinningCard = false;
                }
                else
                {
                    CountAsWinningCard = CountAsWinningCard(IsBiggest);
                }

                /**/
                int smallerCards = (int)analyzeThisCard.CardValue - 2;
                if (CardIsTrump && trumpCard.CardValue < analyzeThisCard.CardValue) smallerCards--;
                smallerCards-= myHand.Count(x => x.CardSuit == analyzeThisCard.CardSuit && x.CardValue < analyzeThisCard.CardValue);
                smallerCards-= playedCards.Count(x => x.CardSuit == analyzeThisCard.CardSuit && x.CardValue < analyzeThisCard.CardValue);

                double otherPlayersHaveSuit = avgCardsEachSuitInHand * (playersInGame - 1);

                Skippable = 1 - (double)smallerCards * ((double)cardsInDeck / (double)(cardsInDeck+otherPlayersCardCount));
                if (smallerCards < otherPlayersHaveSuit)
                {
                    // it is very likely that some one has bigger 
                    CountAsSkippingCard = true;
                }
                if (smallerCards > cardsInDeck)
                {
                    CountAsSkippingCard = false;
                }
                else
                {
                    CountAsSkippingCard = CountAsWinningCard(Skippable);
                }

                /**/
                int biggerCardsWithTrumps = 14 - (int)analyzeThisCard.CardValue;
                if (!CardIsTrump) biggerCardsWithTrumps+= 13;
                biggerCardsWithTrumps-= myHand.Count(x => x.CardSuit == analyzeThisCard.CardSuit && x.CardValue > analyzeThisCard.CardValue);
                biggerCardsWithTrumps-= playedCards.Count(x => x.CardSuit == analyzeThisCard.CardSuit && x.CardValue > analyzeThisCard.CardValue);
                if (CardIsTrump)
                {
                    if (trumpCard.CardValue > analyzeThisCard.CardValue) biggerCardsWithTrumps--;
                }
                else
                {
                    // trump card is not available in game
                    biggerCardsWithTrumps--;

                    // all trump cards are bigger than not trump
                    biggerCardsWithTrumps-= myHand.Count(x => x.CardSuit == trumpCard.CardSuit);
                    biggerCardsWithTrumps-= playedCards.Count(x => x.CardSuit == trumpCard.CardSuit);
                }

                if (biggerCardsWithTrumps > cardsInDeck)
                {
                    CountAsWinningCardWithTrumps = false;
                }
                else
                {
                    CountAsWinningCardWithTrumps = CountAsWinningCard(IsBiggestWithTrumps);
                }

            }
        }

        private static List<AnalyzedCard> ShadowPromises(PlayerAI ai, List<Card> myHand, Card trumpCard, int playersInGame, int[] promisesMade)
        {
            bool iAmFirst = promisesMade.Count() == 0;
            bool iAmLast = promisesMade.Count() == playersInGame - 1;

            int cardsInRound = myHand.Count();

            int cardsInDeck = 52 - 1 - (cardsInRound * playersInGame);

            List<AnalyzedCard> analyzedCards = new List<AnalyzedCard>();

            foreach (Card card in myHand)
            {
                analyzedCards.Add(new AnalyzedCard(ai, card, myHand, trumpCard, playersInGame, new List<Card>()));
            }

            // shadowPromises.Add("IsWinningCard", analyzedCards.Count(x => x.IsWinningCard()));
            // shadowPromises.Add("IsSkippingCard", analyzedCards.Count(x => x.IsSkippingCard()));
            // shadowPromises.Add("IsWinningCardWithTrumps", analyzedCards.Count(x => x.IsWinningCardWithTrumps()));

            return analyzedCards;
        }

        public static Promise MakePromise(PlayerAI ai, List<Card> hand, int playersInGame, Card trumpCard, Promise[] promises, bool debugPromise = false)
        {
            ScreenUtils.ClearDebugRows(debugPromise);
            int debugRow = ScreenUtils.PrintDebugRow(debugPromise, 0, "DEBUG");
            debugRow = ScreenUtils.PrintDebugRow(debugPromise, debugRow, $"ai.SmallValuesInSuit: {ai.SmallValuesInSuit}, ai.BigValuesInSuit: {ai.BigValuesInSuit}, ");

            // how many cards in hand in this round, 1-10
            int cardsInRound = hand.Count();

            // is this 5 or 15 points zero round
            bool smallZeroRound = cardsInRound <= 5;

            // how many players have made promises this far
            int playersPromised = PlayersPromised(promises);

            bool iAmFirst = playersPromised == 0;
            bool iAmLast = playersPromised == playersInGame - 1;

            // average how many points for every player
            double avgPoints = (double)cardsInRound / (double)playersInGame;

            // how many total cards are in hand in this game
            int cardsInGame = cardsInRound * playersInGame;

            // how many trumps are in this game
            double avgTrumpsInGame = ((double)cardsInGame / 4.0) - 1; // substract trump card

            double avgEachSuitInGame = ((double)cardsInGame / 4.0);
            double avgTrumpsAtPlayer = avgTrumpsInGame / (double)playersInGame;
            double avgEachSuitAtPlayer = avgEachSuitInGame / (double)playersInGame;

            // how many points promised this far, how many left for even game
            int promisesMade = PromisesMade(promises);
            int promisesLeft = cardsInRound - promisesMade;

            // is there bigger than average promises and zero promises
            int bigPromises = BigPromises(promises, avgPoints);
            int zeroPromises = ZeroPromises(promises);

            List<Card> suitC = hand.Where(x => x.CardSuit == CardSuit.Clubs).ToList();
            List<Card> suitD = hand.Where(x => x.CardSuit == CardSuit.Diamonds).ToList();
            List<Card> suitH = hand.Where(x => x.CardSuit == CardSuit.Hearts).ToList();
            List<Card> suitS = hand.Where(x => x.CardSuit == CardSuit.Spades).ToList();

            List<Card> myTrumps = hand.Where(x => x.CardSuit == trumpCard.CardSuit).ToList();
            int myTrumpCount = myTrumps.Count();
            int biggestTrumpsInHand = BiggestSuitsInHand(myTrumps, new List<Card>(), trumpCard);
            int smallerTrumpsInHand = myTrumps.Count() - biggestTrumpsInHand;

            AnalyzedSuit analyzedC = new AnalyzedSuit(ai, suitC, CardSuit.Clubs, CardSuit.Clubs == trumpCard.CardSuit, null, playersInGame, cardsInGame, iAmFirst);
            AnalyzedSuit analyzedD = new AnalyzedSuit(ai, suitD, CardSuit.Diamonds, CardSuit.Diamonds == trumpCard.CardSuit, null, playersInGame, cardsInGame, iAmFirst);
            AnalyzedSuit analyzedH = new AnalyzedSuit(ai, suitH, CardSuit.Hearts, CardSuit.Hearts == trumpCard.CardSuit, null, playersInGame, cardsInGame, iAmFirst);
            AnalyzedSuit analyzedS = new AnalyzedSuit(ai, suitS, CardSuit.Spades, CardSuit.Spades == trumpCard.CardSuit, null, playersInGame, cardsInGame, iAmFirst);

            AnalyzedSuit analyzedT = new AnalyzedSuit(ai, myTrumps, trumpCard.CardSuit, true, null, playersInGame, cardsInGame, iAmFirst);

            int promisesAtLeast = KeepsAtLeastInSuit(myTrumps, new List<Card>(), trumpCard);

            List<AnalyzedCard> analyzedCards = ShadowPromises(ai, hand, trumpCard, playersInGame, promises.Where(x => x != null).Select(y => y.PromiseNumber).ToArray());
            int shadowPromiseIsWinningCard = analyzedCards.Count(x => x.CountAsWinningCardWithTrumps && x.Card.CardSuit != trumpCard.CardSuit);
            int shadowPromiseIsTrumpWinningCard = analyzedCards.Count(x => x.CountAsWinningCard && x.Card.CardSuit == trumpCard.CardSuit);
            debugRow = ScreenUtils.PrintDebugRow(debugPromise, debugRow, $"shadowPromiseIsWinningCard: {shadowPromiseIsWinningCard}");
            debugRow = ScreenUtils.PrintDebugRow(debugPromise, debugRow, $"shadowPromiseIsTrumpWinningCard: {shadowPromiseIsTrumpWinningCard}");

            // this is a fact
            double myPromise = promisesAtLeast;
            debugRow = ScreenUtils.PrintDebugRow(debugPromise, debugRow, $"promisesAtLeast: {promisesAtLeast}");
            bool playZero = false;

            double averageSuitMultiplier = Math.Sqrt(avgEachSuitAtPlayer); //
            debugRow = ScreenUtils.PrintDebugRow(debugPromise, debugRow, $"averageSuitMultiplier: {String.Format("{0:0.00}", averageSuitMultiplier)}");

            double promiseMultiplier = ai.PromiseMultiplierBase1; // base multiplier when analyzing biggest cards in my hand
            if (iAmFirst) promiseMultiplier+= ai.PromiseMultiplierChange1A; // first player has advantage in round
            if (iAmLast) promiseMultiplier+= ai.PromiseMultiplierChange1B; // last player has advantage in round
            promiseMultiplier+= zeroPromises * ai.PromiseMultiplierChange1C; // if there are zero promises it is more likely to get bigger points
            promiseMultiplier*= averageSuitMultiplier;
            debugRow = ScreenUtils.PrintDebugRow(debugPromise, debugRow, $"promiseMultiplier: {String.Format("{0:0.00}", promiseMultiplier)}");

            if (trumpCard.CardSuit != CardSuit.Clubs) myPromise+= analyzedC.BiggestValuesInSuit * promiseMultiplier;
            if (trumpCard.CardSuit != CardSuit.Diamonds) myPromise+= analyzedD.BiggestValuesInSuit * promiseMultiplier;
            if (trumpCard.CardSuit != CardSuit.Hearts) myPromise+= analyzedH.BiggestValuesInSuit * promiseMultiplier;
            if (trumpCard.CardSuit != CardSuit.Spades) myPromise+= analyzedS.BiggestValuesInSuit * promiseMultiplier;
            debugRow = ScreenUtils.PrintDebugRow(debugPromise, debugRow, $"myPromise A: {String.Format("{0:0.00}", myPromise)}");
            
            // promiseMultiplier = ai.PromiseMultiplierBase2; // base multiplier when analyzing smallest cards in my hand
            // if (iAmFirst) promiseMultiplier+= ai.PromiseMultiplierChange2A;
            // if (iAmLast) promiseMultiplier+= ai.PromiseMultiplierChange2B;
            // promiseMultiplier+= bigPromises * ai.PromiseMultiplierChange2C;

            // if (trumpCard.CardSuit != CardSuit.Clubs) myPromise-= analyzedC.SmallestValuesInSuit * averageSuitMultiplier * promiseMultiplier;
            // if (trumpCard.CardSuit != CardSuit.Diamonds) myPromise-= analyzedD.SmallestValuesInSuit * averageSuitMultiplier * promiseMultiplier;
            // if (trumpCard.CardSuit != CardSuit.Hearts) myPromise-= analyzedH.SmallestValuesInSuit * averageSuitMultiplier * promiseMultiplier;
            // if (trumpCard.CardSuit != CardSuit.Spades) myPromise-= analyzedS.SmallestValuesInSuit * averageSuitMultiplier * promiseMultiplier;


            // promiseMultiplier = ai.PromiseMultiplierBase3; // base multiplier when analyzing rest of big cards in my hand
            // if (iAmFirst) promiseMultiplier+= ai.PromiseMultiplierChange3A;
            // if (iAmLast) promiseMultiplier+= ai.PromiseMultiplierChange3B;
            // promiseMultiplier+= zeroPromises * ai.PromiseMultiplierChange3C; // if there are zero promises it is more likely to get bigger points

            // if (trumpCard.CardSuit != CardSuit.Clubs) myPromise+= (analyzedC.BigValuesInSuit - analyzedC.BiggestValuesInSuit) * averageSuitMultiplier * promiseMultiplier;
            // if (trumpCard.CardSuit != CardSuit.Diamonds) myPromise+= (analyzedD.BigValuesInSuit - analyzedD.BiggestValuesInSuit) * averageSuitMultiplier * promiseMultiplier;
            // if (trumpCard.CardSuit != CardSuit.Hearts) myPromise+= (analyzedH.BigValuesInSuit - analyzedH.BiggestValuesInSuit) * averageSuitMultiplier * promiseMultiplier;
            // if (trumpCard.CardSuit != CardSuit.Spades) myPromise+= (analyzedS.BigValuesInSuit - analyzedS.BiggestValuesInSuit) * averageSuitMultiplier * promiseMultiplier;

            
            // promiseMultiplier = ai.PromiseMultiplierBase4; // base multiplier when analyzing rest of small cards in my hand
            // if (iAmFirst) promiseMultiplier+= ai.PromiseMultiplierChange4A;
            // if (iAmLast) promiseMultiplier+= ai.PromiseMultiplierChange4B;
            // promiseMultiplier+= bigPromises * ai.PromiseMultiplierChange4C;

            // if (trumpCard.CardSuit != CardSuit.Clubs) myPromise-= (analyzedC.SmallValuesInSuit - analyzedC.SmallestValuesInSuit) * averageSuitMultiplier * promiseMultiplier;
            // if (trumpCard.CardSuit != CardSuit.Diamonds) myPromise-= (analyzedD.SmallValuesInSuit - analyzedD.SmallestValuesInSuit) * averageSuitMultiplier * promiseMultiplier;
            // if (trumpCard.CardSuit != CardSuit.Hearts) myPromise-= (analyzedH.SmallValuesInSuit - analyzedH.SmallestValuesInSuit) * averageSuitMultiplier * promiseMultiplier;
            // if (trumpCard.CardSuit != CardSuit.Spades) myPromise-= (analyzedS.SmallValuesInSuit - analyzedS.SmallestValuesInSuit) * averageSuitMultiplier * promiseMultiplier;

            if (promisesAtLeast == 0)
            {
                debugRow = ScreenUtils.PrintDebugRow(debugPromise, debugRow, $"myTrumpCount: {myTrumpCount}, avgTrumpsAtPlayer: {avgTrumpsAtPlayer}, analyzedT.BigValuesInSuit: {analyzedT.BigValuesInSuit}, analyzedT.SmallValuesInSuit: {analyzedT.SmallValuesInSuit}");
                // should play zero?
                if (myTrumpCount < avgTrumpsAtPlayer && analyzedT.BigValuesInSuit <= analyzedT.SmallValuesInSuit)
                {
                    // i have less trumps than averarage player and more smaller trumps than big ones
                    int miniRisk = smallZeroRound ? ai.MiniRisk : 0; // when playing small zero round it may be wiser promise something else than zero
                    debugRow = ScreenUtils.PrintDebugRow(debugPromise, debugRow, $"miniRisk: {miniRisk}");
                    debugRow = ScreenUtils.PrintDebugRow(debugPromise, debugRow, $"analyzedC.IsDodgeable: {analyzedC.IsDodgeable}, analyzedD.IsDodgeable: {analyzedD.IsDodgeable}, analyzedH.IsDodgeable: {analyzedH.IsDodgeable}, analyzedS.IsDodgeable: {analyzedS.IsDodgeable}");
                    // very likely zero
                    if (CheckRandom(analyzedC.IsDodgeable - miniRisk)
                        && CheckRandom(analyzedD.IsDodgeable - miniRisk)
                        && CheckRandom(analyzedH.IsDodgeable - miniRisk)
                        && CheckRandom(analyzedS.IsDodgeable - miniRisk)
                    )
                    {
                        playZero = true;
                        debugRow = ScreenUtils.PrintDebugRow(debugPromise, debugRow, $"playZero: {playZero}");
                    }
                }
            }

            if (myTrumpCount - promisesAtLeast - analyzedT.BigValuesInSuit > avgTrumpsAtPlayer)
            {
                //myPromise+= myTrumpCount - promisesAtLeast - analyzedT.BigValuesInSuit;
                myPromise+= myTrumpCount - avgTrumpsAtPlayer;
                debugRow = ScreenUtils.PrintDebugRow(debugPromise, debugRow, $"myPromise B: {String.Format("{0:0.00}", myPromise)}");
            }
            else if (myTrumpCount - promisesAtLeast > avgTrumpsAtPlayer)
            {
                if (analyzedT.BigValuesInSuit > 0)
                {
                    myPromise+= analyzedT.BigValuesInSuit - promisesAtLeast;
                    debugRow = ScreenUtils.PrintDebugRow(debugPromise, debugRow, $"myPromise C: {String.Format("{0:0.00}", myPromise)}");
                }
            }
            else if (myTrumpCount > avgTrumpsAtPlayer)
            {
                double trumpChange = myTrumpCount - avgTrumpsAtPlayer;
                int minTrumpChange = (int)trumpChange;
                int maxTrumpChange = minTrumpChange + 1;
                int randTest = (int)((1 + trumpChange - maxTrumpChange) * 100);
                if (randTest == 0)
                {
                    myPromise+= minTrumpChange;
                }
                else
                {
                    myPromise+= (CheckRandom(randTest)) ? maxTrumpChange : minTrumpChange;
                }
                debugRow = ScreenUtils.PrintDebugRow(debugPromise, debugRow, $"myPromise D: {String.Format("{0:0.00}", myPromise)}");
            }
            else if (analyzedT.BigValuesInSuit > 0 && myPromise < 1)
            {
                myPromise++;
                debugRow = ScreenUtils.PrintDebugRow(debugPromise, debugRow, $"myPromise E: {String.Format("{0:0.00}", myPromise)}");
            }


            
            double goingOver = (promisesMade + myPromise) - cardsInRound;
            debugRow = ScreenUtils.PrintDebugRow(debugPromise, debugRow, $"goingOver: {String.Format("{0:0.00}", goingOver)}");
            double normalizing = NormalizeGoingOverOrUnder(myPromise, avgPoints, goingOver, (playersInGame - playersPromised - 1), debugPromise);
            debugRow = ScreenUtils.PrintDebugRow(debugPromise, debugRow, $"normalizing: {String.Format("{0:0.00}", normalizing)}");
            myPromise+= normalizing;
            debugRow = ScreenUtils.PrintDebugRow(debugPromise, debugRow, $"myPromise F: {String.Format("{0:0.00}", myPromise)}");

            int finalPromise;
            if (myPromise <= 0 || playZero)
            {
                finalPromise = 0;
            }
            else
            {
                int minPromise = (int)myPromise;
                int maxPromise = minPromise + 1;
                int randTest = (int)((1 + myPromise - maxPromise) * 100);
                if (randTest == 0)
                {
                    finalPromise = minPromise;
                }
                else
                {
                    // myPromise 2.3 => min 2, max 3, => randTest 30% to get max
                    finalPromise = (CheckRandom(randTest)) ? maxPromise : minPromise;
                }
            }
            
            if (finalPromise != shadowPromiseIsWinningCard + Math.Max(promisesAtLeast, shadowPromiseIsTrumpWinningCard))
            {
                // finalPromise = shadowPromiseIsWinningCard + Math.Max(promisesAtLeast, shadowPromiseIsTrumpWinningCard);
            }

            // last check - do not promise under your biggest trumps!
            if (finalPromise < Math.Max(promisesAtLeast, shadowPromiseIsTrumpWinningCard)) finalPromise = Math.Max(promisesAtLeast, shadowPromiseIsTrumpWinningCard);

            if (finalPromise > cardsInRound) finalPromise = cardsInRound;


            return new Promise(finalPromise);
        }

        private static bool CardWillWin(Card cardToCheck, Card cardInCharge, List<Card> cardsInTable, CardSuit trumpSuit)
        {
            if (!cardsInTable.Any() || cardInCharge == null) return false;
            cardsInTable.RemoveAll(x => x == null);
            Card winningCard = (cardsInTable.Any(x => x.CardSuit == trumpSuit))
                        ? cardsInTable.Where(x => x.CardSuit == trumpSuit).OrderByDescending(x => x.CardValue).First()
                        : cardsInTable.Where(x => x.CardSuit == cardInCharge.CardSuit).OrderByDescending(x => x.CardValue).First();
            
            if (winningCard.CardSuit == trumpSuit)
            {
                return (cardToCheck.CardSuit == trumpSuit && cardToCheck.CardValue >= winningCard.CardValue);
            }
            else
            {
                return (cardToCheck.CardSuit == trumpSuit || (cardToCheck.CardSuit == winningCard.CardSuit && cardToCheck.CardValue >= winningCard.CardValue));
            }
        }

        private static int CardsPlayed(Card[] cards)
        {
            int retVal = 0;
            for (int i = 0; i < cards.Count(); i++)
            {
                if (cards[i] != null) retVal++;
            }
            return retVal;
        }

        private static int GetCardIndexFromHand(List<Card> hand, Card selectedCard)
        {
            int index = 0;
            foreach (Card card in hand)
            {
                if (card.CardSuit == selectedCard.CardSuit && card.CardValue == selectedCard.CardValue) return index;
                index++;
            }

            return 0;
        }

        private static List<Card> WinningCards(List<Card> cards, Card cardInCharge, List<Card> tableCards, CardSuit trumpSuit)
        {
            List<Card> winningCards = new List<Card>();
            foreach (Card card in cards)
            {
                if (CardWillWin(card, cardInCharge, tableCards, trumpSuit)) winningCards.Add(card);
            }

            return winningCards;
        }

        private static List<Card> LosingCards(List<Card> cards, Card cardInCharge, List<Card> tableCards, CardSuit trumpSuit)
        {
            List<Card> losingCards = new List<Card>();
            foreach (Card card in cards)
            {
                if (!CardWillWin(card, cardInCharge, tableCards, trumpSuit)) losingCards.Add(card);
            }

            return losingCards;
        }

        private static List<CardSuit> SureSuits(int playerInd, PlayerInfo[] playerInfos)
        {
            List<CardSuit> sureSuits = new List<CardSuit>();

            bool isSure = true;
            for (int i = 0; i < playerInfos.Count(); i++)
            {
                if (i == playerInd) continue;
                if (playerInfos[i].HasClubs) isSure = false;
            }
            if (isSure) sureSuits.Add(CardSuit.Clubs);

            isSure = true;
            for (int i = 0; i < playerInfos.Count(); i++)
            {
                if (i == playerInd) continue;
                if (playerInfos[i].HasDiamonds) isSure = false;
            }
            if (isSure) sureSuits.Add(CardSuit.Diamonds);

            isSure = true;
            for (int i = 0; i < playerInfos.Count(); i++)
            {
                if (i == playerInd) continue;
                if (playerInfos[i].HasHearts) isSure = false;
            }
            if (isSure) sureSuits.Add(CardSuit.Hearts);

            isSure = true;
            for (int i = 0; i < playerInfos.Count(); i++)
            {
                if (i == playerInd) continue;
                if (playerInfos[i].HasSpades) isSure = false;
            }
            if (isSure) sureSuits.Add(CardSuit.Spades);

            return sureSuits;
        }

        private static CardSuit ChooseDodgeSuit(List<AnalyzedSuit> analyzedSuits, int counter = 0)
        {
            if (counter > 5) return analyzedSuits.OrderByDescending(x => x.IsDodgeable).First().Suit;
            foreach (AnalyzedSuit analyzedSuit in analyzedSuits.OrderByDescending(x => x.IsDodgeable))
            {
                if (CheckRandom(analyzedSuit.IsDodgeable)) return analyzedSuit.Suit;
            }

            return ChooseDodgeSuit(analyzedSuits, ++counter);
        }

        private static CardSuit ChooseDiffultiestDodgeSuit(List<AnalyzedSuit> analyzedSuits, int counter = 0)
        {
            if (counter > 5) return analyzedSuits.OrderBy(x => x.IsDodgeable).First().Suit;
            foreach (AnalyzedSuit analyzedSuit in analyzedSuits.OrderBy(x => x.IsDodgeable))
            {
                if (CheckRandom(100 - analyzedSuit.IsDodgeable)) return analyzedSuit.Suit;
            }

            return ChooseDiffultiestDodgeSuit(analyzedSuits, ++counter);
        }

        private static int[] GetOtherPlayerStatus(int playerInd, Promise[] promises, int[] roundWins)
        {
            int[] otherPlayerStatus = new int[promises.Count()];
            for (int i = 0; i < promises.Count(); i++)
            {
                if (i == playerInd) continue;
                otherPlayerStatus[i] = roundWins[i] - promises[i].PromiseNumber;
            }
            return otherPlayerStatus;
        }

        public static int PlayCard(PlayerAI ai, int playerInd, List<Card> hand, Card cardInCharge, Card trumpCard, Card[] tableCards, int cardsInRound, Promise[] promises, int[] roundWins, List<Card> cardsPlayedInRounds, PlayerInfo[] playerInfos)
        {
            //Logger.Log("PlayCard", "test");

            PlayingMethod myMethod = PlayingMethod.NOTSET;

            int playersInGame = promises.Count();
            int cardsInGame = playersInGame * cardsInRound;
            // int currentRound = roundWins.Sum() + 1;

            // how many trumps are in this game
            double avgTrumpsInGame = ((double)cardsInGame / 4.0) - 1; // substract trump card

            double avgEachSuitInGame = ((double)cardsInGame / 4.0);
            double avgTrumpsAtPlayer = avgTrumpsInGame / (double)playersInGame;
            double avgEachSuitAtPlayer = avgEachSuitInGame / (double)playersInGame;

            int myPromises = promises[playerInd].PromiseNumber;
            int myCurrentWins = roundWins[playerInd];
            
            // 0 = this is good, no more wins
            // negative = have to take wins
            // positive = over
            int myPromiseStatus = myCurrentWins - myPromises;

            int[] otherPlayerStatus = GetOtherPlayerStatus(playerInd, promises, roundWins);

            int roundsLeft = cardsInRound - roundWins.Sum();
            bool firstRound = roundsLeft == cardsInRound;
            bool lastRound = roundsLeft == cardsInRound - 1;
            
            int cardsPlayed = CardsPlayed(tableCards);

            bool iAmFirst = cardsPlayed == 0;
            bool iAmLast = cardsPlayed == tableCards.Count() - 1;

            List<Card> suitC = hand.Where(x => x.CardSuit == CardSuit.Clubs).ToList();
            List<Card> suitD = hand.Where(x => x.CardSuit == CardSuit.Diamonds).ToList();
            List<Card> suitH = hand.Where(x => x.CardSuit == CardSuit.Hearts).ToList();
            List<Card> suitS = hand.Where(x => x.CardSuit == CardSuit.Spades).ToList();

            List<Card> myTrumps = hand.Where(x => x.CardSuit == trumpCard.CardSuit).ToList();
            int myTrumpCount = myTrumps.Count();
            int biggestTrumpsInHand = BiggestSuitsInHand(myTrumps, cardsPlayedInRounds.Where(x => x.CardSuit == trumpCard.CardSuit).ToList(), trumpCard);
            int smallerTrumpsInHand = myTrumps.Count() - biggestTrumpsInHand;
            int sureTrumpCount = KeepsAtLeastInSuit(myTrumps, cardsPlayedInRounds.Where(x => x.CardSuit == trumpCard.CardSuit).ToList(), trumpCard);

            AnalyzedSuit analyzedC = new AnalyzedSuit(ai, suitC, CardSuit.Clubs, CardSuit.Clubs == trumpCard.CardSuit, cardsPlayedInRounds, playersInGame, cardsInGame, iAmFirst);
            AnalyzedSuit analyzedD = new AnalyzedSuit(ai, suitD, CardSuit.Diamonds, CardSuit.Diamonds == trumpCard.CardSuit, cardsPlayedInRounds, playersInGame, cardsInGame, iAmFirst);
            AnalyzedSuit analyzedH = new AnalyzedSuit(ai, suitH, CardSuit.Hearts, CardSuit.Hearts == trumpCard.CardSuit, cardsPlayedInRounds, playersInGame, cardsInGame, iAmFirst);
            AnalyzedSuit analyzedS = new AnalyzedSuit(ai, suitS, CardSuit.Spades, CardSuit.Spades == trumpCard.CardSuit, cardsPlayedInRounds, playersInGame, cardsInGame, iAmFirst);

            AnalyzedSuit analyzedT = myTrumps.Any() ? new AnalyzedSuit(ai, suitS, trumpCard.CardSuit, true, cardsPlayedInRounds, playersInGame, cardsInGame, iAmFirst) : null;

            List<CardSuit> sureSuits = SureSuits(playerInd, playerInfos);

            if (myPromiseStatus + sureTrumpCount > 0)
            {
                // pitkäksi oy:stä päivää
                // try to harm other players
                myMethod = PlayingMethod.TRYTOROB;
                if (!otherPlayerStatus.Any(x => x > 0) && otherPlayerStatus.Sum() + roundsLeft == 0)
                {
                    // all other players are happy if i take all - so skip instead
                    myMethod = PlayingMethod.TRYTOSKIP;
                }
                if (otherPlayerStatus.Sum() + roundsLeft > 0)
                {
                    // other player(s) will go over if i don't take - so skip
                    myMethod = PlayingMethod.TRYTOSKIP;
                }
            }
            else if (myPromiseStatus + roundsLeft < 0)
            {
                // can't get promise anymore
                myMethod = PlayingMethod.TRYTOSKIP;
                if (otherPlayerStatus.Sum() + roundsLeft == 0)
                {
                    // other players try to take all rest, do harm
                    myMethod = PlayingMethod.TRYTOROB;
                }
            }
            else
            {
                // i'm still in game
                myMethod = (myPromises == myCurrentWins) ? PlayingMethod.TRYTODODGE : PlayingMethod.TRYTOWIN;
                if (myPromiseStatus + sureTrumpCount == 0 && roundsLeft > sureTrumpCount)
                {
                    // no need to take yet, it is also possible to skip
                    myMethod = PlayingMethod.SKIPSAFE;
                }
                if (myPromiseStatus + roundsLeft == 0)
                {
                    myMethod = PlayingMethod.TAKEREST;
                }
            }

            int selectedCardInd = 0;
            Card selectedCard;

            if (iAmFirst)
            {
                if (myMethod == PlayingMethod.TRYTOROB || myMethod == PlayingMethod.TRYTOWIN || myMethod == PlayingMethod.TAKEREST)
                {
                    if (avgEachSuitAtPlayer >= 1)
                    {
                        List<Card> biggestCards = new List<Card>();
                        if (trumpCard.CardSuit != CardSuit.Hearts) biggestCards.AddRange(suitH.Skip(suitH.Count() - analyzedH.BiggestValuesInSuit));
                        if (trumpCard.CardSuit != CardSuit.Diamonds) biggestCards.AddRange(suitD.Skip(suitD.Count() - analyzedD.BiggestValuesInSuit));
                        if (trumpCard.CardSuit != CardSuit.Clubs) biggestCards.AddRange(suitC.Skip(suitC.Count() - analyzedC.BiggestValuesInSuit));
                        if (trumpCard.CardSuit != CardSuit.Spades) biggestCards.AddRange(suitS.Skip(suitD.Count() - analyzedS.BiggestValuesInSuit));
                        if (biggestCards.Any()) return GetCardIndexFromHand(hand, biggestCards.OrderBy(x => rand.Next()).First());
                    }

                    if (biggestTrumpsInHand > 0)
                    {
                        return GetCardIndexFromHand(hand, myTrumps.Skip(myTrumpCount - biggestTrumpsInHand).First());
                    }
                    else if (myTrumps.Any())
                    {
                        return GetCardIndexFromHand(hand, myTrumps.OrderByDescending(x => x.CardValue).First());
                    }
                    else
                    {
                        return GetCardIndexFromHand(hand, hand.OrderByDescending(x => x.CardValue).First());
                    }
                }
                else
                {
                    // i should now avoid to win rounds

                    // don't play suresuits if possible
                    List<Card> betterCards = sureSuits.Any() && hand.Any(x => !sureSuits.Contains(x.CardSuit))
                                            ? hand.Where(x => !sureSuits.Contains(x.CardSuit)).ToList()
                                            : hand;
                    
                    List<CardSuit> betterSuits =  betterCards.Select(x => x.CardSuit).Distinct().ToList();
                    List<AnalyzedSuit> betterAnalyzedSuits = new List<AnalyzedSuit>();
                    foreach (CardSuit cardSuit in betterSuits)
                    {
                        switch (cardSuit)
                        {
                            case CardSuit.Hearts: betterAnalyzedSuits.Add(analyzedH); break;
                            case CardSuit.Diamonds: betterAnalyzedSuits.Add(analyzedD); break;
                            case CardSuit.Spades: betterAnalyzedSuits.Add(analyzedS); break;
                            case CardSuit.Clubs: betterAnalyzedSuits.Add(analyzedC); break;
                        }
                    }

                    CardSuit dodgeSuit = ChooseDodgeSuit(betterAnalyzedSuits);
                    betterCards = betterCards.Where(x => x.CardSuit == dodgeSuit).ToList();

                    selectedCardInd = rand.Next(betterCards.Count());
                    selectedCard = betterCards.Skip(selectedCardInd).First();
                    return GetCardIndexFromHand(hand, selectedCard);
                    
                }
            }
            else
            {
                // i'm not in charge in this round

                List<Card> possibleCards = (hand.Any(x => x.CardSuit == cardInCharge.CardSuit))
                                            ? hand.Where(x => x.CardSuit == cardInCharge.CardSuit).ToList()
                                            : hand;

                // this is just a default card
                selectedCardInd = rand.Next(possibleCards.Count());
                selectedCard = possibleCards.Skip(selectedCardInd).First();

                List<Card> losingCards = LosingCards(possibleCards, cardInCharge, tableCards.ToList(), trumpCard.CardSuit);
                List<Card> winningCards = WinningCards(possibleCards, cardInCharge, tableCards.ToList(), trumpCard.CardSuit);

                if (myMethod == PlayingMethod.TRYTODODGE || myMethod == PlayingMethod.TRYTOSKIP)
                {
                    // do everything to avoid getting win

                    if (losingCards.Any())
                    {
                        // take losing card that is difficultiest to dodge
                        List<AnalyzedSuit> betterAnalyzedSuits = new List<AnalyzedSuit>();
                        List<CardSuit> betterSuits =  losingCards.Select(x => x.CardSuit).Distinct().ToList();
                        foreach (CardSuit cardSuit in betterSuits)
                        {
                            switch (cardSuit)
                            {
                                case CardSuit.Hearts: betterAnalyzedSuits.Add(analyzedH); break;
                                case CardSuit.Diamonds: betterAnalyzedSuits.Add(analyzedD); break;
                                case CardSuit.Spades: betterAnalyzedSuits.Add(analyzedS); break;
                                case CardSuit.Clubs: betterAnalyzedSuits.Add(analyzedC); break;
                            }
                        }

                        CardSuit dodgeSuit = ChooseDiffultiestDodgeSuit(betterAnalyzedSuits);
                        losingCards = losingCards.Where(x => x.CardSuit == dodgeSuit).ToList();
                        
                        selectedCardInd = rand.Next(losingCards.Count());
                        selectedCard = losingCards.Skip(selectedCardInd).First();
                    }
                    else
                    {
                        // take card from easy dodge
                        List<AnalyzedSuit> betterAnalyzedSuits = new List<AnalyzedSuit>();
                        List<CardSuit> betterSuits =  possibleCards.Select(x => x.CardSuit).Distinct().ToList();
                        foreach (CardSuit cardSuit in betterSuits)
                        {
                            switch (cardSuit)
                            {
                                case CardSuit.Hearts: betterAnalyzedSuits.Add(analyzedH); break;
                                case CardSuit.Diamonds: betterAnalyzedSuits.Add(analyzedD); break;
                                case CardSuit.Spades: betterAnalyzedSuits.Add(analyzedS); break;
                                case CardSuit.Clubs: betterAnalyzedSuits.Add(analyzedC); break;
                            }
                        }

                        CardSuit dodgeSuit = ChooseDodgeSuit(betterAnalyzedSuits);
                        possibleCards = possibleCards.Where(x => x.CardSuit == dodgeSuit).ToList();
                        
                        selectedCardInd = rand.Next(possibleCards.Count());
                        selectedCard = possibleCards.Skip(selectedCardInd).First();
                    }
                }
                else
                {
                    // we should win some round in this game, but possibly not now...

                    if (winningCards.Any() && myMethod == PlayingMethod.TAKEREST)
                    {
                        // we have to win all rest rounds
                        // start using possible smallest winning card
                        if (iAmLast) return GetCardIndexFromHand(hand, winningCards.OrderBy(x => x.CardValue).First());
                        // otherwise take random
                        return GetCardIndexFromHand(hand, winningCards.OrderBy(x => rand.Next()).First());
                    }
                    if (winningCards.Any())
                    {
                        if (myMethod == PlayingMethod.SKIPSAFE && losingCards.Any())
                        {
                            // no need to take yet, i have enough biggest trumps in my hand
                            return GetCardIndexFromHand(hand, losingCards.OrderByDescending(x => x.CardValue).First());
                        }
                        if (myMethod == PlayingMethod.SKIPSAFE)
                        {
                            // no need to take yet, i have enough biggest trumps in my hand
                            return GetCardIndexFromHand(hand, possibleCards.OrderBy(x => x.CardValue).First());
                        }

                        if (otherPlayerStatus.Sum() + roundsLeft < 0)
                        {
                            // maybe it's better to take now
                            if (rand.Next(100) > 30)
                            {
                                selectedCardInd = rand.Next(winningCards.Count());
                                selectedCard = winningCards.Skip(selectedCardInd).First();
                            }
                        }
                    }
                }
                
                return GetCardIndexFromHand(hand, selectedCard);
            }
        }

    }
}