using System;
using DSI.Deck;
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 

namespace promise
{
    public enum PlayerType
    {
        COMPUTER,
        HUMAN
    }

    class Player
    {
        public PlayerType PlayerType {get; set;}
        public string PlayerName {get; set;}

        public Player(int playerNro)
        {
            int input = 0;
            Console.Write($"Pelaajan {playerNro} tyyppi, 0 = tietokone, 1 = ihminen: ");

            while (!Int32.TryParse(Console.ReadLine(), out input))
            {
                Console.Write($"Pelaajan {playerNro} tyyppi, 0 = tietokone, 1 = ihminen: ");
            }
            if (input == 1)
            {
                this.PlayerType = PlayerType.HUMAN;
                Console.Write($"Pelaajan {playerNro} nimi: ");
                this.PlayerName = Console.ReadLine();
            }
            else
            {
                this.PlayerType = PlayerType.COMPUTER;
                this.PlayerName = $"Computer {playerNro}";
            }
        }
    }
}
