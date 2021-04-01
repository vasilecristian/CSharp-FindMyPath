using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using fmp;

namespace UnitTest
{
    [TestClass]
    public class UnitTest_8x8_map
    {
        // A class that contains MSTest unit tests. (Required)
        [TestClass]
        public class UnitTests
        {
            static NavMesh navMesh = null;
            static FindMyPath pathEngine = null;

            [ClassInitialize]
            public static void ClassInitialize(TestContext context)
            {
                // Executes once for the test class. (Optional)
                Console.WriteLine("ClassInitialize");

                navMesh = new NavMesh();

                pathEngine = new FindMyPath(navMesh);
            }
            
            [ClassCleanup]
            public static void ClassCleanup()
            {
                // Runs once after all tests in this class are executed. (Optional)
                // Not guaranteed that it executes instantly after all tests from the class.
                Console.WriteLine("ClassCleanup");

                pathEngine.Dispose();
                pathEngine = null;

                navMesh = null;
            }


            [TestInitialize]
            public void TestInitialize()
            {
                // Runs before each test. (Optional)
                Console.WriteLine("TestInitialize");
            }

            [TestCleanup]
            public void TestCleanup()
            {
                // Runs after each test. (Optional)
                Console.WriteLine("TestCleanup");
            }

            [DataTestMethod]
            [DynamicData(nameof(GetInputsAndExpectedResults), DynamicDataSourceType.Method)]
            public async Task TestMethod(ulong startX, ulong startY, ulong endX, ulong endY, uint steps, Ticket.STATE state, List<ulong> path)
            {
                Console.WriteLine("TestMethod");

                Ticket ticketInitial = new Ticket(navMesh.GetIndex(startX, startY), navMesh.GetIndex(endX, endY));

                CancellationTokenSource cancelToken = new CancellationTokenSource();
                Ticket ticketFinal = await pathEngine.FindPathAsync(ticketInitial, cancelToken);
                

                Assert.AreEqual(ticketInitial, ticketFinal);
                Assert.AreEqual(state, ticketFinal.State);
                Assert.AreEqual(steps, ticketFinal.Steps);
                Assert.AreEqual(path.Count, ticketFinal.Path.Count);
                CollectionAssert.AreEqual(path, ticketFinal.Path);
            }

            public static IEnumerable<object[]> GetInputsAndExpectedResults()
            {
                yield return new object[] {
                    /*Input Start X*/(ulong)1, /*Start Y*/(ulong)1, /*End X*/(ulong)6, /*End Y*/(ulong)6,
                    /*Expected Steps*/ (uint)20,
                    /*Expected State*/ Ticket.STATE.COMPLETED,
                    /*Expected Path*/ new List<ulong>(){54, 45, 38, 29, 22, 13, 12, 19, 27, 35, 43, 50, 41, 33, 25, 17, 9 }
                };
                yield return new object[] {
                    /*Input Start X*/(ulong)0, /*Start Y*/(ulong)0, /*End X*/(ulong)7, /*End Y*/(ulong)7,
                    /*Expected Steps*/ (uint)24,
                    /*Expected State*/ Ticket.STATE.COMPLETED,
                    /*Expected Path*/ new List<ulong>(){63, 55, 47, 39, 31, 22, 13, 12, 19, 27, 35, 43, 50, 41, 33, 25, 17, 9, 0 }
                };
                yield return new object[] {
                    /*Input Start X*/(ulong)0, /*Start Y*/(ulong)0, /*End X*/(ulong)7, /*End Y*/(ulong)0,
                    /*Expected Steps*/ (uint)8,
                    /*Expected State*/ Ticket.STATE.COMPLETED,
                    /*Expected Path*/ new List<ulong>(){ 7, 6, 5, 4, 3, 2, 1, 0 }
                };
                yield return new object[] {
                    /*Input Start X*/(ulong)0, /*Start Y*/(ulong)0, /*End X*/(ulong)0, /*End Y*/(ulong)7,
                    /*Expected Steps*/ (uint)8,
                    /*Expected State*/ Ticket.STATE.COMPLETED,
                    /*Expected Path*/ new List<ulong>(){ 56, 48, 41, 33, 25, 17, 9, 0 }
                };
                yield return new object[] {
                    /*Input Start X*/(ulong)0, /*Start Y*/(ulong)0, /*End X*/(ulong)5, /*End Y*/(ulong)3,
                    /*Expected Steps*/ (uint)12,
                    /*Expected State*/ Ticket.STATE.COMPLETED,
                    /*Expected Path*/ new List<ulong>(){ 29, 22, 13, 12, 11, 2, 9, 0 }
                };
                yield return new object[] {
                    /*Input Start X*/(ulong)0, /*Start Y*/(ulong)0, /*End X*/(ulong)1, /*End Y*/(ulong)1,
                    /*Expected Steps*/ (uint)1,
                    /*Expected State*/ Ticket.STATE.COMPLETED,
                    /*Expected Path*/ new List<ulong>(){ 9, 0 }
                };
                yield return new object[] {
                    /*Input Start X*/(ulong)0, /*Start Y*/(ulong)0, /*End X*/(ulong)0, /*End Y*/(ulong)0,
                    /*Expected Steps*/ (uint)1,
                    /*Expected State*/ Ticket.STATE.COMPLETED,
                    /*Expected Path*/ new List<ulong>(){ 0 }
                };

                yield return new object[] {
                    /*Input Start X*/(ulong)0, /*Start Y*/(ulong)0, /*End X*/(ulong)0, /*End Y*/(ulong)1,
                    /*Expected Steps*/ (uint)1,
                    /*Expected State*/ Ticket.STATE.INVALID_GOAL,
                    /*Expected Path*/ new List<ulong>(){ }
                };

            }
        }
    }



