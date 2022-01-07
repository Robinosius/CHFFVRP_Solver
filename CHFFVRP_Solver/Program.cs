using System;

namespace CHFFVRP_Solver
{
    public class Program
    {
        static void Main(string[] args)
        {
            var problem = new Problem();
            Solution s = problem.GetInitialSolution();
            Console.ReadLine();
        }
    }
}
