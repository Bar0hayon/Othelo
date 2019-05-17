using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;

namespace Ex02_Othelo
{
    public class GameBoardEngine
    {
        private eCell[,] m_Board;
        private List<Point> m_LegalMoves;
        private int m_BoardSize;
        private const int k_Milliseconds = 500;
        public eCell[,] Board
        {
            get
            {
                return m_Board;
            }
        }

        public GameBoardEngine(int i_BoardSize)
        {
            m_BoardSize = i_BoardSize;
            m_Board = new eCell[i_BoardSize, i_BoardSize];
            initialBoard(i_BoardSize);
            m_LegalMoves = new List<Point>();
        }

        private void initialBoard(int i_BoardSize)
        {
            for (int i = 0; i < i_BoardSize; i++)
            {
                for (int j = 0; j < i_BoardSize; j++)
                {
                    m_Board[i, j] = eCell.Empty;
                }
            }

            m_Board[((i_BoardSize / 2) - 1), ((i_BoardSize / 2) - 1)] = eCell.White;
            m_Board[(i_BoardSize / 2), (i_BoardSize / 2)] = eCell.White;
            m_Board[(i_BoardSize / 2), (i_BoardSize / 2) - 1] = eCell.Black;
            m_Board[(i_BoardSize / 2) - 1, (i_BoardSize / 2)] = eCell.Black;
        }

        private void makeUserMove(Point i_UserMove, eCell i_TurnOf, eCell[,] i_Board)
        {
            foreach (eDirection direction in Direction.AllDirections)
            {
                if (isLegalMove(direction, i_UserMove.X, i_UserMove.Y, i_TurnOf, i_Board))
                {
                    makeMove(direction, i_UserMove.X, i_UserMove.Y, i_TurnOf, i_Board);
                }
            }
        }

        internal void MakeUserMove(Point i_UserMove, eCell i_TurnOf)
        {
            makeUserMove(i_UserMove, i_TurnOf, m_Board);
        }

        private void makeMove(eDirection i_Direction, int i_Row, int i_Column, eCell i_TurnOf, eCell[,] i_Board)
        {
            eCell rivalColor = getOppositeColor(i_TurnOf);
            i_Board[i_Row, i_Column] = i_TurnOf;
            Direction.Move(i_Direction, ref i_Row, ref i_Column);
            while (i_Board[i_Row, i_Column] == rivalColor)
            {
                i_Board[i_Row, i_Column] = i_TurnOf;
                Direction.Move(i_Direction, ref i_Row, ref i_Column);
            }
        }

        private void makeMove(eDirection i_Direction, int i_Row, int i_Column, eCell i_TurnOf)
        {
            makeMove(i_Direction, i_Row, i_Column, i_TurnOf, m_Board);
        }

        internal bool isLegalMoveExists()
        {
            return m_LegalMoves.Count > 0;
        }

        internal void SetLegalMoves(eCell i_TurnOf, eCell[,] i_board, List<Point> o_LegalMoves)
        {
            o_LegalMoves.Clear();
            for (int row = 0; row < m_BoardSize; row++)
            {
                for (int column = 0; column < m_BoardSize; column++)
                {
                    if (i_board[row, column] == eCell.Empty)
                    {
                        setLegalMovesByDirections(row, column, i_TurnOf, o_LegalMoves, i_board);
                    }
                }
            }
        }

        internal void SetLegalMoves(eCell i_TurnOf)
        {
            SetLegalMoves(i_TurnOf, m_Board, m_LegalMoves);
        }

        private void setLegalMovesByDirections(int i_Row, int i_Column, eCell i_TurnOf, List<Point> o_LegalMoves, eCell[,] i_Board)
        {
            if (isLegalMove(eDirection.Down, i_Row, i_Column, i_TurnOf, i_Board) ||
                isLegalMove(eDirection.DownLeft, i_Row, i_Column, i_TurnOf, i_Board) ||
                isLegalMove(eDirection.DownRight, i_Row, i_Column, i_TurnOf, i_Board) ||
                isLegalMove(eDirection.Left, i_Row, i_Column, i_TurnOf, i_Board) ||
                isLegalMove(eDirection.Right, i_Row, i_Column, i_TurnOf, i_Board) ||
                isLegalMove(eDirection.Up, i_Row, i_Column, i_TurnOf, i_Board) ||
                isLegalMove(eDirection.UpLeft, i_Row, i_Column, i_TurnOf, i_Board) ||
                isLegalMove(eDirection.UpRight, i_Row, i_Column, i_TurnOf, i_Board))
            {
                o_LegalMoves.Add(new Point(i_Row, i_Column));
            }
        }

        private void setLegalMovesByDirections(int i_Row, int i_Column, eCell i_TurnOf)
        {
            setLegalMovesByDirections(i_Row, i_Column, i_TurnOf, m_LegalMoves, m_Board);
        }

