using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHFFVRP_Solver
{
    class Vehicle
    {
        public int type; // type = number for simplicity
        public int capacity; // total capacity of this vehicle depending on its type
        public int residualCapacity; // capacity left for this vehicle if it already serves customers;
        public int emissions; // emissions per unit distance of this vehicle depending on its type
        public int variableCosts; // variable cost per unit distance "" "" "" ""
        public Route route; // route this vehicle is currently serving, routes start with tour Depot-Depot for simplicity
    }
}
