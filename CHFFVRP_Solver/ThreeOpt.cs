using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHFFVRP_Solver
{
    public class ThreeOpt
    {
        private double[,] d; //distances

        public ThreeOpt(double[,] distances)
        {
            this.d = distances;
        }

        public List<Node> OptimizeThreeOpt(List<Node> nodes)
        {
            while (true)
            {
                double delta = 0;
                foreach(var segment in GetAllSegments(nodes.Count() - 2))
                {
                    var newNodes = CheckSegments(nodes, segment[0], segment[1], segment[2]);
                    delta += GetTotalDistance(newNodes) - GetTotalDistance(nodes); //negative if improvement
                    nodes = newNodes;
                }
                if(delta >= 0) // no longer improvement
                {
                    break;
                }
            }
            return nodes;
        }

        public List<Node> CheckSegments(List<Node> nodes, int i, int j, int k)
        {
            // 1) Get all possible segments of the given route/tour
            // 2) Evaluate the 7 possible recombinations of the segments/reversed segments
            // 3) Choose combination with best result
            // 4) repeat until all possible combinations in a network have been evaluated

            var depot = nodes[0];

            List<Node> a = nodes.GetRange(i, j - i);
            List<Node> b = nodes.GetRange(j, k - j);
            List<Node> c = nodes.GetRange(k, nodes.Count() - 1 - k);            
            List<Node> _a = new(a);
            _a.Reverse();            
            List<Node> _b = new(b);
            _b.Reverse();
            List<Node> _c = new(c);
            _c.Reverse();

            List<List<Node>> candidates = new();
            // generate 7 possible recombinations
            candidates.Add(a.Concat(b).Concat(_c).ToList());
            candidates.Add(a.Concat(_b).Concat(c).ToList());
            candidates.Add(a.Concat(_b).Concat(_c).ToList());
            candidates.Add(_a.Concat(b).Concat(c).ToList());
            candidates.Add(_a.Concat(b).Concat(_c).ToList());
            candidates.Add(_a.Concat(_b).Concat(c).ToList()); 
            candidates.Add(_a.Concat(_b).Concat(_c).ToList());

            foreach(var candidate in candidates)
            {
                candidate.Insert(0, depot);
                candidate.Add(depot);
                if(GetTotalDistance(candidate) < GetTotalDistance(nodes))
                {
                    List<Node> initialRoute = new();
                    initialRoute.Add(nodes[0]);
                    initialRoute.Add(nodes[0]);
                    nodes = candidate;
                }
            }

            // return minimum distance sequence
            return nodes;
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
                        segments.Add(new int[]{i+1, j+1, k+1}); // +1 because depot not included
                    }
                }
            }         
            return segments;
        }

        public double GetDistance(Node a, Node b)
        {
            return Math.Sqrt(Math.Pow(a.x - b.x, 2) + (Math.Pow(a.y - b.y, 2)));
        }

        public double GetTotalDistance(List<Node> nodes)
        {
            double d = 0;
            for(int i = 1; i < nodes.Count(); i++)
            {
                d += GetDistance(nodes[i - 1], nodes[i]);
            }
            return d;
        }
    }
}
