using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHFFVRP_Solver
{
    class TabuSearch
    {
        private List<Move> tabuList;
        private int minListSize;
        private int maxListSize;

        public TabuSearch(int minTenure, int maxTenure)
        {
            this.tabuList = new();
            this.minListSize = minTenure;
            this.maxListSize = maxTenure;
        }

        public Solution PerformSearch(Solution s)
        {
            // check neighborhood generation for each vehicle route
            for(int i = 0; i < s.Vehicles.Count(); i++)
            { 
                for(int j = i + 1; j < s.Vehicles.Count(); j++)
                {
                    var v1 = s.Vehicles[i];
                    var v2 = s.Vehicles[j];
                    for (int x = 1; x < v1.Route.Count(); x++)//start at 1, depot not included in neighbourhood generation
                    {
                        var n1 = v1.Route.Nodes[x];
                    }
                        
                }
            }
            return s;
        }

        public void PerformInsert(ref Route r1, ref Route r2, int i1, int i2)
        {
            tabuList.Add(new Move(r1, r2, i1, i2));
        }

        public void PerformSwap(Route r1, Route r2, int i1, int i2)
        {
            tabuList.Add(new Move(r1, r2, i1, i2));
        }
    }
    
    class Move
    {
        private Route r1;
        private Route r2;
        private int index1;
        private int index2;

        public Route R1 { get => r1; }
        public Route R2 { get => r2; }
        public int Index1 { get => index1; }
        public int Index2 { get => index2; }

        public Move(Route r1, Route r2, int index1, int index2)
        {
            this.r1 = r1;
            this.r2 = r2;
            this.index1 = index1;
            this.index2 = index2;
        }
    }
}
