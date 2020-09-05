using System;
using DSI.Deck;
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 

namespace promise
{
    class Game
    {
        const int MAXPLAYERS = 5;
        public int PlayerCount {get; set;}

        public Player[] Players {get; set;}

        public int StartRound {get; set;}
        public int TurnRound {get; set;}
        public int EndRound {get; set;}

        public Round[] Rounds {get; set;}

        public Game()
        {
            GetPlayers();
            GetGameRules();
            this.Players = ShufflePlayers();
            InitRounds();
        }

        private void InitRounds()
        {
            this.Rounds = new Round[0];
            int round = 1;
            for (int i = this.StartRound; i >= this.TurnRound; i--)
            {
                this.Rounds.Append(new Round(i, round, this.Players));
                round++;
            }
            for (int i = this.TurnRound+1; i <= this.EndRound; i++)
            {
                this.Rounds.Append(new Round(i, round, this.Players));
                round++;
            }
        }

        private void GetPlayers()
        {
            ScreenUtils.ClearScreen();
            
            string input = "";
            int lkm = 0;

            Console.Write($"Pelaajien lukumäärä (2-{MAXPLAYERS}): ");
            input = Console.ReadLine();
            while (!Int32.TryParse(input, out lkm) || lkm > MAXPLAYERS || lkm < 2)
            {
                Console.Write($"Pelaajien lukumäärä (2-{MAXPLAYERS}): ");
                input = Console.ReadLine();
            }
            // this.PlayerCount = lkm;
            this.Players = new Player[lkm];
            for (int i = 0; i < this.Players.Count(); i++)
            {
                this.Players[i] = new Player(i+1);
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
            input = Console.ReadLine();
            while (!Int32.TryParse(input, out lkm) || lkm > MaximumRounds())
            {
                Console.Write($"Mistä jaosta lähdetään (max {MaximumRounds()}): ");
                input = Console.ReadLine();
            }
            this.StartRound = lkm;

            Console.Write("Missä jaossa käännytään: ");
            input = Console.ReadLine();
            while (!Int32.TryParse(input, out lkm) || lkm < 1 || lkm > this.StartRound)
            {
                Console.Write("Missä jaossa käännytään: ");
                input = Console.ReadLine();
            }
            this.TurnRound = lkm;

            Console.Write($"Mihin jakoon lopetetaan (max {MaximumRounds()}): ");
            input = Console.ReadLine();
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
