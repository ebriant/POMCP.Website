﻿using System.Collections.Generic;
 using POMCP.Website.Models.Cameras;
 using POMCP.Website.Models.Environment.Cells;


 namespace POMCP.Website.Models.Environment
{
    public class World
    {
        //The base map containing only the obstacles (no camera or target)
        public Map Map { get;}

        public Target.Target Target { get; }
        
        public List<Camera> Cameras { get; }
        
        public World(Map map, Target.Target target) {
            Map = map;
            Target = target;
            Cameras = new List<Camera>();
        }

        public void AddCamera(Camera c) {
            Cameras.Add(c);
        }
        
        public void InitializeCamera() {
            CameraVision vision = new CameraVisionCenter(Map);
            foreach (Camera c in Cameras) {
                c.Initialize(vision);
            }
        }
    }
}