using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace POMCP.Website.Controllers
{

    public struct CameraProperties
    {
        public int X { get; }
        public int Y { get; }
        public double Orientation { get; }
        public double Fov { get; }


        public CameraProperties(int x, int y, double orientation, double fov)
        {
            X = x;
            Y = y;
            Orientation = orientation;
            Fov = fov;
        }

        public override string ToString()
        {
            return "Camera. X:" + X +", Y: "+ Y + ", Ori: "+Orientation + ", FOV: "+Fov;
        }
    }
    
    public struct SystemView
    {
        public string[][] TrueState { get; }
        public string[][] CameraView { get; }
        public string[][] DistributionView { get; }
        public CameraProperties[] Cameras { get; }

        public bool[][] MovingOptions { get; }

        public SystemView(string[][] trueState, string[][] cameraView, string[][] distributionView, CameraProperties[] cameras, bool[][] movingOptions)
        {
            TrueState = trueState;
            CameraView = cameraView;
            DistributionView = distributionView;
            Cameras = cameras;
            MovingOptions = movingOptions;
        }
    }
    
    [ApiController]
    [Route("[controller]")]
    public class POMCPController : Controller
    {

        private readonly ILogger<WeatherForecastController> _logger;

        public POMCPController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public SystemView Get([FromQuery] int? dx, [FromQuery] int? dy, [FromQuery] bool? init = false)
        {
            if (init != null && (bool) !init)
            {
                // if (IsMoveAllowed(dx, dy))
                _logger.Log(LogLevel.Information, dx + " - " + dy);
                Models.System.Instance.AdvanceSystem(dx, dy);
                
                _logger.Log(LogLevel.Information, "Hi");
            }
            
            
            _logger.Log(LogLevel.Information, "Hi2");
            return new SystemView(Models.System.Instance.GetTrueStateGrid(),
                Models.System.Instance.GetCameraViewGrid(),
                Models.System.Instance.GetProbaGrid(),
                Models.System.Instance.GetCameras(),
                Models.System.Instance.GetMoveOptions());
        }

        /// <summary>
        /// Return true if the intended moving is allowed, false either
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <returns></returns>
        private bool IsMoveAllowed(int dx, int dy)
        {
            return Math.Abs(dx) < 2 && Math.Abs(dy) < 2 && Models.System.Instance.IsMoveAllowed(dx,dy);
        }
    }
    
}