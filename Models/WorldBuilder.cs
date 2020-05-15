using POMCP.Website.Models.Cameras;
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
            Map map = new Map(10,10);
            
            map.AddObstacle(new Wall(0,4));
            map.AddObstacle(new Wall(1,4));
            map.AddObstacle(new Wall(3,4));
            map.AddObstacle(new Wall(4,4));
            map.AddObstacle(new Wall(4,3));
            map.AddObstacle(new Wall(4,1));
            map.AddObstacle(new Wall(4,0));
            
            Target.Target target = new Target.Target();
            
            World world = new World(map, target);

            Camera camera = new AngularCamera(7,7);
            world.AddCamera(camera);

            return world;
        }
    }
}