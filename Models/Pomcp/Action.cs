#nullable enable
using System;
using System.Collections.Generic;
using POMCP.Website.Models.Cameras;

namespace POMCP.Website.Models.Pomcp
{
    public class Action
    {
        /// <summary>
        /// List all the orientation changes desired for the action
        /// </summary>
        public Dictionary<Camera, double>  OrientationsChanges { get; }

        
        public Action()
        {
            OrientationsChanges = new Dictionary<Camera, double>();
        }
        
        public Action(Dictionary<Camera, double>  orientationsChanges)
        {
            OrientationsChanges = new Dictionary<Camera, double>();
            foreach (KeyValuePair<Camera,double> kv in orientationsChanges)
            {
                OrientationsChanges[kv.Key] = kv.Value;
            }
        }

        public Action(double orientation, Camera cam)
        {
            OrientationsChanges = new Dictionary<Camera, double> ();
            OrientationsChanges[cam] = orientation;
            // for (int i = 0; i < nbCam - 1; i++)
            // {
            //     Orientations.Add(0d);
            // }
        }

        public override string ToString()
        {
            string s = "Action ";
            foreach (double d in OrientationsChanges.Values)
                s += ", " + d * 180 / Math.PI;
            return s;
        }


        public override int GetHashCode()
        {
            int prime = 31;
            int result = 1;
            int i = 0;
            foreach (double orientationsValue in OrientationsChanges.Values)
            {
                result += (prime ^ i) * (int) orientationsValue;
                i++;
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
            if (OrientationsChanges.Count != other.OrientationsChanges.Count)
                return false;
            foreach (KeyValuePair<Camera,double> keyValuePair in OrientationsChanges)
            {
                double value;
                if (!other.OrientationsChanges.TryGetValue(keyValuePair.Key, out value))
                    return false;
                else if (Math.Abs(value - keyValuePair.Value) > 0.001f)
                {
                    return false;
                }
            }
            return true; 
        }
    }
}