using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHFFVRP_Solver
{
    public class Route
    {
        private List<Node> nodes;
        private double distance; // total distance traveled on this route
        
        public List<Node> Nodes { get => nodes; set => nodes = value; }
        public double Distance { get => distance; }

        public Route()
        {
            this.nodes = new();
        }

        public Route(List<Node> nodes)
        {
            this.nodes = nodes;
        }

        public void Add(Node node)
        {
            this.nodes.Add(node);
            GetTotalDistance();
        }

        public void InsertNode(int position, Node node)
        {
            this.nodes.Insert(position, node);
            GetTotalDistance();
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
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + (Math.Pow(a.Y - b.Y, 2)));
        }

        public override string ToString()
        {
            string s = "";
            foreach(var node in this.nodes)
            {
                s += node.Index.ToString() + " ";
            }
            return s;
        }
    }
}
