using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using POMCP.Website.Models;
using POMCP.Website.Models.Environment;
using POMCP.Website.Models.Target;

namespace POMCP.Website.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorldController : Controller
    {
        
        private readonly ILogger<WeatherForecastController> _logger;

        public WorldController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }
        
        [HttpGet]
        public Map Get()
        {
            return WorldBuilder.DefaultWorld.Map;
        }

        [HttpGet]
        [Route("dimensions")]
        public int[] Map()
        {
            return new[] {WorldBuilder.DefaultWorld.Map.Dx, WorldBuilder.DefaultWorld.Map.Dy};
        }
        
        [HttpGet]
        [Route("cells")]
        public Cell[][] GetCellsArray()
        {
            // Cell[][] test = new Cell[1][];
            // test[0] = new Cell[1];
            // test [0][0] = new Wall(0, 0);
            // return test;
            return WorldBuilder.DefaultWorld.Map.GetCellsArray();
        }
        
        [HttpGet]
        [Route("target")]
        public Target GetTarget()
        {
            return WorldBuilder.DefaultWorld.Target;
        }
    }
}