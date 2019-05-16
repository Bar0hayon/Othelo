using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Ex02_Othelo
{
    class GameManager
    {
        private string m_Player1;
        private string m_Player2;
        private bool v_IsVsComputer;
        private eCell m_TurnOff = eCell.White;
        private GameBoardEngine m_GameBoardEngine;
        private int m_BoardSize;

        public void StartMenu()
        {
            setUserName(ref m_Player1);
            v_IsVsComputer = GameUI.IsVsComputer();
            if (!v_IsVsComputer)
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
                GameUI.PrintBoard(m_GameBoardEngine.Board, m_TurnOff);
                m_GameBoardEngine.SetLegalMoves(m_TurnOff);
                if (m_GameBoardEngine.isLegalMoveExists())
                {
                    userMove = getUserMove();
                    m_GameBoardEngine.MakeUserMove(userMove, m_TurnOff);
                    toggleTurn();
                }
                else
                {
                    toggleTurn();
                    m_GameBoardEngine.SetLegalMoves(m_TurnOff); //// Duplicate ----> to change!!!
                    if (m_GameBoardEngine.isLegalMoveExists())
                    {
                        GameUI.PrintMassage("No legal moves! turn skiped...");
                        userMove = getUserMove();
                        m_GameBoardEngine.MakeUserMove(userMove, m_TurnOff);
                    }
                    else
                    {
                        endGame();
                        break;
                    }
                }
            }
        }

        private void endGame()
        {
            CalculateAndPrintPlayersScore(m_GameBoardEngine.Board);
            Console.ReadLine();
           // GameUI.GameOver(countBlacks, countWhites);
        }

        private void toggleTurn()
        {
            if (m_TurnOff == eCell.Black)
            {
                m_TurnOff = eCell.White;
            }
            else
            {
                m_TurnOff = eCell.Black;
            }
        }

        private Point getUserMove()
        {
            Point userMove;
            do
            {
                userMove = GameUI.GetUserMove();
                if (!m_GameBoardEngine.isValidMove(userMove))
                {
                    GameUI.PrintMassage("CONST ILLEAGAL MOVE");
                }
            } while (!m_GameBoardEngine.isValidMove(userMove));

            return userMove;
        }

        internal void setUserName(ref string i_playerName)
        {
            GameUI.PrintMassage("Please enter UserName:");
            i_playerName = Console.ReadLine();
        }

        internal string getUserName(eCell i_SearchNameByItColor)
        {
            string o_playerName;
            if (i_SearchNameByItColor == eCell.Black)
            {
                o_playerName = m_Player1;
            }
            else if (i_SearchNameByItColor == eCell.White)
            {
                o_playerName = m_Player2;
            }
            else
            {
                o_playerName = null;
                Console.WriteLine("Error occured! Could not find the name of the User due to wrong color given in input");
            }
            return o_playerName;
        }

        internal void CalculateAndPrintPlayersScore(eCell[,] i_gameBoard)
        {
            int lengthOfRowsAndColumns = (int)Math.Sqrt(i_gameBoard.Length);
            int countBlack = 0, countWhite = 0;
            for (int i = 0; i < lengthOfRowsAndColumns; i++)
            {
                for (int j = 0; j < lengthOfRowsAndColumns; j++)
                {
                    if (i_gameBoard[i, j] == eCell.Black)
                    {
                        countBlack++;
                    }
                    else if (i_gameBoard[i, j] == eCell.White)
                    {
                        countWhite++;
                    }
                }
            }
            // Ex02.ConsoleUtils.Screen.Clear();
            GameUI.printPlayersScore(countBlack, getUserName(eCell.Black));
            GameUI.printPlayersScore(countWhite, getUserName(eCell.White));
            if (countBlack > countWhite)
            {
                GameUI.PrintMassage("Black color wins!");
            }
            else if (countBlack < countWhite)
            {
                GameUI.PrintMassage("White color wins!");
            }
            else
            {
                GameUI.PrintMassage("Tie! You better go and match it again!");
            }
        }
    }
}