﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHFFVRP_Solver
{
    public class Node
    {
        public int index; // unique index of node to get distances and other parameters from the problem
        public int x; // x coordinate
        public int y; // y coordinate
        public int demand; // demand of goods at this node

        public Node(int index, int x, int y, int demand)
        {
            this.index = index;
            this.x = x;
            this.y = y;
            this.demand = demand;
        }
    }
}
