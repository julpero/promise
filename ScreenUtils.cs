using System;
using DSI.Deck;
using System.Threading;

namespace promise
{
    public static class ScreenUtils
    {
        const int CARDWIDTH = 11;
        const int CARDHEIGHT = 7;
        const int CARDSSTARTY = 22;
        const int TRUMPSTARTY = 12;
        const int CARDSSTARTX = 4;

        const int ANIMATIONTIME = 200;
        const int MAXFRAMETIME = 50;
        const int MAXFRAMES = 10;

        private class FramePosition
        {
            public int X;
            public int Y;

            public FramePosition(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        public enum CardBgType
        {
            BASIC,
            NOTAVAILABLE,
            WINNIG,
            INCHARGE,
            LOSING
        }

        public static void ClearScreen(bool totalTest = false)
        {
            if (totalTest) return;
            try
            {
                Console.Clear();
            }
            catch
            {

            }
        }

        public static void ClearCards(bool totalTest = false)
        {
            if (totalTest) return;
            ClearPlayedCards();
            ClearPlayerCards();
        }

        public static void ClearPlayerCards(bool totalTest = false)
        {
            if (totalTest) return;
            string clearString = "".PadRight(((CARDWIDTH + 1) * 10) + CARDSSTARTX, ' ');
            for (int i = CARDSSTARTY; i < CARDSSTARTY + CARDHEIGHT + 4; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(clearString);
            }
        }

        public static void ClearPlayedCards(bool totalTest = false)
        {
            if (totalTest) return;
            string clearString = "".PadRight(((CARDWIDTH + 1) * 10) + CARDSSTARTX, ' ');
            for (int i = 3; i < 5 + CARDHEIGHT + 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(clearString);
            }
        }

        public static void ClearGetPromise(bool totalTest = false)
        {
            if (totalTest) return;
            Console.SetCursorPosition(0, 5);
            Console.Write("".PadRight(50, ' '));
        }

        public static void ResetConsoleColors()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
            }
            catch
            {

            }
        }

