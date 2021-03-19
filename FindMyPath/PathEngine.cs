using System;
namespace fmp
{
    /// <summary>
    /// This is the Main class that implemnts the generic A * (A star) search algorithm.
    /// </summary>
    public class PathEngine
        : IDisposable
    {
        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="navMesh"> Is a pointer to the user's navmesh class.</param>
        public PathEngine(INavMesh navMesh)
        {
            NavMesh = navMesh;
        }

        /// <summary>
        /// This will release all internal allocated resources when this object will
        /// not be used anymore. Resources = ongoing process, memory, etc...
        /// </summary>
        public void Dispose()
        {
            throw new NotImplementedException();
        }


        private INavMesh NavMesh { get; set; } = null;
    }
}
