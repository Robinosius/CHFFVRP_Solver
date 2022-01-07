using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHFFVRP_Solver
{
    public class Node
    {
        private int index; // unique index of node to get distances and other parameters from the problem
        private int x; // x coordinate
        private int y; // y coordinate
        private int demand; // demand of goods at this node

        public int Index { get => index; }
        public int X { get => x; }
        public int Y { get => y; }
        public int Demand { get => demand; }

        public Node(int index, int x, int y, int demand)
        {
            this.index = index;
            this.x = x;
            this.y = y;
            this.demand = demand;
        }
    }
}
