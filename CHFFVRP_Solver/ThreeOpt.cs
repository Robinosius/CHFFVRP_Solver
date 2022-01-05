using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHFFVRP_Solver
{
    public class ThreeOpt
    {
        public static Route Optimize(Route route)
        {
            // 1) Get all possible segments of the given route/tour
            // 2) Evaluate the 7 possible recombinations of the segments
            // 3) Choose the step with 
            while (true)
            {
                double delta = 0;
                foreach (var segment in GetAllSegments(route.Count() - 2)) // Routes start and end with depot, not include in 3opt!
                {
                    int a = segment[0];
                    int b = segment[1];
                    int c = segment[3];

                    // Reverse segment = traverse segment starting from the last element prior
                    // Combinations: (reversed segments starting with a _):
                    // abc, ab_c, a_bc, a_b_c, _abc. _ab:c, _a_b_c
                }
            }
            return route;
        }

        public static List<int[]> GetAllSegments(int n)
        {
            // n typically number of nodes on route
            // 3 numbers representing the index of the first node of every segment
            List<int[]> segments = new();
            for(int i = 0; i < n; i++)
            {
                for(int j = i + 2; j < n; j++)
                {
                    for (int k = j + 2; k < n; k++)
                    {
                        segments.Add(new int[]{i+1, j+1, k+1});
                    }
                }
            }         
            return segments;
        }
    }
}
