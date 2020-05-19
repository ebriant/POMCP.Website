using System;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using POMCP.Website.Models;

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
        public SystemView Get([FromQuery] int? dx, [FromQuery] int? dy, [FromQuery] bool? init = false)
        {
            // Random random = new Random();
            // const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            // string test = new string(Enumerable.Repeat(chars, 8)
            //     .Select(s => s[random.Next(s.Length)]).ToArray());
            // HttpContext.Session.SetString("test", test);

            Models.System s;
            if (init != null && (bool) !init)
            {
                string testString = HttpContext.Session.GetString("test");
                string systemString = HttpContext.Session.GetString(SessionKeySystem);
                SystemView systemView = JsonSerializer.Deserialize<SystemView>(HttpContext.Session.GetString(SessionKeySystem));
                s = new Models.System(systemView);
                s.AdvanceSystem(dx, dy);
                // Models.System.Instance.AdvanceSystem(dx, dy);
            }
            else
                s = Models.System.GetStartingSystem();


            SystemView newSystemView = s.GetSystemView();
            HttpContext.Session.SetString("test", "J'aime les petit pois");
            string syst = JsonSerializer.Serialize(newSystemView);
            HttpContext.Session.SetString(SessionKeySystem, JsonSerializer.Serialize(newSystemView));
            return newSystemView;
        }

        [HttpGet]
        [Route("modify-cell")]
        public SystemView ModifyCell([FromQuery] int x, [FromQuery] int y, [FromQuery] string cellType)
        {
            SystemView systemView = JsonSerializer.Deserialize<SystemView>(HttpContext.Session.GetString(SessionKeySystem));
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
            SystemView systemView = JsonSerializer.Deserialize<SystemView>(HttpContext.Session.GetString(SessionKeySystem));
            Models.System s = new Models.System(systemView);
            s.ChangeMapSize(dx - 2, dy - 2);
            SystemView newSystemView = s.GetSystemView();
            HttpContext.Session.SetString(SessionKeySystem, JsonSerializer.Serialize(newSystemView));
            return newSystemView;
        }

        [HttpPost]
        [Route("tree-depth")]
        public void SetTreeDepth([FromQuery] int value)
        {
            Models.System.Instance.TreeDepth = value;
        }

        [HttpGet]
        [Route("test")]
        public string GetTest()
        {
            string t = HttpContext.Session.GetString("test");
            HttpContext.Session.SetString("test", t);
            return t;
        }
    }
}