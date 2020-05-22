using System;
using System.Collections.Generic;
using POMCP.Website.Models.Pomcp;

namespace POMCP.Website.Models.Cameras
{
    public abstract class Camera
    {
	    
	    public int X { get;}
	    public int Y { get;}
	    public double FOV { get; } = Math.PI / 8;

	    public bool[,] VisibleCells;
	    
	    
	    
        public Camera(int x, int y)
        {
	        X = x;
	        Y = y;
        }

        public void Initialize(CameraVision vision)
        {
	        VisibleCells =  vision.GetVisible(X, Y);
        }
	
        
        public abstract bool[,] GetVision(double angle);
        
        /// <summary>
        /// Return the observation for the current state
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
         public abstract Distribution<Observation> GetObservation(State state);
        
        public abstract List<double> GetActions();
	
        public virtual double GetValue(State state) 
        {
            return GetVision(state.CamerasOrientations[this])[state.X,state.Y] ? 1 : 0;
        }

        public override string ToString()
        {
	        return ("Camera " +X+", "+Y);
        }
        
        public override int GetHashCode()
        {
	        const int prime = 43;
	        int result = 1;
	        result = prime * result + X + prime ^ 2 * Y;
	        return result;
        }
        
    }
}