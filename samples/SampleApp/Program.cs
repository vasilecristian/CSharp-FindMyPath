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

            NavMesh navMesh = new NavMesh();

            FindMyPath pathEngine = new FindMyPath(navMesh);

            Ticket ticket = new Ticket(navMesh.GetIndex(1, 1), navMesh.GetIndex(6, 6));

            pathEngine.AddTicket(ticket);

            pathEngine.Dispose();

            while (pathEngine.IsRunning)
            {
                Thread.Sleep(100);
            }
        }
    }
}
