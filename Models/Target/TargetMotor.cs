using System;
using System.Collections.Generic;
using POMCP.Website.Models.Pomcp;

namespace POMCP.Website.Models.Target
{
    public class TargetMotor
    {
        public Distribution<State> GetTransition(State s1, List<State> l)
        {
            Distribution<State> d = new Distribution<State>();
            if (s1.Dx == 0 && s1.Dy == 0)
            {
                foreach (State s2 in l)
                {
                    d.SetProba(s2, 1);
                }
            }
            else
            {
                foreach (State s2 in l)
                {
                    if (Math.Abs(s1.Dx - s2.Dx) + Math.Abs(s1.Dy - s2.Dy) <= 1 && !(s2.Dx == 0 && s2.Dy == 0))
                        d.SetProba(s2, 2);
                    else
                        d.SetProba(s2, 1);
                }
            }

            d.Normalise();
            return d;
        }
    }
}