    public class NavMesh : INavMesh
    {
        const uint ROW = 0;
        const uint COL = 1;

        const ulong k_w = 8;
        const ulong k_h = 8;
        ulong[] m_map = new ulong[8 * 8]
        {
            /*     0  1  2  3  4  5  6  7  */
            /*0*/  0, 0, 0, 0, 0, 0, 0, 0, 
            /*1*/  1, 0, 1, 0, 0, 0, 0, 0, 
            /*2*/  1, 0, 1, 0, 1, 1, 0, 0, 
            /*3*/  1, 0, 1, 0, 1, 0, 1, 0, 
            /*4*/  1, 0, 1, 0, 1, 1, 0, 0, 
            /*5*/  0, 0, 1, 0, 1, 0, 1, 0, 
            /*6*/  0, 0, 0, 1, 1, 1, 0, 0, 
            /*7*/  0, 0, 0, 1, 0, 0, 0, 0,
        };

        public NavMesh()
        {
            //Console.WriteLine(this.GetType().FullName);
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

            long dx = Math.Abs((long)(neighborX - nodeX));
            long dy = Math.Abs((long)(neighborY - nodeY));

            return Math.Sqrt(dx * dx + dy * dy);
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
            double dist = Math.Sqrt(dx * dx + dy * dy);

            return dist;
        }

        public List<ulong> GetNeighbors(ulong nodeIndex)
        {
            List<ulong> neighbors = new List<ulong>();

            ulong[] nodeCoords = GetCoords(nodeIndex);
            ulong nodeX = nodeCoords[ROW];
            ulong nodeY = nodeCoords[COL];

            for (long y = (long)nodeY - 1; y <= (long)nodeY + 1; y++)
            {
                for (long x = (long)nodeX - 1; x <= (long)nodeX + 1; x++)
                {
                    
                    /// if is outside the mesh do not add it
                    if ((y < 0) || (y >= (long)k_h))
                        continue;

                    /// if is outside the mesh do not add it
                    if ((x < 0) || (x >= (long)k_w))
                        continue;

                    /// if is the current node do not add it
                    if ((x == (long)nodeX) && (y == (long)nodeY))
                        continue;
                    
                    ulong neighborIndex = GetIndex((ulong)x, (ulong)y);//y * (long)k_w + x;

                    /// If the tile have collision on it, do not add it
                    if (GetNodeType(neighborIndex) != NodeType.AVAILABLE)
                        continue;


                    neighbors.Add((ulong)neighborIndex);
                }
            }

            return neighbors;
        }

        public NodeType GetNodeType(ulong nodeIndex)
        {
            if (nodeIndex >= (k_w * k_h))
            {
                return NodeType.INVALID;
            }
            else if (m_map[nodeIndex] == 1)
            {
                return NodeType.INVALID;
            }

            return NodeType.AVAILABLE;
        }
    }
}
