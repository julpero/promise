using System;
using System.Collections.Generic;

namespace DSI.Deck
{
    //To Do: Interface to be defined...
    public class Card
    {
        private readonly CardSuit suit;
        private readonly CardValue value;

        public Card(int iSuit, int iValue)
        {
            suit = (CardSuit)(iSuit);
            value = (CardValue)(iValue);
        }

        public CardSuit CardSuit
        {
            get { return suit; }
        }

        public CardValue CardValue
        {
            get { return value; }
        }

        /// <summary>
        /// Program Entry Point - Main Function
        /// </summary>
        /// <param name="args"></param>
        /// <returns>Zero and exits program</returns>
        static int TestMain(string[] args)
        { 

            int howMany = 0;

            //cardDeck1 used for Draw function
            Deck cardDeck1 = new Deck();
            List<Card> hand1 = new List<Card>();

            cardDeck1.Shuffle();
            howMany = cardDeck1.sizeOfHand();
            hand1 = cardDeck1.Draw(howMany);

            //cardDeck2 used for DrawSorted function
            Deck cardDeck2 = new Deck();
            List<Card> hand2 = new List<Card>();

            cardDeck2.Shuffle();
            howMany = cardDeck2.sizeOfHand();
            hand2 = cardDeck2.DrawSorted(howMany);

            //Display un-sorted hand
            Console.WriteLine("\nUN-SORTED:");

            for (int i = 0; i < hand1.Count; i++)
            {
                Console.WriteLine("{0} of {1}", hand1[i].CardValue, hand1[i].CardSuit);
            }

            //Display sorted hand
            Console.WriteLine("\nSORTED:");

            for (int i = 0; i < hand2.Count; i++)
            {
                Console.WriteLine("{0} of {1}", hand2[i].CardValue, hand2[i].CardSuit);
            }

            Console.ReadKey();
            return 0;
        }

    }
    //Card Suits
    public enum CardSuit
    {
        Clubs = 0,
        Diamonds = 1,
        Hearts = 2,
        Spades = 3,
    }
    //Card Values - Assuming that Ace is high
    public enum CardValue
    {
        two = 2,
        three = 3,
        four = 4,
        five = 5,
        six = 6,
        seven = 7,
        eight = 8,
        nine = 9,
        ten = 10,
        jack = 11,
        queen = 12,
        king = 13,
        ace = 14,
    }
}