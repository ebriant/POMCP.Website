using System;
using System.Collections.Generic;
using POMCP.Website.Models.Environment;
using POMCP.Website.Models.Environment.Cells;
using POMCP.Website.Models.Pomcp;

namespace POMCP.Website.Models.Cameras
{
    public abstract class Camera: Cell
    {
	    public double FOV { get; } = Math.PI / 8;

	    public bool[,] VisibleCells;

	    private CameraVision _vision;

		   
		 /// <summary>
		 /// Number of the Camera in the world
		 /// </summary>
        public int Num { get; }
	    
	    
        public Camera(int x, int y, int number) : base(x,y)
        {
	        CellType = "camera";
            Num = number;
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
        
        public abstract List<double> GetActions(double angle);
	
        public virtual double GetValue(State state) {
            return GetVision(state.CamerasOrientations[Num])[state.X,state.Y] ? 1 : 0;
        }

        public override string ToString()
        {
	        return ("Camera " +X+", "+Y);
        }
    }
}