using System;
using DSI.Deck;
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 

namespace promise
{
    public class Promise
    {
        public int PromiseNumber {get; set;}
        public bool PromiseKept {get; set;}

        public Promise(int promiseNumber)
        {
            PromiseKept = false;
            PromiseNumber = promiseNumber;
        }
    }

    class Round
    {
        const int COLWIDTH = 20;

        public int CardsInRound {get; set;}
        
        // first round = 0
        public int RoundCount {get; set;}
        public bool RoundPlayed {get; set;}

        public Player[] Players {get; set;}

        // cards that are played
        public List<Card> CardsPlayed {get; set;}
        public List<Card>[] Hands {get; set;}
        public Card TrumpCard {get; set;}

        public Promise[] Promises {get; set;}


        private int PlayerPositionHelper(int i)
        {
            if (i < this.Players.Count()) return i;
            return PlayerPositionHelper(i - this.Players.Count());
        }

        public Round(int cardsInRound, int roundCount, Player[] players)
        {
            this.CardsInRound = cardsInRound;
            this.RoundCount = roundCount;
            this.RoundPlayed = false;

            this.Players = new Player[players.Count()];
            for (int i = 0; i < players.Count(); i++)
            {
                this.Players[i] = players[PlayerPositionHelper(roundCount + i)];
            }

            this.Promises = new Promise[players.Count()];

            MakeDeal();
        }

        private void MakeDeal()
        {
            Deck deckOfCards = new Deck();
            deckOfCards.Shuffle();

            this.Hands = new List<Card>[this.Players.Count()];

            for (int i = 0; i < this.Players.Count(); i++)
            {
                this.Hands[i] = new List<Card>();
                this.Hands[i] = deckOfCards.DrawSorted(this.CardsInRound);
            }
            this.TrumpCard = deckOfCards.Draw(1).First();
        }

        public void MakePromises()
        {
            for (int i = 0; i < this.Players.Count(); i++)
            {
                this.Promises[i] = GetPromise(i);
                
                this.RoundPlayed = true;
            }
        }

        private Promise GetPromise(int i)
        {
            return (this.Players[i].PlayerType == PlayerType.COMPUTER)
                ? ComputerAI.MakePromise(this.CardsInRound)
                : GetPlayerPromise(i);
        }

        private Promise GetPlayerPromise(int playerIndex)
        {

            UiAskPromise(playerIndex);

            string input = "";
            int lkm = 0;

            //Console.SetCursorPosition();

            Console.Write($"Anna lupaus kierrokselle {this.CardsInRound} (0-{this.CardsInRound}): ");
            input = Console.ReadLine();
            while (!Int32.TryParse(input, out lkm) || lkm > this.CardsInRound || lkm < 0)
            {
                return GetPlayerPromise(playerIndex);
            }
            return new Promise(lkm);
        }

        private void UiAskPromise(int playerInd)
        {
            // ScreenUtils.ClearScreen();
            Console.SetCursorPosition(0, 0);

            for (int i = 0; i < this.Players.Count(); i++)
            {
                string nameStr = this.Players[i].PlayerName;
                Console.Write("| ");
                if (nameStr.Length > COLWIDTH) nameStr = nameStr.Substring(0, COLWIDTH);
                Console.Write(nameStr.PadRight(COLWIDTH, ' '));
            }
            Console.WriteLine();

            for (int i = 0; i < playerInd; i++)
            {
                string promiseStr = $"{this.Promises[i].PromiseNumber}";
                Console.Write("|  ");
                Console.Write(promiseStr.PadRight(COLWIDTH - 1, ' '));
            }
            Console.WriteLine();

        }
    }
}
