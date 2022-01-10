using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHFFVRP_Solver
{
    public enum NeighborhoodGenerationMethod
    {
        Insert,
        Swap,
        Hybrid
    }

    
    public enum Mode
    {
        Distance,
        Emission
    }


    class Move
    {
        private Vehicle v1;
        private Vehicle v2;
        private int index1; 
        private int index2;
        private int nodeId1;
        private int nodeId2;

        public Vehicle V1 { get => v1; }
        public Vehicle V2 { get => v2; }
        public int Index1 { get => index1; }
        public int Index2 { get => index2; }
        public int NodeId1 { get => nodeId1; }
        public int NodeId2 { get => nodeId2; }

        public Move(Vehicle v1, Vehicle v2, int index1, int index2, int nodeId1, int nodeId2)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.index1 = index1; // route index of node 
            this.index2 = index2;
            this.nodeId1 = nodeId1; // unique node index
            this.nodeId2 = nodeId2;
        }
    }


    class TabuSearch
    {
        private List<Move> tabuList;
        private int stopAfter; // # of iterations without imoprovement after which to stop
        private int minListSize;
        private int maxListSize;
        private NeighborhoodGenerationMethod ngm; //neighborhood generation method
        private ThreeOpt threeOpt;
        private TwoOpt twoOpt;
        private CheckMove checkMove;
        private PerformMove performMove;

        private delegate double CheckMove(Vehicle v1, Vehicle v2, int i1, int i2);
        private delegate void PerformMove(Vehicle v1, Vehicle v2, int i1, int i2);

        public TabuSearch(int stopAfter, int minTenure, int maxTenure, TwoOpt twoOpt, ThreeOpt threeOpt,  NeighborhoodGenerationMethod ngm, Mode mode = Mode.Emission)
        {
            this.tabuList = new();
            this.stopAfter = stopAfter;
            this.minListSize = minTenure;
            this.maxListSize = maxTenure;
            this.twoOpt = twoOpt;
            this.threeOpt = threeOpt;
            if (ngm == NeighborhoodGenerationMethod.Insert)
            {
                performMove = new(PerformInsert);
            } 
            if (ngm == NeighborhoodGenerationMethod.Swap)
            {
                performMove = new(PerformSwap);
            }
            this.ngm = ngm;
            checkMove = mode == Mode.Distance ? new(CheckMoveDistance) : new(CheckMoveEmissions);
        }

        public Solution PerformTabuSearch(Solution s)
        {
            Solution bestSolution = s.Clone(); // initial solution
            double iterCount = 0; //# of iterations without improvement
            while (iterCount < stopAfter)
            {
                double oldScore = s.TotalCosts;
                s = GetBestNeighborhoodSolution(s);
                double newScore = s.TotalCosts;
                if(newScore < bestSolution.TotalCosts)
                {
                    bestSolution = s.Clone();
                }
                if(newScore < oldScore)
                {
                    iterCount = 0;
                    //Console.WriteLine($"Score: {s.TotalCosts}");
                }
                else
                {
                    iterCount++;
                }
            }
            return bestSolution;
        }

        public Solution GetBestNeighborhoodSolution(Solution s)
        {
            double bestDelta = Int32.MaxValue; // ensure that there is always a move instantiated
            Move highest = null; // best move
            // check neighborhood generation for each vehicle route

            for(int i = 0; i < s.Vehicles.Count(); i++) 
            {
                for (int j = 0; j < s.Vehicles.Count(); j++)
                {
                    var v1 = s.Vehicles[i];

                    var v2 = s.Vehicles[j];
                    if (i != j)
                    {
                        for (int x = 1; x < v1.Route.Count(); x++) //start at 1, depot not included in neighbourhood generation
                        {

                            var n1 = v1.Route.Nodes[x];
                            var maxIndex = ngm == NeighborhoodGenerationMethod.Insert ? 1 : 0;
                            for (int y = 1; y < v2.Route.Count() + maxIndex; y++)
                            {
                                if(x < v1.Route.Count())
                                {
                                    int index2 = y == v2.Route.Count() ? -1 : v2.Route.Nodes[y].Index;
                                    var delta = checkMove(v1, v2, x, y);
                                    if (delta < bestDelta && delta != 0 && !CheckTabu(v1, v2, n1.Index, index2))
                                    {
                                        highest = new Move(s.Vehicles[i], s.Vehicles[j], x, y, n1.Index, index2);
                                        bestDelta = delta;
                                    }
                                }                                
                            }
                        }
                    }                   
                        
                }
            }

            if(highest != null)
            {
                // perform move with best result and add to tabu list
                try{
                    performMove(highest.V1, highest.V2, highest.Index1, highest.Index2);
                }
                catch
                {
                }
                
                tabuList.Add(highest);

                // optimize the two routes with 2-opt/3-opt
                OptimizeRoute(highest.V1);
                OptimizeRoute(highest.V2);

                // remove oldest move from tabu length if it is full
                if (tabuList.Count > maxListSize)
                {
                    tabuList.RemoveAt(0);
                }
                s.CalculateDistance();
                s.CalculateEmission();
                s.CalculateCosts();
            }            
            return s;
        }

        private bool CheckTabu(Vehicle v1, Vehicle v2, int id1, int id2)
        {
            foreach(var move in tabuList)
            {
                if(move.NodeId1 == id1 && move.NodeId2 == id2)
                {
                    return true;
                }
            }
            return false;
        }

        public void OptimizeRoute(Vehicle vehicle)
        {
            vehicle.Route.Nodes = vehicle.Route.Count() >= 6 ?
                            this.threeOpt.OptimizeThreeOpt(vehicle.Route.Nodes)
                            : this.twoOpt.OptimizeTwoOpt(vehicle.Route.Nodes);
        }

        public double CheckMoveDistance(Vehicle v1, Vehicle v2, int i1, int i2)
        {
            double oldDistance = v1.Route.Distance + v2.Route.Distance;
            performMove(v1, v2, i1, i2);

            double newDistance = v1.Route.Distance + v2.Route.Distance;
            if (ngm == NeighborhoodGenerationMethod.Swap)
            {
                // swap back
                performMove(v1, v2, i1, i2);
            }
            else if (newDistance != oldDistance) // revert only if insertion actually happened
            {
                //insert node back to old position on old route
                var tmp = v2.Route.Nodes[i2];
                v2.RemoveNode(tmp);
                v1.InsertNode(i1, tmp);
            }
            return newDistance - oldDistance;
        }

        public double CheckMoveEmissions(Vehicle v1, Vehicle v2, int i1, int i2)
        {
            double oldEmission = v1.CalculateEmission() + v2.CalculateEmission();
            performMove(v1, v2, i1, i2);

            double newEmission = v1.CalculateEmission() + v2.CalculateEmission();
            if (ngm == NeighborhoodGenerationMethod.Swap)
            {
                // swap back
                performMove(v1, v2, i1, i2);
            }
            else if(newEmission != oldEmission) // revert only if insertion actually happened
            {
                //insert node back to old position on old route
                var tmp = v2.Route.Nodes[i2];
                v2.RemoveNode(tmp);
                v1.InsertNode(i1, tmp);
            }
            return newEmission - oldEmission;
        }

        public void PerformInsert(Vehicle v1, Vehicle v2, int i1, int i2)
        {            
            var node = v1.Route.Nodes[i1];
            if (v2.ResidualCapacity >= node.Demand) // sufficient capacity?
            {
                v2.InsertNode(i2, node);
                v1.RemoveNode(node);
            }
        }

        public void PerformSwap(Vehicle v1, Vehicle v2, int i1, int i2)
        {
            var n1 = v1.Route.Nodes[i1];
            var n2 = v2.Route.Nodes[i2];
            // perform only if capacity sufficient
            if(v1.ResidualCapacity + n1.Demand - n2.Demand >= 0)
            {
                if(v2.ResidualCapacity + n2.Demand - n1.Demand >= 0)
                {
                    v1.RemoveNode(n1);
                    v1.InsertNode(i1, n2);
                    v2.RemoveNode(n2);
                    v2.InsertNode(i2, n1);
                }
            }
        }
    }
}
