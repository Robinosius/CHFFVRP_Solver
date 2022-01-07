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

        public void Insert()
        {

        }

        public void Swap()
        {

        }
    }
    
    class Move
    {
        private Route r1;
        private Route r2;
        private int index1;
        private int index2;

        public Move(Route r1, Route r2, int index1, int index2)
        {
            this.r1 = r1;
            this.r2 = r2;
            this.index1 = index1;
            this.index2 = index2;
        }
    }
}
