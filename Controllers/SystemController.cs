using System;
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

        private readonly ILogger<PomcpController> _logger;

        public PomcpController(ILogger<PomcpController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public SystemView Get([FromQuery] int? dx, [FromQuery] int? dy, [FromQuery] bool? init = false)
        {
            if (init != null && (bool) !init)
                Models.System.Instance.AdvanceSystem(dx, dy);
            
            return Models.System.Instance.GetSystemView();
        }

        [HttpGet]
        [Route("modify-cell")]
        public SystemView ModifyCell([FromQuery] int x, [FromQuery] int y, [FromQuery] string cellType)
        {
            Models.System.Instance.ModifyCell(x-1,y-1,cellType);
            return Models.System.Instance.GetSystemView();
        }

        [HttpGet]
        [Route("map-size")]
        public SystemView ChangeMapSize([FromQuery] int dx, [FromQuery] int dy)
        {
            Models.System.Instance.ChangeMapSize(dx-2,dy-2);
            return Models.System.Instance.GetSystemView();
        }
        
        [HttpPost]
        [Route("tree-depth")]
        public void SetTreeDepth([FromQuery] int value)
        {
            Models.System.Instance.TreeDepth = value;
        }
    }
}