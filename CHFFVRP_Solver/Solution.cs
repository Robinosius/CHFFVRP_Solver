using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHFFVRP_Solver
{
    public class Solution
    {
        public List<Route> routes;
        public double distance; // total distance
        public double emission; // total emissions
        public double costs; // total cost

        public Solution(List<Route> routes)
        {
            this.routes = routes;
            CalculateDistance();
            CalculateCosts();
            CalculateEmission();
        }

        private void CalculateDistance()
        {
            double d = 0;
            foreach(var route in routes)
            {
                d += route.distance;
            }
            this.distance = d;
        }

        private void CalculateEmission()
        {
            double e = 0;
            foreach(var route in routes)
            {
                //e += route.emission;
            }
            this.emission = e;
        }

        private void CalculateCosts()
        {

        }
    }
}
