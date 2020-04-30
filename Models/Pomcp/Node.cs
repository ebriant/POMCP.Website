using System.Collections.Generic;

namespace POMCP.Website.Models.Pomcp
{
    public class Node
    {
        
        /// <summary>
        /// Value of the Node : the value is used to select the best action to do (higher value = better action)
        /// </summary>
        public double Value { get; protected set; }
        
        /// <summary>
        /// Number of time the exploration process went through the node
        /// </summary>
        public int Occurrence { get; private set; }
        
        public Node() {
            Occurrence = 1;
            Value = 0;
        }
        
        //
        /// <summary>
        /// Increment the occurrence of the node by 1 (mark a passage through the node)
        /// </summary>
        public void IncrementOccurrence() {
            Occurrence ++;
        }
    }
}