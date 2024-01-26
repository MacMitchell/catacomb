using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Catacomb;
using Catacomb.Vectors;
using Catacomb.Global;
using Catacomb.Maze;
using Catacomb.Visuals;
using System.Collections;
using Catacomb.CombatStuff;
namespace RoomTest
{
    [TestClass]
    public class RoomTests
    {

        Point origin = new Point(0, 0);
        Point end1 = new Point(50, 50);
        Point end2 = new Point(-50, -50);
        Point negativeStart = new Point(-100, -100);
        Point biggerPoint = new Point(800, 800);
        [TestMethod]
        public void Test_Is_Room_Within_Another_Room()
        {
            Room r1 = new Room();
            Room r2 = new Room();
            Room r3 = new Room();
            Room r4 = new Room();
            
            r1.Create(origin,end1);
            r2.Create(origin, end1);
            r3.Create(negativeStart, end2);
            r4.Create(negativeStart, end1);

            r1.Draw();
            r2.Draw();
            r3.Draw();
            r4.Draw();

            DrawnRoom d1 = r1.RoomDrawn;
            DrawnRoom d2 = r2.RoomDrawn;
            DrawnRoom d3 = r3.RoomDrawn;
            DrawnRoom d4 = r4.RoomDrawn;



            Assert.IsTrue(d1.IsWithin(d2));
            Assert.IsFalse(d1.IsWithin(d3));
            Assert.IsTrue(d1.IsWithin(d4));
        }

        [TestMethod]
        public void Test_Is_Spawn_Point_In_Room()
        {
            Room r1 = new Room();
            r1.Create(origin, biggerPoint);
            r1.Draw();
            CatRectangle rep = (CatRectangle)r1.RoomDrawn.Representive;

            for (int j = 0; j < 10; j++)
            {
                double width = 5;
                double height = 5;
                for (; width < 50; width++, height++)
                {
                    //The spawn point is the top left of the monster
                    Point spawnPoint = r1.RoomDrawn.GenerateSpawnPoint(width, height);
                    Assert.IsTrue(rep.IsPointInRectangle(spawnPoint));

                    Point sizeReference = new Point(width, height);
                    Point end = spawnPoint.AddPoint(sizeReference);

                    CatRectangle monster = new CatRectangle(spawnPoint, end);

                    Assert.IsTrue(r1.RoomDrawn.IsWithin(monster));
                    Assert.IsFalse(r1.RoomDrawn.DoesIntersect(monster));
                }
            }
        }
       
    }
}
