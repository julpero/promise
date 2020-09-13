using System;
using DSI.Deck;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

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
        public PlayerAI(string guidStr, PlayerAI bestAi, PlayerAI goodAi, bool average = false)
        {
            AiName = Guid.NewGuid().ToString();

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
                PromiseMultiplierBase2 = AverageDouble(bestAi.PromiseMultiplierBase2, goodAi.PromiseMultiplierBase2); // 0.2;
                PromiseMultiplierBase3 = AverageDouble(bestAi.PromiseMultiplierBase3, goodAi.PromiseMultiplierBase3); // 0.3;
                PromiseMultiplierBase4 = AverageDouble(bestAi.PromiseMultiplierBase4, goodAi.PromiseMultiplierBase4); // 0.25;
                PromiseMultiplierChange1A = AverageDouble(bestAi.PromiseMultiplierChange1A, goodAi.PromiseMultiplierChange1A); // 0.3;
                PromiseMultiplierChange1B = AverageDouble(bestAi.PromiseMultiplierChange1B, goodAi.PromiseMultiplierChange1B); // 0.15;
                PromiseMultiplierChange1C = AverageDouble(bestAi.PromiseMultiplierChange1C, goodAi.PromiseMultiplierChange1C); // 0.1;
                PromiseMultiplierChange2A = AverageDouble(bestAi.PromiseMultiplierChange2A, goodAi.PromiseMultiplierChange2A); // 0.1;
                PromiseMultiplierChange2B = AverageDouble(bestAi.PromiseMultiplierChange2B, goodAi.PromiseMultiplierChange2B); // 0.05;
                PromiseMultiplierChange2C = AverageDouble(bestAi.PromiseMultiplierChange2C, goodAi.PromiseMultiplierChange2C); // 0.1;
                PromiseMultiplierChange3A = AverageDouble(bestAi.PromiseMultiplierChange3A, goodAi.PromiseMultiplierChange3A); // 0.4;
                PromiseMultiplierChange3B = AverageDouble(bestAi.PromiseMultiplierChange3B, goodAi.PromiseMultiplierChange3B); // 0.25;
                PromiseMultiplierChange3C = AverageDouble(bestAi.PromiseMultiplierChange3C, goodAi.PromiseMultiplierChange3C); // 0.1;
                PromiseMultiplierChange4A = AverageDouble(bestAi.PromiseMultiplierChange4A, goodAi.PromiseMultiplierChange4A); // 0.1;
                PromiseMultiplierChange4B = AverageDouble(bestAi.PromiseMultiplierChange4B, goodAi.PromiseMultiplierChange4B); // 0.05;
                PromiseMultiplierChange4C = AverageDouble(bestAi.PromiseMultiplierChange4C, goodAi.PromiseMultiplierChange4C); // 0.1;
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
                PromiseMultiplierBase2 = KeepAiValue() ? bestAi.PromiseMultiplierBase2 : goodAi.PromiseMultiplierBase2; // 0.2;
                PromiseMultiplierBase3 = KeepAiValue() ? bestAi.PromiseMultiplierBase3 : goodAi.PromiseMultiplierBase3; // 0.3;
                PromiseMultiplierBase4 = KeepAiValue() ? bestAi.PromiseMultiplierBase4 : goodAi.PromiseMultiplierBase4; // 0.25;
                PromiseMultiplierChange1A = KeepAiValue() ? bestAi.PromiseMultiplierChange1A : goodAi.PromiseMultiplierChange1A; // 0.3;
                PromiseMultiplierChange1B = KeepAiValue() ? bestAi.PromiseMultiplierChange1B : goodAi.PromiseMultiplierChange1B; // 0.15;
                PromiseMultiplierChange1C = KeepAiValue() ? bestAi.PromiseMultiplierChange1C : goodAi.PromiseMultiplierChange1C; // 0.1;
                PromiseMultiplierChange2A = KeepAiValue() ? bestAi.PromiseMultiplierChange2A : goodAi.PromiseMultiplierChange2A; // 0.1;
                PromiseMultiplierChange2B = KeepAiValue() ? bestAi.PromiseMultiplierChange2B : goodAi.PromiseMultiplierChange2B; // 0.05;
                PromiseMultiplierChange2C = KeepAiValue() ? bestAi.PromiseMultiplierChange2C : goodAi.PromiseMultiplierChange2C; // 0.1;
                PromiseMultiplierChange3A = KeepAiValue() ? bestAi.PromiseMultiplierChange3A : goodAi.PromiseMultiplierChange3A; // 0.4;
                PromiseMultiplierChange3B = KeepAiValue() ? bestAi.PromiseMultiplierChange3B : goodAi.PromiseMultiplierChange3B; // 0.25;
                PromiseMultiplierChange3C = KeepAiValue() ? bestAi.PromiseMultiplierChange3C : goodAi.PromiseMultiplierChange3C; // 0.1;
                PromiseMultiplierChange4A = KeepAiValue() ? bestAi.PromiseMultiplierChange4A : goodAi.PromiseMultiplierChange4A; // 0.1;
                PromiseMultiplierChange4B = KeepAiValue() ? bestAi.PromiseMultiplierChange4B : goodAi.PromiseMultiplierChange4B; // 0.05;
                PromiseMultiplierChange4C = KeepAiValue() ? bestAi.PromiseMultiplierChange4C : goodAi.PromiseMultiplierChange4C; // 0.1;
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
            if (randomAi.NextDouble() > 0.98) MiniRisk = randomAi.Next(0, 50); // 5;
        }

        public PlayerAI(string guidStr)
        {
            AiName = guidStr;

            DodgeBase = randomAi.Next(30, 90); // 50
            DodgeSure = 100; // this is a fact
            DodgeSmallestValuesInSuit = randomAi.Next(30, 101); // 95
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
            PromiseMultiplierBase2 = GetRandomNumber(0.0, 1.0); // 0.2;
            PromiseMultiplierBase3 = GetRandomNumber(0.0, 1.0); // 0.3;
            PromiseMultiplierBase4 = GetRandomNumber(0.0, 1.0); // 0.25;
            PromiseMultiplierChange1A = GetRandomNumber(0.0, 1.0); // 0.3;
            PromiseMultiplierChange1B = GetRandomNumber(0.0, 1.0); // 0.15;
            PromiseMultiplierChange1C = GetRandomNumber(0.0, 1.0); // 0.1;
            PromiseMultiplierChange2A = GetRandomNumber(0.0, 1.0); // 0.1;
            PromiseMultiplierChange2B = GetRandomNumber(0.0, 1.0); // 0.05;
            PromiseMultiplierChange2C = GetRandomNumber(0.0, 1.0); // 0.1;
            PromiseMultiplierChange3A = GetRandomNumber(0.0, 1.0); // 0.4;
            PromiseMultiplierChange3B = GetRandomNumber(0.0, 1.0); // 0.25;
            PromiseMultiplierChange3C = GetRandomNumber(0.0, 1.0); // 0.1;
            PromiseMultiplierChange4A = GetRandomNumber(0.0, 1.0); // 0.1;
            PromiseMultiplierChange4B = GetRandomNumber(0.0, 1.0); // 0.05;
            PromiseMultiplierChange4C = GetRandomNumber(0.0, 1.0); // 0.1;
            MiniRisk = randomAi.Next(0, 50); // 5;
        }

        public PlayerAI(string guidStr, PlayerAI goodAi)
        {
            AiName = Guid.NewGuid().ToString();

            DodgeBase = KeepAiValue() ? goodAi.DodgeBase : randomAi.Next(3, 90); // 50
            DodgeSure = 100; // this is a fact
            DodgeSmallestValuesInSuit = KeepAiValue() ? goodAi.DodgeSmallestValuesInSuit : randomAi.Next(30, 101); // 95
            DodgeSmallestValuesInSuitNOT = KeepAiValue() ? goodAi.DodgeSmallestValuesInSuitNOT : randomAi.Next(0, 101); // 85
            DodgeCardCountAvgOtherPlayersCount1 = KeepAiValue() ? goodAi.DodgeCardCountAvgOtherPlayersCount1 : randomAi.Next(0, 101); // 3
            DodgeBiggestValuesInSuit = KeepAiValue() ? goodAi.DodgeBiggestValuesInSuit : randomAi.Next(0, 101); // 15;
            DodgeBiggestValuesInSuitNOT = KeepAiValue() ? goodAi.DodgeBiggestValuesInSuitNOT : randomAi.Next(0, 101); // 25;
            DodgeCardCountAvgOtherPlayersCount2 = KeepAiValue() ? goodAi.DodgeCardCountAvgOtherPlayersCount2 : randomAi.Next(0, 101); // 7;
            DodgeInChargeAverageCount = KeepAiValue() ? goodAi.DodgeInChargeAverageCount : GetRandomNumber(0.0, 1.0); // 0.8;
            
            // BigValuesInSuit
            BigValuesInSuit = KeepAiValue() ? goodAi.BigValuesInSuit : randomAi.Next(9, 15); // 10;

            // SmallValuesInSuit
            SmallValuesInSuit = KeepAiValue() ? goodAi.SmallValuesInSuit : randomAi.Next(2, 8); // 6;

            // MakePromise
            PromiseMultiplierBase1 = KeepAiValue() ? goodAi.PromiseMultiplierBase1 : GetRandomNumber(0.0, 1.0); // 0.6;
            PromiseMultiplierBase2 = KeepAiValue() ? goodAi.PromiseMultiplierBase2 : GetRandomNumber(0.0, 1.0); // 0.2;
            PromiseMultiplierBase3 = KeepAiValue() ? goodAi.PromiseMultiplierBase3 : GetRandomNumber(0.0, 1.0); // 0.3;
            PromiseMultiplierBase4 = KeepAiValue() ? goodAi.PromiseMultiplierBase4 : GetRandomNumber(0.0, 1.0); // 0.25;
            PromiseMultiplierChange1A = KeepAiValue() ? goodAi.PromiseMultiplierChange1A : GetRandomNumber(0.0, 1.0); // 0.3;
            PromiseMultiplierChange1B = KeepAiValue() ? goodAi.PromiseMultiplierChange1B : GetRandomNumber(0.0, 1.0); // 0.15;
            PromiseMultiplierChange1C = KeepAiValue() ? goodAi.PromiseMultiplierChange1C : GetRandomNumber(0.0, 1.0); // 0.1;
            PromiseMultiplierChange2A = KeepAiValue() ? goodAi.PromiseMultiplierChange2A : GetRandomNumber(0.0, 1.0); // 0.1;
            PromiseMultiplierChange2B = KeepAiValue() ? goodAi.PromiseMultiplierChange2B : GetRandomNumber(0.0, 1.0); // 0.05;
            PromiseMultiplierChange2C = KeepAiValue() ? goodAi.PromiseMultiplierChange2C : GetRandomNumber(0.0, 1.0); // 0.1;
            PromiseMultiplierChange3A = KeepAiValue() ? goodAi.PromiseMultiplierChange3A : GetRandomNumber(0.0, 1.0); // 0.4;
            PromiseMultiplierChange3B = KeepAiValue() ? goodAi.PromiseMultiplierChange3B : GetRandomNumber(0.0, 1.0); // 0.25;
            PromiseMultiplierChange3C = KeepAiValue() ? goodAi.PromiseMultiplierChange3C : GetRandomNumber(0.0, 1.0); // 0.1;
            PromiseMultiplierChange4A = KeepAiValue() ? goodAi.PromiseMultiplierChange4A : GetRandomNumber(0.0, 1.0); // 0.1;
            PromiseMultiplierChange4B = KeepAiValue() ? goodAi.PromiseMultiplierChange4B : GetRandomNumber(0.0, 1.0); // 0.05;
            PromiseMultiplierChange4C = KeepAiValue() ? goodAi.PromiseMultiplierChange4C : GetRandomNumber(0.0, 1.0); // 0.1;
            MiniRisk = KeepAiValue() ? goodAi.MiniRisk : randomAi.Next(0, 50); // 5;
        }

        public PlayerAI()
        {
            // this is my best guess when coding
            // now with very small random
            AiName = "Fuison"; // "ee865a0c-a62d-49ec-98b8-1dc7a953c7f6";
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
            BigValuesInSuit = PlayerRandInt(10, 1);

            // SmallValuesInSuit
            SmallValuesInSuit = PlayerRandInt(6, 1);

            // MakePromise
            PromiseMultiplierBase1 = PlayerRandDouble(0.6);
            PromiseMultiplierBase2 = PlayerRandDouble(0.2);
            PromiseMultiplierBase3 = PlayerRandDouble(0.3);
            PromiseMultiplierBase4 = PlayerRandDouble(0.25);
            PromiseMultiplierChange1A = PlayerRandDouble(0.3);
            PromiseMultiplierChange1B = PlayerRandDouble(0.15);
            PromiseMultiplierChange1C = PlayerRandDouble(0.1);
            PromiseMultiplierChange2A = PlayerRandDouble(0.1);
            PromiseMultiplierChange2B = PlayerRandDouble(0.06);
            PromiseMultiplierChange2C = PlayerRandDouble(0.1);
            PromiseMultiplierChange3A = PlayerRandDouble(0.4);
            PromiseMultiplierChange3B = PlayerRandDouble(0.25);
            PromiseMultiplierChange3C = PlayerRandDouble(0.1);
            PromiseMultiplierChange4A = PlayerRandDouble(0.1);
            PromiseMultiplierChange4B = PlayerRandDouble(0.06);
            PromiseMultiplierChange4C = PlayerRandDouble(0.1);
            MiniRisk = PlayerRandInt(5);
        }

        public static double GetRandomNumber(double minimum, double maximum)
        {
            return randomAi.NextDouble() * (maximum - minimum) + minimum;
        }
        
        private static int PlayerRandInt(int mean, int variance = 2)
        {
            if (variance <= 0) return mean;
            return randomAi.Next(mean - variance, mean + variance + 1);
        }

        private static double PlayerRandDouble(double mean, double variance = 0.05)
        {
            if (variance <= 0) return mean;
            return GetRandomNumber(mean - variance, mean + variance);
        }

        public PlayerAI(int playerId, bool mutate = false)
        {
            switch (playerId)
            {
                case 0: AiName = "Jaska"; break;
                case 1: AiName = "Pera"; break;
                case 2: AiName = "Lissu"; break;
                case 3: AiName = "Repa"; break;
                case 4: AiName = "Arska"; break;
                default: AiName = "Kake"; break;
            }
                // 26732ad9-ce54-4e1d-ad07-d1ffbd5c4e9c
                // Player of 210 points and all 19 kepts in game
                // avg 101.6 points and 10.35 keeps

            DodgeBase = PlayerRandInt(53);
            DodgeSure = 100; // this is a fact
            DodgeSmallestValuesInSuit = PlayerRandInt(61);
            DodgeSmallestValuesInSuitNOT = PlayerRandInt(41);
            DodgeCardCountAvgOtherPlayersCount1 = PlayerRandInt(13);
            DodgeBiggestValuesInSuit = PlayerRandInt(66);
            DodgeBiggestValuesInSuitNOT = PlayerRandInt(8);
            DodgeCardCountAvgOtherPlayersCount2 = PlayerRandInt(63);
            DodgeInChargeAverageCount = PlayerRandDouble(0.30);
            
            // BigValuesInSuit
            BigValuesInSuit = randomAi.Next(13, 15); // 14;

            // SmallValuesInSuit
            SmallValuesInSuit = PlayerRandInt(6, 1);

            // MakePromise
            PromiseMultiplierBase1 = PlayerRandDouble(0.95);
            PromiseMultiplierBase2 = PlayerRandDouble(0.26);
            PromiseMultiplierBase3 = PlayerRandDouble(0.60);
            PromiseMultiplierBase4 = PlayerRandDouble(0.34);
            PromiseMultiplierChange1A = PlayerRandDouble(0.04);
            PromiseMultiplierChange1B = PlayerRandDouble(0.43);
            PromiseMultiplierChange1C = PlayerRandDouble(0.04);
            PromiseMultiplierChange2A = PlayerRandDouble(0.54);
            PromiseMultiplierChange2B = PlayerRandDouble(0.31);
            PromiseMultiplierChange2C = PlayerRandDouble(0.43);
            PromiseMultiplierChange3A = PlayerRandDouble(0.61);
            PromiseMultiplierChange3B = PlayerRandDouble(0.98);
            PromiseMultiplierChange3C = PlayerRandDouble(0.35);
            PromiseMultiplierChange4A = PlayerRandDouble(0.32);
            PromiseMultiplierChange4B = PlayerRandDouble(0.53);
            PromiseMultiplierChange4C = PlayerRandDouble(0.27);
            MiniRisk = PlayerRandInt(27);

            if (playerId == 1)
            {
                AiName = "Pera";
                // 7d759a25-8426-444e-935e-1add386adaa9

                DodgeBase = PlayerRandInt(52);
                DodgeSure = 100; // this is a fact
                DodgeSmallestValuesInSuit = PlayerRandInt(65);
                DodgeSmallestValuesInSuitNOT = PlayerRandInt(74);
                DodgeCardCountAvgOtherPlayersCount1 = PlayerRandInt(40);
                DodgeBiggestValuesInSuit = PlayerRandInt(14);
                DodgeBiggestValuesInSuitNOT = PlayerRandInt(78);
                DodgeCardCountAvgOtherPlayersCount2 = PlayerRandInt(2);
                DodgeInChargeAverageCount = PlayerRandDouble(0.38);
                
                // BigValuesInSuit
                BigValuesInSuit = randomAi.Next(13, 15); // 14;

                // SmallValuesInSuit
                SmallValuesInSuit = PlayerRandInt(5, 1);

                // MakePromise
                PromiseMultiplierBase1 = PlayerRandDouble(0.46);
                PromiseMultiplierBase2 = PlayerRandDouble(0.83);
                PromiseMultiplierBase3 = PlayerRandDouble(0.71);
                PromiseMultiplierBase4 = PlayerRandDouble(0.14);
                PromiseMultiplierChange1A = PlayerRandDouble(0.35);
                PromiseMultiplierChange1B = PlayerRandDouble(0.40);
                PromiseMultiplierChange1C = PlayerRandDouble(0.67);
                PromiseMultiplierChange2A = PlayerRandDouble(0.75);
                PromiseMultiplierChange2B = PlayerRandDouble(0.33);
                PromiseMultiplierChange2C = PlayerRandDouble(0.31);
                PromiseMultiplierChange3A = PlayerRandDouble(0.61);
                PromiseMultiplierChange3B = PlayerRandDouble(0.63);
                PromiseMultiplierChange3C = PlayerRandDouble(0.49);
                PromiseMultiplierChange4A = PlayerRandDouble(0.46);
                PromiseMultiplierChange4B = PlayerRandDouble(0.50);
                PromiseMultiplierChange4C = PlayerRandDouble(0.91);
                MiniRisk = PlayerRandInt(48);
            }

            if (playerId == 2)
            {
                AiName = "Lissu";
                // b7cac4f6-7974-422b-a616-b58eed21f54a

                DodgeBase = PlayerRandInt(77);
                DodgeSure = 100; // this is a fact
                DodgeSmallestValuesInSuit = PlayerRandInt(36);
                DodgeSmallestValuesInSuitNOT = PlayerRandInt(44);
                DodgeCardCountAvgOtherPlayersCount1 = PlayerRandInt(39);
                DodgeBiggestValuesInSuit = PlayerRandInt(74);
                DodgeBiggestValuesInSuitNOT = PlayerRandInt(41);
                DodgeCardCountAvgOtherPlayersCount2 = PlayerRandInt(46);
                DodgeInChargeAverageCount = PlayerRandDouble(0.65);
                
                // BigValuesInSuit
                BigValuesInSuit = randomAi.Next(13, 15); // 14;

                // SmallValuesInSuit
                SmallValuesInSuit = PlayerRandInt(4, 1);

                // MakePromise
                PromiseMultiplierBase1 = PlayerRandDouble(0.49);
                PromiseMultiplierBase2 = PlayerRandDouble(0.84);
                PromiseMultiplierBase3 = PlayerRandDouble(0.68);
                PromiseMultiplierBase4 = PlayerRandDouble(0.44);
                PromiseMultiplierChange1A = PlayerRandDouble(0.43);
                PromiseMultiplierChange1B = PlayerRandDouble(0.50);
                PromiseMultiplierChange1C = PlayerRandDouble(0.39);
                PromiseMultiplierChange2A = PlayerRandDouble(0.53);
                PromiseMultiplierChange2B = PlayerRandDouble(0.62);
                PromiseMultiplierChange2C = PlayerRandDouble(0.53);
                PromiseMultiplierChange3A = PlayerRandDouble(0.78);
                PromiseMultiplierChange3B = PlayerRandDouble(0.52);
                PromiseMultiplierChange3C = PlayerRandDouble(0.45);
                PromiseMultiplierChange4A = PlayerRandDouble(0.40);
                PromiseMultiplierChange4B = PlayerRandDouble(0.54);
                PromiseMultiplierChange4C = PlayerRandDouble(0.52);
                MiniRisk = PlayerRandInt(13);
            }

            if (playerId == 3)
            {
                AiName = "Repa";
                // 9fe8e8f4-e921-48c7-bfa8-214bf9b3d87d

                DodgeBase = PlayerRandInt(75);
                DodgeSure = 100; // this is a fact
                DodgeSmallestValuesInSuit = PlayerRandInt(83);
                DodgeSmallestValuesInSuitNOT = PlayerRandInt(67);
                DodgeCardCountAvgOtherPlayersCount1 = PlayerRandInt(38);
                DodgeBiggestValuesInSuit = PlayerRandInt(86);
                DodgeBiggestValuesInSuitNOT = PlayerRandInt(49);
                DodgeCardCountAvgOtherPlayersCount2 = PlayerRandInt(21);
                DodgeInChargeAverageCount = PlayerRandDouble(0.97);
                
                // BigValuesInSuit
                BigValuesInSuit = randomAi.Next(13, 15); // 14;

                // SmallValuesInSuit
                SmallValuesInSuit = PlayerRandInt(3, 1);

                // MakePromise
                PromiseMultiplierBase1 = PlayerRandDouble(0.52);
                PromiseMultiplierBase2 = PlayerRandDouble(0.95);
                PromiseMultiplierBase3 = PlayerRandDouble(0.70);
                PromiseMultiplierBase4 = PlayerRandDouble(0.14);
                PromiseMultiplierChange1A = PlayerRandDouble(0.26);
                PromiseMultiplierChange1B = PlayerRandDouble(0.65);
                PromiseMultiplierChange1C = PlayerRandDouble(0.03);
                PromiseMultiplierChange2A = PlayerRandDouble(0.24);
                PromiseMultiplierChange2B = PlayerRandDouble(0.28);
                PromiseMultiplierChange2C = PlayerRandDouble(0.90);
                PromiseMultiplierChange3A = PlayerRandDouble(0.76);
                PromiseMultiplierChange3B = PlayerRandDouble(0.62);
                PromiseMultiplierChange3C = PlayerRandDouble(0.14);
                PromiseMultiplierChange4A = PlayerRandDouble(0.49);
                PromiseMultiplierChange4B = PlayerRandDouble(0.56);
                PromiseMultiplierChange4C = PlayerRandDouble(0.59);
                MiniRisk = PlayerRandInt(3);
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
    }
    
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
            SKIPSAFE
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
                    return ai.DodgeSure;
                }
                else if (this.SmallValuesInSuit > this.BigValuesInSuit)
                {
                    if (this.SmallestValuesInSuit > 0)
                    {
                        retVal = ai.DodgeSmallestValuesInSuit;
                    }
                    else
                    {
                        retVal = ai.DodgeSmallestValuesInSuitNOT;
                    }
                    if (CardCount < this.AvgOtherPlayersCount)
                    {
                        retVal+= ai.DodgeCardCountAvgOtherPlayersCount1;
                    }
                }
                else
                {
                    if (this.BiggestValuesInSuit > 0)
                    {
                        retVal = ai.DodgeBiggestValuesInSuit;
                    }
                    else
                    {
                        retVal = ai.DodgeBiggestValuesInSuitNOT;
                    }
                    if (CardCount > this.AvgOtherPlayersCount)
                    {
                        retVal=- ai.DodgeCardCountAvgOtherPlayersCount2;
                    }
                }
                if (inCharge && this.AvgOtherPlayersCount < 1)
                {
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
                BiggestValuesInSuit = BiggestValuesInSuit(cards, playedCardsInSuit);
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
            return promiseNbr >= avgPoints; 
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

        private static int BiggestTrumpsInHand(List<Card> myHand, Card trumpCard, List<Card> playedTrumps)
        {
            int retVal = 0;

            for (int i = 14; i > 1; i--)
            {
                if (myHand.Any(x => (int)x.CardValue == i))
                {
                    retVal++;
                }
                else if ((int)trumpCard.CardValue == i || playedTrumps.Any(x => (int)x.CardValue == i))
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

        public static Promise MakePromise(PlayerAI ai, List<Card> hand, int playersInGame, Card trumpCard, Promise[] promises)
        {
            int cardsInRound = hand.Count();
            bool smallZeroRound = cardsInRound <= 5;

            int playersPromised = PlayersPromised(promises);
            bool iAmFirst = playersPromised == 0;
            bool iAmLast = playersPromised == playersInGame - 1;

            double avgPoints = (double)cardsInRound / (double)playersInGame;
            int cardsInGame = cardsInRound * playersInGame;
            double avgTrumpsInGame = ((double)cardsInGame / 4.0) - 1;
            double avgEachSuitInGame = ((double)cardsInGame / 4.0);
            double avgTrumpsAtPlayer = avgTrumpsInGame / (double)playersInGame;
            double avgEachSuitAtPlayer = avgEachSuitInGame / (double)playersInGame;

            int promisesMade = PromisesMade(promises);
            int promisesLeft = cardsInRound - promisesMade;
            int bigPromises = BigPromises(promises, avgPoints);
            int zeroPromises = ZeroPromises(promises);

            List<Card> suitC = hand.Where(x => x.CardSuit == CardSuit.Clubs).ToList();
            List<Card> suitD = hand.Where(x => x.CardSuit == CardSuit.Diamonds).ToList();
            List<Card> suitH = hand.Where(x => x.CardSuit == CardSuit.Hearts).ToList();
            List<Card> suitS = hand.Where(x => x.CardSuit == CardSuit.Spades).ToList();

            List<Card> myTrumps = hand.Where(x => x.CardSuit == trumpCard.CardSuit).ToList();
            int myTrumpCount = myTrumps.Count();
            int biggestTrumpsInHand = BiggestTrumpsInHand(myTrumps, trumpCard, new List<Card>());
            int smallerTrumpsInHand = myTrumps.Count() - biggestTrumpsInHand;

            AnalyzedSuit analyzedC = new AnalyzedSuit(ai, suitC, CardSuit.Clubs, CardSuit.Clubs == trumpCard.CardSuit, null, playersInGame, cardsInGame, iAmFirst);
            AnalyzedSuit analyzedD = new AnalyzedSuit(ai, suitD, CardSuit.Diamonds, CardSuit.Diamonds == trumpCard.CardSuit, null, playersInGame, cardsInGame, iAmFirst);
            AnalyzedSuit analyzedH = new AnalyzedSuit(ai, suitH, CardSuit.Hearts, CardSuit.Hearts == trumpCard.CardSuit, null, playersInGame, cardsInGame, iAmFirst);
            AnalyzedSuit analyzedS = new AnalyzedSuit(ai, suitS, CardSuit.Spades, CardSuit.Spades == trumpCard.CardSuit, null, playersInGame, cardsInGame, iAmFirst);

            AnalyzedSuit analyzedT = new AnalyzedSuit(ai, myTrumps, trumpCard.CardSuit, true, null, playersInGame, cardsInGame, iAmFirst);

            // this is a fact
            double myPromise = biggestTrumpsInHand;
            bool playZero = false;

            double promiseMultiplier = ai.PromiseMultiplierBase1;
            double averageSuitMultiplier = Math.Sqrt(avgEachSuitAtPlayer);
            if (iAmFirst) promiseMultiplier+= ai.PromiseMultiplierChange1A;
            if (iAmLast) promiseMultiplier+= ai.PromiseMultiplierChange1B;
            promiseMultiplier+= zeroPromises * ai.PromiseMultiplierChange1C;

            if (trumpCard.CardSuit != CardSuit.Clubs) myPromise+= analyzedC.BiggestValuesInSuit * averageSuitMultiplier * promiseMultiplier;
            if (trumpCard.CardSuit != CardSuit.Diamonds) myPromise+= analyzedD.BiggestValuesInSuit * averageSuitMultiplier * promiseMultiplier;
            if (trumpCard.CardSuit != CardSuit.Hearts) myPromise+= analyzedH.BiggestValuesInSuit * averageSuitMultiplier * promiseMultiplier;
            if (trumpCard.CardSuit != CardSuit.Spades) myPromise+= analyzedS.BiggestValuesInSuit * averageSuitMultiplier * promiseMultiplier;

            
            promiseMultiplier = ai.PromiseMultiplierBase2;
            if (iAmFirst) promiseMultiplier+= ai.PromiseMultiplierChange2A;
            if (iAmLast) promiseMultiplier+= ai.PromiseMultiplierChange2B;
            promiseMultiplier+= bigPromises * ai.PromiseMultiplierChange2C;

            if (trumpCard.CardSuit != CardSuit.Clubs) myPromise-= analyzedC.SmallestValuesInSuit * averageSuitMultiplier * promiseMultiplier;
            if (trumpCard.CardSuit != CardSuit.Diamonds) myPromise-= analyzedD.SmallestValuesInSuit * averageSuitMultiplier * promiseMultiplier;
            if (trumpCard.CardSuit != CardSuit.Hearts) myPromise-= analyzedH.SmallestValuesInSuit * averageSuitMultiplier * promiseMultiplier;
            if (trumpCard.CardSuit != CardSuit.Spades) myPromise-= analyzedS.SmallestValuesInSuit * averageSuitMultiplier * promiseMultiplier;


            promiseMultiplier = ai.PromiseMultiplierBase3;
            if (iAmFirst) promiseMultiplier+= ai.PromiseMultiplierChange3A;
            if (iAmLast) promiseMultiplier+= ai.PromiseMultiplierChange3B;
            promiseMultiplier+= zeroPromises * ai.PromiseMultiplierChange3C;

            if (trumpCard.CardSuit != CardSuit.Clubs) myPromise+= (analyzedC.BigValuesInSuit - analyzedC.BiggestValuesInSuit) * averageSuitMultiplier * promiseMultiplier;
            if (trumpCard.CardSuit != CardSuit.Diamonds) myPromise+= (analyzedD.BigValuesInSuit - analyzedD.BiggestValuesInSuit) * averageSuitMultiplier * promiseMultiplier;
            if (trumpCard.CardSuit != CardSuit.Hearts) myPromise+= (analyzedH.BigValuesInSuit - analyzedH.BiggestValuesInSuit) * averageSuitMultiplier * promiseMultiplier;
            if (trumpCard.CardSuit != CardSuit.Spades) myPromise+= (analyzedS.BigValuesInSuit - analyzedS.BiggestValuesInSuit) * averageSuitMultiplier * promiseMultiplier;

            
            promiseMultiplier = ai.PromiseMultiplierBase4;
            if (iAmFirst) promiseMultiplier+= ai.PromiseMultiplierChange4A;
            if (iAmLast) promiseMultiplier+= ai.PromiseMultiplierChange4B;
            promiseMultiplier+= bigPromises * ai.PromiseMultiplierChange4C;

            if (trumpCard.CardSuit != CardSuit.Clubs) myPromise-= (analyzedC.SmallValuesInSuit - analyzedC.SmallestValuesInSuit) * averageSuitMultiplier * promiseMultiplier;
            if (trumpCard.CardSuit != CardSuit.Diamonds) myPromise-= (analyzedD.SmallValuesInSuit - analyzedD.SmallestValuesInSuit) * averageSuitMultiplier * promiseMultiplier;
            if (trumpCard.CardSuit != CardSuit.Hearts) myPromise-= (analyzedH.SmallValuesInSuit - analyzedH.SmallestValuesInSuit) * averageSuitMultiplier * promiseMultiplier;
            if (trumpCard.CardSuit != CardSuit.Spades) myPromise-= (analyzedS.SmallValuesInSuit - analyzedS.SmallestValuesInSuit) * averageSuitMultiplier * promiseMultiplier;

            if (biggestTrumpsInHand == 0)
            {
                // should play zero?
                if (myTrumpCount < avgTrumpsAtPlayer && analyzedT.BigValuesInSuit < analyzedT.SmallValuesInSuit)
                {
                    int miniRisk = smallZeroRound ? ai.MiniRisk : 0;
                    // very likely zero
                    if (CheckRandom(analyzedC.IsDodgeable - miniRisk)
                        && CheckRandom(analyzedD.IsDodgeable - miniRisk)
                        && CheckRandom(analyzedH.IsDodgeable - miniRisk)
                        && CheckRandom(analyzedS.IsDodgeable - miniRisk)
                    )
                    {
                        playZero = true;
                    }
                }
            }

            if (myTrumpCount - biggestTrumpsInHand - analyzedT.BigValuesInSuit > avgTrumpsAtPlayer)
            {
                myPromise+= myTrumpCount - biggestTrumpsInHand - analyzedT.BigValuesInSuit;
            }
            else if (myTrumpCount - biggestTrumpsInHand > avgTrumpsAtPlayer)
            {
                if (analyzedT.BigValuesInSuit > 0)
                {
                    myPromise+= analyzedT.BigValuesInSuit - biggestTrumpsInHand;
                }
            }

            List<double> goingOverList = new List<double>();
            double goingOver = (promisesMade + myPromise) - cardsInRound;
            double changePromiseBy = 0;
            if (iAmLast && goingOver > 1 && myPromise > -1)
            {
                for (double i = 0; i < goingOver; i+= 0.5)
                {
                    for (double j = 0; j <= i; j+= 0.5) goingOverList.Add(j);
                }
                changePromiseBy = goingOverList.OrderBy(x => rand.Next()).First() * -1;
            }
            else if (iAmLast && goingOver < -1 && myPromise > -1)
            {
                for (double i = 0; i > goingOver; i-= 0.5)
                {
                    for (double j = 0; j >= i; j-= 0.5) goingOverList.Add(j);
                }
                changePromiseBy = goingOverList.OrderBy(x => rand.Next()).First() * -1;
            }
            myPromise+= changePromiseBy;

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
                finalPromise = (CheckRandom(randTest)) ? maxPromise : minPromise;
            }
            
            // last check - do not promise under your biggest trumps!
            if (finalPromise < biggestTrumpsInHand) finalPromise = biggestTrumpsInHand;

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

            RuntimeHelpers.EnsureSufficientExecutionStack();
            return ChooseDodgeSuit(analyzedSuits, ++counter);
        }

        private static CardSuit ChooseDiffultiestDodgeSuit(List<AnalyzedSuit> analyzedSuits, int counter = 0)
        {
            if (counter > 5) return analyzedSuits.OrderBy(x => x.IsDodgeable).First().Suit;
            foreach (AnalyzedSuit analyzedSuit in analyzedSuits.OrderBy(x => x.IsDodgeable))
            {
                if (CheckRandom(100 - analyzedSuit.IsDodgeable)) return analyzedSuit.Suit;
            }

            RuntimeHelpers.EnsureSufficientExecutionStack();
            return ChooseDiffultiestDodgeSuit(analyzedSuits, ++counter);
        }

        public static int PlayCard(PlayerAI ai, int playerInd, List<Card> hand, Card cardInCharge, Card trumpCard, Card[] tableCards, int cardsInRound, Promise[] promises, int[] roundWins, List<Card> cardsPlayedInRounds, PlayerInfo[] playerInfos)
        {
            //Logger.Log("PlayCard", "test");

            PlayingMethod myMethod = PlayingMethod.NOTSET;

            int playersInGame = promises.Count();
            int cardsInGame = playersInGame * cardsInRound;
            // int currentRound = roundWins.Sum() + 1;

            int myPromises = promises[playerInd].PromiseNumber;
            int myCurrentWins = roundWins[playerInd];
            
            // 0 = this is good, no more wins
            // negative = have to take wins
            // positive = over
            int myPromiseStatus = myCurrentWins - myPromises;

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
            int biggestTrumpsInHand = BiggestTrumpsInHand(myTrumps, trumpCard, cardsPlayedInRounds.Where(x => x.CardSuit == trumpCard.CardSuit).ToList());
            int smallerTrumpsInHand = myTrumps.Count() - biggestTrumpsInHand;

            AnalyzedSuit analyzedC = new AnalyzedSuit(ai, suitC, CardSuit.Clubs, CardSuit.Clubs == trumpCard.CardSuit, cardsPlayedInRounds, playersInGame, cardsInGame, iAmFirst);
            AnalyzedSuit analyzedD = new AnalyzedSuit(ai, suitD, CardSuit.Diamonds, CardSuit.Diamonds == trumpCard.CardSuit, cardsPlayedInRounds, playersInGame, cardsInGame, iAmFirst);
            AnalyzedSuit analyzedH = new AnalyzedSuit(ai, suitH, CardSuit.Hearts, CardSuit.Hearts == trumpCard.CardSuit, cardsPlayedInRounds, playersInGame, cardsInGame, iAmFirst);
            AnalyzedSuit analyzedS = new AnalyzedSuit(ai, suitS, CardSuit.Spades, CardSuit.Spades == trumpCard.CardSuit, cardsPlayedInRounds, playersInGame, cardsInGame, iAmFirst);

            AnalyzedSuit analyzedT = myTrumps.Any() ? new AnalyzedSuit(ai, suitS, trumpCard.CardSuit, true, cardsPlayedInRounds, playersInGame, cardsInGame, iAmFirst) : null;

            List<CardSuit> sureSuits = SureSuits(playerInd, playerInfos);

            if (myPromiseStatus + biggestTrumpsInHand > 0)
            {
                // pitkäksi oy:stä päivää
                myMethod = PlayingMethod.TRYTOROB;
            }
            else if (myPromiseStatus + roundsLeft < 0)
            {
                // can't get promise anymore
                myMethod = PlayingMethod.TRYTOSKIP;
            }
            else
            {
                myMethod = (myPromises == myCurrentWins) ? PlayingMethod.TRYTODODGE : PlayingMethod.TRYTOWIN;
                if (myPromiseStatus + biggestTrumpsInHand == 0 && roundsLeft > biggestTrumpsInHand) myMethod = PlayingMethod.SKIPSAFE;
            }

            int selectedCardInd = 0;
            Card selectedCard;

            if (iAmFirst)
            {
                if (myMethod == PlayingMethod.TRYTOROB || myMethod == PlayingMethod.TRYTOWIN)
                {
                    if (biggestTrumpsInHand > 0)
                    {
                        return GetCardIndexFromHand(hand, myTrumps.OrderByDescending(x => x.CardValue).First());
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

                    try
                    {
                        CardSuit dodgeSuit = ChooseDodgeSuit(betterAnalyzedSuits);
                        betterCards = betterCards.Where(x => x.CardSuit == dodgeSuit).ToList();
                    }
                    catch (InsufficientExecutionStackException)
                    {
                        throw;
                    }
                    catch
                    {
                        throw;
                    }

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
                selectedCardInd = rand.Next(possibleCards.Count());
                // this is just a default card
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

                        try
                        {
                            CardSuit dodgeSuit = ChooseDiffultiestDodgeSuit(betterAnalyzedSuits);
                            losingCards = losingCards.Where(x => x.CardSuit == dodgeSuit).ToList();
                        }
                        catch (InsufficientExecutionStackException)
                        {
                            throw;
                        }
                        catch
                        {
                            throw;
                        }
                        
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

                        try
                        {
                            CardSuit dodgeSuit = ChooseDodgeSuit(betterAnalyzedSuits);
                            possibleCards = possibleCards.Where(x => x.CardSuit == dodgeSuit).ToList();
                        }
                        catch (InsufficientExecutionStackException)
                        {
                            throw;
                        }
                        catch
                        {
                            throw;
                        }
                        
                        selectedCardInd = rand.Next(possibleCards.Count());
                        selectedCard = possibleCards.Skip(selectedCardInd).First();
                    }
                }
                else
                {
                    // we should win some round in this game, possibly not now...

                    if (winningCards.Any() && myPromiseStatus + roundsLeft == 0)
                    {
                        // we have to win all next rounds
                        // start using possible smallest winning card
                        if (iAmLast) return GetCardIndexFromHand(hand, winningCards.OrderBy(x => x.CardValue).First());
                        // otherwise take random
                        return GetCardIndexFromHand(hand, winningCards.Skip(rand.Next(winningCards.Count())).First());
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
                        selectedCardInd = rand.Next(winningCards.Count());
                        selectedCard = winningCards.Skip(selectedCardInd).First();
                    }
                }
                
                return GetCardIndexFromHand(hand, selectedCard);
            }
        }

    }
}