        private static char SuiteToImg(CardSuit cardSuit)
        {
            switch (cardSuit)
            {
                case CardSuit.Clubs: return '♣'; //((char)0xA67);
                case CardSuit.Hearts: return '♥'; //((char)0xA69);
                case CardSuit.Diamonds: return '♦'; //((char)0xA6A);
                case CardSuit.Spades: return '♠'; //((char)0xA64);
            }
            return ' ';
        }

#region cardimages
        private static void PrintCardImg2(int x, int y, char suite)
        {
            Console.SetCursorPosition(x + 5, y + 1);
            Console.Write(suite);
            Console.SetCursorPosition(x + 5, y + 5);
            Console.Write(suite);
        }
        private static void PrintCardImg3(int x, int y, char suite)
        {
            Console.SetCursorPosition(x + 5, y + 1);
            Console.Write(suite);
            Console.SetCursorPosition(x + 5, y + 3);
            Console.Write(suite);
            Console.SetCursorPosition(x + 5, y + 5);
            Console.Write(suite);
        }
        private static void PrintCardImg4(int x, int y, char suite)
        {
            Console.SetCursorPosition(x + 3, y + 1);
            Console.Write(suite);
            Console.SetCursorPosition(x + 7, y + 1);
            Console.Write(suite);
            Console.SetCursorPosition(x + 3, y + 5);
            Console.Write(suite);
            Console.SetCursorPosition(x + 7, y + 5);
            Console.Write(suite);
        }
        private static void PrintCardImg5(int x, int y, char suite)
        {
            Console.SetCursorPosition(x + 3, y + 1);
            Console.Write(suite);
            Console.SetCursorPosition(x + 7, y + 1);
            Console.Write(suite);
            Console.SetCursorPosition(x + 5, y + 3);
            Console.Write(suite);
            Console.SetCursorPosition(x + 3, y + 5);
            Console.Write(suite);
            Console.SetCursorPosition(x + 7, y + 5);
            Console.Write(suite);
        }
        private static void PrintCardImg6(int x, int y, char suite)
        {
            Console.SetCursorPosition(x + 3, y + 1);
            Console.Write(suite);
            Console.SetCursorPosition(x + 7, y + 1);
            Console.Write(suite);
            Console.SetCursorPosition(x + 3, y + 3);
            Console.Write(suite);
            Console.SetCursorPosition(x + 7, y + 3);
            Console.Write(suite);
            Console.SetCursorPosition(x + 3, y + 5);
            Console.Write(suite);
            Console.SetCursorPosition(x + 7, y + 5);
            Console.Write(suite);
        }
        private static void PrintCardImg7(int x, int y, char suite)
        {
            Console.SetCursorPosition(x + 3, y + 1);
            Console.Write(suite);
            Console.SetCursorPosition(x + 7, y + 1);
            Console.Write(suite);
            Console.SetCursorPosition(x + 5, y + 2);
            Console.Write(suite);
            Console.SetCursorPosition(x + 3, y + 3);
            Console.Write(suite);
            Console.SetCursorPosition(x + 7, y + 3);
            Console.Write(suite);
            Console.SetCursorPosition(x + 3, y + 5);
            Console.Write(suite);
            Console.SetCursorPosition(x + 7, y + 5);
            Console.Write(suite);
        }
        private static void PrintCardImg8(int x, int y, char suite)
        {
            Console.SetCursorPosition(x + 3, y + 1);
            Console.Write(suite);
            Console.SetCursorPosition(x + 7, y + 1);
            Console.Write(suite);
            Console.SetCursorPosition(x + 5, y + 2);
            Console.Write(suite);
            Console.SetCursorPosition(x + 3, y + 3);
            Console.Write(suite);
            Console.SetCursorPosition(x + 7, y + 3);
            Console.Write(suite);
            Console.SetCursorPosition(x + 5, y + 4);
            Console.Write(suite);
            Console.SetCursorPosition(x + 3, y + 5);
            Console.Write(suite);
            Console.SetCursorPosition(x + 7, y + 5);
            Console.Write(suite);
        }
        private static void PrintCardImg9(int x, int y, char suite)
        {
            Console.SetCursorPosition(x + 3, y + 1);
            Console.Write(suite);
            Console.SetCursorPosition(x + 5, y + 1);
            Console.Write(suite);
            Console.SetCursorPosition(x + 7, y + 1);
            Console.Write(suite);
            Console.SetCursorPosition(x + 3, y + 3);
            Console.Write(suite);
            Console.SetCursorPosition(x + 5, y + 3);
            Console.Write(suite);
            Console.SetCursorPosition(x + 7, y + 3);
            Console.Write(suite);
            Console.SetCursorPosition(x + 3, y + 5);
            Console.Write(suite);
            Console.SetCursorPosition(x + 5, y + 5);
            Console.Write(suite);
            Console.SetCursorPosition(x + 7, y + 5);
            Console.Write(suite);
        }
        private static void PrintCardImg10(int x, int y, char suite)
        {
            Console.SetCursorPosition(x + 4, y + 1);
            Console.Write(suite);
            Console.SetCursorPosition(x + 6, y + 1);
            Console.Write(suite);
            Console.SetCursorPosition(x + 5, y + 2);
            Console.Write(suite);
            Console.SetCursorPosition(x + 2, y + 3);
            Console.Write(suite);
            Console.SetCursorPosition(x + 4, y + 3);
            Console.Write(suite);
            Console.SetCursorPosition(x + 6, y + 3);
            Console.Write(suite);
            Console.SetCursorPosition(x + 8, y + 3);
            Console.Write(suite);
            Console.SetCursorPosition(x + 5, y + 4);
            Console.Write(suite);
            Console.SetCursorPosition(x + 4, y + 5);
            Console.Write(suite);
            Console.SetCursorPosition(x + 6, y + 5);
            Console.Write(suite);
        }
        private static void PrintCardImgJ(int x, int y, char suite)
        {
            Console.SetCursorPosition(x + 4, y + 1);
            Console.Write(suite);
            Console.Write(suite);
            Console.Write(suite);
            Console.SetCursorPosition(x + 6, y + 2);
            Console.Write(suite);
            Console.SetCursorPosition(x + 6, y + 3);
            Console.Write(suite);
            Console.SetCursorPosition(x + 2, y + 4);
            Console.Write(suite);
            Console.SetCursorPosition(x + 6, y + 4);
            Console.Write(suite);
            Console.SetCursorPosition(x + 3, y + 5);
            Console.Write(suite);
            Console.Write(suite);
            Console.Write(suite);
        }
        private static void PrintCardImgQ(int x, int y, char suite)
        {
            Console.SetCursorPosition(x + 3, y + 1);
            Console.Write(suite);
            Console.Write(suite);
            Console.Write(suite);
            Console.SetCursorPosition(x + 2, y + 2);
            Console.Write(suite);
            Console.SetCursorPosition(x + 6, y + 2);
            Console.Write(suite);
            Console.SetCursorPosition(x + 2, y + 3);
            Console.Write(suite);
            Console.SetCursorPosition(x + 5, y + 3);
            Console.Write(suite);
            Console.Write(suite);
            Console.SetCursorPosition(x + 2, y + 4);
            Console.Write(suite);
            Console.SetCursorPosition(x + 6, y + 4);
            Console.Write(suite);
            Console.SetCursorPosition(x + 3, y + 5);
            Console.Write(suite);
            Console.Write(suite);
            Console.Write(suite);
            Console.SetCursorPosition(x + 7, y + 5);
            Console.Write(suite);
        }
        private static void PrintCardImgK(int x, int y, char suite)
        {
            Console.SetCursorPosition(x + 3, y + 1);
            Console.Write(suite);
            Console.SetCursorPosition(x + 6, y + 1);
            Console.Write(suite);
            Console.SetCursorPosition(x + 3, y + 2);
            Console.Write(suite);
            Console.SetCursorPosition(x + 5, y + 2);
            Console.Write(suite);
            Console.SetCursorPosition(x + 3, y + 3);
            Console.Write(suite);
            Console.Write(suite);
            Console.SetCursorPosition(x + 3, y + 4);
            Console.Write(suite);
            Console.SetCursorPosition(x + 5, y + 4);
            Console.Write(suite);
            Console.SetCursorPosition(x + 3, y + 5);
            Console.Write(suite);
            Console.SetCursorPosition(x + 6, y + 5);
            Console.Write(suite);
        }
        private static void PrintCardImgA(int x, int y, char suite)
        {
            Console.SetCursorPosition(x + 5, y + 1);
            Console.Write(suite);
            Console.SetCursorPosition(x + 4, y + 2);
            Console.Write(suite);
            Console.SetCursorPosition(x + 6, y + 2);
            Console.Write(suite);
            Console.SetCursorPosition(x + 3, y + 3);
            Console.Write(suite);
            Console.SetCursorPosition(x + 7, y + 3);
            Console.Write(suite);
            Console.SetCursorPosition(x + 3, y + 4);
            Console.Write(suite);
            Console.Write(suite);
            Console.Write(suite);
            Console.Write(suite);
            Console.Write(suite);
            Console.SetCursorPosition(x + 3, y + 5);
            Console.Write(suite);
            Console.SetCursorPosition(x + 7, y + 5);
            Console.Write(suite);
        }
#endregion

