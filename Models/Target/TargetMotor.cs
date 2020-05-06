﻿using System.Collections.Generic;
 using POMCP.Website.Models.Pomcp;

 namespace POMCP.Website.Models.Target
{
    public class TargetMotor
    {
        public Distribution<State> GetTransition(State s1, List<State> l)
        {
            Distribution<State> d = new Distribution<State>();
            foreach (State s2 in l) {
                d.SetProba(s2, 1);
            }
            d.Normalise();
            return d;
        }
    }
}