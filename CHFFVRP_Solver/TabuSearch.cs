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

        public Vehicle V1 { get => v1; }
        public Vehicle V2 { get => v2; }
        public int Index1 { get => index1; }
        public int Index2 { get => index2; }

        public Move(Vehicle v1, Vehicle v2, int index1, int index2)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.index1 = index1;
            this.index2 = index2;
        }
    }


    class TabuSearch
    {
        private List<Move> tabuList;
        private int minListSize;
        private int maxListSize;
        private string ngm; //neighborhood generation method
        private CheckMove checkMove;
        private PerformMove performMove;
        
        public TabuSearch(int minTenure, int maxTenure, NeighborhoodGenerationMethod ngm, Mode mode = Mode.Emission)
        {
            this.tabuList = new();
            this.minListSize = minTenure;
            this.maxListSize = maxTenure;
            this.ngm = ngm.ToString();
            if (ngm == NeighborhoodGenerationMethod.Insert)
            {
                performMove = new(PerformInsert);
            } 
            if (ngm == NeighborhoodGenerationMethod.Swap)
            {
                performMove = new(PerformSwap);
            }
            checkMove = mode == Mode.Distance ? new(CheckMoveDistance) : new(CheckMoveEmissions);

        }

        public Solution PerformSearch(Solution s)
        {
            double bestDelta = 0;
            Move highest = null; // best move
            // check neighborhood generation for each vehicle route
            for(int i = 0; i < s.Vehicles.Count(); i++)
            { 
                for(int j = i + 1; j < s.Vehicles.Count(); j++)
                {
                    var v1 = s.Vehicles[i];
                    var v2 = s.Vehicles[j];
                    for (int x = 1; x < v1.Route.Count(); x++) //start at 1, depot not included in neighbourhood generation
                    {
                        var n1 = v1.Route.Nodes[x];
                        for (int y = 1; y < v2.Route.Count(); y++)
                        {
                            var n2 = v2.Route.Nodes[y];
                            var delta = checkMove(v1, v2, i, j);
                            if(delta < bestDelta)
                            {
                                highest = new Move(v1, v2, i, j);
                                bestDelta = delta;
                            }
                        }
                    }
                        
                }
            }

            // perform move with best result and add to tabu list
            performMove(highest.V1, highest.V2, highest.Index1, highest.Index2);
            tabuList.Add(highest);

            return s;
        }

        private delegate double CheckMove(Vehicle v1, Vehicle v2, int i1, int i2);

        private delegate void PerformMove(Vehicle v1, Vehicle v2, int i1, int i2);

        public double CheckMoveDistance(Vehicle v1, Vehicle v2, int i1, int i2)
        {
            double oldDistance = v1.Route.Distance + v2.Route.Distance;
            performMove(v1, v2, i1, i2);
            double newDistance = v1.Route.Distance + v2.Route.Distance;
            performMove(v1, v2, i1, i2);
            return newDistance - oldDistance;
        }

        public double CheckMoveEmissions(Vehicle v1, Vehicle v2, int i1, int i2)
        {
            double oldDistance = v1.CalculateEmission() + v2.CalculateEmission();
            performMove(v1, v2, i1, i2);
            double newDistance = v1.CalculateEmission() + v2.CalculateEmission();
            performMove(v1, v2, i1, i2);
            return newDistance - oldDistance;
        }

        public void PerformInsert(Vehicle v1, Vehicle v2, int i1, int i2)
        {
            var node = v1.Route.Nodes[i1];
            v2.InsertNode(i2, node);
            v1.RemoveNode(node);
        }

        public void PerformSwap(Vehicle v1, Vehicle v2, int i1, int i2)
        {
            var tmp = v1.Route.Nodes[i1];
            v1.Route.Nodes[i1] = v2.Route.Nodes[i2];
            v2.Route.Nodes[i2] = tmp;
        }
    }
}
