﻿using System;
using DSI.Deck;
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 

namespace promise
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.SetWindowSize(170, 40);
            Console.BackgroundColor = ConsoleColor.Black;
            ScreenUtils.ClearScreen();
            Game promiseGame = new Game();
            
            // Console.ForegroundColor = ConsoleColor.Black;
            // Console.WriteLine("Hello World!");
        }
    }
}
