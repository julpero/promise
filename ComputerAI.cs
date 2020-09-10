using System;
using DSI.Deck;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

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

            return retVal < 0 ? 0 : retVal;
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