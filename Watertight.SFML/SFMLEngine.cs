using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Reflection;
using Watertight.Filesystem;
using Watertight.Tickable;

namespace Watertight.SFML
{
    public abstract class SFMLEngine : Engine
    {

        TickFunction RenderEndFunc = new TickFunction
        {
            CanTick = true,
            TickGroup = TickGroup.Last,
        };

        TickFunction RenderStartFunc = new TickFunction
        {
            CanTick = true,
            TickGroup = TickGroup.HighPriority,
        };

        public Vector2i ScreenSize
        {
            get;
            protected set;
        } = new Vector2i(800, 600);

        public RenderWindow Window
        {
            get;
            set;
        }

        public override void OnInit()
        {
            FileSystem.ScanAssemblyForResourceFactories(Assembly.GetExecutingAssembly());
            
            Window = new RenderWindow(new VideoMode((uint)ScreenSize.X, (uint)ScreenSize.Y), Name);
            Window.Closed += (s, e) => this.Shutdown();

            RenderEndFunc.TickFunc = RenderEnd;
            AddTickfunc(RenderEndFunc);

            RenderStartFunc.TickFunc = RenderStart;
            AddTickfunc(RenderStartFunc);

            base.OnInit();
        }

        public override void Tick(float DeltaTime)
        {
            Window.DispatchEvents();

            base.Tick(DeltaTime);
        }

        public void RenderStart(float DeltaTime)
        {
            Window.SetTitle(string.Format("{0} - FPS: {1}", Name, FPS.ToString("0")));
            Window.Clear(new Color(0x6699ff));
        }

        public void RenderEnd(float DeltaTime)
        {
            Window.Display();
        }
    }
}
