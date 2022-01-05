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
        public double emission; // total emissions by vehicle serving this route
        public Vehicle vehicle;
        
        public Route(List<Node> nodes, Vehicle vehicle)
        {
            this.nodes = nodes;
            this.distance = GetTotalDistance();
            this.emission = GetTotalEmission();
        }

        public void InsertNode(Node node, int position)
        {

        }

        public void RemoveNode(Node node, int position)
        {

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
            return distance;
        }

        public double GetDistance(Node a, Node b)
        {
            return Math.Sqrt(Math.Pow(a.x - b.x, 2) + (Math.Pow(a.y - b.y, 2)));
        }

        public double GetTotalEmission()
        {
            return this.distance * this.vehicle.emissions;
        }
    }
}
