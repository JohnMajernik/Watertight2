using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Numerics;
using System.Reflection;
using Watertight.Filesystem;
using Watertight.SFML.Components;
using Watertight.Tickable;

namespace Watertight.SFML
{
    public abstract class SFMLEngine : Engine
    {

        public static SFMLEngine SFMLInstance
        {
            get
            {
                return Instance as SFMLEngine;
            }
        }
        

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

        internal SFMLCameraComponent MainCamera
        {
            get
            {
                return _MainCamera;
            }
            set
            {
                _MainCamera = value;
                if(_MainCamera != null && Window != null)
                {
                    Window.SetView(_MainCamera.Camera);
                }
            }
        }
        private SFMLCameraComponent _MainCamera;

        public override void OnInit()
        {
            FileSystem.ScanAssemblyForResourceFactories(Assembly.GetExecutingAssembly());
            
            Window = new RenderWindow(new VideoMode((uint)ScreenSize.X, (uint)ScreenSize.Y), Name);
            Window.Closed += (s, e) => this.Shutdown();

            Window.Resized += Window_Resized;
            Window.KeyPressed += Window_KeyPressed;
            Window.KeyReleased += Window_KeyReleased;

            RenderEndFunc.TickFunc = RenderEnd;
            AddTickfunc(RenderEndFunc);

            RenderStartFunc.TickFunc = RenderStart;
            AddTickfunc(RenderStartFunc);

            base.OnInit();
        }

        private void Window_KeyReleased(object sender, KeyEventArgs e)
        {
            
        }

        private void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            if(e.Code == Keyboard.Key.A)
            {
                if(MainCamera != null)
                {
                    MainCamera.Owner.Location += new Vector3(-10, 0, 0);
                }
            }
            if (e.Code == Keyboard.Key.D)
            {
                if (MainCamera != null)
                {
                    MainCamera.Owner.Location += new Vector3(10, 0, 0);
                }
            }
            if (e.Code == Keyboard.Key.S)
            {
                if (MainCamera != null)
                {
                    MainCamera.Owner.Location += new Vector3(0, 10, 0);
                }
            }
            if (e.Code == Keyboard.Key.W)
            {
                if (MainCamera != null)
                {
                    MainCamera.Owner.Location += new Vector3(0, -10, 0);
                }
            }
        }

        private void Window_Resized(object sender, SizeEventArgs e)
        {
            if (MainCamera != null)
            {
                ScreenSize = new Vector2i((int)e.Width, (int)e.Height);
            }
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
            if (MainCamera != null)
            {
                Window.SetView(MainCamera.Camera);
            }
        }

        public void RenderEnd(float DeltaTime)
        {
            Window.Display();
        }
    }
}
