using System;

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
            s = problem.StartTabuSearch(s);
            watch.Stop();
            Console.WriteLine($"Result: {s}");
            Console.WriteLine($"Solving time: {watch.ElapsedMilliseconds}");
            Console.ReadLine();
        }
    }
}
