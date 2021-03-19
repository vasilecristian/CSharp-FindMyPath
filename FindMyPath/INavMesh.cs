using System;
using System.Collections.Generic;

namespace fmp
{
    /// <summary>
    /// This is the interface that must be implemented by user.
    /// </summary>
    public interface INavMesh
    {
        /// <summary>
        /// This will calculate the movement cost from a node to goal.
        /// Is the Heuristic, that will return the distance from nodeIndex to goalIndex.
        /// </summary>
        /// <param name="goalIndex"> is the index of the node that represent the goal.</param>
        /// <param name="nodeIndex"> is the index of the node that you want to calculate the distance.</param>
        /// <returns> the distance from nodeIndex to goalIndex.</returns>
        int ComputeCostToTarget(int goalIndex, int nodeIndex);

        /// <summary>
        /// This will calculate the movement cost from a node to a neighbor node.
        /// </summary>
        /// <param name="neighborIndex"></param>
        /// <param name="nodeIndex"></param>
        /// <returns></returns>
        int ComputeCostToNeighbor(int neighborIndex, int nodeIndex);

        /// <summary>
        /// Use this to get all the valid heighbod nodes. A valid node
        /// is considered a node that can be used in the path => do not return the
        /// nodes that are invalid (aka are walla, edges, holes, etc...).
        /// </summary>
        /// <param name="nodeIndex"></param>
        /// <returns></returns>
        List<int> GetHeighbors(int nodeIndex);

    }
}
