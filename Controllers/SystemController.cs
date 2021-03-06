﻿using System;
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
            return SetSystem(s);
        }

        [HttpGet]
        [Route("modify-cell")]
        public SystemView ModifyCell([FromQuery] int x, [FromQuery] int y, [FromQuery] string cellType)
        {
            Models.System s = GetSystem();
            s.ModifyCell(x - 1, y - 1, cellType);
            return SetSystem(s);
        }

        [HttpGet]
        [Route("map-size")]
        public SystemView ChangeMapSize([FromQuery] int dx, [FromQuery] int dy)
        {
            Models.System s = GetSystem();
            s.ChangeMapSize(dx - 2, dy - 2);
            return SetSystem(s);
        }

        [HttpGet]
        [Route("move")]
        public SystemView MoveTarget([FromQuery] int? dx, 
            [FromQuery] int? dy, 
            [FromQuery] int treeSamplesCount,
            [FromQuery] int treeDepth,
            [FromQuery] float gama,
            [FromQuery] float c)
        {
            treeSamplesCount = Math.Clamp(treeSamplesCount, 10, 1000);
            treeDepth = Math.Clamp(treeDepth, 1, 5);
            gama = Math.Clamp(gama, 0, 1);
            c = Math.Clamp(c, 0, 1);
            
            Models.System s = GetSystem();
            s.AdvanceSystem(dx, dy, treeSamplesCount, treeDepth, gama, c);
            return SetSystem(s);
        }

        public Models.System GetSystem()
        {
            Models.System s;
            string systemString = HttpContext.Session.GetString(SessionKeySystem);
            if (systemString != null)
            {
                SystemView systemView =
                                JsonSerializer.Deserialize<SystemView>(HttpContext.Session.GetString(SessionKeySystem));
                s = new Models.System(systemView);
            }
            else
            {
                World world = new WorldBuilder().GetMuseumWorld();
                State initialState = new State(0, 0, world.Cameras);
                s = new Models.System(world, initialState);
            }

            return s;
        }

        public SystemView SetSystem(Models.System s)
        {
            SystemView newSystemView = s.GetSystemView();
            HttpContext.Session.SetString(SessionKeySystem, JsonSerializer.Serialize(newSystemView));
            return newSystemView;
        }
    }
}