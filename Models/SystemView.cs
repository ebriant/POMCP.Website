using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text.Json.Serialization;
using POMCP.Website.Models.Cameras;
using POMCP.Website.Models.Environment.Cells;
using POMCP.Website.Models.Pomcp;

namespace POMCP.Website.Models
{
    [Serializable]
    public struct CameraProperties
    {
        public int X { get; set; }
        public int Y { get; set; }
        public double Orientation { get; set; }
        public double Fov { get; set; }
        
        public CameraProperties(int x, int y, double orientation, double fov)
        {
            X = x;
            Y = y;
            Orientation = orientation;
            Fov = fov;
        }

        public override string ToString()
        {
            return "Camera. X:" + X + ", Y: " + Y + ", Ori: " + Orientation + ", FOV: " + Fov;
        }
    }
    
    /// <summary>
    /// Structure that represents the current state of the system in an easy processable way for the view
    /// </summary>
    [Serializable]
    public struct SystemView
    {
        
        public string[][] Map { get; set; }
        public int[] TrueState { get; set; }
        public CameraProperties[] Cameras { get; set; }
        public double[][] Probabilities { get; set; }
        public int[][] CamerasVision { get; set; }
        public bool[][] MovingOptions { get; set; }
        public double[][] StatesProbabilities { get; set; }
        // public Dictionary<int[],double> ProbaDict { get; set; }
        
        public SystemView(string[][] map, int[] trueState, CameraProperties[] cameras, double[][] probabilities,
            int[][] camerasVision, bool[][] movingOptions, double[][] statesProbabilities)
        {
            Map = map;
            TrueState = trueState;
            Cameras = cameras;
            Probabilities = probabilities;
            CamerasVision = camerasVision;
            MovingOptions = movingOptions;
            StatesProbabilities = statesProbabilities;

        }
    }
}