using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.CombatStuff
{
    public abstract class Command
    {
        public List<Command> children;
        private Command next;
        private Command prev;
        private Command parent;
        public int id;
        private static int idCounter = 0;
        public Command Next { get => next; set => next = value; }
        public Command Prev { get => prev; set => prev = value; }
        public Command Parent { get => parent; set => parent = value; }

        public Command(Command parentIn)
        {
            Parent = parentIn;
            if (Parent != null)
            {
                Parent.children.Add(this);
            }
            children = new List<Command>();
            id = idCounter++;
        }

        
        public abstract void Execute(CombatEntity castor, CombatEntity target);
        public void AfterExecute(CombatEntity castor, CombatEntity target) { }

        public Command GetChild(int index)
        {
            if(index >= children.Count)
            {
                return null;
            }
            return children[index];
        }
    }

    public class CommandIterator
    {
        Command currentCommand;
        //this keeps track of the current child number
        //EXAMPLE: the start as 2 children, you visit the 0-th child then you push 0 to the stack. 
        //when you finally get back to the start command and go to the next child you would push one to the stack
        //going to the parent pops the stack
        Stack<int> currentChildIndex;

        public Command CurrentCommand { get => currentCommand; set => currentCommand = value; }

        public CommandIterator(Command start)
        {
            CurrentCommand = start;
            currentChildIndex = new Stack<int>();
            currentChildIndex.Push(0);
        }

        private void VisitParent()
        {
            CurrentCommand = CurrentCommand.Parent;
            int childsIndex = currentChildIndex.Pop();
            int oldParentIndex = currentChildIndex.Pop();
            oldParentIndex++;
            currentChildIndex.Push(oldParentIndex);
        }

        private void VisitChild(int index)
        {
            CurrentCommand = CurrentCommand.GetChild(index);
            currentChildIndex.Push(0);
        }


        /**
         * You are at the child element and you want ot visit its sibling 
         */
        private void VisitNextChild()
        {
            VisitParent();
            int index = currentChildIndex.Peek();
            Command child = CurrentCommand.GetChild(index);

            //there are no more siblings, so you actually go the orginal child's parent
            if (child == null)
            {
                //there are no more parents so, go to next
                if (CurrentCommand.Parent == null)
                {
                    CurrentCommand = CurrentCommand.Next;
                    //reset child index
                    currentChildIndex.Pop();
                    currentChildIndex.Push(0);
                }
                else
                {
                    VisitNextChild();
                }
            }
            else{
                VisitChild(index);
            }
        }

        public void Next()
        {
            //order is execute self then children -> if no children then parent -> if no parent then next
            int currentIndex = currentChildIndex.Peek();
            Command child = CurrentCommand.GetChild(currentIndex);

            if(child != null)
            {
                VisitChild(currentIndex);
                return;
            }
            VisitNextChild();
        }
        
    }
}
