using System;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using POMCP.Website.Models;
using POMCP.Website.Models.Environment;
using POMCP.Website.Models.Pomcp;

namespace POMCP.Website.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PomcpController : Controller
    {
        public const string SessionKeySystem = "_System";
        private readonly ILogger<PomcpController> _logger;

        public PomcpController(ILogger<PomcpController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public SystemView Get([FromQuery] int? index)
        {
            WorldBuilder wb = new WorldBuilder();
            Models.System s;
            World world;
            State initialState;
            switch (index)
            {
                case 1:
                    world = wb.GetMuseumWorld();
                    initialState = new State(0, 0, world.Cameras);
                    s = new Models.System(world, initialState);
                    break;
                case 2:
                    world = wb.GetEmptyWorld();
                    initialState = new State(0, 0, world.Cameras);
                    s = new Models.System(world, initialState);
                    break;
                default:
                    world = wb.GetDefaultWorld();
                    initialState = new State(0, 0, world.Cameras);
                    s = new Models.System(world, initialState);
                    break;
            }

            

            SystemView newSystemView = s.GetSystemView();
            HttpContext.Session.SetString(SessionKeySystem, JsonSerializer.Serialize(newSystemView));
            return newSystemView;
            
            
            
        }

        [HttpGet]
        [Route("modify-cell")]
        public SystemView ModifyCell([FromQuery] int x, [FromQuery] int y, [FromQuery] string cellType)
        {
            SystemView systemView =
                JsonSerializer.Deserialize<SystemView>(HttpContext.Session.GetString(SessionKeySystem));
            Models.System s = new Models.System(systemView);
            s.ModifyCell(x - 1, y - 1, cellType);
            SystemView newSystemView = s.GetSystemView();
            HttpContext.Session.SetString(SessionKeySystem, JsonSerializer.Serialize(newSystemView));
            return newSystemView;
        }

        [HttpGet]
        [Route("map-size")]
        public SystemView ChangeMapSize([FromQuery] int dx, [FromQuery] int dy)
        {
            SystemView systemView =
                JsonSerializer.Deserialize<SystemView>(HttpContext.Session.GetString(SessionKeySystem));
            Models.System s = new Models.System(systemView);
            s.ChangeMapSize(dx - 2, dy - 2);
            SystemView newSystemView = s.GetSystemView();
            HttpContext.Session.SetString(SessionKeySystem, JsonSerializer.Serialize(newSystemView));
            return newSystemView;
        }

        [HttpGet]
        [Route("move")]
        public SystemView MoveTarget([FromQuery] int? dx, [FromQuery] int? dy)
        {
            SystemView systemView =
                JsonSerializer.Deserialize<SystemView>(HttpContext.Session.GetString(SessionKeySystem));
            Models.System s = new Models.System(systemView);
            s.AdvanceSystem(dx, dy);
            SystemView newSystemView = s.GetSystemView();
            HttpContext.Session.SetString(SessionKeySystem, JsonSerializer.Serialize(newSystemView));
            return newSystemView;
        }
    }
}