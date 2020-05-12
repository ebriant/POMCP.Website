using System;
using System.Collections.Generic;
using POMCP.Website.Models.Environment;

namespace POMCP.Website.Models.Pomcp
{
    public class State
    {
        
        public int X { get; set; }

        public int Y { get; set; }

        public List<double> CamerasOrientations { get; }


        public State(int x, int y, List<Double> camerasOrientations)
        {
            X = x;
            Y = y;
            CamerasOrientations = camerasOrientations;
        }
        
        public override int GetHashCode()
        {
            const int prime = 31;
            int result = 1;
            result = prime * result + X + prime ^ 2 * Y;
            return result;
        }


        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;
            if (obj == null)
                return false;
            if (!(obj is State))
                return false;
            State other = (State) obj;
            if (CamerasOrientations.Count != other.CamerasOrientations.Count)
                return false;
            if (X != other.X || Y != other.Y)
                return false;
            for (int i = 0; i < CamerasOrientations.Count; i++)
            {
                if (Math.Abs(CamerasOrientations[i] - other.CamerasOrientations[i]) > 0.001)
                    return false;
            }

            return true;
        }
    }
}