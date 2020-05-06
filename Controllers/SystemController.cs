using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using POMCP.Website.Models;
using POMCP.Website.Models.Environment;
using POMCP.Website.Models.Environment.Cells;

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
        public SystemView Get([FromQuery] int? dx, [FromQuery] int? dy, [FromQuery] bool? init = false)
        {
            if (init != null && (bool) !init)
            {
                
            }
            
            Console.Out.WriteLine(dx + " - " + dy);
            return new SystemView(Models.System.Instance.GetTrueStateGrid(),
                Models.System.Instance.GetCameraViewGrid(),
                Models.System.Instance.GetProbaGrid(),
                Models.System.Instance.GetCameras(),
                Models.System.Instance.GetMoveOptions());
        }
        
    }
    
}