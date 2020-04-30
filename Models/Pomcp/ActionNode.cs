using System.Collections.Generic;

namespace POMCP.Website.Models.Pomcp
{
    public class ActionNode : Node
    {
        public Action Action { get; }
        
        public List<BeliefNode> Children { get; }
        
        /// <summary>
        /// Parent of the Node
        /// </summary>
        public BeliefNode Parent { get; }

        
        //
        /// <summary>
        /// Create node representing an action
        /// </summary>
        /// <param name="action">Action</param>
        /// <param name="n">Parent node</param>
        public ActionNode(Action action, BeliefNode n) : base()
        {
            
            Action = action;
            Parent = n;
            Children = new List<BeliefNode>();
        }
        
        /// <summary>
        /// Add a child node at the end of the children list
        /// </summary>
        /// <param name="childNode">Child node</param>
        public void AddChild(BeliefNode childNode) {
            Children.Add(childNode);
        }
		
        /// <summary>
        /// Search children for a belief node
        /// </summary>
        /// <param name="observation"></param>
        /// <returns></returns>
        public BeliefNode SearchChildren(Observation observation) {
            foreach (BeliefNode beliefNode in Children) {
                if (beliefNode.Observation.Equals(observation))
                    return beliefNode;
            }
            return null;
        }		
		
        /// <summary>
        /// Update the value of the node according to the actual values and occurrence of its children
        /// </summary>
        /// <param name="gama"></param>
        public void UpdateValue(float gama) {
            Value = 0;
            foreach (BeliefNode beliefNode in Children) {
                Value += beliefNode.Value * beliefNode.Occurrence / Occurrence;
            }
            Parent.UpdateValue(gama);
        }
    }
}