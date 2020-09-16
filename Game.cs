using System;
using System.Linq;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Bson;

namespace promise
{

    class Game
    {
        const bool DEBUGMODE = true;
        const int MAXPLAYERS = 5;
        const int SCOREBOARDSTART = 130;
        const int PROMISEBOARDX = 10;
        const int PROMISEBOARDY = 33;

        public Player[] Players {get; set;}
        public List<MongoAI> MongoAIs {get; set;}

        public int StartRound {get; set;}
        public int TurnRound {get; set;}
        public int EndRound {get; set;}

        public Round[] Rounds {get; set;}
        
        public int[] TotalPoints {get; set;}
        public int[] PromisesKept {get; set;}

        public bool IsBotMatch {get; set;}
        public bool ShowCards {get; set;}
        public bool IsTotalTest {get; set;}

        private void ClearScreen()
        {
            ScreenUtils.ClearScreen(this.IsTotalTest);
        }

        public Game(bool isBotMatch = false
                    , bool showCards = true
                    , List<MongoAI> mongoAIs = null
                    , IMongoCollection<MongoAI> collection = null
                    , bool isTotalTest = false
                    )
        {
            this.IsBotMatch = isBotMatch;
            this.ShowCards = showCards;
            this.IsTotalTest = isTotalTest;

            this.MongoAIs = mongoAIs;

            GetPlayers();
            this.Players = ShufflePlayers();

            this.TotalPoints = new int[this.Players.Count()];
            this.PromisesKept = new int[this.Players.Count()];

            GetGameRules();
            ClearScreen();
            InitRounds();
            PlayPromise();

            if (collection != null)
            {
                SaveGameResults(collection);
            }
        }

        private void SaveGameResults(IMongoCollection<MongoAI> collection)
        {
            string gameGuid = Guid.NewGuid().ToString();
            for (int i = 0; i < this.Players.Count(); i++)
            {
                MongoAI newPlayerAi;
                if (this.Players[i].PlayerType == PlayerType.COMPUTER)
                {
                    newPlayerAi = new MongoAI(this.Players[i].AI.AiName, this.Players[i].AI, this.TotalPoints[i], this.PromisesKept[i], this.MongoAIs.Skip(i).First().Evolution);
                }
                else
                {
                    newPlayerAi = new MongoAI(this.Players[i].PlayerName, this.Players[i].AI, this.TotalPoints[i], this.PromisesKept[i], -1);
                }
                newPlayerAi.SetGameGuid(gameGuid);
                collection.InsertOne(newPlayerAi);
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
            for (int i = 0; i < this.Players.Count(); i++)
            {
                this.TotalPoints[i] = 0;
            }
            for (int i = 0; i < this.Rounds.Count(); i++)
            {
                if (!this.Rounds[i].RoundPlayed) continue;
                for (int j = 0; j < this.Players.Count(); j++)
                {
                    int promiseSum = CountPoint(this.Rounds[i].CardsInRound, this.Rounds[i].Promises[PlayerPositionHelper(j - i)]);
                    promiseSums[j]+= promiseSum;
                    this.TotalPoints[j]+= promiseSum;
                }
            }
            if (this.IsTotalTest) return;

            promiseSums = new int[this.Players.Count()];

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
                    //this.TotalPoints[j]+= promiseSum;
                    string sumStr = (promiseSum > 0) ? $"{promiseSums[j]}" : "";
                    Console.Write(sumStr.PadLeft(3, ' '));
                    Console.Write(" ");
                }
            }
        }

        private void CalculateAndPrintPromiseBoard(int inGame = -1)
        {
            for (int i = 0; i < this.Players.Count(); i++)
            {
                this.PromisesKept[i] = 0;
                for (int j = 0; j < this.Rounds.Count(); j++)
                {
                    if (this.Rounds[j].RoundPlayed)
                    {
                        if (inGame != j && this.Rounds[j].Promises[PlayerPositionHelper(i - j)].PromiseKept) this.PromisesKept[i]++;
                    }
                }
            }
            if (this.IsTotalTest) return;

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
                this.PromisesKept[i] = 0;
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
                        this.PromisesKept[i]++;
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
            CalculateAndPrintScoreBoard();
            CalculateAndPrintPromiseBoard();
            var roundToPlay = this.Rounds[roundNbr];
            roundToPlay.MakePromises();
            CalculateAndPrintPromiseBoard(roundNbr);
            roundToPlay.PlayRound();
            CalculateAndPrintScoreBoard();
            CalculateAndPrintPromiseBoard();

            if (!this.IsBotMatch && !this.IsTotalTest) Console.ReadKey();
        }

        private void PlayPromise()
        {
            for (int i = 0; i < this.Rounds.Count(); i++)
            {
                ClearScreen();
                PlayRound(i);
            }

            if (!this.IsTotalTest) Console.ReadKey();
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
                this.Rounds[round] = new Round(i, round, this.Players, this.IsBotMatch, this.ShowCards, this.IsTotalTest);
                round++;
            }
            for (int i = this.TurnRound+1; i <= this.EndRound; i++)
            {
                this.Rounds[round] = new Round(i, round, this.Players, this.IsBotMatch, this.ShowCards, this.IsTotalTest);
                round++;
            }
        }

        private void GetPlayers()
        {
            ClearScreen();
            
            int lkm = 0;

            if (this.IsBotMatch || this.IsTotalTest)
            {
                lkm = 5;
            }
            else
            {
                Console.Write($"Pelaajien lukumäärä (2-{MAXPLAYERS}): ");
                ConsoleKeyInfo input = Console.ReadKey();
                while (!Int32.TryParse(input.KeyChar.ToString(), out lkm) || lkm > MAXPLAYERS || lkm < 2)
                {
                    ClearScreen();
                    Console.Write($"Pelaajien lukumäärä (2-{MAXPLAYERS}): ");
                    input = Console.ReadKey();
                }
                Console.WriteLine();
            }

            this.Players = new Player[lkm];
            for (int i = 0; i < this.Players.Count(); i++)
            {
                this.Players[i] = new Player(i+1, this.MongoAIs.Skip(i).First().PlayerAI, this.IsBotMatch || this.IsTotalTest);
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
            if (this.IsTotalTest || this.IsBotMatch)
            {
                this.StartRound = 10;
                this.TurnRound = 1;
                this.EndRound = 10;
                return;
            }

            ClearScreen();
            
            string input = "";
            int lkm = 0;

            Console.Write($"Mistä jaosta lähdetään (max {MaximumRounds()}): ");
            input = DEBUGMODE ? "10" : Console.ReadLine();
            while (!Int32.TryParse(input, out lkm) || lkm > MaximumRounds())
            {
                Console.Write($"Mistä jaosta lähdetään (max {MaximumRounds()}): ");
                input = Console.ReadLine();
            }
            this.StartRound = lkm;

            Console.Write("Missä jaossa käännytään: ");
            input = DEBUGMODE ? "1" : Console.ReadLine();
            while (!Int32.TryParse(input, out lkm) || lkm < 1 || lkm > this.StartRound)
            {
                Console.Write("Missä jaossa käännytään: ");
                input = Console.ReadLine();
            }
            this.TurnRound = lkm;

            Console.Write($"Mihin jakoon lopetetaan (max {MaximumRounds()}): ");
            input = DEBUGMODE ? "10" : Console.ReadLine();
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
