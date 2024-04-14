using System;

namespace CustomStateMachine
{
    public class Transition
    {
        public State To { get; private set; }
        public Func<bool> Condition { get; private set; }

        public Transition(State to)
        {
            To = to;
            Condition = to.UniversalTransition;
        }

        public Transition(State to, Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }
    }
}