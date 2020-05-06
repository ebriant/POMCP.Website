using System;
using System.Collections.Generic;
using POMCP.Website.Models.Cameras;
using POMCP.Website.Models.Environment;

namespace POMCP.Website.Models.Pomcp
{
    /// <summary>
    /// Class representing the Markov Decision Process
    /// </summary>
    public class MDP
    {
        public World World { get; }

        public MDP(World world)
        {
            World = world;
        }

        public List<State> GetAllState(State state)
        {
            List<State> statesList = new List<State>();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (World.Map.IsCellFree(state.X + i, state.Y + j))
                        statesList.Add(new State(state.X + i, state.Y + j, state.CamerasOrientations));
                }
            }
            return statesList;
        }

        public List<Action> GetAllActions(State state)
        {
            List<Action> actionsList = null;
            for (int i = 0; i < World.Cameras.Count; i++)
            {
                Camera camera = World.Cameras[i];
                if (actionsList == null)
                {
                    actionsList = new List<Action>();
                    foreach (double o in camera.GetActions(state.CamerasOrientations[i]))
                    {
                        actionsList.Add(new Action(o, World.Cameras.Count));
                    }
                }
                else
                {
                    List<Action> L2 = new List<Action>();

                    foreach (Action a in actionsList)
                    {
                        foreach (double o in camera.GetActions(state.CamerasOrientations[i]))
                        {
                            List<double> orient = a.Orientations;
                            orient[camera.Num] = o;

                            L2.Add(new Action(orient));
                        }
                    }

                    actionsList = L2;
                }
            }

            return actionsList;
        }

        /// <summary>
        /// Merge distributions of observations
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public Distribution<Observation> CrossDistributions(Distribution<Observation> d1, Distribution<Observation> d2)
        {
            Distribution<Observation> dnew = new Distribution<Observation>();
            if (d1.ContainsKey(new Observation(false)))
                return d2;
            if (d2.ContainsKey(new Observation(false)))
                return d1;
            
            double p;
            foreach (Observation observation in d1.GetKeys()) {
                p = d1.getProba(observation)*d2.getProba(observation);
                if (p>0)
                    dnew.setProba(observation, p);
            }
            if (dnew.GetKeys().Count > 0) {
                dnew.Normalise();
            }
            return dnew;
        }

        public Distribution<Observation> GetAllObservations(State state)
        {
            Distribution<Observation> observationDistribution = new Distribution<Observation>();
            foreach (Camera camera in World.Cameras)
            {
                if (observationDistribution.GetKeys().Count == 0)
                {
                    observationDistribution = camera.GetObservation(state);
                }
                else
                {
                    observationDistribution = CrossDistributions(observationDistribution, camera.GetObservation(state));
                }
            }

            return observationDistribution;
        }

        public double GetStateValue(State s)
        {
            double v = 0;
            foreach (Camera c in World.Cameras)
            {
                v += c.GetValue(s);
            }
            return v; //Math.min(1, v);
        }

        public State GetActionResult(State state, Action action)
        {
            List<double> L = new List<double>();
            for (int i = 0; i < state.CamerasOrientations.Count; i++)
                L.Add(state.CamerasOrientations[i] + action.Orientations[i]);
            return new State(state.X, state.Y, L);
        }


        public Distribution<State> UpdateTransition(State s, Action a)
        {
            Distribution<State> d = new Distribution<State>();
            d.setProba(s, 1);
            return UpdateTransition(d, a);
        }

        public Distribution<State> UpdateTransition(Distribution<State> d, Action a)
        {
            Distribution<State> d1 = new Distribution<State>();
            foreach (State s in d.GetKeys()) {
                if (d.getProba(s) > 0)
                    d1.setProba(GetActionResult(s, a), d.getProba(s));
            }
            Distribution<State> dnew = new Distribution<State>();

            Distribution<State> transition;

            foreach (State s1 in d1.GetKeys()) {
                transition = World.Target.GetTransition(s1, this.GetAllState(s1));

                foreach (State s2 in transition.GetKeys()){
                    dnew.setProba(s2, dnew.getProba(s2) + d1.getProba(s1) * transition.getProba(s2));
                }
            }
            return dnew;
        }
        
        
        //
        /// <summary>
        /// Update a belief given an observation
        /// </summary>
        /// <param name="d"></param>
        /// <param name="o"></param>
        /// <returns></returns>
        public Distribution<State> UpdateObservation(Distribution<State> d, Distribution<Observation> o)
        {
            Distribution<State> dnew = new Distribution<State>();

            if (o.ContainsKey(new Observation(false)))
            {
                foreach (State s in d.GetKeys()) {
                    bool visible = false;
                    foreach (Camera c in World.Cameras) {
                        if (c.GetVision(s.CamerasOrientations[c.Num])[s.X,s.Y])
                            visible = true;
                    }
                    if (!visible && Math.Abs(d.getProba(s)) > 0.001f)
                        dnew.setProba(s, d.getProba(s));
                }
            }
            else
            {
                foreach (State s in d.GetKeys())
                {
                    double p = 0;
                    p += d.getProba(s) * o.getProba(new Observation(true, s.X, s.Y));
                    if (p > 0)
                        dnew.setProba(s, p);
                }
            }

            dnew.Normalise();
            return dnew;
        }
    }
}