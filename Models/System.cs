using System;
using System.Collections.Generic;
using System.ComponentModel;
using POMCP.Website.Models.Cameras;
using POMCP.Website.Models.Environment;
using POMCP.Website.Models.Environment.Cells;
using POMCP.Website.Models.Pomcp;
using Action = POMCP.Website.Models.Pomcp.Action;

namespace POMCP.Website.Models
{
    [Serializable]
    public struct CameraProperties
    {
        public int X { get; set; }
        public int Y { get; set; }
        public double Orientation { get; set; }
        public double Fov { get; set; }
        
        
        public CameraProperties(int x, int y, double orientation, double fov)
        {
            X = x;
            Y = y;
            Orientation = orientation;
            Fov = fov;
        }

        public override string ToString()
        {
            return "Camera. X:" + X + ", Y: " + Y + ", Ori: " + Orientation + ", FOV: " + Fov;
        }
    }
    
    
    /// <summary>
    /// Structure that represents the current state of the system in an easy processable way for the view
    /// </summary>
    [Serializable]
    public struct SystemView
    {
        public string[][] Map { get; set; }
        public int[] TrueState { get; set; }
        public CameraProperties[] Cameras { get; set; }
        public double[][] Probabilities { get; set; }
        public int[][] CamerasVision { get; set; }
        public bool[][] MovingOptions { get; set; }

        public SystemView(string[][] map, int[] trueState, CameraProperties[] cameras, double[][] probabilities,
            int[][] camerasVision, bool[][] movingOptions)
        {
            Map = map;
            TrueState = trueState;
            Cameras = cameras;
            Probabilities = probabilities;
            CamerasVision = camerasVision;
            MovingOptions = movingOptions;
        }
    }

    
    public class System
    {
        
        private World World { get; set; }

        private readonly MarkovModel _model;

        private Distribution<State> CurrentDistribution { get; set; }
        
        private State TrueState { get; set; }

        // Number of iteration perform for the building of a sampling tree
        public int TreeSamplesCount { get; set; } = 500;

        // Maximum depth of a tree
        public int TreeDepth { get; set; } = 3;
        
        // 
        public float Gama { get; set; }= 0.3f;
        
        // Exploration parameter of the UCB searching,
        public float C { get; set; } = 1.4f;
        
        private int MaxMapSize { get; } = 12;

        public System(World world, State initialState)
        {
            TrueState = initialState;
            World = world;
            _model = new MarkovModel(World);
            InitializeSystem();
        }

        /// <summary>
        /// Create a system given the system view. This is used to restore the System for each Session
        /// </summary>
        /// <param name="systemView"></param>
        public System(SystemView systemView)
        {
            
            World = new World(systemView.Map);
            Dictionary<Camera, double> camerasOrientation = new Dictionary<Camera, double>();
            foreach (CameraProperties cameraProp in systemView.Cameras)
            {
                //Correct the offset due to the addition of the walls in the system view
                Camera camera = new AngularCamera(cameraProp.X-1, cameraProp.Y-1);
                World.AddCamera(camera);
                camerasOrientation[camera] = cameraProp.Orientation;
            }
            // To remove if possible
            World.InitializeCameras();
            
            _model = new MarkovModel(World);

            TrueState = new State(systemView.TrueState[0]-1,systemView.TrueState[1]-1, camerasOrientation);
            Distribution<State> d = new Distribution<State>();
            // Ignore the first and last row/column to account for the offset caused by the outside walls
            for (int i = 1; i < systemView.Probabilities.Length-1; i++)
            {
                for (int j = 1; j < systemView.Probabilities[i].Length-1; j++)
                {
                    if (systemView.Probabilities[i][j] > 0)
                        d.SetProba(new State(i-1,j-1,camerasOrientation), systemView.Probabilities[i][j]);
                }
            }
            CurrentDistribution = d;
            
        }

