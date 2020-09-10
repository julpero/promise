using System;
using System.Linq; 

namespace promise
{
    class Game
    {
        const bool DEBUGMODE = false;
        const int MAXPLAYERS = 5;
        const int SCOREBOARDSTART = 130;
        const int PROMISEBOARDX = 10;
        const int PROMISEBOARDY = 33;

        // public int PlayerCount {get; set;}

        public Player[] Players {get; set;}

        public int StartRound {get; set;}
        public int TurnRound {get; set;}
        public int EndRound {get; set;}

        public Round[] Rounds {get; set;}

        public bool IsBotMatch {get; set;}

        public Game()
        {
            this.IsBotMatch = true;
            GetPlayers();
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
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }

                    string thisPromiseStr = this.Rounds[j].Promises[PlayerPositionHelper(i - j)].PromiseNumber.ToString();
                    Console.Write(thisPromiseStr.PadLeft(2, ' '));
                    Console.Write(" ");
                }
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
                this.Rounds[round] = new Round(i, round, this.Players, this.IsBotMatch);
                round++;
            }
            for (int i = this.TurnRound+1; i <= this.EndRound; i++)
            {
                this.Rounds[round] = new Round(i, round, this.Players, this.IsBotMatch);
                round++;
            }
        }

        private void GetPlayers()
        {
            ScreenUtils.ClearScreen();
            
            int lkm = 0;

            Console.Write($"Pelaajien lukumäärä (2-{MAXPLAYERS}): ");
            ConsoleKeyInfo input = Console.ReadKey();
            while (!Int32.TryParse(input.KeyChar.ToString(), out lkm) || lkm > MAXPLAYERS || lkm < 2)
            {
                ScreenUtils.ClearScreen();
                Console.Write($"Pelaajien lukumäärä (2-{MAXPLAYERS}): ");
                input = Console.ReadKey();
            }
            Console.WriteLine();
            // this.PlayerCount = lkm;
            this.Players = new Player[lkm];
            for (int i = 0; i < this.Players.Count(); i++)
            {
                this.Players[i] = new Player(i+1);
                if (this.Players[i].PlayerType == PlayerType.HUMAN) this.IsBotMatch = false;
            }
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
            Random rnd=new Random();
            Player[] shuffledPlayers = this.Players.OrderBy(x => rnd.Next()).ToArray();
            return shuffledPlayers;
        }

    }
}
