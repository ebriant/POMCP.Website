﻿namespace POMCP.Website.Models.Target
{
    public class Target
    {
        public int X { get;}
        public int Y { get;}
        
        private TargetMotor _motor;

        /// <summary>
        /// Instantiate a Target with its initial position
        /// </summary>
        /// <param name="x">initial abscissa</param>
        /// <param name="y">initial ordinate</param>
        public Target(int x, int y)
        {
            X = x;
            Y = y;
            _motor = new TargetMotor();
        }
    }
}