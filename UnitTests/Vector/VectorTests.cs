using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Catacomb;
using Catacomb.Vectors;
using Catacomb.Global;
namespace UnitTests
{
    [TestClass]
    public class VectorTests
    {
        [TestMethod]
        public void Line_Intersect_Another_Line_In_The_Middle()
        {
            CatLine l1 = new CatLine(-1, -3, 2, 9);
            CatLine l2 = new CatLine(-2.5, 2, 0, -3);
            double expectedIntersecpt = -2.0000 / 3.000;
            
            Assert.IsTrue(l1.DoesIntersect(l2));
            Assert.IsTrue(Math.Abs(l1.GetIntersectPointX(l2) - expectedIntersecpt) < Globals.TOLERANCE);
        }
        [TestMethod]
        public void Line_Would_Intersect_But_They_Are_Too_short()
        {
            //2x+1
            CatLine l1 = new CatLine(1, 4, 2, 9);
            //-2x-3
            CatLine l2 = new CatLine(-2.5, 2, 0, -3);
            Assert.IsFalse(l1.DoesIntersect(l2));
        }
        [TestMethod]
        public void Lines_Without_Intersects()
        {
            //x
            CatLine l1 = new CatLine(-10, -10, 10, 10);
            //2x
            CatLine l2 = new CatLine(-5, -10,5,10);
            Assert.IsTrue(l1.DoesIntersect(l2));
        }

        [TestMethod]
        public void Lines_Intersect_On_Edge()
        {
            //x
            CatLine l1 = new CatLine(0, -0, 10, 10);
            //2x
            CatLine l2 = new CatLine(0, 0, 5, 10);
            Assert.IsTrue(l1.DoesIntersect(l2));
        }

        [TestMethod]
        public void Parallel_Line()
        {
            //x
            CatLine l1 = new CatLine(0, 1, 10, 11);
            //2x
            CatLine l2 = new CatLine(0, 0, 20, 20);

            Assert.IsFalse(l1.DoesIntersect(l2));

        }
    }
}
