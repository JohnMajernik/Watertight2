using System;
using System.Collections.Generic;
using System.Text;
using Watertight.Interfaces;
using Watertight.Scripts;
using Watertight.Tickable;

namespace Watertight.Framework
{
    public class ActorComponent : IHasScript, INamed
    {
        public Actor Owner
        {
            get;
            protected internal set;
        }
        public ObjectScript Script 
        { 
            get; 
            set;
        }

        public bool Registered
        {
            get;
            set;
        }
        public string Name 
        { 
            get; 
            set; 
        }

        internal protected TickFunction PrimaryTick = new TickFunction()
        {
            TickPriority = TickFunction.World,
            CanTick = true,
        };

        internal protected ActorComponent()
        {
            PrimaryTick.TickFunc = OnTick;
        }

        public ActorComponent(Actor Owner)
            : this()
        {
            this.Owner = Owner;
        }

        public virtual void PostScriptApplied()
        {
            
        }

        public void Register()
        {
            Registered = true;
            Owner.RegisterComponent_Internal(this);
            Engine.Instance.AddTickfunc(PrimaryTick);
            OnRegister();
        }

        protected virtual void OnRegister()
        {

        }

        public virtual void OnTick(float DeltaTime)
        {

        }
    }
}
