using System;

namespace promise
{
    public static class ComputerAI
    {
        private static Random rand = new Random();
        public static Promise MakePromise(int cardsInRound)
        {
            return new Promise(rand.Next(0, cardsInRound + 1));
        }

    }
}