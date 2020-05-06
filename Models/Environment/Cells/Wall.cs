﻿using System.Runtime.ConstrainedExecution;

 namespace POMCP.Website.Models.Environment.Cells
{
    public class Wall: Obstacle, Opaque
    {
        public static readonly string CellTypeString = "wall";
        public Wall(int x, int y) : base(x, y)
        {
            CellType = CellTypeString;
        }
    }
}