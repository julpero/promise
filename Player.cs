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
        public string PlayerInitials {get; set;}

        private string GenerateInitials(string name)
        {
            return name.Substring(0, 3);
        }

        public Player(int playerNro)
        {
            int playerTypeInt = 0;
            Console.Write($"Pelaajan {playerNro} tyyppi, 0 = tietokone, 1 = ihminen: ");
            var input = Console.ReadKey();
            while (!Int32.TryParse(input.KeyChar.ToString(), out playerTypeInt))
            {
                // Console.Write($"Pelaajan {playerNro} tyyppi, 0 = tietokone, 1 = ihminen: ");
                input = Console.ReadKey();
            }
            Console.WriteLine();
            if (playerTypeInt == 1)
            {
                this.PlayerType = PlayerType.HUMAN;
                Console.Write($"Pelaajan {playerNro} nimi: ");
                this.PlayerName = Console.ReadLine();
                this.PlayerInitials = GenerateInitials(this.PlayerName);
            }
            else
            {
                this.PlayerType = PlayerType.COMPUTER;
                this.PlayerName = $"Computer {playerNro}";
                this.PlayerInitials = $"Co{playerNro}";
            }
        }
    }
}
