﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02_Othelo
{
    class Direction
    {
        public static void Move(eDirection i_direction, ref int io_x, ref int io_y)
        {
            switch(i_direction)
            {
                case (eDirection.Down):
                    io_x--;
                    break;
                case (eDirection.Up):
                    io_x++;
                    break;
                case (eDirection.Left):
                    io_y--;
                    break;
                case (eDirection.Right):
                    io_y++;
                    break;
                case (eDirection.UpLeft):
                    io_x--;
                    io_y--;
                    break;
                case (eDirection.UpRight):
                    io_x--;
                    io_y++;
                    break;
                case (eDirection.DownLeft):
                    io_x++;
                    io_y--;
                    break;
                case (eDirection.DownRight):
                    io_x++;
                    io_y++;
                    break;
                default:
                    break;
            }
        }
    }
}