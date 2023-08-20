using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Catacomb;
using Catacomb.Vectors;
using Catacomb.Global;
using System.Collections;

namespace UnitTests
{

    [TestClass]
    public class PointTests
    {
        [TestMethod]
        public void Get_Min_X_And_Get_Min_Y_And_Max()
        {
            Point p1 = new Point(4, 10);
            Point p2 = new Point(2, 2525);
            
            Assert.AreEqual(2,p1.GetMinX(p2));
            Assert.AreEqual(2, p2.GetMinX(p1));

            Assert.AreEqual(4, p2.GetMaxX(p1));
            Assert.AreEqual(10, p1.GetMinY(p2));
            Assert.AreEqual(2525, p1.GetMaxY(p2));

        }
    }
    [TestClass]
    public class LineTests
    {
        [TestMethod]
        public void Line_Intersect_Another_Line_In_The_Middle()
        {
            CatLine l1 = new CatLine(-1, -3, 2, 9);
            CatLine l2 = new CatLine(-2.5, 2, 0, -3);
            double expectedIntersecpt = -2.0000 / 3.000;
            
            Assert.IsTrue(l1.DoesIntersect(l2));
            Assert.IsTrue(Math.Abs(l1.GetIntersectPoint(l2).GetX() - expectedIntersecpt) < Globals.TOLERANCE);
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
        [TestMethod]
        public void Intersecpt_check_happy()
        {

            Point[] expectedPoints = { new Point(1, 2),new Point(8,9), };
            ArrayList calculatedPoints = new ArrayList(); 
            CatLine l1 = new CatLine(0, 1, 10, 11);
            calculatedPoints.Add(l1.GetPointWithXVal(1));
            calculatedPoints.Add(l1.GetPointWithYVal(9));

            
            for(int i =0; i < expectedPoints.Length; i++)
            {
                Assert.IsTrue(expectedPoints[i].AreEqual((Point)calculatedPoints[i]));
            }

        }
        [TestMethod]
        public void Intersecpt_check_zero_slope_and_infinite_slope()
        {

            Point[] expectedInfinite = { new Point(1, 5), new Point(1, 0),new Point(1,12) };
            Point[] expectedZero = { new Point(10, 2), new Point(25, 2),new Point(0,2) };
            ArrayList calculatedInfinite = new ArrayList();
            CatLine infinite = new CatLine(1,0,1,12);
            CatLine zero = new CatLine(0,2,25,2);


            Assert.IsNull(infinite.GetPointWithXVal(2));
            Assert.IsNull(infinite.GetPointWithXVal(-10));
            Assert.IsNull(infinite.GetPointWithYVal(25));
            calculatedInfinite.Add(infinite.GetPointWithYVal(5));
            calculatedInfinite.Add(infinite.GetPointWithYVal(0));


            Assert.IsNull(zero.GetPointWithYVal(25));
            Assert.IsNull(zero.GetPointWithYVal(-10));
            Assert.IsNull(zero.GetPointWithXVal(30));

            
            for(int i =0; i < expectedInfinite.Length; i++)
            {
                Assert.IsTrue(expectedInfinite[i].AreEqual(infinite.GetPointWithYVal(expectedInfinite[i].GetY())));
            }
            for (int i = 0; i < expectedZero.Length; i++)
            {
                Assert.IsTrue(expectedZero[i].AreEqual(zero.GetPointWithXVal(expectedZero[i].GetX())));
            }

        }

        [TestMethod]
        public void X_And_Y_Intercept_Do_Not_Intersect()
        {
            CatLine y = new CatLine(5, 5, 5, 10);
            CatLine x = new CatLine(0, 0, 10, 0);

            Assert.IsFalse(y.DoesIntersect(x));
        }
    }
    [TestClass]
    public class RectangleTests
    {
        CatRectangle r = new CatRectangle(1, 1, 10, 10);

        [TestMethod]
        public void point_test_inside()
        {
            Point inside = new Point(5, 5);



            Assert.IsTrue(r.IsPointInRectangle(inside));
        }    
        [TestMethod]
        public void point_test_on_corner()
        {
                Point corner1 = new Point(10, 10);
                Point corner2 = new Point(1, 10);
                

                
                Assert.IsTrue(r.IsPointInRectangle(corner1));
                Assert.IsTrue(r.IsPointInRectangle(corner2));
            }
        [TestMethod]
        public void point_test_on_line()
        {
                Point onLine1 = new Point(1, 5);
                Point onLine2 = new Point(5, 1);


                Assert.IsTrue(r.IsPointInRectangle(onLine1));
                Assert.IsTrue(r.IsPointInRectangle(onLine2));
            }
        [TestMethod]
        public void point_test_outside()
            {
                Point outside1 = new Point(0, 0);
                Point outside2 = new Point(25, 25);

                Assert.IsFalse(r.IsPointInRectangle(outside1));
                Assert.IsFalse(r.IsPointInRectangle(outside2));
            }

