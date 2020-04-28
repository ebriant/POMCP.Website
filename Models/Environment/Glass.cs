﻿namespace POMCP.Website.Models.Environment
{
    public class Glass: Obstacle
    {
        public Glass(int x, int y) : base(x, y)
        {
            CellType = "glass";
        }
    }
}