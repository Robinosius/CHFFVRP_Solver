using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/**
########## VRP CONSTRAINTS ##########
- no subtours
- visit every customer exactly once while satisfying demand
- do not use more vehicles of (of a certain type) than available
- do not exceed vehicle capacity

########## NEEDED ##########
- C# object representation for different sets and parameters
- C# objects for Tours and Solutions (set of Tours)
- Calculation method for the objective function(calculate cost of entire solution)
- calculation method for PC = difference between the carbon consumption

 and the upper limit for carbon emissions
- C# objects for Tabu Search algorithm(Tabu List, move(and its benefit), etc)
- 3 opt algo in Python

########## Get Initial solution ###########
1) Start with all nodes in set of unnasigned nodes
2) Select node with highest demand
3) Assign node to the vehicle with highest capacity, if not applicable select next highest
4) Stop if all demands are met
5) Perform 3 opt on this solution

At each assignment:
-every node is only being assigned once and will thus never be visited twice
- vehicle capacity is not being exceeded
- vehicle limits will not be exceeded because assignments are only carried out to
  vehicles with residual capacity
- ensure that each route of assigned vehicles starts and ends at the depot
  --> add new nodes as second to last element of respective list
  --> initialize route for each vehicle with depot at start and end
- because all nodes are being assigned, all demands are being met

################## OPTIMIZE #####################
- init empty tabu list with specific size
- evaluate by neighborhood generation method (insertion, swap, hybrid) and choose best
  --> exchange between different routes
- possibility to accept tabu move by aspiration criteria
- perform 3 opt on each of the CHANGED ROUTES, meaning only the ones affected by
  the neighborhood generation methods
- iterate over all capacitative possible non-tabu 3-opt moves and calculate their benefit
- terminate optimization if no improvement has been made after Lts consecutive iterations
**/

namespace CHFFVRP_Solver
{
    public class Problem
    {
        //parameters
        List<int> q_j; //demand at node
        int[,] coordinates; //coordinates of nodes
        double[,] d; //distance btw nodes
        List<int> v; //variable operation costs of vehicles
        List<int> Q; //capacity of type k vehicles
        List<int> e; //emission of vehicles per distance
        int ae; //upper limit for carb emissions
        int c; //net benefit of carbon trading

        List<int> typeCount; // number of type k vehicles available
        List<Vehicle> vehicles; // fleet of heterogeneous vehicles available for the vrp

        List<Node> nodes;

        TwoOpt twoOpt;
        ThreeOpt threeOpt;

        //variables
        bool[,,] x; // = 1 if a type k vehicle travels from node i to j, 0 otherwise
        int[,] load; //from i to j
        int pc; //difference btw carb consumption and upper emissions limit

        #region CTOR
        public Problem()
        {
            string filepath = "beispiel_kwon.vrp";// input file
            List<string> input = File.ReadAllLines(filepath).ToList();
            var tmp = input.GetRange(24, 16);
            q_j = tmp.Select(var => Int32.Parse(var.Split(" ")[1])).ToList();
            tmp = input.GetRange(7, 16);

            coordinates = new int[16,2];
            nodes = new();
            var index = 0;
            foreach(var line in tmp)
            {
                coordinates[index,0] = Int32.Parse(line.Split(" ")[1]); // x
                coordinates[index, 1] = Int32.Parse(line.Split(" ")[2]); // y
                nodes.Add(new Node(index, coordinates[index, 0], coordinates[index, 1], q_j[index]));
                index++;
            }
            d = new double[16, 16];
            for (int i = 0; i < 16; i++)
            {
                for(int j = 0; j < 16; j++)
                {
                    d[i, j] = Math.Sqrt(Math.Pow(coordinates[i,0] - coordinates[j,0],2) + Math.Pow(coordinates[i, 1] - coordinates[j, 1], 2));
                    d[j, i] = d[i, j];
                }
            }

            tmp = input.GetRange(54,3);
            v = tmp.Select(var => Int32.Parse(var.Split(" ")[1])).ToList();
            tmp = input.GetRange(50,3);
            Q = tmp.Select(var => Int32.Parse(var.Split(" ")[1])).ToList();
            tmp = input.GetRange(58, 3);
            e = tmp.Select(var => Int32.Parse(var.Split(" ")[1])).ToList();
            ae = Int32.Parse(input[62]);
            c = Int32.Parse(input[64]);

            // create vehicle fleet
            tmp = input.GetRange(46, 3);
            typeCount = tmp.Select(var => Int32.Parse(var.Split(" ")[1])).ToList();
            CreateVehicles();

            x = new bool[16, 16, 3];
            load = new int[16,16];
            pc = 0;

            twoOpt = new(d);
            threeOpt = new(d);
        }
        #endregion

        public void CreateVehicles()
        {
            this.vehicles = new();
            for(int i = 0; i < typeCount.Count(); i++)
            {
                for(int j = 0; j < typeCount[i]; j++)
                {
                    vehicles.Add(new Vehicle(i, Q[i], e[i], v[i], nodes[0]));
                }
            }
        }

        public Solution GetInitialSolution()
        {
            List<Node> sorted = nodes.OrderBy(var => var.demand).ToList(); //customers sort by demand
            vehicles = vehicles.OrderBy(var => var.capacity).ToList();

            while(sorted.Count() > 1) //last remaining node is the depot
            {
                var nextCustomer = sorted.Last();
                // infeasible
                if(nextCustomer.demand > vehicles.Last().capacity)
                {
                    Console.WriteLine("Problem infeasible: Max. vehicle capacity insufficient!");
                    return null;
                }
                for(int i = vehicles.Count() - 1; i >= 0; i--)
                {
                    if(vehicles[i].residualCapacity >= nextCustomer.demand)
                    {
                        vehicles[i].AddNode(nextCustomer);
                        sorted.Remove(nextCustomer);
                        break;
                    }
                }
            }
            foreach(var vehicle in vehicles)
            {
                Console.WriteLine($"Vehicle {vehicle}");
                Console.WriteLine(vehicle.route);
                Console.WriteLine(vehicle.route.GetTotalDistance());
                vehicle.route.nodes = this.threeOpt.OptimizeThreeOpt(vehicle.route.nodes);
                Console.WriteLine(vehicle.route);
                Console.WriteLine(vehicle.route.GetTotalDistance());
            }
            Solution s = new(vehicles);
            return s;
        }

        public double GetCost(Solution s)
        {
            return 0;
        }

        public bool CheckSolutionCandidate(Solution s)
        {
            return false;
        }

        public Route OptimizeThreeOpt(Route r)
        {
            return null;
        }
    }
}
