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

            double width = 5;
            double height = 5;
            for (; width < 50; width++, height++)
            {
                Point spawnPoint = r1.RoomDrawn.GenerateSpawnPoint(width, height);
                Assert.IsTrue(rep.IsPointInRectangle(spawnPoint));

                //the spawn point is the center of the new entity; thus, we will test creating a CatRectangle with giving size and make sure it works
                Point sizeReference = new Point(width / 2.0, height / 2.0);
                Point start = spawnPoint.MinusPoint(sizeReference);
                Point end = spawnPoint.AddPoint(sizeReference);

                CatRectangle monster = new CatRectangle(start, end);

                Assert.IsTrue(r1.RoomDrawn.IsWithin(monster));
                Assert.IsFalse(r1.RoomDrawn.DoesIntersect(monster));
            }
        }
        /*
        [TestMethod]
        public void Test_Command_Iterator()
        {
            //first executed
            
            Command start = new Attack(null); //id =0

            //second excuted
            Command child1 = new Attack(start); //id =1
            //grandChild1x are executed next
            Command grandChild11 = new Attack(child1); //id = 2
            Command grandChild12 = new Attack(child1); //id =3

            //executed after grandChild1x
            Command child2 = new Attack(start); //id =4
            //lsat to execute
            Command grandChild21 = new Attack(child2);//id =5

            Command grandhild13 = new Attack(child1); // id =6
            CommandIterator it = new CommandIterator(start);
            int counter = 0;
            int[] expectedId = { 0, 1, 2, 3, 6, 4, 5 };
            while(it.CurrentCommand != null)
            {
                Assert.AreEqual(expectedId[counter], it.CurrentCommand.id);
                it.Next();
                counter++;
            }
            
           
            


        }
        */
    }
}
