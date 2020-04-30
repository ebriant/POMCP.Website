using System;
using System.Collections.Generic;

namespace POMCP.Website.Models.Pomcp
{
    public class BeliefNode : Node
    {
        
        //
        /// <summary>
        /// Observation that characterize the node
        /// </summary>
        public Observation Observation { get; }
        
        /// <summary>
        /// Distribution representing the states sampled, it is not a distribution of probability but rather
        /// a count of different samples, that once normalized, give an approximation of the distribution of probability
        /// </summary>
        public Distribution<State> Belief { get; private set; }

        /**
		 * Children of the node
		 */
        public List<ActionNode> Children { get; }

        /**
		 * Parent of the node : represent the action that led to this belief node
		 * Can be null if the Node is the root of the tree
		 */

        public ActionNode Parent { get; private set; }


        public MDP Mdp;
        
        /// <summary>
        /// Build a new belief node with a given starting distribution
        /// </summary>
        /// <param name="d">Starting distribution</param>
        /// <param name="o">Observation that characterize the node</param>
        /// <param name="n">Parent of the node</param>
        public BeliefNode(Distribution<State> d, Observation o, ActionNode n, MDP mdp) : base()
        {
            Observation = o;
            Parent = n;
            Mdp = mdp;
            Belief = d;
            Children = new List<ActionNode>();
        }

        /// <summary>
        /// Build a new belief node with an empty belief
        /// </summary>
        /// <param name="observation">Observation that characterize the node</param>
        /// <param name="parentNode">Parent of the node</param>
        public BeliefNode(Observation observation, ActionNode parentNode, MDP mdp)
        {
            Observation = observation;
            Parent = parentNode;
            Mdp = mdp;
            Belief = new Distribution<State>();
            Children = new List<ActionNode>();
        }

        public void SetAsRoot(Distribution<State> d)
        {
            Parent = null;
            Belief = d;
        }

        /// <summary>
        /// Add a child node at the end of the children list
        /// </summary>
        /// <param name="childNode"></param>
        public void AddChild(ActionNode childNode)
        {
            Children.Add(childNode);
        }
        
        /// <summary>
        /// Search a node representing the given action in the children list
        /// </summary>
        /// <param name="action">action to search</param>
        /// <returns></returns>
        public ActionNode SearchChildren(Action action)
        {
            foreach (ActionNode actionNode in Children)
            {
                if (actionNode.Action.Equals(action))
                    return actionNode;
            }

            return null;
        }

        /// <summary>
        /// Add the given state to the belief of the node
        /// </summary>
        /// <param name="state"></param>
        public void AddState(State state)
        {
            Belief.setProba(state, Belief.getProba(state) + 1);
        }

        //
        /// <summary>
        /// Return the minimum value among the children nodes. Useful as values can be negative 
        /// </summary>
        /// <returns>minimum value of all the children nodes</returns>
        public double GetMinChildrenValue()
        {
            double value = 0;
            foreach (ActionNode actionNode in Children)
            {
                if (actionNode.Value < value)
                    value = actionNode.Value;
            }

            return value;
        }

        /// <summary>
        /// Draw an action
        /// </summary>
        /// <param name="state"></param>
        /// <param name="stocking"></param>
        /// <param name="C"></param>
        /// <returns></returns>
        public Action DrawAction(State state, bool stocking, float C)
        {
            List<Action> allActions = Mdp.GetAllActions(state);
            if (stocking)
            {
                Distribution<Action> d = new Distribution<Action>();
                double minV = GetMinChildrenValue();
                foreach (Action action in allActions)
                {
                    ActionNode actionNode = SearchChildren(action);
                    if (actionNode == null)
                    {
                        return (action);
                    }
                    d.setProba(action, 
                        actionNode.Value + (C * Math.Sqrt(Math.Log(Occurrence) / actionNode.Occurrence)) - minV);
                    
                }

                d.Normalise();
                return (d.Draw());
            }
            
            Random RNG = new Random();
            return (allActions[RNG.Next(allActions.Count)]);
            
        }

        /// <summary>
        /// Update the value of the Node depending of the
        /// </summary>
        public void UpdateValue(float gama)
        {
            Value = 0;
            Distribution<State> prob = Belief.GetNormalisedCopy();
            foreach (State state in Belief.GetKeys())
            {
                Value += Mdp.GetStateValue(state) * prob.getProba(state);
            }

            foreach (ActionNode n in Children)
            {
                Value += n.Value * n.Occurrence * gama / Occurrence;
            }

            Parent?.UpdateValue(gama);
        }


        /// <summary>
        /// Evaluate if the node is terminal, i.e. if all the states in the belief are terminal states
        /// In this case, a node is never terminal (the system can go on forever
        /// </summary>
        /// <returns></returns>
        public bool IsTerminal()
        {
            return false;
        }
        
        
        public override String ToString()
        {
            return ("Belief Node: " + Belief.GetNormalisedCopy());
        }
    }
}