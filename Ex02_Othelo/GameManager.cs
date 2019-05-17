using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Ex02_Othelo
{
    class GameManager
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
            setUserName(ref m_Player1);
            v_IsVsComputer = GameUI.IsVsComputer();
            if (v_IsVsComputer)
            {
                m_Player2 = "COMPUTER";
            }
            else
            {
                do
                {
                    setUserName(ref m_Player2);
                    if (m_Player1 == m_Player2)
                    {
                        GameUI.PrintMassage("UserName is allready in use! please use a defferent UserName");
                    }
                } while (m_Player1 == m_Player2);
            }
            m_BoardSize = GameUI.GetBoardSize();
            m_GameBoardEngine = new GameBoardEngine(m_BoardSize);
            startGame();
        }

        private void startGame()
        {
            Point userMove;
            while (true) ///////TO CHANGE!!
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
                        GameUI.PrintMassage("No legal moves! turn skiped...");
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
            string i_userInput = null;
            do
            {
                GameUI.PrintMassage("\nGo for a rematch? (yes/no)");
                i_userInput = Console.ReadLine();
                if ((i_userInput).ToLower() == "yes")
                {
                    m_CountBlack = 0;
                    m_CountWhite = 0;
                    m_TurnOf = eCell.White;
                    m_GameBoardEngine = new GameBoardEngine(m_BoardSize);
                    startGame();
                }
                else if ((i_userInput).ToLower() != "no")
                {
                    GameUI.PrintMassage("ERROR: please enter a valid input");
                }
            } while ((i_userInput).ToLower() != "no");
            GameUI.endGameMessage();
        }

        private void endMatch()
        {
            CalculateAndPrintPlayersScore(m_GameBoardEngine.Board, ref m_CountBlack, ref m_CountWhite);
            GameUI.printGameFinalResults(m_CountBlack, m_CountWhite, getUserName(eCell.Black), getUserName(eCell.White));
        }

        private void endGame()
        {
            CalculateAndPrintPlayersScore(m_GameBoardEngine.Board, ref m_CountBlack, ref m_CountWhite);
            GameUI.printGameFinalResults(m_CountBlack, m_CountWhite, getUserName(eCell.Black), getUserName(eCell.White));
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
                } while (!m_GameBoardEngine.isValidMove(userMove));
            }
            else
            {
                userMove = m_GameBoardEngine.GetPcMove(m_TurnOf);
            }

            return userMove;
        }

        internal void setUserName(ref string i_playerName)
        {
            GameUI.PrintMassage("Please enter UserName:");
            i_playerName = Console.ReadLine();
        }

        public string getUserName(eCell i_SearchNameByItColor)
        {
            string o_playerName;
            if (i_SearchNameByItColor == eCell.Black)
            {
                o_playerName = m_Player2;
            }
            else if (i_SearchNameByItColor == eCell.White)
            {
                o_playerName = m_Player1;
            }
            else
            {
                o_playerName = null;
            }
            if (o_playerName == null)
            {
                Console.WriteLine("Error occured! Could not find the name of the User!");
            }
            return o_playerName;
        }

        internal void CalculateAndPrintPlayersScore(eCell[,] i_gameBoard, ref int io_CountBlack, ref int io_CountWhite)
        {
            int lengthOfRowsAndColumns = (int)Math.Sqrt(i_gameBoard.Length);
            for (int i = 0; i < lengthOfRowsAndColumns; i++)
            {
                for (int j = 0; j < lengthOfRowsAndColumns; j++)
                {
                    if (i_gameBoard[i, j] == eCell.Black)
                    {
                        io_CountBlack++;
                    }
                    else if (i_gameBoard[i, j] == eCell.White)
                    {
                        io_CountWhite++;
                    }
                }
            }
        }
    }
}