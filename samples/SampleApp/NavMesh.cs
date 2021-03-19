using System;
using System.Collections.Generic;
using fmp;

namespace SampleApp
{
    public class NavMesh : INavMesh
    {
        public NavMesh()
        {
        }

        public int ComputeCostToNeighbor(int neighborIndex, int nodeIndex)
        {
            throw new NotImplementedException();
        }

        public int ComputeCostToTarget(int goalIndex, int nodeIndex)
        {
            throw new NotImplementedException();
        }

        public List<int> GetHeighbors(int nodeIndex)
        {
            throw new NotImplementedException();
        }
    }
}
