using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02_Othelo
{
    public class Direction
    {
        private static eDirection[] m_AllDirections =
            {
            eDirection.Down, eDirection.DownLeft, eDirection.DownRight, eDirection.Left, eDirection.Right, eDirection.Up, eDirection.UpLeft, eDirection.UpRight
            };

        internal static eDirection[] AllDirections
        {
            get
            {
                return m_AllDirections;
            }
        }

        public static void Move(eDirection i_Direction, ref int io_X, ref int io_Y)
        {
            switch (i_Direction)
            {
                case eDirection.Down:
                    io_X--;
                    break;
                case eDirection.Up:
                    io_X++;
                    break;
                case eDirection.Left:
                    io_Y--;
                    break;
                case eDirection.Right:
                    io_Y++;
                    break;
                case eDirection.UpLeft:
                    io_X--;
                    io_Y--;
                    break;
                case eDirection.UpRight:
                    io_X--;
                    io_Y++;
                    break;
                case eDirection.DownLeft:
                    io_X++;
                    io_Y--;
                    break;
                case eDirection.DownRight:
                    io_X++;
                    io_Y++;
                    break;
                default:
                    break;
            }
        }
    }
}