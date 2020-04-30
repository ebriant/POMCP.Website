using POMCP.Website.Models.Cameras;
using POMCP.Website.Models.Environment;
using POMCP.Website.Models.Pomcp;

namespace POMCP.Website.Models
{
    public class System
    {
        public World World;
        
        private MDP _model;
        
        public Distribution<State> CurrentDistribution { get; set; }
        
        public Observation LastObservation { get; }
        
        private Action LastAction { get; set; }
        
        private State _trueState;

        private BeliefNode lastNode = null;

        SamplingTree _tree;

        public int samplesCount;

        public int maxIteration;

        
        public System(World m, MDP mdp, Distribution<State> initiale, State etatInitial, int samples, int maxIt)
        {
            CurrentDistribution = initiale;
            _trueState = etatInitial;
            _model = mdp;
            World = m;
            samplesCount = samples;
            maxIteration = maxIt;
        }

        public System(World m, MDP mdp, Distribution<State> initial, int samples, int maxIt)
        {
            CurrentDistribution = initial;
            _trueState = CurrentDistribution.Draw();
            _model = mdp;
            World = m;
            samplesCount = samples;
            maxIteration = maxIt;
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
                _tree = new SamplingTree(CurrentDistribution, samplesCount, maxIteration, _model);
            }
            else
            {
                lastNode.SetAsRoot(CurrentDistribution);
                _tree = new SamplingTree(lastNode, samplesCount, maxIteration, _model);
            }

            _tree = new SamplingTree(CurrentDistribution, samplesCount, maxIteration, _model);

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
                _tree = new SamplingTree(CurrentDistribution, samplesCount, maxIteration, _model);
            }
            else
            {
                lastNode.SetAsRoot(CurrentDistribution);
                _tree = new SamplingTree(lastNode, samplesCount, maxIteration, _model);
            }

            _tree = new SamplingTree(CurrentDistribution, samplesCount, maxIteration, _model);

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