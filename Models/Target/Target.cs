﻿using POMCP.Website.Models.Environment;

 namespace POMCP.Website.Models.Target
{
    public class Target : Cell
    {
        private TargetMotor _motor;

        /// <summary>
        /// Instantiate a Target with its initial position
        /// </summary>
        /// <param name="x">initial abscissa</param>
        /// <param name="y">initial ordinate</param>
        public Target(int x, int y) : base(x,y)
        {
            CellType = "target";
            _motor = new TargetMotor();
        }
    }
}