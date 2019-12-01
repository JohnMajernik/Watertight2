using System;
using System.Collections.Generic;
using System.Text;

namespace Watertight.Tickable
{
    public enum TickGroup
    {
        HighPriority,
        InputPoll,
        World,
        Network,
        Last,
    }

    public class TickFunction 
    {
        public delegate void Tick(float DeltaTime);

        public Tick TickFunc
        {
            get;
            set;
        }

        public List<TickFunction> TickPrerequisite
        {
            get;
            private set;
        } 

        public TickGroup TickGroup
        {
            get;
            set;
        }

        public void TickAfter(TickFunction otherTickfunc)
        {
            if(TickPrerequisite == null)
            {
                TickPrerequisite = new List<TickFunction>();
            }
            TickPrerequisite.Add(otherTickfunc);
        }

        internal long LastTickTime
        {
            get;
            set;
        }

        public bool CanTick
        {
            get;
            set;
        }

    }
}