        private static void PrintCardVal(int x, int y, CardValue cardValue)
        {
            int cardValueInt = (int)cardValue;
            string cardValueStr = "";
            switch (cardValueInt)
            {
                case 11: cardValueStr = "J";break;
                case 12: cardValueStr = "Q";break;
                case 13: cardValueStr = "K";break;
                case 14: cardValueStr = "A";break;
                default: cardValueStr = cardValueInt.ToString();break;
            }
            Console.SetCursorPosition(x + 1, y);
            Console.Write(cardValueStr);
            Console.SetCursorPosition(x + (CARDWIDTH - 3), y + CARDHEIGHT - 1);
            Console.Write(cardValueStr.PadLeft(2, ' '));
        }

        private static void PrintCardBackGround(int x, int y)
        {
            for (int i = 0; i < CARDHEIGHT; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write("".PadLeft(CARDWIDTH, ' '));
            }
        }

        private static void printCardImg(int x, int y, Card card)
        {
            char suite = SuiteToImg(card.CardSuit);
            switch (card.CardValue)
            {
                case CardValue.two: PrintCardImg2(x, y, suite);break;
                case CardValue.three: PrintCardImg3(x, y, suite);break;
                case CardValue.four: PrintCardImg4(x, y, suite);break;
                case CardValue.five: PrintCardImg5(x, y, suite);break;
                case CardValue.six: PrintCardImg6(x, y, suite);break;
                case CardValue.seven: PrintCardImg7(x, y, suite);break;
                case CardValue.eight: PrintCardImg8(x, y, suite);break;
                case CardValue.nine: PrintCardImg9(x, y, suite);break;
                case CardValue.ten: PrintCardImg10(x, y, suite);break;
                case CardValue.jack: PrintCardImgJ(x, y, suite);break;
                case CardValue.queen: PrintCardImgQ(x, y, suite);break;
                case CardValue.king: PrintCardImgK(x, y, suite);break;
                case CardValue.ace: PrintCardImgA(x, y, suite);break;
            }
        }

