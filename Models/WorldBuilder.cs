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
            Map map = new Map(20,20);
            for (int i = 1; i < 14; i++)
            {
                map.AddObstacle(new Wall(i,15));
            }
            
            map.AddObstacle(new Glass(5,10));
            Target.Target target = new Target.Target(10 ,10);
            World world = new World(map, target);

            return world;
        }
    }
}