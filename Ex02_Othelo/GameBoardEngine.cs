using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Ex02_Othelo
{
    public class GameBoardEngine
    {
        private eCell[,] m_Board;
        private List<Point> m_LegalMoves;
        private int m_BoardSize;

        public eCell[,] Board
        {
            get
            {
                return m_Board;
            }
        }

        public GameBoardEngine(int i_boardSize)
        {
            m_BoardSize = i_boardSize;
            m_Board = new eCell[i_boardSize, i_boardSize];
            initialBoard(i_boardSize);
            m_LegalMoves = new List<Point>();
        }

        private void initialBoard(int i_boardSize)
        {
            for (int i = 0; i < i_boardSize; i++)
            {
                for (int j = 0; j < i_boardSize; j++)
                {
                    m_Board[i, j] = eCell.Empty;
                }
            }
              m_Board[((i_boardSize / 2) - 1), ((i_boardSize / 2) - 1)] = eCell.White;
              m_Board[(i_boardSize / 2), (i_boardSize / 2)] = eCell.White;
              m_Board[(i_boardSize / 2), ((i_boardSize / 2) - 1)] = eCell.Black;
              m_Board[((i_boardSize / 2) - 1), (i_boardSize / 2)] = eCell.Black;
        }

        internal void MakeUserMove(Point i_userMove, eCell i_turnOf)
        {
            eDirection[] allDirections = {eDirection.Down, eDirection.DownLeft, eDirection.DownRight
            , eDirection.Left, eDirection.Right, eDirection.Up, eDirection.UpLeft, eDirection.UpRight};
            foreach (eDirection direction in allDirections)
            {
                if (isLegalMove(direction, i_userMove.X, i_userMove.Y, i_turnOf))
                {
                    makeMove(direction, i_userMove.X, i_userMove.Y, i_turnOf);
                    m_LegalMoves.Remove(i_userMove);
                }
            }
        }

        private void makeMove(eDirection i_direction, int i_row, int i_column, eCell i_turnOf)
        {
            eCell rivalColor = getOppositeColor(i_turnOf);
            m_Board[i_row, i_column] = i_turnOf;
            Direction.Move(i_direction, ref i_row, ref i_column);
            while (m_Board[i_row, i_column] == rivalColor)
            {
                m_Board[i_row, i_column] = i_turnOf;
                Direction.Move(i_direction, ref i_row, ref i_column);
            }
        }

        internal bool isLegalMoveExists()
        {
            return (m_LegalMoves.Count > 0);
        }

        internal void SetLegalMoves(eCell i_TurnOf)
        {
            m_LegalMoves.Clear();
            for (int row = 0; row < m_BoardSize; row++)
            {
                for (int column = 0; column < m_BoardSize; column++)
                {
                    if (m_Board[row, column] == eCell.Empty)
                    {
                        setLegalMovesByDirections(row, column, i_TurnOf);
                    }
                }
            }
        }

        private void setLegalMovesByDirections(int i_row, int i_column, eCell i_turnOf)
        {
            if (isLegalMove(eDirection.Down, i_row, i_column, i_turnOf) ||
                isLegalMove(eDirection.DownLeft, i_row, i_column, i_turnOf) ||
                isLegalMove(eDirection.DownRight, i_row, i_column, i_turnOf) ||
                isLegalMove(eDirection.Left, i_row, i_column, i_turnOf) ||
                isLegalMove(eDirection.Right, i_row, i_column, i_turnOf) ||
                isLegalMove(eDirection.Up, i_row, i_column, i_turnOf) ||
                isLegalMove(eDirection.UpLeft, i_row, i_column, i_turnOf) ||
                isLegalMove(eDirection.UpRight, i_row, i_column, i_turnOf))
            {
                m_LegalMoves.Add(new Point(i_row, i_column));
            }
        }

        private bool isLegalMove(eDirection i_direction, int i_row, int i_column, eCell i_turnOf)
        {
            bool isLegal = false;
            eCell rivalColor = getOppositeColor(i_turnOf);
            Direction.Move(i_direction, ref i_row, ref i_column);
            if ((isValidCoordinates(i_row, i_column)) && (m_Board[i_row, i_column] == rivalColor))
            {
                while ((isValidCoordinates(i_row, i_column)) && (m_Board[i_row, i_column] == rivalColor))
                {
                    Direction.Move(i_direction, ref i_row, ref i_column);
                }

                if (isValidCoordinates(i_row, i_column) && m_Board[i_row, i_column] == i_turnOf)
                {
                    isLegal = true;
                }
            }
            return isLegal;
        }

        private bool isValidCoordinates(int i_row, int i_column)
        {
            return ((i_row < m_BoardSize) && (i_row >= 0) &&
                (i_column < m_BoardSize) && (i_column >= 0));
        }

        private eCell getOppositeColor(eCell i_color)
        {
            eCell oppositeColor;
            if (i_color == eCell.Black)
            {
                oppositeColor = eCell.White;
            }
            else
            {
                oppositeColor = eCell.Black;
            }

            return oppositeColor;
        }

        internal bool isValidMove(Point i_userMove)
        {
            return ((m_LegalMoves.IndexOf(i_userMove) > -1) &&
                isValidCoordinates(i_userMove.X, i_userMove.Y));
        }
    }
}
