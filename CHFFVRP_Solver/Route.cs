using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHFFVRP_Solver
{
    public class Route
    {
        public List<Node> nodes;
        public double distance; // total distance traveled on this route
        
        public Route()
        {
            this.nodes = new();
            this.distance = GetTotalDistance();
        }

        public Route(List<Node> nodes)
        {
            this.nodes = nodes;
            this.distance = GetTotalDistance();
        }

        public void Add(Node node)
        {
            this.nodes.Add(node);
        }

        public void InsertNode(int position, Node node)
        {
            this.nodes.Insert(position, node);
        }

        public void RemoveNode(Node node)
        {
            this.nodes.Remove(node);
        }

        public int Count()
        {
            return this.nodes.Count();
        }

        public double GetTotalDistance()
        {
            this.distance = GetTotalDistance(this.nodes);
            return this.distance;
        }

        public double GetTotalDistance(List<Node> r)
        {
            double distance = 0;
            for(int i = 1; i < r.Count; i++)
            {
                distance += GetDistance(r[i - 1], r[i]);
            }
            // from last node to depot
            distance += GetDistance(r.Last(), r.First());
            return distance;
        }

        public double GetDistance(Node a, Node b)
        {
            return Math.Sqrt(Math.Pow(a.x - b.x, 2) + (Math.Pow(a.y - b.y, 2)));
        }
    }
}
