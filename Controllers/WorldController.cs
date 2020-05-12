using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using POMCP.Website.Models;
using POMCP.Website.Models.Cameras;
using POMCP.Website.Models.Environment;
using POMCP.Website.Models.Environment.Cells;
using POMCP.Website.Models.Target;

namespace POMCP.Website.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorldController : Controller
    {
        
        private readonly ILogger<WorldController> _logger;

        public WorldController(ILogger<WorldController> logger)
        {
            _logger = logger;
        }
        
        [HttpGet]
        public Map Get()
        {
            return WorldBuilder.DefaultWorld.Map;
        }

        [HttpGet]
        [Route("cells")]
        public string[][] GetCellsArray()
        {
            string[][] cellArray = WorldBuilder.DefaultWorld.Map.GetCellsArray();
            return cellArray;
        }
        
        [HttpGet]
        [Route("target")]
        public Target GetTarget()
        {
            return WorldBuilder.DefaultWorld.Target;
        }
        
        // [HttpGet]
        // [Route("camera")]
        // public IEnumerable<DisplayCamera> GetCameras()
        // {
        //     return Models.System.;
        // }
    }
}