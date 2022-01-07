using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHFFVRP_Solver
{
    public class Solution
    {
        private List<Vehicle> vehicles;

        // parameters
        private double distance; // total distance
        private double emission; // total emissions
        private double routingCosts; // costs related to vehicle routing (no carbon trading)
        private double tradingSaldo; // costs/benefit of carbon trading
        private double tradingCosts; // benefit/cost for selling/acquiring carbon emission rights
        private double carbonLimit; // limit for carbon emissions
        private double totalCosts;

        public List<Vehicle> Vehicles { get => vehicles; }
        public double Distance { get => distance; }
        public double Emission { get => emission; }
        public double RoutingCosts { get => routingCosts; }
        public double TradingSaldo { get => tradingSaldo; }
        public double TotalCosts { get => totalCosts; }

        public Solution(List<Vehicle> vehicles, double tradingCosts, double carbonLimit)
        {
            this.vehicles = vehicles;
            this.tradingCosts = tradingCosts;
            this.carbonLimit = carbonLimit;
            CalculateDistance();
            CalculateEmission();
            CalculateCosts();            
        }

        private void CalculateDistance()
        {
            double d = 0;
            var routes = vehicles.Select(var => var.Route).ToList();
            foreach(var route in routes)
            {
                d += route.Distance;
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
            routingCosts = 0;

            foreach(var vehicle in vehicles)
            {
                routingCosts += vehicle.CalculateVariableCosts();
            }

            // if saldo positive, emission rights need to be acquired for a price
            // if saldo negative, emission rights can be solved for a benefit
            this.tradingSaldo = tradingCosts * (this.emission - carbonLimit);
            totalCosts = routingCosts + tradingSaldo;
        }
    }
}
