using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Catacomb;
using Catacomb.Vectors;
using Catacomb.Global;
using Catacomb.Maze;
using Catacomb.Visuals;
using System.Collections;
using Catacomb.CombatStuff;
namespace CombatTest
{
    [TestClass]
    public class CombatTest
    {

       
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
            while (it.CurrentCommand != null)
            {
                Assert.AreEqual(expectedId[counter], it.CurrentCommand.id);
                it.Next();
                counter++;
            }





        }
    }
}
