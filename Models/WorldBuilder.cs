using POMCP.Website.Models.Cameras;
using POMCP.Website.Models.Environment;
using POMCP.Website.Models.Environment.Cells;

namespace POMCP.Website.Models
{
    public class WorldBuilder
    {
        /// <summary>
        /// Builds the default world
        /// </summary>
        /// <returns></returns>
        public World GetDefaultWorld()
        {
            Map map = new Map(7,7);
            
            map.AddObstacle(6,4,new Wall());
            map.AddObstacle(4,4,new Wall());
            map.AddObstacle(3,4,new Wall());
            map.AddObstacle(2,4,new Wall());
            map.AddObstacle(2,3,new Wall());
            map.AddObstacle(2,2,new Wall());
            map.AddObstacle(2,0,new Wall());
            
            World world = new World(map);

            Camera camera = new AngularCamera(0,6);
            world.AddCamera(camera);

            return world;
        }

        /// <summary>
        /// Build a template world
        /// </summary>
        /// <returns></returns>
        public World GetMuseumWorld()
        {
            Map map = new Map(11,11);
            
            map.AddObstacle(3,0,new Wall());
            map.AddObstacle(3,1,new Wall());
            map.AddObstacle(3,3,new Wall());
            map.AddObstacle(3,4,new Wall());
            map.AddObstacle(3,5,new Wall());
            map.AddObstacle(2,5,new Wall());
            map.AddObstacle(0,5,new Wall());
            map.AddObstacle(7,0,new Wall());
            map.AddObstacle(7,1,new Wall());
            map.AddObstacle(7,3,new Wall());
            map.AddObstacle(7,4,new Wall());
            map.AddObstacle(7,5,new Wall());
            map.AddObstacle(8,5,new Wall());
            map.AddObstacle(10,5,new Wall());
            map.AddObstacle(0,8,new Wall());
            map.AddObstacle(2,8,new Wall());
            map.AddObstacle(3,8,new Wall());
            map.AddObstacle(3,9,new Wall());
            map.AddObstacle(10,8,new Wall());
            map.AddObstacle(8,8,new Wall());
            map.AddObstacle(7,8,new Wall());
            map.AddObstacle(7,9,new Wall());
            map.AddObstacle(4,5,new Glass());
            map.AddObstacle(5,5,new Glass());
            map.AddObstacle(6,5,new Glass());

            World world = new World(map);
            Camera camera = new AngularCamera(5,7);
            world.AddCamera(camera);
            return world;
        }
        
        public World GetEmptyWorld()
        {
            Map map = new Map(8,8);
            World world = new World(map);

            Camera camera = new AngularCamera(0,7);
            world.AddCamera(camera);

            return world;
        }
    }
}