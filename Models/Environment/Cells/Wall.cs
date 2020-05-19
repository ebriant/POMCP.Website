﻿using System;
 using System.Runtime.ConstrainedExecution;

 namespace POMCP.Website.Models.Environment.Cells
{
    
    public class Wall: Obstacle, Opaque
    {
        public Wall()
        {
            CellType = "wall";
        }
    }
}