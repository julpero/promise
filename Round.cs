using System;
using DSI.Deck;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace promise
{
    public enum PromiseKeptTypeEnum
    {
        UNDER,
        OVER
    }

    public class Promise
    {
        public int PromiseNumber {get; set;}
        public bool PromiseKept {get; set;}
        public PromiseKeptTypeEnum PromiseKeptType {get; set;}

        public Promise(int promiseNumber)
        {
            PromiseKept = false;
            PromiseNumber = promiseNumber;
        }
    }

    public class PlayerInfo
    {
        public bool HasTrumps {get; set;}
        public bool HasSpades {get; set;}
        public bool HasClubs {get; set;}
        public bool HasHearts {get; set;}
        public bool HasDiamonds {get; set;}

        public PlayerInfo()
        {
            HasClubs = true;
            HasHearts = true;
            HasDiamonds = true;
            HasSpades = true;
            HasTrumps = true;
        }

        public bool HasSuit(CardSuit suit)
        {
            switch (suit)
            {
                case CardSuit.Spades: return HasSpades;
                case CardSuit.Hearts: return HasHearts;
                case CardSuit.Clubs: return HasClubs;
                case CardSuit.Diamonds: return HasDiamonds;
            }
            return true;
        }

    }

    class Round
    {
        const int WAITTIME = 400; // milliseconds
        const int COLWIDTH = 20;
        const int CARDSSTARTX = 4;
        const int CARDSSTARTY = 22;
        const int TRUMPSTARTY = 12;
        const int CARDWIDTH = 11;
        const int CARDHEIGHT = 7;

        public GameDebugSettings GameSettings {get; set;}

        public int CardsInRound {get; set;}
        
        // first round = 0
        public int RoundCount {get; set;}
        public bool RoundPlayed {get; set;}

        public Player[] Players {get; set;}
        public int PlayerInCharge {get; set;}

        // cards that are played - going to use this in AI
        public List<Card> CardsPlayedInRounds {get; set;}
        public PlayerInfo[] PlayerInfos {get; set;}
        
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

        public Round(int cardsInRound, int roundCount, Player[] players, GameDebugSettings gamesettings)
        {
            this.GameSettings = gamesettings;

            this.CardsInRound = cardsInRound;
            this.RoundCount = roundCount;
            this.RoundPlayed = false;

            this.Players = new Player[players.Count()];
            this.PlayerInfos = new PlayerInfo[players.Count()];
            
            for (int i = 0; i < players.Count(); i++)
            {
                this.Players[i] = players[PlayerPositionHelper(roundCount + i)];
            }

            this.Promises = new Promise[players.Count()];
            this.RoundWins = new int[players.Count()];
            for (int i = 0; i < players.Count(); i++)
            {
                this.RoundWins[i] = 0;
                this.PlayerInfos[i] = new PlayerInfo();
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

        private int NextPlayerIndex(int currentPlayerIndex)
        {
            for (int i = 1; i < this.Players.Count(); i++)
            {
                int j = currentPlayerIndex + i;
                if (j > this.Players.Count()) j = 0;
            }
            return -1;
        }

        private Card AskCard(int playerInd)
        {
            int cardIndex = -1;
            if (this.Players[playerInd].PlayerType == PlayerType.COMPUTER)
            {
                if (this.GameSettings.ShowCards && !this.GameSettings.IsTotalTest) PrintPlayerCards(NextPlayerIndex(playerInd), false);
                if (!this.GameSettings.IsBotMatch && !this.GameSettings.IsTotalTest) Thread.Sleep(WAITTIME);
                cardIndex = ComputerAI.PlayCard(this.Players[playerInd].AI, playerInd
                                                , this.Hands[playerInd]
                                                , this.CardInCharge
                                                , this.TrumpCard
                                                , this.TableCards
                                                , this.CardsInRound
                                                , this.Promises
                                                , this.RoundWins
                                                , this.CardsPlayedInRounds
                                                , this.PlayerInfos
                                                );
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
            this.TableCards[playerInd] = playedCard;

            if (playedCard.CardSuit != this.CardInCharge.CardSuit)
            {
                switch (this.CardInCharge.CardSuit)
                {
                    case CardSuit.Clubs: this.PlayerInfos[playerInd].HasClubs = false; break;
                    case CardSuit.Hearts: this.PlayerInfos[playerInd].HasHearts = false; break;
                    case CardSuit.Diamonds: this.PlayerInfos[playerInd].HasDiamonds = false; break;
                    case CardSuit.Spades: this.PlayerInfos[playerInd].HasSpades = false; break;
                }
            }

            this.Hands[playerInd].RemoveAt(cardIndex);

            if (this.Players[playerInd].PlayerType == PlayerType.HUMAN)
            {
                int[] positionFrom = CardPosition(cardIndex);
                int[] positionTo = PlayedCardPosition(playerInd);
                ScreenUtils.AnimateCard(positionFrom[0], positionFrom[1], positionTo[0], positionTo[1], playedCard, ScreenUtils.CardBgType.BASIC, this.TrumpCard, this.GameSettings.IsTotalTest);
                ScreenUtils.ClearPlayerCards();
                PrintPlayerCards(playerInd, false);
            }
            PrintPlayedCards();

            return playedCard;
        }

        private bool CardWillWin(Card cardToCheck, Card cardInCharge, List<Card> cardsInTable, CardSuit trumpSuit)
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
            this.CardsPlayedInRounds = new List<Card>();
            
            UIShowNames(0);
            UIShowPromises();
            for (int i = 0; i < this.CardsInRound; i++)
            {
                List<Card> cardsInThisRound = new List<Card>();

                this.TableCards = new Card[this.Players.Count()];
                this.CardInCharge = null;

                for (int j = 0; j < this.Players.Count(); j++)
                {
                    int currentPlayerIndex = CardAskHelper(j);
                    Card cardPlayed = AskCard(currentPlayerIndex);
                    cardsInThisRound.Add(cardPlayed);
                }

                int winnerOfRound = CheckWinner();
                this.RoundWins[winnerOfRound]++;
                this.PlayerInCharge = winnerOfRound;
                UIShowNames(winnerOfRound);
                UIShowPromises();

                this.CardsPlayedInRounds.AddRange(cardsInThisRound);

                UIShowWinner(this.Players[winnerOfRound].PlayerName);
                if (!this.GameSettings.IsBotMatch && !this.GameSettings.IsTotalTest) Console.ReadKey();
                ScreenUtils.ClearCards(this.GameSettings.IsTotalTest);
            }

            for (int i = 0; i < this.Players.Count(); i++)
            {
                if (this.Promises[i].PromiseNumber == this.RoundWins[i]) this.Promises[i].PromiseKept = true;
                if (this.Promises[i].PromiseNumber > this.RoundWins[i]) this.Promises[i].PromiseKeptType = PromiseKeptTypeEnum.UNDER;
                if (this.Promises[i].PromiseNumber < this.RoundWins[i]) this.Promises[i].PromiseKeptType = PromiseKeptTypeEnum.OVER;
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
            if (this.Players[i].PlayerType == PlayerType.COMPUTER)
            {
                if (this.GameSettings.DebugPromise)
                {
                    ScreenUtils.PrintTrumpCard(this.TrumpCard);
                    PrintPlayerCards(i);
                }
                Promise computerPromise = ComputerAI.MakePromise(this.Players[i].AI, this.Hands[i], this.Players.Count(), this.TrumpCard, this.Promises, this.GameSettings.DebugPromise);
                if (this.GameSettings.DebugPromise)
                {
                    Console.SetCursorPosition(CARDSSTARTX + (CARDWIDTH / 2) - 1, CARDSSTARTY + CARDHEIGHT + 2);
                    Console.Write("                                                                                      ");
                    Console.SetCursorPosition(CARDSSTARTX + (CARDWIDTH / 2) - 1, CARDSSTARTY + CARDHEIGHT + 2);
                    Console.Write($"Computer {this.Players[i].PlayerName} promised {computerPromise.PromiseNumber}, press X to get new promise");
                    ConsoleKeyInfo input = Console.ReadKey();
                    if (input.KeyChar.ToString().ToLower() == "x") return GetPromise(i);
                }

                return computerPromise;
            }
            else
            {
                return GetPlayerPromise(i);
            }
        }

        private int GetPlayerCard(int playerIndex)
        {
            ScreenUtils.ClearPlayerCards();
            ScreenUtils.PrintTrumpCard(this.TrumpCard);
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
            ScreenUtils.PrintTrumpCard(this.TrumpCard);
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

        private int[] CardPosition(int cardIndex)
        {
            int[] position = new int[2];
            position[0] = CARDSSTARTX + (cardIndex * (CARDWIDTH + 1));
            position[1] = CARDSSTARTY;
            return position;
        }

        private void PrintPlayerCards(int playerInd, bool printCardNumber = false)
        {
            if (playerInd < 0) return;

            for (int i = 0; i < this.Hands[playerInd].Count(); i++)
            {
                int[] position = CardPosition(i);
                int x = position[0];
                int y = position[1];
                Card cardToPrint = this.Hands[playerInd].Skip(i).First();
                ScreenUtils.CardBgType cardBgType = ScreenUtils.CardBgType.BASIC;
                bool cardIsAvailable = true;
                if (this.CardInCharge != null)
                {
                    cardIsAvailable = IsValidCard(this.Hands[playerInd], i);
                    if (!cardIsAvailable) cardBgType = ScreenUtils.CardBgType.NOTAVAILABLE;
                }
                bool cardWillWin = printCardNumber && cardIsAvailable && CardWillWin(cardToPrint, this.CardInCharge, this.TableCards.ToList(), this.TrumpCard.CardSuit);
                if (cardWillWin) cardBgType = ScreenUtils.CardBgType.WINNIG;
                ScreenUtils.PrintCard(x, y, cardToPrint, cardBgType);

                if (printCardNumber)
                {
                    string cardNumberStr = (cardIsAvailable)
                                            ? (i == 9) ? "0" : $"{i+1}"
                                            : " ";
                    if (cardWillWin)
                    {
                        Console.SetCursorPosition(x + (CARDWIDTH / 2) - 2, y + CARDHEIGHT);
                        cardNumberStr = $"*{cardNumberStr}*";
                    }
                    else
                    {
                        Console.SetCursorPosition(x + (CARDWIDTH / 2) - 1, y + CARDHEIGHT);
                    }
                    
                    Console.Write($"({cardNumberStr})");
                }
            }
        }

        private void PrintPlayedCards()
        {
            if (!this.GameSettings.ShowCards || this.GameSettings.IsTotalTest) return;
            for (int i = 0; i < this.Players.Count(); i++)
            {
                if (this.TableCards[i] != null)
                {
                    Card playedCard = this.TableCards[i];
                    ScreenUtils.CardBgType cardBgType = ScreenUtils.CardBgType.LOSING;
                    if (i == this.PlayerInCharge)
                    {
                        cardBgType = ScreenUtils.CardBgType.INCHARGE;
                    }
                    else
                    {
                        if (CardWillWin(playedCard, this.CardInCharge, this.TableCards.ToList(), this.TrumpCard.CardSuit))
                            cardBgType = ScreenUtils.CardBgType.WINNIG;
                    }

                    PrintPlayedCard(i, playedCard, cardBgType);
                }
            }
        }

        private int[] PlayedCardPosition(int playerInd)
        {
            int[] position = new int[2];
            position[0] = (playerInd * (COLWIDTH + 1)) + 5;
            position[1] = 3;
            return position;
        }


        private void PrintPlayedCard(int playerInd, Card playedCard, ScreenUtils.CardBgType cardBgType)
        {
            int[] position = PlayedCardPosition(playerInd);
            ScreenUtils.PrintCard(position[0], position[1], playedCard, cardBgType);
        }

        private void UIShowWinner(string winnerName)
        {
            if (this.GameSettings.IsTotalTest) return;
            Console.SetCursorPosition(CARDSSTARTX + (CARDWIDTH / 2) - 1, CARDSSTARTY + CARDHEIGHT + 2);
            Console.Write($"Kierroksen voitti {winnerName}");
        }

        private void UIShowPromises()
        {
            if (this.GameSettings.IsTotalTest) return;
            int totalPromises = 0;
            Console.SetCursorPosition(0, 1);
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < this.Players.Count(); i++)
            {
                totalPromises+= this.Promises[i].PromiseNumber;
                string promiseStr = $"{this.Promises[i].PromiseNumber}";
                promiseStr+= $" / {this.RoundWins[i]}";
                Console.Write("|  ");
                int promiseStatus = this.RoundWins[i] - this.Promises[i].PromiseNumber;
                Console.BackgroundColor = ConsoleColor.Black;
                if (promiseStatus == 0) Console.BackgroundColor = ConsoleColor.Green;
                if (promiseStatus > 0) Console.BackgroundColor = ConsoleColor.Red;
                Console.Write(promiseStr.PadRight(COLWIDTH - 6, ' '));
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write("     ");
            }
            Console.Write($"  {totalPromises} / {this.CardsInRound}");

            Console.SetCursorPosition(0, 2);
            for (int i = 0; i < this.Players.Count(); i++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("|  ");

                Console.ForegroundColor = ConsoleColor.Green;
                for (int j = 0; j < this.RoundWins[i] && j < this.Promises[i].PromiseNumber; j++)
                {
                    Console.Write("●");
                }
                Console.ForegroundColor = ConsoleColor.White;
                for (int j = this.RoundWins[i]; j < this.Promises[i].PromiseNumber; j++)
                {
                    Console.Write("○");
                }
                Console.ForegroundColor = ConsoleColor.Red;
                for (int j = this.RoundWins[i]; j > this.Promises[i].PromiseNumber; j--)
                {
                    Console.Write("●");
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("".PadRight(COLWIDTH - Math.Max(this.RoundWins[i], this.Promises[i].PromiseNumber) - 1, ' '));
            }


        }

        private void UIShowNames(int boldPlayer = -1)
        {
            if (this.GameSettings.IsTotalTest) return;
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
            Console.Write("| TOTAL");
        }

        private void UiAskPromise(int playerInd)
        {
            UIShowNames();
            Console.SetCursorPosition(0, 1);
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < playerInd; i++)
            {
                string promiseStr = $"{this.Promises[i].PromiseNumber}";
                Console.Write("|  ");
                if (!this.GameSettings.IsBotMatch) Thread.Sleep(WAITTIME);
                Console.Write(promiseStr.PadRight(COLWIDTH - 1, ' '));
            }

        }
    }
}
