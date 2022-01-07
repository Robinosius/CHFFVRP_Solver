using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHFFVRP_Solver
{
    public class Solution
    {
        public List<Vehicle> vehicles;
        public double distance; // total distance
        public double emission; // total emissions
        public double routingCosts; // costs related to vehicle routing (no carbon trading)

        public Solution(List<Vehicle> vehicles)
        {
            this.vehicles = vehicles;
            CalculateDistance();
            CalculateCosts();
            CalculateEmission();
        }

        private void CalculateDistance()
        {
            double d = 0;
            var routes = vehicles.Select(var => var.route).ToList();
            foreach(var route in routes)
            {
                d += route.distance;
            }
            this.distance = d;
        }

        private void CalculateEmission()
        {
            double e = 0;
            foreach (var vehicle in vehicles)
            {
                e += vehicle.CalculateEmission();
            }
            this.emission = e;
        }

        private void CalculateCosts()
        {

        }
    }
}
