using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading;
using Watertight.Interfaces;
using Watertight.Tickable;

namespace Watertight.Rendering
{
    public abstract class Renderer
    {       

        public TickManager TickManager
        {
            get;
            set;
        } = new TickManager();

        internal TickFunction RenderTickFunction
        {
            get;
        } = new TickFunction
        {
            TickPriority = TickFunction.World - 0x007F,
            CanTick = true,
        };

        public event Action OnWindowCreated;
        
        public Renderer()
        {
            RenderTickFunction.TickFunc = RenderScene;
        }

               
        public void CreateWindow()
        {
            CreateWindow_Internal();

            OnWindowCreated?.Invoke();
        }

        protected abstract void CreateWindow_Internal();

        public abstract void CreateRenderer();

        public abstract void AddRenderable(IRenderable Renderable);

        public abstract void RemoveRenderable(IRenderable Renderable);



        public virtual void RenderScene(float deltaTime)
        {
            TickManager.ExecuteSingleTick();
        }

        
    }
}
