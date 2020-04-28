﻿using System.Collections.Generic;
using POMCP.Website.Models.Target;
 
namespace POMCP.Website.Models.Environment
{
    public class World
    {
        //The base map containing only the obstacles (no camera or target)
        public Map Map { get;}

        public Target.Target Target { get; }
        
        public World(Map map, Target.Target target) {
            Map = map;
            Target = target;
        }

        public Cell[][] GetCellsArray()
        {
            Cell[][] result = Map.GetCellsArray();
            result[Target.X][Target.Y] = Target;
            return result;
        }
    }
}