        private bool isLegalMove(eDirection i_Direction, int i_Row, int i_Column, eCell i_TurnOf, eCell[,] i_Board)
        {
            bool isLegal = false;
            eCell rivalColor = getOppositeColor(i_TurnOf);
            Direction.Move(i_Direction, ref i_Row, ref i_Column);
            if (isValidCoordinates(i_Row, i_Column) && i_Board[i_Row, i_Column] == rivalColor)
            {
                while (isValidCoordinates(i_Row, i_Column) && i_Board[i_Row, i_Column] == rivalColor)
                {
                    Direction.Move(i_Direction, ref i_Row, ref i_Column);
                }

                if (isValidCoordinates(i_Row, i_Column) && i_Board[i_Row, i_Column] == i_TurnOf)
                {
                    isLegal = true;
                }
            }

            return isLegal;
        }

        private bool isLegalMove(eDirection i_Direction, int i_Row, int i_Column, eCell i_TurnOf)
        {
            return isLegalMove(i_Direction, i_Row, i_Column, i_TurnOf, m_Board);
        }

        internal Point GetPcMove(eCell i_TurnOf)
        {
            int indexOfBestMove = getIndexOfBestMoveFromLegalMoves(i_TurnOf);
            return m_LegalMoves[indexOfBestMove];
        }

        private int getIndexOfBestMoveFromLegalMoves(eCell i_TurnOf)
        {
            Thread.Sleep(k_Milliseconds);
            int bestMoveIndex;
            int[] countMovePoints = new int[m_LegalMoves.Count];
            for (int i = 0; i < m_LegalMoves.Count; i++)
            {
                countMovePoints[i] = getAmountOfAdittionalDiscsAfter2Moves(m_LegalMoves[i], i_TurnOf);
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

        private int getAmountOfAdittionalDiscsAfter2Moves(Point i_PotentialMove, eCell i_TurnOf)
        {
            int amountOfAdittionalDiscs;
            int currentPoints = getCurrentPoints(i_TurnOf, m_Board);
            eCell[,] potentialFutureBoard = copyBoard(m_Board);
            makeUserMove(i_PotentialMove, i_TurnOf, potentialFutureBoard);
            eCell rivalColor = getOppositeColor(i_TurnOf);
            List<Point> PotentialRivalLegalMoves = new List<Point>();
            SetLegalMoves(rivalColor, potentialFutureBoard, PotentialRivalLegalMoves);
            if (PotentialRivalLegalMoves.Count > 0)
            {
                int minPointsAfter2Moves = getMinPointsAfterRivalTurn(PotentialRivalLegalMoves, potentialFutureBoard, i_TurnOf);
                amountOfAdittionalDiscs = minPointsAfter2Moves - currentPoints;
            }
            else
            {
                amountOfAdittionalDiscs = currentPoints;
            }

            return amountOfAdittionalDiscs;
        }

        private int getMinPointsAfterRivalTurn(List<Point> i_RivalLegalMoves, eCell[,] i_Board, eCell i_TurnOf)
        {
            int minPointsAfterRivalMove;
            int[] countPointsAfterRivalMove = new int[i_RivalLegalMoves.Count];
            eCell rivalColor = getOppositeColor(i_TurnOf);
            eCell[,] futureBoardInMove;
            for (int i = 0; i < countPointsAfterRivalMove.Length; i++)
            {
                futureBoardInMove = copyBoard(i_Board);
                makeUserMove(i_RivalLegalMoves[i], rivalColor, futureBoardInMove);
                countPointsAfterRivalMove[i] = getCurrentPoints(i_TurnOf, futureBoardInMove);
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

        private int getCurrentPoints(eCell i_TurnOf, eCell[,] i_Board)
        {
            int countPoints = 0;
            foreach (eCell cell in i_Board)
            {
                if (cell == i_TurnOf)
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

        private eCell[,] copyBoard(eCell[,] i_Board)
        {
            eCell[,] copyedBoard = new eCell[m_BoardSize, m_BoardSize];
            for (int i = 0; i < m_BoardSize; i++)
            {
                for (int j = 0; j < m_BoardSize; j++)
                {
                    copyedBoard[i, j] = i_Board[i, j];
                }
            }

            return copyedBoard;
        }

        private bool isValidCoordinates(int i_Row, int i_Column)
        {
            return i_Row < m_BoardSize && i_Row >= 0 && i_Column < m_BoardSize && i_Column >= 0;
        }

        private eCell getOppositeColor(eCell i_Color)
        {
            eCell oppositeColor;
            if (i_Color == eCell.Black)
            {
                oppositeColor = eCell.White;
            }
            else
            {
                oppositeColor = eCell.Black;
            }

            return oppositeColor;
        }

        internal bool isValidMove(Point i_UserMove)
        {
            return m_LegalMoves.IndexOf(i_UserMove) > -1 && isValidCoordinates(i_UserMove.X, i_UserMove.Y);
        }
    }
}