        private void InitializeSystem()
        {
            World.InitializeCameras();
            CurrentDistribution = new Distribution<State>();
            CurrentDistribution.SetProba(TrueState, 1);
        }

        private ActionNode GetBestAction()
        {
            SamplingTree tree = new SamplingTree(
                CurrentDistribution, 
                _model,
                TreeSamplesCount,
                TreeDepth,
                Gama,
                C);
            return tree.GetBestAction();
        }

        public void AdvanceSystem(int? dx, int? dy)
        {
            Action action = GetBestAction().Action;

            if (dx != null && dy != null && World.Map.IsCellFree((int) (TrueState.X + dx), (int) (TrueState.Y + dy)))
            {
                TrueState = _model.GetActionResult(
                    new State((int) (TrueState.X + dx), (int) (TrueState.Y + dy), TrueState.CamerasOrientations),
                    action
                );
            }
            else
            {
                TrueState = _model.ApplyTransition(TrueState, action).Draw();
            }


            Distribution<Observation> observation = new Distribution<Observation>();
            foreach (Camera c in World.Cameras)
            {
                if (observation.Prob.Count == 0)
                {
                    observation = c.GetObservation(TrueState);
                }
                else
                {
                    observation =
                        _model.CrossDistributions(observation, c.GetObservation(TrueState));
                }
            }

            CurrentDistribution = _model.ApplyTransition(CurrentDistribution, action);
            CurrentDistribution = _model.ApplyObservation(CurrentDistribution, observation);
        }


        /// <summary>
        /// Return the grid describing the true state of the system
        /// </summary>
        /// <returns></returns>
        public string[][] GetMapArray()
        {
            string[][] cellArray = World.Map.GetCellsArray();
            return AddWalls(cellArray);
        }


        /// <summary>
        /// Add walls to the cell array representing the map. The wall do not technically exist in the model
        /// but are added for visualization to represent the boundaries of the map.
        /// </summary>
        /// <param name="cellArray"></param>
        /// <returns></returns>
        public string[][] AddWalls(string[][] cellArray)
        {
            string[][] result = new string[cellArray.Length + 2][];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new string[cellArray[0].Length + 2];
                for (int j = 0; j < result[i].Length; j++)
                {
                    if (i == 0 || j == 0 || i == result.Length - 1 || j == result[i].Length - 1)
                        result[i][j] = new Wall().CellType;
                    else
                    {
                        result[i][j] = cellArray[i - 1][j - 1];
                    }
                }
            }

            return result;
        }


        /// <summary>
        /// Return the grid describing the current distribution of the system
        /// (the probabilities of the target for every cell)
        /// </summary>
        /// <returns></returns>
        public double[][] GetProbaGrid()
        {
            // Size is map size +2 to account for the walls
            double[][] cellArray = new double[World.Map.Dx + 2][];
            for (int i = 0; i < cellArray.Length; i++)
            {
                cellArray[i] = new double[World.Map.Dy + 2];
            }

            foreach (KeyValuePair<State, double> keyValuePair in CurrentDistribution.Prob)
            {
                State key = keyValuePair.Key;
                cellArray[key.X + 1][key.Y + 1] = keyValuePair.Value;
            }

            return cellArray;
        }


        /// <summary>
        /// Return the view of the camera: 0 is invisible to the camera, 1 is potentially visible, 2 is observed
        /// </summary>
        /// <returns></returns>
        public int[][] GetCameraViewGrid()
        {
            int[][] cellArray = new int[World.Map.Dx + 2][];
            for (int i = 0; i < cellArray.Length; i++)
            {
                cellArray[i] = new int[World.Map.Dy + 2];
            }

            foreach (Camera camera in World.Cameras)
            {
                for (int i = 0; i < camera.VisibleCells.GetLength(0); i++)
                {
                    for (int j = 0; j < camera.VisibleCells.GetLength(1); j++)
                    {
                        if (camera.VisibleCells[i, j])
                            cellArray[i + 1][j + 1] = 1;
                    }
                }
            }

            foreach (Camera camera in World.Cameras)
            {
                bool[,] vision = camera.GetVision(TrueState.CamerasOrientations[camera]);
                for (int i = 0; i < camera.VisibleCells.GetLength(0); i++)
                {
                    for (int j = 0; j < camera.VisibleCells.GetLength(1); j++)
                    {
                        if (vision[i, j])
                        {
                            cellArray[i + 1][j + 1] = 2;
                        }
                    }
                }
            }

            return cellArray;
        }


