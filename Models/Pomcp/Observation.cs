using System;
using System.Collections.Generic;

namespace POMCP.Website.Models.Pomcp
{
    public class Observation
    {
        public bool HasObservation { get; }

        // Position of the observation
        public int X { get; }

        public int Y { get; }

        /**
	 * l'ensemble des observations
	 */
        public static List<Observation> AllObservations { get; }


        public Observation(bool o,int x, int y) {
            X = x;
            Y = y;
            HasObservation = o;
        }

        public Observation(bool o) {
            if (!o){
                X=-1;
                Y=-1;
                HasObservation=o;
            }
            else 
                throw new Exception("No observation coordinates");
        }

        public override int GetHashCode() {
            const int prime = 31;
            int result = 1;
            result = prime * result +X + Y*prime^2;
            return result;
        }
        
        public override bool Equals(Object obj) {
            if (this == obj)
                return true;
            if (obj == null)
                return false;
            if (GetType() != obj.GetType())
                return false;
		
            Observation other = (Observation) obj;
            if (!HasObservation && !other.HasObservation)
                return true;
            if (HasObservation==other.HasObservation && X == other.X && Y == other.Y)
                return true;
		
            return false;
        }
	
        public override String ToString() {
            if (!HasObservation)
                return ("Nothing is observed");
            return("Observation at " + X + " , " + Y);
        }
    }
}