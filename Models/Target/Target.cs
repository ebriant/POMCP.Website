﻿using System;
 using System.Collections.Generic;
 using POMCP.Website.Models.Environment;
 using POMCP.Website.Models.Environment.Cells;
 using POMCP.Website.Models.Pomcp;

 namespace POMCP.Website.Models.Target
{
    
    public class Target
    {
        private TargetMotor _motor;

        /// <summary>
        /// Instantiate a Target
        /// </summary>
        public Target()
        {
            _motor = new TargetMotor();
        }

        public Distribution<State> GetTransition(State currentState, List<State> l)
        {
            return _motor.GetTransition(currentState, l);
        }
    }
}