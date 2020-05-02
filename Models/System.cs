using System.Collections.Generic;
using POMCP.Website.Models.Cameras;
using POMCP.Website.Models.Environment;
using POMCP.Website.Models.Pomcp;

namespace POMCP.Website.Models
{
    public class System
    {

        public static System instance;

        private static System GetInstance()
        {
            World world = WorldBuilder.DefaultWorld;
            Distribution<State> d = new Distribution<State>();
            List<double> camerasOrientations = new List<double>();
            foreach (Camera camera in world.Cameras)
            {
                camerasOrientations.Add(0d);
            }
            d.setProba(new State(7,1,camerasOrientations), 1);	
            MDP mdp = new MDP(world);
            return new System(world, mdp, d, 500, 3);
        }


        public World World { get; }

        private MDP _model;
        
        public Distribution<State> CurrentDistribution { get; set; }
        
        public Observation LastObservation { get; }
        
        private Action LastAction { get; set; }
        
        private State _trueState;

        private BeliefNode lastNode = null;

        SamplingTree _tree;

        public int TreeSamplesCountCount;

        public int maxIteration;

        
        public System(World world, MDP mdp, Distribution<State> initial, State initialState, int treeSamplesCount, int treeDepth)
        {
            CurrentDistribution = initial;
            _trueState = initialState;
            _model = mdp;
            World = world;
            TreeSamplesCountCount = treeSamplesCount;
            maxIteration = treeDepth;
        }

        public System(World world, MDP mdp, Distribution<State> initial, int treeSamplesCount, int treeDepth)
        {
            CurrentDistribution = initial;
            _trueState = CurrentDistribution.Draw();
            _model = mdp;
            World = world;
            TreeSamplesCountCount = treeSamplesCount;
            maxIteration = treeDepth;
        }

        
        public void AdvanceSystem(int n)
        {
            for (int i = 0; i < n; i++)
            {
                AdvanceSystem();
            }
        }

        /**
	 * permet de faire evoluer l'etat reel et la connaissance sur l'etat
	 */
        public void AdvanceSystem()
        {
            if (lastNode == null)
            {
                _tree = new SamplingTree(CurrentDistribution, TreeSamplesCountCount, maxIteration, _model);
            }
            else
            {
                lastNode.SetAsRoot(CurrentDistribution);
                _tree = new SamplingTree(lastNode, TreeSamplesCountCount, maxIteration, _model);
            }

            _tree = new SamplingTree(CurrentDistribution, TreeSamplesCountCount, maxIteration, _model);

            ActionNode a = _tree.GetBestAction();
            LastAction = a.Action;

            _trueState = _model.UpdateTransition(_trueState, LastAction).Draw();

            Distribution<Observation> observationDistribution = new Distribution<Observation>();
            foreach (Camera c in World.Cameras)
            {
                if (observationDistribution.GetKeys().Count == 0)
                {
                    observationDistribution = c.GetObservation(_trueState);
                }
                else
                {
                    observationDistribution = _model.CrossDistributions(observationDistribution,c.GetObservation(_trueState));
                }
            }

            CurrentDistribution = _model.UpdateTransition(CurrentDistribution, LastAction);
            CurrentDistribution = _model.UpdateObservation(CurrentDistribution, observationDistribution);
        }

        /**
	 * permet de faire evoluer l'etat reel et la connaissance sur l'etat
	 */
        public void AdvanceSystem(State s)
        {
            if (lastNode == null)
            {
                _tree = new SamplingTree(CurrentDistribution, TreeSamplesCountCount, maxIteration, _model);
            }
            else
            {
                lastNode.SetAsRoot(CurrentDistribution);
                _tree = new SamplingTree(lastNode, TreeSamplesCountCount, maxIteration, _model);
            }

            _tree = new SamplingTree(CurrentDistribution, TreeSamplesCountCount, maxIteration, _model);

            ActionNode actionNode = _tree.GetBestAction();
            LastAction = actionNode.Action;

            if (World.Map.IsCellFree(s.X, s.Y))
            {
                _trueState = s;
            }

            _trueState = _model.GetActionResult(s, LastAction);

            Distribution<Observation> o = new Distribution<Observation>();
            foreach (Camera c in World.Cameras)
            {
                if (o.GetKeys().Count == 0)
                {
                    o = c.GetObservation(_trueState);
                }
                else
                {
                    o = _model.CrossDistributions(o, c.GetObservation(_trueState));
                }
            }

            CurrentDistribution = _model.UpdateTransition(CurrentDistribution, LastAction);
            CurrentDistribution = _model.UpdateObservation(CurrentDistribution, o);
        }
    }
}