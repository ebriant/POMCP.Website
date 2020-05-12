#nullable enable
using System;
using System.Collections.Generic;

namespace POMCP.Website.Models.Pomcp
{
    public class Action
    {
        public List<double> Orientations { get; }

        public Action(List<double> orientations)
        {
            Orientations = orientations;
        }

        public Action(double orientation, int nbCam)
        {
            Orientations = new List<double>();
            Orientations.Add(orientation);
            for (int i = 0; i < nbCam - 1; i++)
            {
                Orientations.Add(0d);
            }
        }

        public override string ToString()
        {
            string s = "Action ";
            foreach (double d in this.Orientations)
                s = s + ", " + d * 180 / Math.PI;
            return s;
        }


        public override int GetHashCode()
        {
            int prime = 31;
            int result = 1;
            for (int i = 0; i < Orientations.Count; i++)
            {
                result += (prime ^ i) * (int) Orientations[i];
            }
            return result;
        }


        public override bool Equals(object? obj)
        {
            if (this == obj)
                return true;
            if (obj == null)
                return false;
            if (!(obj is Action))
                return false;
            Action other = (Action) obj;

            for (int i = 0; i <Orientations.Count; i++)
            {
                if (Math.Abs(Orientations[i] - other.Orientations[i]) > 0.01)
                    return false;
            }

            return true; 
        }
    }
}