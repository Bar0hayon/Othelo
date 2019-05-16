using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Ex02_Othelo
{
    class GameUI
    {
        public static void PrintBoard(eCell[,] i_Board, eCell i_turnOf)
        {
            Ex02.ConsoleUtils.Screen.Clear();
            printTurnOf(i_turnOf);
            int lengthOfRowsAndColumns = (int)Math.Sqrt(i_Board.Length);
            printColumnIndexes(lengthOfRowsAndColumns);
            printRowSaparator(lengthOfRowsAndColumns);
            for (int row = 0; row < lengthOfRowsAndColumns; row++)
            {
                Console.Write((row + 1).ToString() + " ");
                for (int column = 0; column < lengthOfRowsAndColumns; column++)
                {
                    if (i_Board[row, column] == eCell.Empty)
                    {
                        Console.Write("|   ");
                    }
                    else if (i_Board[row, column] == eCell.White)
                    {
                        Console.Write("| O ");
                    }
                    else if (i_Board[row, column] == eCell.Black)
                    {
                        Console.Write("| X ");
                    }
                }
                Console.WriteLine("|");
                printRowSaparator(lengthOfRowsAndColumns);
            }
        }

        internal static int GetBoardSize()
        {
            bool isNumber;
            int userChose;
            int boardSize;
            do
            {
                Console.WriteLine("Please choose a Board size:");
                Console.WriteLine("\t1. 6X6");
                Console.WriteLine("\t2. 8X8");
                string userInput = Console.ReadLine();
                isNumber = int.TryParse(userInput, out userChose);
                if (!isNumber || userChose > 2 || userChose < 1)
                {
                    Console.WriteLine("ERROR: please enter a valid input");
                }
            } while (!isNumber || userChose > 2 || userChose < 1);
            if(userChose == 1)
            {
                boardSize = 6;
            }
            else
            {
                boardSize = 8;
            }

            return boardSize;
        }

        internal static bool IsVsComputer()
        {
            bool isNumber;
            int userChoise;
            do
            {
                Console.WriteLine("please choose one of the followings:");
                Console.WriteLine("\t1.start a 2 players game.");
                Console.WriteLine("\t2.start a game VS the computer");
                string userInput = Console.ReadLine();
                isNumber = int.TryParse(userInput, out userChoise);
                if (!isNumber || userChoise > 2 || userChoise < 1)
                {
                    Console.WriteLine("ERROR: please enter a valid input");
                }
            } while (!isNumber || userChoise > 2 || userChoise < 1);

            return userChoise == 2;
        }

        private static void printTurnOf(eCell i_turnOf)
        {
            Console.Write("Turn Of: ");
            if (i_turnOf == eCell.Black)
            {
                Console.WriteLine("Black");
            }
            else
            {
                Console.WriteLine("White");
            }
        }

        private static void printColumnIndexes(int i_length)
        {
            Console.Write("  ");
            for (int i = 0; i < i_length; i++)
            {
                Console.Write("  " + (char)('A' + i) + " ");
            }
            Console.WriteLine();
        }

        private static void printRowSaparator(int i_length)
        {
            Console.Write("  ");
            for (int i = 0; i < i_length * 4; i++)
            {
                Console.Write("=");
            }
            Console.WriteLine("=");
        }

        internal static Point GetUserMove()
        {
            int x = 0;
            int y = 0;
            Console.WriteLine("Enter Coordinates: (for example: E4)");
            string stringCoordinats = Console.ReadLine();
            char[] charArrayCoordinates = stringCoordinats.ToCharArray();
            if (charArrayCoordinates.Length > 0 && charArrayCoordinates.Length <= 2) 
            {
                if(charArrayCoordinates.Length == 1)
                {
                    y = (int)(charArrayCoordinates[0] - 'A');
                }
                else
                {
                    y = (int)(charArrayCoordinates[0] - 'A');
                    x = (int)(charArrayCoordinates[1] - '0' - 1);
                }
            }
            return new Point(x, y);
        }

        internal static void PrintMassage(string i_massage)
        {
            Console.WriteLine(i_massage);
        }

        internal static void GameOver(eCell countBlacks, eCell countWhites)
        {
            throw new NotImplementedException();
        }

        internal static void printPlayersScore(int i_CountByColor, String i_PlayerName)
        {
            StringBuilder i_countColorString = new StringBuilder();
            i_countColorString.Append(i_PlayerName).Append(" achieved ").Append(i_CountByColor).Append(" points.");
            Console.WriteLine(i_countColorString);
        }

        internal static void printGameFinalResults(int countBlack, int countWhite, string i_BlackPlayerName, string i_WhitePlayerName)
        {
            printPlayersScore(countBlack, i_BlackPlayerName);
            printPlayersScore(countWhite, i_WhitePlayerName);
            if (countBlack > countWhite)
            {
                PrintMassage("Black color wins!");
            }
            else if (countBlack < countWhite)
            {
                PrintMassage("White color wins!");
            }
            else
            {
                PrintMassage("Tie! You better go and match it again!");
            }
        }
    }
}