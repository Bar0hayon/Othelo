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
            m_Player1 = GameUI.GetUserName();
            v_IsVsComputer = GameUI.IsVsComputer();
            if (!v_IsVsComputer)
            {
                do
                {
                    m_Player2 = GameUI.GetUserName();
                    if(m_Player1 == m_Player2)
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
                    }
                }
            }
        }

        private void endGame()
        {
            eCell countBlacks = m_GameBoardEngine.GetCountByColor(eCell.Black);
            eCell countWhites = m_GameBoardEngine.GetCountByColor(eCell.White);
            GameUI.GameOver(countBlacks, countWhites);
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
    }
}