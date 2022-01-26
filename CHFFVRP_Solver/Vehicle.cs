using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHFFVRP_Solver
{
    public class Vehicle
    {
        private int type; // type = number for simplicity
        private int capacity; // total capacity of this vehicle depending on its type
        private int residualCapacity; // capacity left for this vehicle if it already serves customers;
        private double emissions; // emissions per unit distance of this vehicle depending on its type
        private double variableCosts; // variable cost per unit distance "" "" "" ""
        private Route route; // route this vehicle is currently serving, routes start with tour Depot-Depot for simplicity
    
        public int Type { get => type; }
        public int Capacity { get => capacity; }
        public int ResidualCapacity { get => residualCapacity; }
        public Route Route { get => route; }

        public Vehicle(int type, int capacity, double emissions, double variableCosts, Node depot)
        {
            this.type = type;
            this.capacity = capacity;
            this.residualCapacity = capacity;
            this.emissions = emissions;
            this.variableCosts = variableCosts;
            this.route = new();
            route.Add(depot);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void AddNode(Node node)
        {
            route.Add(node);
            residualCapacity -= node.Demand;
        }

        public void InsertNode(int position, Node node)
        {
            route.InsertNode(position, node);
            residualCapacity -= node.Demand;
        }

        public void RemoveNode(Node node)
        {
            this.route.RemoveNode(node);
            this.residualCapacity += node.Demand;
        }

        public double CalculateVariableCosts()
        {
            return route.GetTotalDistance() * this.variableCosts;
        }

        public double CalculateEmission()
        {
            var emissions = this.emissions * route.GetTotalDistance();
            return emissions;
        }
    }
}
