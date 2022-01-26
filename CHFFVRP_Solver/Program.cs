using System;
using System.Collections.Generic;

namespace CHFFVRP_Solver
{
    public class Program
    {
        static void Main(string[] args)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            var problem = new Problem();
            Solution s = problem.GetInitialSolution();
            Console.WriteLine($"Initial result: {s}");
            Console.WriteLine("#############################");
            s = problem.StartTabuSearch(s);
            watch.Stop();
            Console.WriteLine($"Result: {s}");
            foreach(var vehicle in s.Vehicles)
            {
                Console.WriteLine($"Type:{vehicle.Type + 1} Route:{vehicle.Route}");
            }
            Console.WriteLine($"Solving time: {watch.ElapsedMilliseconds}");
            Console.ReadLine();
        }
    }
}
