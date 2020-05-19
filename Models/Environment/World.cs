﻿using System;
 using System.Collections.Generic;
 using POMCP.Website.Models.Cameras;


 namespace POMCP.Website.Models.Environment
{
    
    public class World
    {
        //The base map containing only the obstacles (no camera or target)
        public Map Map { get;}

        public Target.Target Target { get; }
        
        public List<Camera> Cameras { get; }
        
        public World(Map map) {
            Map = map;
            Target = new Target.Target();
            Cameras = new List<Camera>();
        }
        
        public World(string[][] mapStringArray) {
            Map = new Map(mapStringArray);
            Target = new Target.Target();
            Cameras = new List<Camera>();
        }

        public void AddCamera(Camera c) {
            Cameras.Add(c);
        }
        
        public void InitializeCameras() {
            CameraVision vision = new CameraVisionCenter(Map);
            foreach (Camera c in Cameras) {
                c.Initialize(vision);
            }
        }

        public bool IsCamera(int x, int y, out Camera outCamera)
        {
            foreach (Camera camera in Cameras)
            {
                if (camera.X == x && camera.Y == y)
                {
                    outCamera = camera;
                    return true;
                }
                
            }
            outCamera = null;
            return false;
        }
    }
}