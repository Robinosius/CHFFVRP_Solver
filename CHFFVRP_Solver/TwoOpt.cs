using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHFFVRP_Solver
{
    class TwoOpt
    {
        private double[,] d; //distances

        public TwoOpt(double[,] distances)
        {
            this.d = distances;
        }

        public List<Node> OptimizeTwoOpt(List<Node> nodes)
        {
            var segments = GetAllSegments(nodes.Count());
            //List<Node> a = nodes.GetRange(i, j - i);
            //List<Node> b = nodes.GetRange(j, k - j);
            foreach (var segment in GetAllSegments(nodes.Count()))
            {
                var newNodes = CheckSegments(nodes, segment[0], segment[1], segment[2]);
                delta += GetTotalDistance(newNodes) - GetTotalDistance(nodes); //negative if improvement
                nodes = newNodes;
            }
            if (delta >= 0) // no longer improvement
            {
                break;
            }
            return nodes;
        }

        public static List<Node> CheckSegments(List<Node> nodes, int i, int j)
        {
            List<Node> a = nodes.GetRange(i, j - 1);
            List<Node> b = nodes.GetRange(j, nodes.Count() - j).Concat(nodes.GetRange(0, i)).ToList();

        }

        public static List<int[]> GetAllSegments(int n)
        {
            // n typically number of nodes on route
            // 3 numbers representing the index of the first node of every segment
            List<int[]> segments = new();
            for (int i = 0; i < n; i++)
            {                
                for (int j = i + 2; i < n; i++)
                {
                    // if the first segment is 0, then the last segment must not start at the last node
                    // otherwise the last segment would consist of only one node
                    if (j == n - 1 && i == 0)
                    {
                        continue;
                    }
                    segments.Add(new int[] { i + 1, j + 1 }); // +1 because depot not included
                }
            }
            return segments;
        }

        public double GetDistance(Node a, Node b)
        {
            return d[a.index, b.index];
        }

        public double GetTotalDistance(List<Node> nodes)
        {
            double d = 0;
            for (int i = 1; i < nodes.Count(); i++)
            {
                d += GetDistance(nodes[i - 1], nodes[i]);
            }
            // from last node back to depot
            d += GetDistance(nodes.Last(), nodes.First());
            return d;
        }
    }
}
