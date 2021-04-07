using System;
using fmp;
using System.Threading;
using System.Threading.Tasks;

namespace SampleApp
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Mutex mutex = new Mutex();

            NavMesh navMesh = new NavMesh();

            FindMyPath pathEngine = new FindMyPath(navMesh);

            Ticket ticket = new Ticket(navMesh.GetIndex(0, 0), navMesh.GetIndex(7, 7));

            CancellationTokenSource cancelToken = new CancellationTokenSource();
            Task<Ticket> task = pathEngine.FindPathAsync(ticket, cancelToken);
            task.ContinueWith(previousTask =>
            {
                mutex.WaitOne();
                Console.WriteLine("Result Steps=" + previousTask.Result.Steps);
                Console.WriteLine("Result State=" + previousTask.Result.State);
                navMesh.PrintSolution(previousTask.Result.Path);
                navMesh.PrintMap(previousTask.Result.Path);
                mutex.ReleaseMutex();
            });

            
            Ticket ticket2 = new Ticket(navMesh.GetIndex(1, 1), navMesh.GetIndex(6, 6));
            CancellationTokenSource cancelToken2 = new CancellationTokenSource();
            Task<Ticket> task2 = pathEngine.FindPathAsync(ticket2, cancelToken2);
            task2.ContinueWith(previousTask =>
            {
                mutex.WaitOne();
                Console.WriteLine("Result2 Steps=" + previousTask.Result.Steps);
                Console.WriteLine("Result2 State=" + previousTask.Result.State);
                navMesh.PrintSolution(previousTask.Result.Path);
                navMesh.PrintMap(previousTask.Result.Path);
                mutex.ReleaseMutex();
            });

            Ticket ticket3 = new Ticket(navMesh.GetIndex(1, 1), navMesh.GetIndex(6, 6));
            CancellationTokenSource cancelToken3 = new CancellationTokenSource();
            Task<Ticket> task3 = pathEngine.FindPathAsync(ticket3, cancelToken3);
            task3.ContinueWith(previousTask =>
            {
                mutex.WaitOne();
                Console.WriteLine("Result3 Steps=" + previousTask.Result.Steps);
                Console.WriteLine("Result3 State=" + previousTask.Result.State);
                navMesh.PrintSolution(previousTask.Result.Path);
                navMesh.PrintMap(previousTask.Result.Path);
                mutex.ReleaseMutex();
            });


            //cancelToken.CancelAfter(TimeSpan.FromMilliseconds(1));

            //Thread.Sleep(1000);
            pathEngine.CalcelAll();

            Thread.Sleep(1000);

            pathEngine.Dispose();
        }


        public static bool TaskIsCompleted(Task task)
        {
            if ((task != null) && (task.IsCompleted == false ||
                                   task.Status == TaskStatus.Running ||
                                   task.Status == TaskStatus.WaitingToRun ||
                                   task.Status == TaskStatus.WaitingForActivation))
            {
                //Console.WriteLine("Task is already running");
                return false;
            }
            else
            {

                //task = Task.Factory.StartNew(() =>
                //{
                //    Logger.Log("Task has been started");
                //    // Do other things here               
                //});
            }

            return true;
        }
    }
}
