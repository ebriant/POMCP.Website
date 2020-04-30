using POMCP.Website.Models.Environment;
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
            Map map = new Map(15,15);
            for (int i = 1; i < 10; i++)
            {
                map.AddObstacle(new Wall(i,10));
            }
            
            map.AddObstacle(new Glass(5,5));
            Target.Target target = new Target.Target(1 ,1);
            World world = new World(map, target);

            return world;
        }
    }
}