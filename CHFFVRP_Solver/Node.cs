using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHFFVRP_Solver
{
    class Node
    {
        public int index; // unique index of node to get distances and other parameters from the problem
        public int x; // x coordinate
        public int y; // y coordinate
        public int demand; // demand of goods at this node
    }
}
