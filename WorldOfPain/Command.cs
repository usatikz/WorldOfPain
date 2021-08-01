using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WorldOfPain
{
    interface ICommand
    {
        void Execute(); 
        void Undo();
        void Redo();
    }
    class OneMoveCommand : ICommand
    {
        private Battlefield battlefield;
        private Army firstBeforeMove;
        private Army secondBeforeMove;
        private Army firstAfterMove;
        private Army secondAfterMove;
        public OneMoveCommand(Battlefield field)
        {
            battlefield = field;
        }
       

        public void Undo()
        {
            battlefield.FirstArmy = firstBeforeMove;
            battlefield.SecondArmy = secondBeforeMove;
        }

        public void Redo()
        {
            battlefield.FirstArmy = firstAfterMove;
            battlefield.SecondArmy = secondAfterMove;
        }
        public void Execute()
        {
            firstBeforeMove = battlefield.FirstArmy.GetSnapshot();
            secondBeforeMove = battlefield.SecondArmy.GetSnapshot();
            battlefield.Move();
            firstAfterMove = battlefield.FirstArmy.GetSnapshot();
            secondAfterMove = battlefield.SecondArmy.GetSnapshot();
        }
    }
    class PlayToEndCommand : ICommand
    {
        private Battlefield battlefield;
        private Army firstBeforeMove;
        private Army secondBeforeMove;
        private Army firstAfterMove;
        private Army secondAfterMove;
        public PlayToEndCommand(Battlefield field)
        {
            battlefield = field;
        }
        public void Execute()
        {
            firstBeforeMove = battlefield.FirstArmy.GetSnapshot();
            secondBeforeMove = battlefield.SecondArmy.GetSnapshot();
            battlefield.PlayToTheEnd();
            firstAfterMove = battlefield.FirstArmy.GetSnapshot();
            secondAfterMove = battlefield.SecondArmy.GetSnapshot();
        }

        public void Undo()
        {
            battlefield.FirstArmy = firstBeforeMove;
            battlefield.SecondArmy = secondBeforeMove;
        }

        public void Redo()
        {
            battlefield.FirstArmy = firstAfterMove;
            battlefield.SecondArmy = secondAfterMove;
        }
    }
    class CommandInvoker
    {
        private Stack<ICommand> StackUndo = new Stack<ICommand>();
        private Stack<ICommand> StackRedo = new Stack<ICommand>();
        private Battlefield battlefield;

        public CommandInvoker(Battlefield field)
        {
            this.battlefield = field;
        }

        public void Invoke(ICommand cmd)
        {
            cmd.Execute();
            StackUndo.Push(cmd);
            StackRedo.Clear();
        }

        public bool Undo()
        {
            if (StackUndo.Count != 0)
            {
                ICommand cmd = StackUndo.Pop();
                cmd.Undo();
                StackRedo.Push(cmd);
                return true;
            }
            return false;
        }

        public bool Redo()
        {
            if (StackRedo.Count != 0)
            {
                ICommand cmd = StackRedo.Pop();
                cmd.Redo();
                StackUndo.Push(cmd);// Changed
                return true;
            }
            return false;
        }

        public void Move()
        {
            Invoke(new OneMoveCommand(battlefield));
        }

        public void PlayToTheEnd()
        {
            Invoke(new PlayToEndCommand(battlefield));
        }
    }

}

