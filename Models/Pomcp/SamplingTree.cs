using System;

namespace POMCP.Website.Models.Pomcp
{
    public class SamplingTree
    {
        /// <summary>
        /// Root of the Tree
        /// </summary>
        public BeliefNode Root { get; }

        /// <summary>
        /// Markov model used
        /// </summary>
        private MDP mdp;

        public const float gama = 0.3f;

        
        // exploration parameter of the UCB searching,
        private const float C = 1.4f;
        
        //
        /// <summary>
        /// Create the Sampling tree of potential events
        /// </summary>
        /// <param name="d">Actual distribution of states of the system</param>
        /// <param name="sampleNumber">number of samples in the tree</param>
        /// <param name="maxDepth">maximum depth for each branch</param>
        /// <param name="mdp">model</param>
        public SamplingTree(Distribution<State> d, int sampleNumber, int maxDepth, MDP mdp)
        {
            // The root is the given distribution
            this.mdp = mdp;
            Root = new BeliefNode(d, null, null, mdp);
            for (int i = 0; i < sampleNumber; i++)
            {
                GrowTree(maxDepth);
            }
        }

        /// <summary>
        /// Initialize a sampling tree with an  existing belief node
        /// </summary>
        /// <param name="n"></param>
        /// <param name="sampleNumber"></param>
        /// <param name="maxDepth"></param>
        /// <param name="mdp"></param>
        public SamplingTree(BeliefNode n, int sampleNumber, int maxDepth, MDP mdp)
        {
            // The root is an existing beliefNode, useful to conserve the previous explorations
            Root = n;
            this.mdp = mdp;
            for (int i = 0; i < sampleNumber; i++)
            {
                GrowTree(maxDepth);
            }
        }
        
        
        //
        /// <summary>
        /// Function that perform one exploration of the tree, and make it grow by a pair of action & belief nodes
        /// </summary>
        /// <param name="maxDepth"></param>
        public void GrowTree(int maxDepth)
        {
            BeliefNode beliefNode = Root;
            Root.IncrementOccurrence();
            
            
            // Explore the tree until it reaches the finish point or the number of iteration is too high
            int depth = 1;
            
            ActionNode actionNode;
            Action action;
            State state;
            bool stocking = true;
            while (depth < maxDepth & !beliefNode.IsTerminal())
            {
                // Pick a state in the belief of the actual belief node
                state = beliefNode.Belief.GetNormalisedCopy().Draw();

                // Draw the next action 
                action = beliefNode.DrawAction(state, stocking, C);

                // If the new node is not in the children, it is added, else its occurrence is incremented
                actionNode = beliefNode.SearchChildren(action);
                if (actionNode == null)
                {
                    actionNode = new ActionNode(action, beliefNode);
                    if (stocking)
                        beliefNode.AddChild(actionNode);
                }
                else
                {
                    actionNode.IncrementOccurrence();
                }

                // Get a new state of the system			
                state = mdp.UpdateTransition(state, action).Draw();

                Observation observation = mdp.GetAllObservations(state).Draw();

                // If the new node is not in the children, it is added, else its occurrence is incremented

                beliefNode = actionNode.SearchChildren(observation);
                if (beliefNode == null)
                {
                    beliefNode = new BeliefNode(observation, actionNode, mdp);
                    if (stocking)
                        actionNode.AddChild(beliefNode);
                    stocking = false;
                }
                else
                {
                    beliefNode.IncrementOccurrence();
                }

                beliefNode.AddState(state);

                depth++;
            }

            beliefNode.UpdateValue(gama);
        }
        
        

        public ActionNode GetBestAction()
        {
            ActionNode chosenNode = null;
            foreach (ActionNode actionNode in Root.Children)
            {
                if (chosenNode == null)
                    chosenNode = actionNode;
                else if (chosenNode.Value < actionNode.Value)
                    chosenNode = actionNode;
            }
            return chosenNode;
        }
    }
}