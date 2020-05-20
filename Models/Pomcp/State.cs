using System;
using System.Collections.Generic;
using POMCP.Website.Models.Cameras;
using POMCP.Website.Models.Environment;

namespace POMCP.Website.Models.Pomcp
{
    
    public class State
    {
        
        public int X { get; set; }

        public int Y { get; set; }

        public Dictionary<Camera, double> CamerasOrientations { get; }


        public State(int x, int y, Dictionary<Camera, double> camerasOrientations)
        {
            X = x;
            Y = y;
            CamerasOrientations = camerasOrientations;
        }
        
        public State(int x, int y, IEnumerable<Camera> cameraslist)
        {
            X = x;
            Y = y;
            CamerasOrientations = new Dictionary<Camera, double>();
            foreach (Camera camera in cameraslist)
            {
                CamerasOrientations[camera] = -Math.PI / 2;
            }
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
            if (X != other.X || Y != other.Y)
                return false;
            if (CamerasOrientations.Count != other.CamerasOrientations.Count)
                return false;
            foreach (KeyValuePair<Camera,double> keyValuePair in CamerasOrientations)
            {
                double value;
                if (!other.CamerasOrientations.TryGetValue(keyValuePair.Key, out value))
                    return false;
                else if (Math.Abs(value - keyValuePair.Value) > 0.0001f)
                {
                    return false;
                }
            }
            return true;
        }
    }
}