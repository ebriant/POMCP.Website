﻿namespace POMCP.Website.Models.Environment
{
    public class Wall: Obstacle, Opaque
    {
        public Wall(int x, int y) : base(x, y)
        {
            CellType = "wall";
        }
    }
}