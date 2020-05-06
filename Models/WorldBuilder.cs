﻿using POMCP.Website.Models.Cameras;
using POMCP.Website.Models.Environment;
using POMCP.Website.Models.Environment.Cells;
using POMCP.Website.Models.Target;

namespace POMCP.Website.Models
{
    public class WorldBuilder
    {
        // contains the default world
        public static readonly World DefaultWorld = GetDefaultWorld();
        
        /// <summary>
        /// Builds the default world
        /// </summary>
        /// <returns></returns>
        private static World GetDefaultWorld()
        {
            Map map = new Map(8,8);
            
            map.AddObstacle(new Wall(1,4));
            map.AddObstacle(new Wall(3,4));
            map.AddObstacle(new Wall(4,4));
            map.AddObstacle(new Wall(4,3));
            map.AddObstacle(new Wall(4,1));
            
            Target.Target target = new Target.Target();
            
            World world = new World(map, target);

            Camera camera = new AngularCamera(6,6, world.Cameras.Count);
            world.AddCamera(camera);
            world.InitializeCamera();
                
                
            return world;
        }
    }
}