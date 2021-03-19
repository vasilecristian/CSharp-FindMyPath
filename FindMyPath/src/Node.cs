using System;
using System.Collections.Generic;

namespace fmp
{
    /// <summary>
    /// This is the helper class used internally to store node's props.
    /// </summary>
    public class Node
    {
        /// <summary>
        /// Is the index of the node.
        /// </summary>
        public ulong Index { get; set; } = 0;

        /// <summary>
        /// Is the parent of this node.
        /// </summary>
        public Node Parent { get; set; } = null;

        /// <summary>
        /// This is the cost to move to this tile. Is called also "G" value. This must be
		/// a sum of parent cost and the cost to move to this node starting from parent.
		/// By default this is -1 which means that this was not calculated yet.
        /// </summary>
        public double Cost { get; set; } = 0;

        /// <summary>
        /// This the distance to target. It is calculated using a heuristic. It is called 
		/// also the "H" value. The function ComputeGoalDistanceEstimate will compute it.
        /// </summary>
        public double CostToTarget { get; set; } = 0;

        /// <summary>
        /// This is "F" a sum of "G" and "H"
        /// </summary>
        public double F { get { return Cost + CostToTarget; } }

        /// <summary>
        /// The list with neighbors.
        /// </summary>
        public List<Node> Neighbors { get; set; } = null;


        public Node(ulong index)
        {
            Console.WriteLine(this.GetType().FullName);

            Index = index;
        }
    }
}
