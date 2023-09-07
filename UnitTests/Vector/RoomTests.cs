using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Catacomb;
using Catacomb.Vectors;
using Catacomb.Global;
using Catacomb.Maze;
using Catacomb.Visuals;
using System.Collections;

namespace RoomTest
{
    [TestClass]
    public class RoomTests
    {

        Point origin = new Point(0, 0);
        Point end1 = new Point(50, 50);
        Point end2 = new Point(-50, -50);
        Point negativeStart = new Point(-100, -100);
        [TestMethod]
        public void Test_Is_Room_Within_Another_Room()
        {
            Room r1 = new Room();
            Room r2 = new Room();
            Room r3 = new Room();
            Room r4 = new Room();
            
            r1.Draw(origin,end1);
            r2.Draw(origin, end1);
            r3.Draw(negativeStart, end2);
            r4.Draw(negativeStart, end1);

            DrawnRoom d1 = r1.RoomDrawn;
            DrawnRoom d2 = r2.RoomDrawn;
            DrawnRoom d3 = r3.RoomDrawn;
            DrawnRoom d4 = r4.RoomDrawn;



            Assert.IsTrue(d1.IsWithin(d2));
            Assert.IsFalse(d1.IsWithin(d3));
            Assert.IsTrue(d1.IsWithin(d4));
        }
    }
}
