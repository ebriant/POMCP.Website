using System;
using System.Collections.Generic;
using POMCP.Website.Models.Cameras;
using POMCP.Website.Models.Environment;

namespace POMCP.Website.Models.Pomcp
{
    /// <summary>
    /// Class representing the Markov Model that dictates how the system changes at every time step
    /// </summary>
    
    public class MarkovModel
    {
        private World World { get; }

        public MarkovModel(World world)
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
                        statesList.Add(new State(state.X + i, state.Y + j, i, j, state.CamerasOrientations));
                }
            }
            return statesList;
        }

        public List<Action> GetAllActions(State state)
        {
            List<Action> actionsList = new List<Action>();
            actionsList.Add(new Action());
            
            foreach (Camera camera in state.CamerasOrientations.Keys)
            {
                List<Action> newActionsList = new List<Action>();
                foreach (Action a in actionsList)
                {
                    foreach (double o in camera.GetActions())
                    {
                        a.OrientationsChanges[camera] = o;

                        newActionsList.Add(new Action(a.OrientationsChanges));
                    }

                    actionsList = newActionsList;
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
                p = d1.GetProba(observation)*d2.GetProba(observation);
                if (p>0)
                    dnew.SetProba(observation, p);
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

        /// <summary>
        /// Return the value of a state. The value is used in 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public double GetStateValue(State s)
        {
            double value = 0;
            foreach (Camera camera in s.CamerasOrientations.Keys)
            {
                value += camera.GetValue(s);
            }
            return value;
        }

        /// <summary>
        /// Return the state that is the result of the given action on the given state
        /// </summary>
        /// <param name="state">given state</param>
        /// <param name="action">given action</param>
        /// <returns></returns>
        public State GetActionResult(State state, Action action)
        {
            Dictionary<Camera, double> orientations = new Dictionary<Camera, double> ();
            foreach (KeyValuePair<Camera,double> keyValuePair in state.CamerasOrientations)
            {
                orientations[keyValuePair.Key] = keyValuePair.Value + action.OrientationsChanges[keyValuePair.Key];
            }
            return new State(state.X, state.Y, state.Dx, state.Dy, orientations);
        }


        /// <summary>
        /// Apply an action and the Markov transition to a state, return the resulting distribution
        /// </summary>
        /// <param name="s"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Distribution<State> ApplyTransition(State s, Action action)
        {
            Distribution<State> d = new Distribution<State>();
            d.SetProba(s, 1);
            return ApplyTransition(d, action);
        }

        /// <summary>
        /// Apply an action to a distribution and returns the result distribution.
        /// This method first apply the action to the states of the distribution.
        /// Then, it apply the normal state transition, the evolution of the system independent from the action
        /// (i.e here the movement of the target)
        /// </summary>
        /// <param name="d"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Distribution<State> ApplyTransition(Distribution<State> d, Action action)
        {
            // Create a new distribution that contains the states after applying the action
            Distribution<State> d1 = new Distribution<State>();
            foreach (State s in d.GetKeys()) {
                if (d.GetProba(s) > 0)
                    d1.SetProba(GetActionResult(s, action), d.GetProba(s));
            }
            
            // New distribution that contains the states after the transition (i.e after movement of the target)
            Distribution<State> dnew = new Distribution<State>();
            foreach (State s1 in d1.GetKeys()) {
                Distribution<State> transition = World.Target.GetTransition(s1, GetAllState(s1));

                foreach (State s2 in transition.GetKeys()){
                    dnew.SetProba(s2, dnew.GetProba(s2) + d1.GetProba(s1) * transition.GetProba(s2));
                }
            }
            return dnew;
        }
        
        
        //
        /// <summary>
        /// Update a distribution of state probability given an observation
        /// </summary>
        /// <param name="d"></param>
        /// <param name="o"></param>
        /// <returns></returns>
        public Distribution<State> ApplyObservation(Distribution<State> d, Distribution<Observation> o)
        {
            Distribution<State> dnew = new Distribution<State>();

            if (o.ContainsKey(new Observation(false)))
            {
                foreach (State s in d.GetKeys()) {
                    bool visible = false;
                    foreach (Camera c in s.CamerasOrientations.Keys) {
                        if (c.GetVision(s.CamerasOrientations[c])[s.X,s.Y])
                            visible = true;
                    }
                    if (!visible && Math.Abs(d.GetProba(s)) > 0.001f)
                        dnew.SetProba(s, d.GetProba(s));
                }
            }
            else
            {
                foreach (State s in d.GetKeys())
                {
                    double p = 0;
                    p += d.GetProba(s) * o.GetProba(new Observation(true, s.X, s.Y));
                    if (p > 0)
                        dnew.SetProba(s, p);
                }
            }
            dnew.Normalise();
            return dnew;
        }
    }
}