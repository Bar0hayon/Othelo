using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Ex02_Othelo
{
    internal class GameUI
    {
        public class Messages
        {
            internal const string k_InvalidInputUserNameMassage = "Could not use the current name. Please enter a different and valid name";
            internal const string k_UserInputStringEqualsNo = "no";
            internal const string k_UserInputStringEqualsYes = "yes";
            internal const string k_IsRematchGameMassage = "\nGo for a rematch? (yes/no)";
            internal const string k_BothPlayersDoesNotHaveAnyLegalMoveMassage = "\nThere aren't any legal moves for both players! Game is over!\n";
            internal const string k_PlayerDoesNotHaveAnyLegalMoveMassage = "No legal moves! turn skipped...";
            internal const string k_ComputerName = "Computer";
            internal const string k_InvalidUserNameInputMassage = "User name is allready in use! please use a different user name";
            internal const string k_UserNameDidNotFoundErrorMassage = "Error occured! Could not find the name of the User!";
            internal const string k_SetUserNameMassage = "Please enter player name:";
            internal const string k_IllegalMoveMassage = "Move is illegal. Please call for different valid move";
            internal const string k_ComputersTurnMassage = "Computer's turn ...";
            internal const string k_InvalidInputErrorMassage = "Error occured. Please enter a valid input";
            internal const string k_ChooseBoardSizeMassage = "Please choose a Board size:";
            internal const string k_BoardSizeIsSixPowerTwoMassage = "\t1. 6X6";
            internal const string k_BoardSizeIsEightPowerTwoMassage = "\t2. 8X8";
            internal const string k_ChooseOpponentToCompitteAgainstMassage = "Please choose your opponent";
            internal const string k_StartTwoPlayersGameMassage = "\t1.Start a 2 players game.";
            internal const string k_StartPlayerVsComputerGameMassage = "\t2.Start a game VS the computer";
            internal const string k_TurnOfMassage = "Turn Of: ";
            internal const string k_Black = "Black";
            internal const string k_White = "White";
            internal const string k_BlackColorWinsMassage = "Black color wins!";
            internal const string k_WhiteColorWinsMassage = "White color wins!";
            internal const string k_TieMassage = "Tie! You better go and match it again!";
            internal const string k_SetNextUserMoveMassage = "Please insert your next move: (for example: E4)";
            internal const string k_Achieved = " achieved ";
            internal const string k_Points = " points.";
            internal const string k_EndGameMassage = "\nThank You for playing our Othelo game!\n" +
                                                     "Have a good day!\n\n" +
                                                     "Press any key to continue...";
        }

        public static void PrintBoard(eCell[,] i_Board, eCell i_TurnOf)
        {
            int lengthOfRowsAndColumns = (int)Math.Sqrt(i_Board.Length);
            Ex02.ConsoleUtils.Screen.Clear();
            printTurnOf(i_TurnOf);
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
            int userChoise;
            int boardSize;
            do
            {
                PrintMassageLine(Messages.k_ChooseBoardSizeMassage);
                PrintMassageLine(Messages.k_BoardSizeIsSixPowerTwoMassage);
                PrintMassageLine(Messages.k_BoardSizeIsEightPowerTwoMassage);
                string userInput = Console.ReadLine();
                isNumber = int.TryParse(userInput, out userChoise);
                if (!isNumber || userChoise > 2 || userChoise < 1)
                {
                    PrintMassageLine(Messages.k_InvalidInputErrorMassage);
                }
            }
            while (!isNumber || userChoise > 2 || userChoise < 1);

            if (userChoise == 1)
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
                PrintMassageLine(Messages.k_ChooseOpponentToCompitteAgainstMassage);
                PrintMassageLine(Messages.k_StartTwoPlayersGameMassage);
                PrintMassageLine(Messages.k_StartPlayerVsComputerGameMassage);
                string userInput = Console.ReadLine();
                isNumber = int.TryParse(userInput, out userChoise);
                if (!isNumber || userChoise > 2 || userChoise < 1)
                {
                    PrintMassageLine(Messages.k_InvalidInputErrorMassage);
                }
            }
            while (!isNumber || userChoise > 2 || userChoise < 1);

            return userChoise == 2;
        }

        private static void printTurnOf(eCell i_TurnOf)
        {
            PrintMassageLine(Messages.k_TurnOfMassage);
            if (i_TurnOf == eCell.Black)
            {
                PrintMassageLine(Messages.k_Black);
            }
            else
            {
                PrintMassageLine(Messages.k_White);
            }
        }

        private static void printColumnIndexes(int i_Length)
        {
            Console.Write("  ");
            for (int i = 0; i < i_Length; i++)
            {
                Console.Write("  " + (char)('A' + i) + " ");
            }

            Console.WriteLine();
        }

        private static void printRowSaparator(int i_Length)
        {
            Console.Write("  ");
            for (int i = 0; i < i_Length * 4; i++)
            {
                Console.Write("=");
            }

            PrintMassageLine("=");
        }

        internal static Point GetUserMove()
        {
            int x = 0;
            int y = 0;
            string stringCoordinats;
            char[] charArrayCoordinates;
            PrintMassageLine(Messages.k_SetNextUserMoveMassage);
            stringCoordinats = Console.ReadLine();
            charArrayCoordinates = stringCoordinats.ToCharArray();
            if (charArrayCoordinates.Length > 0 && charArrayCoordinates.Length <= 2)
            {
                if (charArrayCoordinates.Length == 1)
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

        internal static void PrintMassageLine(string i_Massage)
        {
            Console.WriteLine(i_Massage);
        }

        internal static void PrintPlayersScore(int i_CountByColor, string i_PlayerName)
        {
            StringBuilder i_CountColorString = new StringBuilder();
            i_CountColorString.Append(i_PlayerName).Append(Messages.k_Achieved).Append(i_CountByColor).Append(Messages.k_Points);
            Console.WriteLine(i_CountColorString);
        }

        internal static void PrintGameFinalResults(int io_CountBlack, int io_CountWhite, string i_BlackPlayerName, string i_WhitePlayerName)
        {
            PrintPlayersScore(io_CountBlack, i_BlackPlayerName);
            PrintPlayersScore(io_CountWhite, i_WhitePlayerName);
            if (io_CountBlack > io_CountWhite)
            {
                PrintMassageLine(Messages.k_BlackColorWinsMassage);
            }
            else if (io_CountBlack < io_CountWhite)
            {
                PrintMassageLine(Messages.k_WhiteColorWinsMassage);
            }
            else
            {
                PrintMassageLine(Messages.k_TieMassage);
            }
        }

        internal static void EndGameMessage()
        {
            Ex02.ConsoleUtils.Screen.Clear();
            PrintMassageLine(GameUI.Messages.k_EndGameMassage);
            Console.ReadLine();
        }
    }
}