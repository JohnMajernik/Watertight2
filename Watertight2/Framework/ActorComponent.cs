using System;
using System.Collections.Generic;
using System.Text;
using Watertight.Interfaces;
using Watertight.Scripts;
using Watertight.Tickable;

namespace Watertight.Framework
{
    public class ActorComponent : IHasScript
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

        internal protected TickFunction PrimaryTick = new TickFunction()
        {
            TickGroup = TickGroup.World,
            CanTick = true,
        };

        public ActorComponent()
        {
            PrimaryTick.TickFunc = OnTick;
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
