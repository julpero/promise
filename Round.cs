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
        const int CARDSSTARTX = 4;
        const int CARDSSTARTY = 22;
        const int TRUMPSTARTY = 12;
        const int CARDWIDTH = 11;
        const int CARDHEIGHT = 7;

        public int CardsInRound {get; set;}
        
        // first round = 0
        public int RoundCount {get; set;}
        public bool RoundPlayed {get; set;}

        public Player[] Players {get; set;}
        public int PlayerInCharge {get; set;}

        // cards that are played - going to use this in AI
        public List<Card> CardsPlayed {get; set;}
        public List<Card>[] Hands {get; set;}
        public Card TrumpCard {get; set;}
        public Card CardInCharge {get; set;}
        public Card[] TableCards {get; set;}

        public Promise[] Promises {get; set;}
        public int[] RoundWins {get; set;}


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
            this.RoundWins = new int[players.Count()];
            for (int i = 0; i < players.Count(); i++)
            {
                this.RoundWins[i] = 0;
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

        private int CardAskHelper(int playerInd)
        {
            int retVal = this.PlayerInCharge + playerInd;
            if (retVal < this.Players.Count()) return retVal;
            return retVal - this.Players.Count();
        }

        private bool SuiteIsInHand(List<Card> hand)
        {
            return hand.Any(x => x.CardSuit == this.CardInCharge.CardSuit);
        }

        private bool IsValidCard(List<Card> hand, int cardIndex)
        {
            Card cardToCheck = hand.Skip(cardIndex).First();
            if (SuiteIsInHand(hand) && cardToCheck.CardSuit != this.CardInCharge.CardSuit) return false;
            return true;
        }

        private Card AskCard(int playerInd)
        {
            int cardIndex = -1;
            if (this.Players[playerInd].PlayerType == PlayerType.COMPUTER)
            {
                cardIndex = ComputerAI.PlayCard(this.Hands[playerInd], this.CardInCharge);
            }
            else
            {
                cardIndex = GetPlayerCard(playerInd);
            }

            if (playerInd == this.PlayerInCharge)
            {
                this.CardInCharge = this.Hands[playerInd].Skip(cardIndex).First();
            }
            else
            {
                if (this.Players[playerInd].PlayerType == PlayerType.HUMAN)
                {
                    // if not in charge check that played card is valid
                    if (!IsValidCard(this.Hands[playerInd], cardIndex)) return AskCard(playerInd);
                }
            }
            Card playedCard = this.Hands[playerInd].Skip(cardIndex).First();

            PrintPlayedCard(playerInd, playedCard);
            this.Hands[playerInd].RemoveAt(cardIndex);

            if (this.Players[playerInd].PlayerType == PlayerType.HUMAN)
            {
                ScreenUtils.ClearPlayerCards();
                PrintPlayerCards(playerInd, false);
            }

            return playedCard;
        }

        private int CheckWinner()
        {
            // initally player in charge is winner
            int currentWinner = this.PlayerInCharge;
            Card winningCard = this.TableCards[currentWinner];

            for (int i = 0; i < this.TableCards.Count(); i++)
            {
                if (i == this.PlayerInCharge) continue;
                Card currentCard = this.TableCards[i];
                bool wins = false;
                if (winningCard.CardSuit == this.TrumpCard.CardSuit)
                {
                    // has to be trump to win
                    wins = currentCard.CardSuit == this.TrumpCard.CardSuit && currentCard.CardValue > winningCard.CardValue;
                }
                else if (currentCard.CardSuit == this.TrumpCard.CardSuit)
                {
                    // wins with trump
                    wins = true;
                }
                else
                {
                    // wins with greater value if same suit
                    wins = currentCard.CardSuit == winningCard.CardSuit && currentCard.CardValue > winningCard.CardValue;
                }
                if (wins)
                {
                    currentWinner = i;
                    winningCard = currentCard;
                }
            }
            return currentWinner;
        }

        public void PlayRound()
        {
            this.PlayerInCharge = 0;
            Console.ForegroundColor = ConsoleColor.White;
            UIShowNames(0);
            UIShowPromises();
            for (int i = 0; i < this.CardsInRound; i++)
            {
                // List<string> debugInfo = new List<string>();

                this.TableCards = new Card[this.Players.Count()];
                this.CardInCharge = null;

                for (int j = 0; j < this.Players.Count(); j++)
                {
                    int currentPlayerIndex = CardAskHelper(j);
                    // string debugStr = $"{this.PlayerInCharge} - {j} -> {currentPlayerIndex}";
                    // debugInfo.Add(debugStr);
                    Card cardPlayed = AskCard(currentPlayerIndex);
                    this.TableCards[currentPlayerIndex] = cardPlayed;
                }

                int winnerOfRound = CheckWinner();
                this.RoundWins[winnerOfRound]++;
                this.PlayerInCharge = winnerOfRound;
                UIShowNames(winnerOfRound);
                UIShowPromises();
                Console.SetCursorPosition(CARDSSTARTX + (CARDWIDTH / 2) - 1, CARDSSTARTY + CARDHEIGHT + 2);
                Console.Write($"Kierroksen voitti {this.Players[winnerOfRound].PlayerName}");
                Console.ReadKey();
                ScreenUtils.ClearPlayedCards();
                ScreenUtils.ClearPlayerCards();

            }

            for (int i = 0; i < this.Players.Count(); i++)
            {
                if (this.Promises[i].PromiseNumber == this.RoundWins[i]) this.Promises[i].PromiseKept = true;
            }

            this.RoundPlayed = true;
        }

        public void MakePromises()
        {
            for (int i = 0; i < this.Players.Count(); i++)
            {
                this.Promises[i] = GetPromise(i);
            }
        }

        private Promise GetPromise(int i)
        {
            return (this.Players[i].PlayerType == PlayerType.COMPUTER)
                ? ComputerAI.MakePromise(this.CardsInRound, this.Players.Count())
                : GetPlayerPromise(i);
        }

        private int GetPlayerCard(int playerIndex)
        {
            ScreenUtils.ClearPlayerCards();
            PrintTrumpCard();
            PrintPlayerCards(playerIndex, true);

            int lkm = -1;

            Console.SetCursorPosition(CARDSSTARTX + (CARDWIDTH / 2) - 1, CARDSSTARTY + CARDHEIGHT + 2);
            Console.Write($"Pelaa kortti: ");
            ConsoleKeyInfo input = Console.ReadKey();
            if (!Int32.TryParse(input.KeyChar.ToString(), out lkm)) return GetPlayerCard(playerIndex);
            if (lkm == 0) lkm = 10;
            lkm--; // convert input to array index
            if (lkm < 0 || lkm >= this.Hands[playerIndex].Count()) return GetPlayerCard(playerIndex);

            return lkm;
        }

        private Promise GetPlayerPromise(int playerIndex)
        {
            UiAskPromise(playerIndex);
            PrintTrumpCard();
            PrintPlayerCards(playerIndex);

            string input = "";
            int lkm = 0;

            Console.SetCursorPosition(1, 5);
            Console.Write($"Anna lupaus kierrokselle {this.CardsInRound} (0-{this.CardsInRound}): ");
            input = Console.ReadLine();
            while (!Int32.TryParse(input, out lkm) || lkm > this.CardsInRound || lkm < 0)
            {
                return GetPlayerPromise(playerIndex);
            }
            ScreenUtils.ClearGetPromise();
            return new Promise(lkm);
        }

        private void PrintTrumpCard()
        {
            Console.SetCursorPosition(CARDSSTARTX + 2, TRUMPSTARTY);
            Console.Write("VALTTI");
            ScreenUtils.PrintCard(CARDSSTARTX, TRUMPSTARTY + 1, this.TrumpCard);
        }

        private void PrintPlayerCards(int playerInd, bool printCardNumber = false)
        {
            for (int i = 0; i < this.Hands[playerInd].Count(); i++)
            {
                int x = CARDSSTARTX + (i * (CARDWIDTH + 1));
                int y = CARDSSTARTY;
                Card cardToPrint = this.Hands[playerInd].Skip(i).First();
                bool cardIsAvailable = true;
                if (this.CardInCharge != null)
                {
                    cardIsAvailable = IsValidCard(this.Hands[playerInd], i);
                }
                ScreenUtils.PrintCard(x, y, cardToPrint, cardIsAvailable);
                if (printCardNumber)
                {
                    Console.SetCursorPosition(x + (CARDWIDTH / 2) - 1, y + CARDHEIGHT);
                    string cardNumberStr = (i == 9) ? "0" : $"{i+1}";
                    Console.Write($"({cardNumberStr})");
                }
            }
        }

        private void PrintPlayedCard(int playerInd, Card playedCard)
        {
            ScreenUtils.PrintCard((playerInd * (COLWIDTH + 1)) + 5, 3, playedCard);
        }

        private void UIShowPromises()
        {
            Console.SetCursorPosition(0, 1);
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < this.Players.Count(); i++)
            {
                string promiseStr = $"{this.Promises[i].PromiseNumber}";
                promiseStr+= $" / {this.RoundWins[i]}";
                Console.Write("|  ");
                Console.Write(promiseStr.PadRight(COLWIDTH - 1, ' '));
            }
        }

        private void UIShowNames(int boldPlayer = -1)
        {
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < this.Players.Count(); i++)
            {
                string nameStr = this.Players[i].PlayerName;
                if (i == boldPlayer) nameStr = "** " + nameStr.ToUpper() + " **";
                Console.Write("| ");
                if (nameStr.Length > COLWIDTH) nameStr = nameStr.Substring(0, COLWIDTH);
                Console.Write(nameStr.PadRight(COLWIDTH, ' '));
            }
            Console.WriteLine();

        }

        private void UiAskPromise(int playerInd)
        {
            UIShowNames();
            Console.ForegroundColor = ConsoleColor.White;
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
