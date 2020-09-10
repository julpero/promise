using System;
using System.Linq;
using System.Collections.Generic;

namespace promise
{
    public class PlayerAI
    {
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

        public static double GetRandomNumber(double minimum, double maximum)
        { 
            
            return randomAi.NextDouble() * (maximum - minimum) + minimum;
        }
        
        public PlayerAI(string joo)
        {
            DodgeBase = randomAi.Next(100) + 1; // 50
            // DodgeSure = randomAi.Next(100) + 1; // this is a fact
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

        public bool IsBotMatch {get; set;}
        public bool ShowCards {get; set;}

        public Game(bool isBotMatch = false, bool showCards = true, bool randomizedBots = false)
        {
            this.IsBotMatch = isBotMatch;
            this.ShowCards = showCards;
            this.PlayerAIs = new List<PlayerAI>();
            if (randomizedBots)
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
            GetGameRules();
            ScreenUtils.ClearScreen();
            InitRounds();
            PlayPromise();
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

        private void PrintScoreBoard()
        {
            int[] promiseSums = new int[this.Players.Count()];

            Console.SetCursorPosition(SCOREBOARDSTART, 1);
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < this.Players.Count(); i++)
            {
                Console.Write("|");
                Console.Write(this.Players[i].PlayerInitials);
            }

            for (int i = 0; i < this.Rounds.Count(); i++)
            {
                Console.SetCursorPosition(SCOREBOARDSTART - 3, 2 + i);
                string roundStr = $"{this.Rounds[i].CardsInRound}";
                Console.Write(roundStr.PadLeft(2, ' '));
                Console.Write(" |");
                
                if (!this.Rounds[i].RoundPlayed) continue;

                for (int j = 0; j < this.Players.Count(); j++)
                {
                    int promiseSum = CountPoint(this.Rounds[i].CardsInRound, this.Rounds[i].Promises[PlayerPositionHelper(j - i)]);
                    promiseSums[j]+= promiseSum;
                    string sumStr = (promiseSum > 0) ? $"{promiseSums[j]}" : "";
                    Console.Write(sumStr.PadLeft(3, ' '));
                    Console.Write(" ");
                }
            }
        }

        private void PrintPromiseBoard(int inGame = -1)
        {
            Console.SetCursorPosition(PROMISEBOARDX + 15, PROMISEBOARDY);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("|");
            for (int i = 0; i < this.Rounds.Length; i++)
            {
                string promiseBoardStr = this.Rounds[i].CardsInRound.ToString();
                Console.Write(promiseBoardStr.PadLeft(2, ' '));
                Console.Write("|");
            }

            for (int i = 0; i < this.Players.Count(); i++)
            {
                Console.SetCursorPosition(PROMISEBOARDX, PROMISEBOARDY + 1 + i);
                Console.ForegroundColor = ConsoleColor.White;
                var nameStr = this.Players[i].PlayerName;
                if (nameStr.Length > 14) nameStr = nameStr.Substring(0, 14);
                Console.Write(nameStr.PadLeft(14, ' '));
                Console.Write(" |");
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
                    }
                    else
                    {
                        Console.ForegroundColor = (this.Rounds[j].Promises[PlayerPositionHelper(i - j)].PromiseKeptType == PromiseKeptTypeEnum.UNDER)
                                                ? ConsoleColor.DarkGray
                                                : ConsoleColor.DarkRed;
                    }

                    string thisPromiseStr = this.Rounds[j].Promises[PlayerPositionHelper(i - j)].PromiseNumber.ToString();
                    Console.Write(thisPromiseStr.PadLeft(2, ' '));
                    Console.Write(" ");
                }
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(PROMISEBOARDX, PROMISEBOARDY + 1 + this.Players.Count());
            Console.Write("".PadRight(16 + (this.Rounds.Count() * 3), '-'));
            Console.SetCursorPosition(PROMISEBOARDX, PROMISEBOARDY + 1 + this.Players.Count()+1);
            Console.Write("TOTAL".PadLeft(14, ' '));
            Console.Write(" |");
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
                Console.Write(thisPromiseStr.PadLeft(2, ' '));
                Console.Write(" ");
            }
        }

        private void PlayRound(int roundNbr)
        {
            PrintScoreBoard();
            PrintPromiseBoard();
            var roundToPlay = this.Rounds[roundNbr];
            roundToPlay.MakePromises();
            PrintPromiseBoard(roundNbr);
            roundToPlay.PlayRound();
            PrintScoreBoard();
            PrintPromiseBoard();

            if (!this.IsBotMatch) Console.ReadKey();
        }

        private void PlayPromise()
        {
            for (int i = 0; i < this.Rounds.Count(); i++)
            {
                ScreenUtils.ClearScreen();
                PlayRound(i);
            }

            Console.ReadKey();
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
                this.Rounds[round] = new Round(i, round, this.Players, this.IsBotMatch, this.ShowCards);
                round++;
            }
            for (int i = this.TurnRound+1; i <= this.EndRound; i++)
            {
                this.Rounds[round] = new Round(i, round, this.Players, this.IsBotMatch, this.ShowCards);
                round++;
            }
        }

        private void GetPlayers(List<PlayerAI> playerAIs)
        {
            ScreenUtils.ClearScreen();
            
            int lkm = 0;

            if (this.IsBotMatch)
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
            ScreenUtils.ClearScreen();
            
            string input = "";
            int lkm = 0;

            Console.Write($"Mistä jaosta lähdetään (max {MaximumRounds()}): ");
            input = this.IsBotMatch ? "10" : DEBUGMODE ? "4" : Console.ReadLine();
            while (!Int32.TryParse(input, out lkm) || lkm > MaximumRounds())
            {
                Console.Write($"Mistä jaosta lähdetään (max {MaximumRounds()}): ");
                input = Console.ReadLine();
            }
            this.StartRound = lkm;

            Console.Write("Missä jaossa käännytään: ");
            input = this.IsBotMatch ? "1" : DEBUGMODE ? "1" : Console.ReadLine();
            while (!Int32.TryParse(input, out lkm) || lkm < 1 || lkm > this.StartRound)
            {
                Console.Write("Missä jaossa käännytään: ");
                input = Console.ReadLine();
            }
            this.TurnRound = lkm;

            Console.Write($"Mihin jakoon lopetetaan (max {MaximumRounds()}): ");
            input = this.IsBotMatch ? "10" : DEBUGMODE ? "4" : Console.ReadLine();
            while (!Int32.TryParse(input, out lkm) || lkm < this.TurnRound || lkm > MaximumRounds())
            {
                Console.Write($"Mihin jakoon lopetetaan (max {MaximumRounds()}): ");
                input = Console.ReadLine();
            }
            this.EndRound = lkm;
            Console.WriteLine();

        }

        private Player[] ShufflePlayers(int shuffleTime = 0)
        {
            Random rnd = new Random();
            Player[] shuffledPlayers = this.Players.OrderBy(x => rnd.Next()).ToArray();
            return shuffledPlayers;
        }

    }
}
