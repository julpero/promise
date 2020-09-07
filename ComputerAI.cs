using System;
using DSI.Deck;
using System.Collections.Generic;
using System.Linq;

namespace promise
{
    public static class ComputerAI
    {
        private static Random rand = new Random();
        public static Promise MakePromise(int cardsInRound, int playersInGame)
        {
            return new Promise(rand.Next(0, cardsInRound / playersInGame + 1));
        }

        public static int PlayCard(List<Card> hand, Card cardInCharge)
        {
            if (cardInCharge == null) return 0;
            
            List<Card> possibleCards = (hand.Any(x => x.CardSuit == cardInCharge.CardSuit))
                                        ? hand.Where(x => x.CardSuit == cardInCharge.CardSuit).ToList()
                                        : hand;
            Card selectedCard = possibleCards.First();

            int index = 0;
            foreach (Card card in hand)
            {
                if (card.CardSuit == selectedCard.CardSuit && card.CardValue == selectedCard.CardValue) return index;
                index++;
            }

            return 0;
        }

    }
}