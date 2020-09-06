using System;
using DSI.Deck;
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 

namespace promise
{
    class Round
    {
        public int CardsInRound {get; set;}
        
        // first round = 0
        public int RoundCount {get; set;}
        public bool RoundPlayed {get; set;}

        public Player[] Players {get; set;}

        // cards that are played
        public List<Card> CardsPlayed {get; set;}
        public List<Card>[] Hands {get; set;}
        public Card TrumpCard {get; set;}

        public int[] Promises {get; set;}

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
            }
        }

        private int GetPromise(int i)
        {
            return (this.Players[i].PlayerType == PlayerType.COMPUTER)
                ? ComputerAI.MakePromise()
                : 1;
        }
    }
}
