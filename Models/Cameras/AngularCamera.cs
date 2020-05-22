using System;
using System.Collections.Generic;
using POMCP.Website.Models.Pomcp;

namespace POMCP.Website.Models.Cameras
{
    public class AngularCamera : Camera
    {
        public AngularCamera(int x, int y) : base(x, y) {}

        /// <summary>
        /// Return the cameras's field of view using the angle
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public override bool[,] GetVision(double angle)
        {
            bool[,] result = new bool[VisibleCells.GetLength(0), VisibleCells.GetLength(1)];
            // Vector representing the direction of the camera
            double[] cameraViewVector =
            {
                Math.Cos(angle),
                Math.Sin(angle)
            };
            // tolerance on the cosinus
            double cosTolerance = Math.Cos(FOV);
            
            // for each cell
            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    if (!VisibleCells[i, j])
                    {
                        result[i, j] = false;
                    }
                    else
                    {
                        // get the vector camera -> cell
                        double dx = i - X;
                        double dy = j - Y;
                        double normSq = dx * dx + dy * dy;
                        if (normSq > 0)
                        {
                            // get the cosinus cos = (a.b) / |a||b| (. is the dot product of the vectors)
                            double cos = (dx * cameraViewVector[0] + dy * cameraViewVector[1]) / Math.Sqrt(normSq);
                            // if the cos is above than tolerance, the cell is visible
                            if (cos > cosTolerance)
                                result[i, j] = true;
                        }
                    }
                }
            }
            return result;
        }

        public override Distribution<Observation> GetObservation(State s)
        {
            Distribution<Observation> o = new Distribution<Observation>();
            if (GetVision(s.CamerasOrientations[this])[s.X, s.Y])
                o.SetProba(new Observation(true, s.X, s.Y), 1);
            else
                o.SetProba(new Observation(false), 1);
            return o;
        }


        public override List<double> GetActions()
        {

            List<double> actions = new List<double>();
            for (int i = - 1; i < 2; i++)
            {
                actions.Add(Math.PI * i / 4);
            }

            return actions;
        }

        // public override double GetValue(State state)
        // {
        //     bool[,] vision = GetVision(state.CamerasOrientations[this]);
        //     double count = 0;
        //     foreach (bool b in vision)
        //     {
        //         if (b) count += 1;
        //     }
        //
        //     return vision[state.X, state.Y] ? 1 : 0 + count / (vision.GetLength(0) * vision.GetLength(1));
        // }
    }
}