using System;
using System.Collections.Generic;
using POMCP.Website.Models.Environment;

namespace POMCP.Website.Models.Pomcp
{
    public class State
    {
        
        public int X { get; }

        public int Y { get; }

        public List<double> CamerasOrientations { get; }

        /**
	 * l'ensemble des etats
	 */
        private static List<State> allS;

        /**
	 * l'ensemble des etats accessibles (i.e. non bloqués par un mur)
	 */
        private static List<State> allSf;

        
        public State(int x, int y, List<Double> camerasOrientations)
        {
            X = x;
            Y = y;
            CamerasOrientations = camerasOrientations;
        }

        
        /**
	 * retourne tous les etats patron singleton.
	 */
        public static List<State> GetAll(State state, World world)
        {
            if (allS == null)
            {
                allS = new List<State>();
                for (int x = 0; x < world.Map.Dx; x++)
                {
                    for (int y = 0; y < world.Map.Dy; y++)
                    {
                        allS.Add(new State(x, y, state.CamerasOrientations));
                    }
                }
            }
            return allS;
        }

        public static List<State> GetAllFree(Map map)
        {
            allSf = new List<State>();
            for (int x = 0; x < map.Dx; x++)
            {
                for (int y = 0; y < map.Dy; y++)
                {
                    if (map.IsCellFree(x, y))
                        allSf.Add(new State(x, y, null));
                }
            }

            return allSf;
        }

        public int[] GetCoord()
        {
            int[] coord = {X, Y};
            return coord;
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