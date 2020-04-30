﻿using System;
 using POMCP.Website.Models.Environment;

namespace POMCP.Website.Models.Cameras
{
    public abstract class CameraVision
    {
        protected Map map;

        public CameraVision(Map map) {
            map = map;
        }
        
        
        /// <summary>
        /// Method that return the visible cells from the position of the camera
        /// </summary>
        /// <param name="xCam"></param>
        /// <param name="yCam"></param>
        /// <returns> 2D array of booleans: true if the cell is visible, false either</returns>
        public abstract bool[,] GetVisible(int xCam, int yCam);
    }
}