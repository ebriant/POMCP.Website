using System;
using System.Collections.Generic;
using System.Drawing;
using POMCP.Website.Models.Pomcp;

namespace POMCP.Website.Models.Cameras
{
    public class AngularCamera : Camera
    {
        public double FOV = Math.PI / 8;
        
        public AngularCamera(int x, int y, int number, CameraVision vision) : base(x, y, number, vision)
        {
        }

        
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
            for (int j = 0; j < result.GetLength(1); j++)
            {
                // get the vector camera -> cell
                double dx = i - X;
                double dy = j - Y;
                double norm = Math.Sqrt(dx * dx + dy * dy);

                if (norm > 0)
                {
                    // get the cosinus
                    double cos = (dx * cameraViewVector[0] + dy
                                     * cameraViewVector[1])
                                 / norm;
                    // if the cos is less than tolerance, invisible
                    if (cos < cosTolerance)
                        result[i, j] = false;
                }
            }

            return result;
        }

        public override Distribution<Observation> GetObservation(State s)
        {
            Distribution<Observation> o = new Distribution<Observation>();
            if (GetVision(s.CamerasOrientations[Num])[s.X, s.Y])
                o.setProba(new Observation(true, s.X, s.Y), 1);
            else
                o.setProba(new Observation(false), 1);
            return o;
        }


        public override List<double> GetActions(double angle)
        {
            int p = (int) Math.Round(angle / Math.PI);
            
            List<double> actions = new List<double>();
            for (int i = p-2; i < p+3; i++)
            {
                actions.Add(Math.PI * i / 4);
            }
            return actions;
        }
        
        public override double GetValue(State state)
        {
            bool[,] vision = GetVision(state.CamerasOrientations[Num]);
            double count = 0;
            foreach (bool b in vision)
            {
                if (b) count+=1;
            }
            return vision[state.X,state.Y] ? 1 : 0 + count / (vision.GetLength(0) * vision.GetLength(1));
        }
    }
}