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
            m_Board[(i_boardSize / 2), (i_boardSize / 2) - 1] = eCell.Black;
            m_Board[(i_boardSize / 2) - 1, (i_boardSize / 2)] = eCell.Black;
        }

        private void MakeUserMove(Point i_userMove, eCell i_turnOf, eCell[,] i_board)
        {
            foreach (eDirection direction in Direction.AllDirections)
            {
                if (isLegalMove(direction, i_userMove.X, i_userMove.Y, i_turnOf, i_board))
                {
                    makeMove(direction, i_userMove.X, i_userMove.Y, i_turnOf, i_board);
                }
            }
        }

        internal void MakeUserMove(Point i_userMove, eCell i_turnOf)
        {
            MakeUserMove(i_userMove, i_turnOf, m_Board);
        }

        private void makeMove(eDirection i_direction, int i_row, int i_column, eCell i_turnOf, eCell[,] i_board)
        {
            eCell rivalColor = getOppositeColor(i_turnOf);
            i_board[i_row, i_column] = i_turnOf;
            Direction.Move(i_direction, ref i_row, ref i_column);
            while (i_board[i_row, i_column] == rivalColor)
            {
                i_board[i_row, i_column] = i_turnOf;
                Direction.Move(i_direction, ref i_row, ref i_column);
            }
        }

        private void makeMove(eDirection i_direction, int i_row, int i_column, eCell i_turnOf)
        {
            makeMove(i_direction, i_row, i_column, i_turnOf, m_Board);
        }

        internal bool isLegalMoveExists()
        {
            return (m_LegalMoves.Count > 0);
        }

        internal void SetLegalMoves(eCell i_TurnOf, eCell[,] i_board, List<Point> o_legalMoves)
        {
            o_legalMoves.Clear();
            for (int row = 0; row < m_BoardSize; row++)
            {
                for (int column = 0; column < m_BoardSize; column++)
                {
                    if (i_board[row, column] == eCell.Empty)
                    {
                        setLegalMovesByDirections(row, column, i_TurnOf, o_legalMoves, i_board);
                    }
                }
            }
        }

        internal void SetLegalMoves(eCell i_TurnOf)
        {
            SetLegalMoves(i_TurnOf, m_Board, m_LegalMoves);
        }

        private void setLegalMovesByDirections(int i_row, int i_column, eCell i_turnOf, List<Point> o_legalMoves, eCell[,] i_board)
        {
            if (isLegalMove(eDirection.Down, i_row, i_column, i_turnOf, i_board) ||
                isLegalMove(eDirection.DownLeft, i_row, i_column, i_turnOf, i_board) ||
                isLegalMove(eDirection.DownRight, i_row, i_column, i_turnOf, i_board) ||
                isLegalMove(eDirection.Left, i_row, i_column, i_turnOf, i_board) ||
                isLegalMove(eDirection.Right, i_row, i_column, i_turnOf, i_board) ||
                isLegalMove(eDirection.Up, i_row, i_column, i_turnOf, i_board) ||
                isLegalMove(eDirection.UpLeft, i_row, i_column, i_turnOf, i_board) ||
                isLegalMove(eDirection.UpRight, i_row, i_column, i_turnOf, i_board))
            {
                o_legalMoves.Add(new Point(i_row, i_column));
            }
        }

        private void setLegalMovesByDirections(int i_row, int i_column, eCell i_turnOf)
        {
            setLegalMovesByDirections(i_row, i_column, i_turnOf, m_LegalMoves, m_Board);
        }

        private bool isLegalMove(eDirection i_direction, int i_row, int i_column, eCell i_turnOf, eCell[,] i_board)
        {
            bool isLegal = false;
            eCell rivalColor = getOppositeColor(i_turnOf);
            Direction.Move(i_direction, ref i_row, ref i_column);
            if ((isValidCoordinates(i_row, i_column)) && (i_board[i_row, i_column] == rivalColor))
            {
                while ((isValidCoordinates(i_row, i_column)) && (i_board[i_row, i_column] == rivalColor))
                {
                    Direction.Move(i_direction, ref i_row, ref i_column);
                }

                if (isValidCoordinates(i_row, i_column) && i_board[i_row, i_column] == i_turnOf)
                {
                    isLegal = true;
                }
            }
            return isLegal;
        }

        private bool isLegalMove(eDirection i_direction, int i_row, int i_column, eCell i_turnOf)
        {
            return isLegalMove(i_direction, i_row, i_column, i_turnOf, m_Board);
        }

        internal Point GetPcMove(eCell i_turnOf)
        {
            int indexOfBestMove = getIndexOfBestMoveFromLegalMoves(i_turnOf);
            return m_LegalMoves[indexOfBestMove];
        }

        private int getIndexOfBestMoveFromLegalMoves(eCell i_turnOf)
        {
            int bestMoveIndex;
            int[] countMovePoints = new int[m_LegalMoves.Count];
            for (int i = 0; i < m_LegalMoves.Count; i++)
            {
                countMovePoints[i] = getAmountOfAdittionalDiscsAfter2Moves(m_LegalMoves[i], i_turnOf);
            }
            bestMoveIndex = 0;
            for (int i = 1; i < countMovePoints.Length; i++)
            {
                if (countMovePoints[i] > bestMoveIndex)
                {
                    bestMoveIndex = i;
                }
            }

            return bestMoveIndex;
        }

        private int getAmountOfAdittionalDiscsAfter2Moves(Point i_potentialMove, eCell i_turnOf)
        {
            int amountOfAdittionalDiscs;
            int currentPoints = getCurrentPoints(i_turnOf, m_Board);
            eCell[,] potentialFutureBoard = copyBoard(m_Board);
            MakeUserMove(i_potentialMove, i_turnOf, potentialFutureBoard);
            eCell rivalColor = getOppositeColor(i_turnOf);
            List<Point> potentialRivalLegalMoves = new List<Point>();
            SetLegalMoves(rivalColor, potentialFutureBoard, potentialRivalLegalMoves);
            if (potentialRivalLegalMoves.Count > 0)
            {
                int minPointsAfter2Moves = getMinPointsAfterRivalTurn(potentialRivalLegalMoves, potentialFutureBoard, i_turnOf);
                amountOfAdittionalDiscs = minPointsAfter2Moves - currentPoints;
            }
            else
            {
                amountOfAdittionalDiscs = currentPoints;
            }

            return amountOfAdittionalDiscs;
        }

        private int getMinPointsAfterRivalTurn(List<Point> i_rivalLegalMoves, eCell[,] i_board, eCell i_turnOf)
        {
            int minPointsAfterRivalMove;
            int[] countPointsAfterRivalMove = new int[i_rivalLegalMoves.Count];
            eCell rivalColor = getOppositeColor(i_turnOf);
            eCell[,] futureBoardInMove;
            for (int i = 0; i < countPointsAfterRivalMove.Length; i++)
            {
                futureBoardInMove = copyBoard(i_board);
                MakeUserMove(i_rivalLegalMoves[i], rivalColor, futureBoardInMove);
                countPointsAfterRivalMove[i] = getCurrentPoints(i_turnOf, futureBoardInMove);
            }

            minPointsAfterRivalMove = countPointsAfterRivalMove[0];
            for (int i = 1; i < countPointsAfterRivalMove.Length; i++)
            {
                if (countPointsAfterRivalMove[i] < minPointsAfterRivalMove)
                {
                    minPointsAfterRivalMove = countPointsAfterRivalMove[i];
                }
            }

            return minPointsAfterRivalMove;
        }

        private int getCurrentPoints(eCell i_turnOf, eCell[,] i_board)
        {
            int countPoints = 0;
            foreach (eCell cell in i_board)
            {
                if (cell == i_turnOf)
                {
                    countPoints++;
                }
                else if (cell != eCell.Empty)
                {
                    countPoints--;
                }
            }
            return countPoints;
        }

        private eCell[,] copyBoard(eCell[,] i_board)
        {
            eCell[,] copyedBoard = new eCell[m_BoardSize, m_BoardSize];
            for (int i = 0; i < m_BoardSize; i++)
            {
                for (int j = 0; j < m_BoardSize; j++)
                {
                    copyedBoard[i, j] = i_board[i, j];
                }
            }

            return copyedBoard;
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