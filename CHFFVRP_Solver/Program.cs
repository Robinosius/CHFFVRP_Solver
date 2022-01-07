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

            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);
            Console.ReadLine();
        }
    }
}