        public static void PrintTrumpCard(Card trumpCard)
        {
            Console.SetCursorPosition(CARDSSTARTX + 2, TRUMPSTARTY);
            Console.Write("VALTTI");
            ScreenUtils.PrintCard(CARDSSTARTX, TRUMPSTARTY + 1, trumpCard);
        }

        public static void AnimateCard(int fromX, int fromY, int toX, int toY, Card card, CardBgType cardBgType = CardBgType.BASIC, Card trumpCard = null, bool totalTest = false)
        {
            if (totalTest) return;
            int frameCountX = Math.Abs(fromX - toX) + 1;
            int frameCountY = Math.Abs(fromY - toY) + 1;
            int frameCount = Math.Min(Math.Max(frameCountX, frameCountY), MAXFRAMES);
            if (frameCount <= 1) return;

            int frameDuration = Math.Min(ANIMATIONTIME / frameCount, MAXFRAMETIME);
            double movementX = (double)(frameCountX-1) / (double)frameCount;
            double movementY = (double)(frameCountY-1) / (double)frameCount;

            FramePosition[] framePositions = new FramePosition[frameCount];
            for (int i = 0; i < frameCount; i++)
            {
                if (i == 0)
                {
                    framePositions[i] = new FramePosition(fromX, fromY);
                    continue;
                }
                else if (i == frameCount - 1)
                {
                    framePositions[i] = new FramePosition(toX, toY);
                    continue;
                }
                else
                {
                    int x = (fromX < toX) ? (int)(fromX + (movementX * i)) : (int)(fromX - (movementX * i));
                    int y = (fromY < toY) ? (int)(fromY + (movementY * i)) : (int)(fromY - (movementY * i));
                    framePositions[i] = new FramePosition(x, y);
                    continue;
                }
            }

            for (int i = 0; i < frameCount; i++)
            {
                if (i > 0)
                {
                    ClearCard(framePositions[i-1].X, framePositions[i-1].Y);
                    if (trumpCard != null && OverLapsTrumpCard(framePositions[i-1].X, framePositions[i-1].Y)) PrintTrumpCard(trumpCard);
                }
                PrintCard(framePositions[i].X, framePositions[i].Y, card, cardBgType, totalTest);
                if (i != frameCount - 1)
                {
                    Thread.Sleep(frameDuration);
                }
            }
        }

        private static bool OverLapsTrumpCard(int x, int y)
        {
            if (x > CARDSSTARTX + CARDWIDTH + 2) return false;
            if (y > TRUMPSTARTY + CARDHEIGHT + 2) return false;
            if (y + CARDHEIGHT < TRUMPSTARTY) return false;
            return true;
        }

        public static void ClearCard(int x, int y)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            PrintCardBackGround(x, y);
        }

        public static void PrintCard(int x, int y, Card card, CardBgType cardBgType = CardBgType.BASIC, bool totalTest = false)
        {
            if (totalTest) return;
            switch (cardBgType)
            {
                case CardBgType.WINNIG: Console.BackgroundColor = ConsoleColor.Yellow; break;
                case CardBgType.INCHARGE: Console.BackgroundColor = ConsoleColor.White; break;
                case CardBgType.LOSING: Console.BackgroundColor = ConsoleColor.Gray; break;
                case CardBgType.NOTAVAILABLE: Console.BackgroundColor = ConsoleColor.Gray; break;
                default: Console.BackgroundColor = ConsoleColor.White; break;
            }
            switch (card.CardSuit)
            {
                case CardSuit.Clubs: Console.ForegroundColor = ConsoleColor.DarkGreen; break;
                case CardSuit.Hearts: Console.ForegroundColor = ConsoleColor.Red; break;
                case CardSuit.Diamonds: Console.ForegroundColor = ConsoleColor.Blue; break;
                case CardSuit.Spades: Console.ForegroundColor = ConsoleColor.Black; break;
            }
            
            PrintCardBackGround(x, y);
            PrintCardVal(x, y, card.CardValue);
            printCardImg(x, y, card);

            ScreenUtils.ResetConsoleColors();
        }

    }
}