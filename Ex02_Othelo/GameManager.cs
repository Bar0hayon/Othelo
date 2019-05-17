using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Ex02_Othelo
{
    public class GameManager
    {
        private const int k_CapitalLetterQ = 16;
        private string m_Player1;
        private string m_Player2;
        private bool v_IsVsComputer;
        private eCell m_TurnOf = eCell.White;
        private GameBoardEngine m_GameBoardEngine;
        private int m_BoardSize;
        private int m_CountBlack = 0, m_CountWhite = 0;

        public void StartMenu()
        {
            SetUserName(ref m_Player1);
            v_IsVsComputer = GameUI.IsVsComputer();
            if (v_IsVsComputer)
            {
                m_Player2 = "Computer";
            }
            else
            {
                do
                {
                    SetUserName(ref m_Player2);
                    if (m_Player1 == m_Player2)
                    {
                        GameUI.PrintMassage("UserName is allready in use! please use a defferent UserName");
                    }
                }
                while (m_Player1 == m_Player2);
            }

            m_BoardSize = GameUI.GetBoardSize();
            m_GameBoardEngine = new GameBoardEngine(m_BoardSize);
            startGame();
        }

        private void startGame()
        {
            Point userMove;
            while (true)
            {
                GameUI.PrintBoard(m_GameBoardEngine.Board, m_TurnOf);
                m_GameBoardEngine.SetLegalMoves(m_TurnOf);
                if (m_GameBoardEngine.isLegalMoveExists())
                {
                    userMove = getUserMove();
                    m_GameBoardEngine.MakeUserMove(userMove, m_TurnOf);
                    toggleTurn();
                }
                else
                {
                    toggleTurn();
                    m_GameBoardEngine.SetLegalMoves(m_TurnOf); //// Duplicate ----> to change!!!
                    if (m_GameBoardEngine.isLegalMoveExists())
                    {
                        GameUI.PrintBoard(m_GameBoardEngine.Board, m_TurnOf);
                        GameUI.PrintMassage("No legal moves! turn skipped...");
                        userMove = getUserMove();
                        m_GameBoardEngine.MakeUserMove(userMove, m_TurnOf);
                    }
                    else
                    {
                        GameUI.PrintMassage("\nThere aren't any legal moves for both players! Game is over!\n");
                        endMatch();
                        playRematchGame();
                        Environment.Exit(0);
                    }
                }
            }
        }

        private void playRematchGame()
        {
            string userInput = null;
            do
            {
                GameUI.PrintMassage("\nGo for a rematch? (yes/no)");
                userInput = Console.ReadLine();
                if (userInput.ToLower() == "yes")
                {
                    m_CountBlack = 0;
                    m_CountWhite = 0;
                    m_TurnOf = eCell.White;
                    m_GameBoardEngine = new GameBoardEngine(m_BoardSize);
                    startGame();
                }
                else if (userInput.ToLower() != "no")
                {
                    GameUI.PrintMassage("ERROR: please enter a valid input");
                }
            }
            while (userInput.ToLower() != "no");
            GameUI.EndGameMessage();
        }

        private void endMatch()
        {
            CalculateAndPrintPlayersScore(m_GameBoardEngine.Board, ref m_CountBlack, ref m_CountWhite);
            GameUI.PrintGameFinalResults(m_CountBlack, m_CountWhite, GetUserName(eCell.Black), GetUserName(eCell.White));
        }

        private void endGame()
        {
            CalculateAndPrintPlayersScore(m_GameBoardEngine.Board, ref m_CountBlack, ref m_CountWhite);
            GameUI.PrintGameFinalResults(m_CountBlack, m_CountWhite, GetUserName(eCell.Black), GetUserName(eCell.White));
            Console.ReadLine();
        }

        private void toggleTurn()
        {
            if (m_TurnOf == eCell.Black)
            {
                m_TurnOf = eCell.White;
            }
            else
            {
                m_TurnOf = eCell.Black;
            }
        }

        private Point getUserMove()
        {
            Point userMove;
            if (m_TurnOf == eCell.White || !v_IsVsComputer)
            {
                do
                {
                    userMove = GameUI.GetUserMove();
                    if (userMove.X == 0 && userMove.Y == k_CapitalLetterQ)
                    {
                        Environment.Exit(0);
                    }
                    else if (!m_GameBoardEngine.isValidMove(userMove))
                    {
                        GameUI.PrintMassage("CONST ILLEAGAL MOVE");
                    }
                }
                while (!m_GameBoardEngine.isValidMove(userMove));
            }
            else
            {
                userMove = m_GameBoardEngine.GetPcMove(m_TurnOf);
            }

            return userMove;
        }

        internal void SetUserName(ref string i_PlayerName)
        {
            GameUI.PrintMassage("Please enter player name:");
            i_PlayerName = Console.ReadLine();
        }

        internal string GetUserName(eCell i_SearchNameByItColor)
        {
            string PlayerName;
            if (i_SearchNameByItColor == eCell.Black)
            {
                PlayerName = m_Player2;
            }
            else if (i_SearchNameByItColor == eCell.White)
            {
                PlayerName = m_Player1;
            }
            else
            {
                PlayerName = null;
            }

            if (PlayerName == null)
            {
                Console.WriteLine("Error occured! Could not find the name of the User!");
            }

            return PlayerName;
        }

        internal void CalculateAndPrintPlayersScore(eCell[,] i_GameBoard, ref int io_CountBlack, ref int io_CountWhite)
        {
            int LengthOfRowsAndColumns = (int)Math.Sqrt(i_GameBoard.Length);
            for (int i = 0; i < LengthOfRowsAndColumns; i++)
            {
                for (int j = 0; j < LengthOfRowsAndColumns; j++)
                {
                    if (i_GameBoard[i, j] == eCell.Black)
                    {
                        io_CountBlack++;
                    }
                    else if (i_GameBoard[i, j] == eCell.White)
                    {
                        io_CountWhite++;
                    }
                }
            }
        }
    }
}