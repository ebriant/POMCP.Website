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
        public string[][] Map { get; }
        public int[] TrueState { get; }
        public CameraProperties[] Cameras { get; }
        public double[][] Probabilities { get; }
        public int[][] CamerasVision { get; }
        public bool[][] MovingOptions { get; }

        public SystemView(string[][] map, int[] trueState, CameraProperties[] cameras, double[][] probabilities, int[][] camerasVision, bool[][] movingOptions)
        {
            Map = map;
            TrueState = trueState;
            Cameras = cameras;
            Probabilities = probabilities;
            CamerasVision = camerasVision;
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
        public SystemView Get([FromQuery] int? dx, [FromQuery] int? dy, [FromQuery] bool? init = false)
        {
            if (init != null && (bool) !init)
            {
                
                Models.System.Instance.AdvanceSystem(dx, dy);
            }
            Models.System s = Models.System.Instance;


            return new SystemView(
                s.GetMapArray(), 
                new[] {s.TrueState.X+1, s.TrueState.Y+1}, 
                s.GetCameras(), 
                s.GetProbaGrid(), 
                s.GetCameraViewGrid(),
                s.GetMoveOptions());

            // [s.TrueState.X, s.TrueState.Y],
            // Models.System.Instance.GetProbaGrid(),
            // Models.System.Instance.GetCameras(),
            // Models.System.Instance.GetMoveOptions());
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