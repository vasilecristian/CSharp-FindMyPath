using System;
using System.Collections.Generic;
using fmp;

namespace SampleApp
{
    public class NavMesh : INavMesh
    {
        const uint ROW = 0;
        const uint COL = 1;

        const ulong k_w = 8;
        const ulong k_h = 8;
        const ulong k_meshSize = k_w * k_h;


        ulong[] map = new ulong[8 * 8]
        {
            /*     0  1  2  3  4  5  6  7  */
            /*0*/  1, 1, 1, 1, 1, 1, 1, 1, 
            /*1*/  1, 0, 1, 0, 0, 0, 0, 1, 
            /*2*/  1, 0, 1, 0, 1, 1, 0, 1, 
            /*3*/  1, 0, 1, 0, 1, 0, 1, 1, 
            /*4*/  1, 0, 1, 0, 1, 1, 0, 1, 
            /*5*/  1, 0, 1, 0, 1, 0, 1, 1, 
            /*6*/  1, 0, 0, 1, 1, 1, 0, 1, 
            /*7*/  1, 1, 1, 1, 1, 1, 1, 1, 
        };

        public NavMesh()
        {
            Console.WriteLine(this.GetType().FullName);
        }

        public ulong GetIndex(ulong x, ulong y)
        {
            return y * k_w + x;
        }

        public ulong[] GetCoords(ulong index)
        {
            ulong x = index % k_w;
            ulong y = index / k_w;

            return new ulong[2] { x, y };
        }

        public double ComputeCostToNeighbor(ulong neighborIndex, ulong nodeIndex)
        {
            ulong[] neighborCoords = GetCoords(neighborIndex);
            ulong neighborX = neighborCoords[ROW];
            ulong neighborY = neighborCoords[COL];

            ulong[] nodeCoords = GetCoords(nodeIndex);
            ulong nodeX = nodeCoords[ROW];
            ulong nodeY = nodeCoords[COL];

            long x = Math.Abs((long)(neighborX - nodeX));
            long y = Math.Abs((long)(neighborY - nodeY));

            if ((x + y) >= 2)
            {
                return 14;
            }

            if ((x + y) == 1)
            {
                return 10;
            }

            /// in case the x = 0, y = 0 -> neighbor = this
            /// so, the cost is 0
            return 0;
        }

        public double ComputeDistanceToGoal(ulong goalIndex, ulong nodeIndex)
        {
            ulong[] goalCoords = GetCoords(goalIndex);
            ulong goalX = goalCoords[ROW];
            ulong goalY = goalCoords[COL];

            ulong[] nodeCoords = GetCoords(nodeIndex);
            ulong nodeX = nodeCoords[ROW];
            ulong nodeY = nodeCoords[COL];

            long dx = Math.Abs((long)(nodeX - goalX));
            long dy = Math.Abs((long)(nodeY - goalY));
            double dist = Math.Sqrt(dx * dx + dy * dy) * 1000;

            return dist;
        }

        public List<ulong> GetNeighbors(ulong nodeIndex)
        {
            List<ulong> neighbors = new List<ulong>();

            ulong[] nodeCoords = GetCoords(nodeIndex);
            ulong nodeX = nodeCoords[ROW];
            ulong nodeY = nodeCoords[COL];

            for (ulong y = nodeY - 1; y <= nodeY + 1; y++)
            {
                for (ulong x = nodeX - 1; x <= nodeX + 1; x++)
                {
                    /// if is outside the mesh do not add it
                    if ((y < 0) || (y >= k_h))
                        continue;

                    /// if is outside the mesh do not add it
                    if ((x < 0) || (x >= k_w))
                        continue;

                    /// if is the current node do not add it
                    if ((x == nodeX) && (y == nodeY))
                        continue;

                    ulong neighbor = y * k_w + x;

                    /// If the tile have collision on it, do not add it
                    if (map[neighbor] == 1)
                        continue;


                    neighbors.Add(neighbor);
                }
            }

            return neighbors;
        }
    }
}
