using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace fmp
{
    /// <summary>
    /// This is the Main class that implemnts the generic A * (A star) search algorithm.
    /// </summary>
    public class FindMyPath
        : IDisposable
    {
        /// <summary>
        /// Running status property.
        /// </summary>
        public bool IsRunning { get; private set; } = false;

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="navMesh"> Is a pointer to the user's navmesh class.</param>
        public FindMyPath(INavMesh navMesh)
        {
            NavMesh = navMesh;

            // init cancellation token
            cancellationToken = new CancellationTokenSource();

            workerTask = Task.Run(async () =>
            {
                try
                {
                    await WorkerRoutineAsync(cancellationToken.Token);
                }
                finally
                {

                    IsRunning = false;
                }
            });

            IsRunning = true;
        }

        /// <summary>
        /// This will release all internal allocated resources when this object will
        /// not be used anymore. Resources = ongoing process, memory, etc...
        /// </summary>
        public void Dispose()
        {
            Console.WriteLine(this.GetType().FullName + ".Dispose");

            try
            {
                cancellationToken.Cancel();
                workerTask.ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("Scheduler worker task was canceled");
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected error " + e);
            }

            // cleanup cancellation token source
            cancellationToken.Dispose();
            cancellationToken = null;

            IsRunning = false;
        }

        /// <summary>
        /// Add a new request to determine a path.
        /// </summary>
        /// <param name="ticket"></param>
        public void AddTicket(Ticket ticket)
        {
            Console.WriteLine(this.GetType().FullName + ".AddTicket");

            Tickets.Add(ticket);
        }

        private async Task WorkerRoutineAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                Console.WriteLine(this.GetType().FullName + ".WorkerRoutineAsync");

                // enter wait state
                await Task.Delay(2, token).ConfigureAwait(false);
            }
        }


        private INavMesh NavMesh { get; set; } = null;

        private Task workerTask;
        private CancellationTokenSource cancellationToken;

        private List<Ticket> Tickets { get; set; } = new List<Ticket>();

  
    }
}
