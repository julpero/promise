using System;
using DSI.Deck;
using System.Collections.Generic;
using System.Linq;

namespace promise
{
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

            private int AnalyzeDodgeable()
            {
                int retVal = 50;
                if (this.CardCount == 0)
                {
                    retVal = 100;
                } 
                else if (this.SmallValuesInSuit > this.BigValuesInSuit)
                {
                    if (this.SmallestValuesInSuit > 0)
                    {
                        retVal = 95;
                    }
                    else
                    {
                        retVal = 85;
                    }
                }
                else
                {
                    if (this.BiggestValuesInSuit > 0)
                    {
                        retVal = 15;
                    }
                    else
                    {
                        retVal = 25;
                    }
                }
                return retVal;
            }

            public AnalyzedSuit(List<Card> cards, CardSuit suit, bool isTrump = false)
            {
                CardCount = cards == null ? 0 : cards.Count();
                Suit = suit;
                IsTrump = isTrump;
                BiggestValuesInSuit = BiggestValuesInSuit(cards);
                BigValuesInSuit = BigValuesInSuit(cards);
                SmallestValuesInSuit = SmallestValuesInSuit(cards);
                SmallValuesInSuit = SmallValuesInSuit(cards);
                IsDodgeable = AnalyzeDodgeable();
            }
        }

        private static Random rand = new Random();

        private static bool CheckRandom(int checkValue)
        {
            int testInt = rand.Next(0, 101);
            return (testInt < checkValue);
        }

        private static int BiggestValuesInSuit(List<Card> cards)
        {
            if (cards == null || !cards.Any()) return 0;
            int retVal = 0;
            for (int i = 14; i > 1; i--)
            {
                if (cards.Any(x => (int)x.CardValue == i))
                {
                    retVal++;
                }
                else
                {
                    break;
                }
            }
            return retVal;
        }

        private static int BigValuesInSuit(List<Card> cards)
        {
            if (cards == null || !cards.Any()) return 0;
            int retVal = 0;
            for (int i = 14; i >= 11; i--)
            {
                if (cards.Any(x => (int)x.CardValue == i))
                {
                    retVal++;
                }
            }
            return retVal;
        }

        private static int SmallestValuesInSuit(List<Card> cards)
        {
            if (cards == null || !cards.Any()) return 0;
            int retVal = 0;
            for (int i = 2; i <= 14; i++)
            {
                if (cards.Any(x => (int)x.CardValue == i))
                {
                    retVal++;
                }
                else
                {
                    break;
                }
            }
            return retVal;
        }

        private static int SmallValuesInSuit(List<Card> cards)
        {
            if (cards == null || !cards.Any()) return 0;
            int retVal = 0;
            for (int i = 2; i <= 6; i++)
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

        private static int BiggestTrumpsInHand(List<Card> myHand, Card trumpCard)
        {
            int retVal = 0;

            for (int i = 14; i > 1; i--)
            {
                if (myHand.Any(x => (int)x.CardValue == i))
                {
                    retVal++;
                }
                else if ((int)trumpCard.CardValue == i)
                {
                    continue;
                }
                else
                {
                    break;
                }
            }

            return retVal < 0 ? 0 : retVal;
        }

        public static Promise MakePromise(List<Card> hand, int playersInGame, Card trumpCard, Promise[] promises)
        {
            int cardsInRound = hand.Count();
            bool bigZeroRound = cardsInRound > 5;

            int playersPromised = PlayersPromised(promises);
            bool iAmFirst = playersPromised == 0;
            bool iAmLast = playersPromised == playersInGame - 1;

            double avgPoints = (double)cardsInRound / (double)playersInGame;
            int cardsInGame = cardsInRound * playersInGame;
            double avgTrumpsInGame = ((double)cardsInGame / 4.0) - 1;
            double avgEachSuitInGame = ((double)cardsInGame / 4.0);
            double avgTrumpsAtPlayer = avgTrumpsInGame / (double)playersInGame;
            double avgEachSuitAtPlayer = avgEachSuitInGame / (double)playersInGame;

            int promisesLeft = cardsInRound - PromisesMade(promises);
            int bigPromises = BigPromises(promises, avgPoints);
            int zeroPromises = ZeroPromises(promises);

            List<Card> suitC = hand.Where(x => x.CardSuit == CardSuit.Clubs).ToList();
            List<Card> suitD = hand.Where(x => x.CardSuit == CardSuit.Diamonds).ToList();
            List<Card> suitH = hand.Where(x => x.CardSuit == CardSuit.Hearts).ToList();
            List<Card> suitS = hand.Where(x => x.CardSuit == CardSuit.Spades).ToList();

            List<Card> myTrumps = hand.Where(x => x.CardSuit == trumpCard.CardSuit).ToList();
            int myTrumpCount = myTrumps.Count();
            int biggestTrumpsInHand = BiggestTrumpsInHand(myTrumps, trumpCard);
            int smallerTrumpsInHand = myTrumps.Count() - biggestTrumpsInHand;

            AnalyzedSuit analyzedC = new AnalyzedSuit(suitC, CardSuit.Clubs, CardSuit.Clubs == trumpCard.CardSuit);
            AnalyzedSuit analyzedD = new AnalyzedSuit(suitD, CardSuit.Diamonds, CardSuit.Diamonds == trumpCard.CardSuit);
            AnalyzedSuit analyzedH = new AnalyzedSuit(suitH, CardSuit.Hearts, CardSuit.Hearts == trumpCard.CardSuit);
            AnalyzedSuit analyzedS = new AnalyzedSuit(suitS, CardSuit.Spades, CardSuit.Spades == trumpCard.CardSuit);

            AnalyzedSuit analyzedT = new AnalyzedSuit(myTrumps, trumpCard.CardSuit, true);
            // AnalyzedSuit analyzedT = myTrumps.Any() ? new AnalyzedSuit(myTrumps, trumpCard.CardSuit, true) : null;

            // this is a fact
            double myPromise = biggestTrumpsInHand;
            bool playZero = false;

            double promiseMultiplier = 0.6;
            double averageSuitMultiplier = Math.Sqrt(avgEachSuitAtPlayer);
            if (iAmFirst) promiseMultiplier+= 0.3;
            if (iAmLast) promiseMultiplier+= 0.15;

            if (trumpCard.CardSuit != CardSuit.Clubs) myPromise+= analyzedC.BiggestValuesInSuit * averageSuitMultiplier * promiseMultiplier;
            if (trumpCard.CardSuit != CardSuit.Diamonds) myPromise+= analyzedD.BiggestValuesInSuit * averageSuitMultiplier * promiseMultiplier;
            if (trumpCard.CardSuit != CardSuit.Hearts) myPromise+= analyzedH.BiggestValuesInSuit * averageSuitMultiplier * promiseMultiplier;
            if (trumpCard.CardSuit != CardSuit.Spades) myPromise+= analyzedS.BiggestValuesInSuit * averageSuitMultiplier * promiseMultiplier;


            promiseMultiplier = 0.3;
            if (iAmFirst) promiseMultiplier+= 0.4;
            if (iAmLast) promiseMultiplier+= 0.25;

            if (trumpCard.CardSuit != CardSuit.Clubs) myPromise+= (analyzedC.BigValuesInSuit - analyzedC.BiggestValuesInSuit) * averageSuitMultiplier * promiseMultiplier;
            if (trumpCard.CardSuit != CardSuit.Diamonds) myPromise+= (analyzedD.BigValuesInSuit - analyzedD.BiggestValuesInSuit) * averageSuitMultiplier * promiseMultiplier;
            if (trumpCard.CardSuit != CardSuit.Hearts) myPromise+= (analyzedH.BigValuesInSuit - analyzedH.BiggestValuesInSuit) * averageSuitMultiplier * promiseMultiplier;
            if (trumpCard.CardSuit != CardSuit.Spades) myPromise+= (analyzedS.BigValuesInSuit - analyzedS.BiggestValuesInSuit) * averageSuitMultiplier * promiseMultiplier;

            if (biggestTrumpsInHand == 0)
            {
                // should play zero?
                if (myTrumpCount < avgTrumpsAtPlayer && analyzedT.BigValuesInSuit < analyzedT.SmallValuesInSuit)
                {
                    // very likely zero
                    if (CheckRandom(analyzedC.IsDodgeable) && CheckRandom(analyzedD.IsDodgeable) && CheckRandom(analyzedH.IsDodgeable) && CheckRandom(analyzedS.IsDodgeable))
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


            int finalPromise;
            if (myPromise == 0 || playZero)
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
            

            if (finalPromise > cardsInRound) finalPromise = cardsInGame;

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

        public static int PlayCard(int playerInd, List<Card> hand, Card cardInCharge, Card trumpCard, Card[] tableCards, int cardsInRound, Promise[] promises, int[] roundWins, List<Card> cardsPlayedInRounds, PlayerInfo[] playerInfos)
        {
            PlayingMethod myMethod = PlayingMethod.NOTSET;

            int myPromises = promises[playerInd].PromiseNumber;
            int myCurrentWins = roundWins[playerInd];
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
            int biggestTrumpsInHand = BiggestTrumpsInHand(myTrumps, trumpCard);
            int smallerTrumpsInHand = myTrumps.Count() - biggestTrumpsInHand;

            AnalyzedSuit analyzedC = new AnalyzedSuit(suitC, CardSuit.Clubs, CardSuit.Clubs == trumpCard.CardSuit);
            AnalyzedSuit analyzedD = new AnalyzedSuit(suitD, CardSuit.Diamonds, CardSuit.Diamonds == trumpCard.CardSuit);
            AnalyzedSuit analyzedH = new AnalyzedSuit(suitH, CardSuit.Hearts, CardSuit.Hearts == trumpCard.CardSuit);
            AnalyzedSuit analyzedS = new AnalyzedSuit(suitS, CardSuit.Spades, CardSuit.Spades == trumpCard.CardSuit);

            AnalyzedSuit analyzedT = myTrumps.Any() ? new AnalyzedSuit(suitS, trumpCard.CardSuit, true) : null;

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
            }

            int selectedCardInd = 0;

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
                    selectedCardInd = rand.Next(hand.Count());
                    return selectedCardInd;
                }
            }
            else
            {
                List<Card> possibleCards = (hand.Any(x => x.CardSuit == cardInCharge.CardSuit))
                                            ? hand.Where(x => x.CardSuit == cardInCharge.CardSuit).ToList()
                                            : hand;
                selectedCardInd = rand.Next(possibleCards.Count());
                // this is default card
                Card selectedCard = possibleCards.Skip(selectedCardInd).First();

                if (myMethod == PlayingMethod.TRYTODODGE || myMethod == PlayingMethod.TRYTOSKIP)
                {
                    List<Card> losingCards = LosingCards(possibleCards, cardInCharge, tableCards.ToList(), trumpCard.CardSuit);
                    if (losingCards.Any())
                    {
                        selectedCardInd = rand.Next(losingCards.Count());
                        selectedCard = losingCards.Skip(selectedCardInd).First();
                    }
                }
                else
                {
                    List<Card> winningCards = WinningCards(possibleCards, cardInCharge, tableCards.ToList(), trumpCard.CardSuit);
                    if (winningCards.Any())
                    {
                        selectedCardInd = rand.Next(winningCards.Count());
                        selectedCard = winningCards.Skip(selectedCardInd).First();
                    }
                }
                
                return GetCardIndexFromHand(hand, selectedCard);
            }
        }

    }
}