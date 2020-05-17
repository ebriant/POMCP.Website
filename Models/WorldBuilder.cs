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
            Map map = new Map(7,7);
            
            map.AddObstacle(new Wall(0,4));
            map.AddObstacle(new Wall(2,4));
            map.AddObstacle(new Wall(3,4));
            map.AddObstacle(new Wall(4,4));
            map.AddObstacle(new Wall(4,3));
            map.AddObstacle(new Wall(4,2));
            map.AddObstacle(new Wall(4,0));
            
            Target.Target target = new Target.Target();
            
            World world = new World(map, target);

            Camera camera = new AngularCamera(6,6);
            world.AddCamera(camera);

            return world;
        }

        private World GetMuseumWorld()
        {
            Map map = new Map(11,11);
            
            map.AddObstacle(new Wall(3,0));
            map.AddObstacle(new Wall(3,1));
            map.AddObstacle(new Wall(3,3));
            map.AddObstacle(new Wall(3,4));
            map.AddObstacle(new Wall(3,5));
            map.AddObstacle(new Wall(2,5));
            map.AddObstacle(new Wall(0,5));
            
            map.AddObstacle(new Wall(7,0));
            map.AddObstacle(new Wall(7,1));
            map.AddObstacle(new Wall(7,3));
            map.AddObstacle(new Wall(7,4));
            map.AddObstacle(new Wall(7,5));
            map.AddObstacle(new Wall(8,5));
            map.AddObstacle(new Wall(10,5));
            
            map.AddObstacle(new Wall(0,8));
            map.AddObstacle(new Wall(2,8));
            map.AddObstacle(new Wall(3,8));
            map.AddObstacle(new Wall(3,9));
            
            map.AddObstacle(new Wall(10,8));
            map.AddObstacle(new Wall(8,8));
            map.AddObstacle(new Wall(7,8));
            map.AddObstacle(new Wall(7,9));
            
            map.AddObstacle(new Glass(4,5));
            map.AddObstacle(new Glass(5,5));
            map.AddObstacle(new Glass(6,5));
            
            Target.Target target = new Target.Target();
            
            World world = new World(map, target);

            Camera camera = new AngularCamera(5,7);
            world.AddCamera(camera);

            return world;
        }
    }
}