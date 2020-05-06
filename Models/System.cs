using System;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.AspNetCore.Authentication;
using POMCP.Website.Controllers;
using POMCP.Website.Models.Cameras;
using POMCP.Website.Models.Environment;
using POMCP.Website.Models.Pomcp;
using Action = POMCP.Website.Models.Pomcp.Action;

namespace POMCP.Website.Models
{
    public class System
    {
        public static System Instance = GetInstance();

        private static System GetInstance()
        {
            World world = WorldBuilder.DefaultWorld;
            Distribution<State> d = new Distribution<State>();
            List<double> camerasOrientations = new List<double>();

            foreach (Camera camera in world.Cameras)
            {
                camerasOrientations.Add(-Math.PI / 2);
            }

            d.setProba(new State(6, 1, camerasOrientations), 0.5);
            d.setProba(new State(6, 2, camerasOrientations), 0.5);
            MDP mdp = new MDP(world);
            return new System(world, mdp, d, 500, 3);
        }


        public World World { get; }

        private MDP _model;

        private Distribution<State> CurrentDistribution { get; set; }

        public Observation LastObservation { get; }

        private Action LastAction { get; set; }

        private State TrueState { get; set; }

        private BeliefNode LastNode { get; } = null;

        SamplingTree _tree;

        private int TreeSamplesCount { get; }

        private int TreeDepth { get; }

        private System(World world, MDP mdp, Distribution<State> initial, int treeSamplesCount, int treeDepth)
        {
            CurrentDistribution = initial;
            TrueState = CurrentDistribution.Draw();
            _model = mdp;
            World = world;
            TreeSamplesCount = treeSamplesCount;
            TreeDepth = treeDepth;
        }

        public void AdvanceSystem(int n)
        {
            for (int i = 0; i < n; i++)
            {
                AdvanceSystem(null);
            }
        }


        private ActionNode GetBestAction()
        {
            if (LastNode == null)
            {
                _tree = new SamplingTree(CurrentDistribution, TreeSamplesCount, TreeDepth, _model);
            }
            else
            {
                LastNode.SetAsRoot(CurrentDistribution);
                _tree = new SamplingTree(LastNode, TreeSamplesCount, TreeDepth, _model);
            }

            return _tree.GetBestAction();
        }

        public void AdvanceSystem(State s)
        {
            ActionNode actionNode = GetBestAction();
            LastAction = actionNode.Action;

            if (s != null)
            {
                if (World.Map.IsCellFree(s.X, s.Y))
                {
                    TrueState = s;
                }
            }
            else
            {
                TrueState = _model.UpdateTransition(TrueState, LastAction).Draw();
            }


            Distribution<Observation> observationDistribution = new Distribution<Observation>();
            foreach (Camera c in World.Cameras)
            {
                if (observationDistribution.Prob.Count == 0)
                {
                    observationDistribution = c.GetObservation(TrueState);
                }
                else
                {
                    observationDistribution =
                        _model.CrossDistributions(observationDistribution, c.GetObservation(TrueState));
                }
            }

            CurrentDistribution = _model.UpdateTransition(CurrentDistribution, LastAction);
            CurrentDistribution = _model.UpdateObservation(CurrentDistribution, observationDistribution);
        }


        /// <summary>
        /// Return the grid describing the true state of the system
        /// </summary>
        /// <returns></returns>
        public string[][] GetTrueStateGrid()
        {
            string[][] cellArray = World.Map.GetCellsArray();
            cellArray[TrueState.X][TrueState.Y] = "target";
            return cellArray;
        }

        /// <summary>
        /// Return the grid describing the current distribution of the system
        /// (the probabilities of the target for every cell)
        /// </summary>
        /// <returns></returns>
        public string[][] GetProbaGrid()
        {
            string[][] cellArray = World.Map.GetCellsArray();
            foreach (KeyValuePair<State, double> keyValuePair in CurrentDistribution.Prob)
            {
                State key = keyValuePair.Key;
                cellArray[key.X][key.Y] = keyValuePair.Value.ToString();
            }

            return cellArray;
        }

        /// <summary>
        /// Return the view of the camera
        /// </summary>
        /// <returns></returns>
        public string[][] GetCameraViewGrid()
        {
            string[][] cellArray = World.Map.GetCellsArray("invisible");

            foreach (Camera camera in World.Cameras)
            {
                for (int i = 0; i < camera.VisibleCells.GetLength(0); i++)
                {
                    for (int j = 0; j < camera.VisibleCells.GetLength(1); j++)
                    {
                        if (camera.VisibleCells[i, j])
                            cellArray[i][j] = "visible";
                    }
                }
            }

            foreach (Camera camera in World.Cameras)
            {
                bool[,] vision = camera.GetVision(TrueState.CamerasOrientations[camera.Num]);
                for (int i = 0; i < camera.VisibleCells.GetLength(0); i++)
                {
                    for (int j = 0; j < camera.VisibleCells.GetLength(1); j++)
                    {
                        if (vision[i, j])
                        {
                            if (i == TrueState.X & j == TrueState.Y)
                                cellArray[i][j] = "target";
                            else
                                cellArray[i][j] = "undefined";
                        }
                    }
                }

                cellArray[camera.X][camera.Y] = "camera";
            }

            return cellArray;
        }


        public CameraProperties[] GetCameras()
        {
            List<CameraProperties> result = new List<CameraProperties>();
            foreach (Camera camera in World.Cameras)
            {
                result.Add(new CameraProperties(camera.X, camera.Y
                    , TrueState.CamerasOrientations[camera.Num], camera.FOV));
            }

            return result.ToArray();
        }

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
    }
}