        [TestMethod]
        public void line_contained_in_rectangle()
        {
            Vector l1 = new CatLine(5, 6, 7, 8);
            Vector l2 = new CatLine(2, 2, 9, 9);
            Vector l3 = new CatLine(9, 9, 2, 2);



            Assert.IsTrue(r.DoesIntersect(l1));
            Assert.IsTrue(r.DoesIntersect(l2));
            Assert.IsTrue(r.DoesIntersect(l3));
        }

        [TestMethod]
        public void line_one_point_outside_one_point_inside() 
        {
            Vector l1 = new CatLine(0, 0, 5, 5);
            Vector l2 = new CatLine(5, 5, 0, 0);
            Vector l3 = new CatLine(5, 5, 110, 10);

            Assert.IsTrue(r.DoesIntersect(l1));
            Assert.IsTrue(r.DoesIntersect(l2));
            Assert.IsTrue(r.DoesIntersect(l3));
        }
        [TestMethod]
        public void line_one_point_on_edge_of_rectangle()
        {
            Vector l1 = new CatLine(0, 0, 1, 10);
            Vector l2 = new CatLine(10, 10, 50, 50);
            Vector l3 = new CatLine(0, 0, 1, 5);
            Vector l4 = new CatLine(0, 0, 10, 1);

            Assert.IsTrue(r.DoesIntersect(l1));
            Assert.IsTrue(r.DoesIntersect(l2));
            Assert.IsTrue(r.DoesIntersect(l3));
            Assert.IsTrue(r.DoesIntersect(l4));
        }
        

        [TestMethod]
        public void line_intersects_with_no_point_in_rectangle()
        {
            Vector l1 = new CatLine(0, 0, 12, 12);
            Vector l2 = new CatLine(5, 5, 25, 5);
            Vector l3 = new CatLine(0, 20, 20, 0);
            Vector l4 = new CatLine(6, 0, 6, 12);
            Vector l5 = new CatLine(0, 5, 12, 5);
            
            Assert.IsTrue(r.DoesIntersect(l1));
            Assert.IsTrue(r.DoesIntersect(l2));
            Assert.IsTrue(r.DoesIntersect(l3));
            Assert.IsTrue(r.DoesIntersect(l4));
            Assert.IsTrue(r.DoesIntersect(l5));
        }
        [TestMethod]
        public void line_does_not_intersect_rectangle()
        {
            Vector l1 = new CatLine(0, 0, .99, .99);
            Vector l2 = new CatLine(10.1, 10.1, 11, 11);
            Vector l3 = new CatLine(1, 0, -1, 10);

            Assert.IsFalse(r.DoesIntersect(l1));
            Assert.IsFalse(r.DoesIntersect(l2));
            Assert.IsFalse(r.DoesIntersect(l3));
        }
        [TestMethod]
        public void rectangle_intersect_rectangle()
        {
            //one point in rectangle
            CatRectangle r2 = new CatRectangle(0, 0, 5, 5);
            CatRectangle r3 = new CatRectangle(2, 2, 11, 25);
            //whole rectangle in other rectangle
            CatRectangle r4 = new CatRectangle(5, 5, 6, 5);
            //intersects a corner or edge
            CatRectangle r5 = new CatRectangle(0, 0, 1, 1);
            CatRectangle r6 = new CatRectangle(0, 0, 1, 10);
            CatRectangle r7 = new CatRectangle(20, 10, 10, 20);

            CatRectangle r8 = new CatRectangle(5, 5, 12, 6);
            CatRectangle r9 = new CatRectangle(6, 0, 4, 12);


            Assert.IsTrue(r.DoesIntersect(r2));
            Assert.IsTrue(r.DoesIntersect(r3));
            Assert.IsTrue(r.DoesIntersect(r4));
            Assert.IsTrue(r.DoesIntersect(r5));
            Assert.IsTrue(r.DoesIntersect(r6));
            Assert.IsTrue(r.DoesIntersect(r7));
            Assert.IsTrue(r.DoesIntersect(r8));
            Assert.IsTrue(r.DoesIntersect(r9));
        }

        [TestMethod]
        public void rectangle_does_not_intersect_other_rectangle()
        {
            CatRectangle r2 = new CatRectangle(0, 0, 1, 0);
            CatRectangle r3 = new CatRectangle(-10, -10, -1, 0);
            CatRectangle r4 = new CatRectangle(1, -2, 1, 0);
            CatRectangle r5 = new CatRectangle(10.1, 1, 12, 3);
            CatRectangle r6 = new CatRectangle(3, 10.01, 6, 10.0001);
            CatRectangle r7 = new CatRectangle(-5, 6, -2, -6);

            Assert.IsFalse(r.DoesIntersect(r2));
            Assert.IsFalse(r.DoesIntersect(r3));
            Assert.IsFalse(r.DoesIntersect(r4));
            Assert.IsFalse(r.DoesIntersect(r5));
            Assert.IsFalse(r.DoesIntersect(r6));
            Assert.IsFalse(r.DoesIntersect(r7));
        }
    }
}
