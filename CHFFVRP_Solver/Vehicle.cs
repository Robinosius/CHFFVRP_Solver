using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHFFVRP_Solver
{
    public class Vehicle
    {
        public int type; // type = number for simplicity
        public int capacity; // total capacity of this vehicle depending on its type
        public int residualCapacity; // capacity left for this vehicle if it already serves customers;
        private int emissions; // emissions per unit distance of this vehicle depending on its type
        private int variableCosts; // variable cost per unit distance "" "" "" ""
        public Route route; // route this vehicle is currently serving, routes start with tour Depot-Depot for simplicity
    
        public Vehicle(int type, int capacity, int emissions, int variableCosts, Node depot)
        {
            this.type = type;
            this.capacity = capacity;
            this.residualCapacity = capacity;
            this.emissions = emissions;
            this.variableCosts = variableCosts;
            this.route = new();
            route.Add(depot);
        }

        public void AddNode(Node node)
        {
            route.Add(node);
            residualCapacity -= node.demand;
        }

        public void InsertNode(int position, Node node)
        {
            route.InsertNode(position, node);
            residualCapacity -= node.demand;
        }

        public double CalculateVariableCosts()
        {
            return route.GetTotalDistance() * this.variableCosts;
        }

        public double CalculateEmission()
        {
            return this.emissions * route.GetTotalDistance(); ;
        }
    }
}