        /// <summary>
        /// Return a list of the cameras properties for display (X, Y, current orientation and FOV)
        /// </summary>
        /// <returns></returns>
        public CameraProperties[] GetCameras()
        {
            List<CameraProperties> result = new List<CameraProperties>();
            
            foreach (Camera camera in World.Cameras)
            {
                result.Add(new CameraProperties(camera.X + 1, camera.Y + 1
                    , TrueState.CamerasOrientations[camera], camera.FOV));
            }

            return result.ToArray();
        }

        /// <summary>
        /// Return a matrix representing the possibility of movement from the present state
        /// options are a 3x3 matrix from (-1,-1) to (+1,+1) 
        /// </summary>
        /// <returns></returns>
        public bool[][] GetMoveOptions()
        {
            bool[][] result = new bool[3][];
            for (int i = 0; i < 3; i++)
            {
                result[i] = new bool[3];
                for (int j = 0; j < 3; j++)
                {
                    result[i][j] = World.Map.IsCellFree(TrueState.X + i - 1, TrueState.Y + j - 1);
                }
            }

            return result;
        }


        /// <summary>
        /// Return the System view object that represents the current state of the system formatted for view
        /// </summary>
        /// <returns></returns>
        public SystemView GetSystemView()
        {
            return new SystemView(
                GetMapArray(),
                new[] {TrueState.X + 1, TrueState.Y + 1},
                GetCameras(),
                GetProbaGrid(),
                GetCameraViewGrid(),
                GetMoveOptions());
        }


        /// <summary>
        /// Updates the system at a certain position, the update depend on the keyword passed with the position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="cellType"></param>
        public void ModifyCell(int x, int y, string cellType)
        {
            if (!(World.Map.IsInMap(x, y)))
                return;
            
            switch (cellType)
            {
                case ("wall"):
                    if (!(TrueState.X == x && TrueState.Y == y))
                    {
                        World.Map.Cells[x,y] =new Wall();
                    }
                        
                    break;
                case ("glass"):
                    if (!(TrueState.X == x && TrueState.Y == y))
                        World.Map.Cells[x,y] = new Glass();
                    break;
                case "target":
                    if (World.Map.IsCellFree(x, y))
                    {
                        TrueState.X = x;
                        TrueState.Y = y;
                    }
                    break;
                case "camera":
                    if (World.IsCamera(x, y, out Camera camera))
                    {
                        RemoveCamera(camera);
                    }
                    else if (World.Map.IsCellFree(x, y))
                    {
                        Camera newCamera = new AngularCamera(x, y);
                        World.AddCamera(newCamera);
                        TrueState.CamerasOrientations[newCamera]= 0d;
                    }
                    break;
                default:
                    World.Map.Cells[x,y] = null;
                    break;
            }
            InitializeSystem();
        }

        public void RemoveCamera(Camera camera)
        {
            World.Cameras.Remove(camera);
            TrueState.CamerasOrientations.Remove(camera);
        }

        public void ChangeMapSize(int dx, int dy)
        {
            dx = Math.Clamp(dx, TrueState.X + 1, MaxMapSize);
            dy = Math.Clamp(dy, TrueState.Y + 1, MaxMapSize);

            World.Map.ResizeMap(dx, dy);
            
            foreach (Camera camera in World.Cameras.ToArray())
            {
                if (camera.X >= dx || camera.Y >= dy)
                    RemoveCamera(camera);
            }
            
            InitializeSystem();
        }
        
    }
}