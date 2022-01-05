using Microsoft.VisualStudio.TestTools.UnitTesting;
using CHFFVRP_Solver;

namespace TabuSearchTests
{
    [TestClass]
    public class ThreeOptTests
    {
        [TestMethod]
        public void GetSegmentsTest()
        {
            var segments = ThreeOpt.GetAllSegments(10);
            foreach(var s in segments)
            {
                System.Console.WriteLine(s);
            }
        }
    }
}
