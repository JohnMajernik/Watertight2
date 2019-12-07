using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Numerics;
using System.Reflection;
using Watertight.Filesystem;
using Watertight.Input;
using Watertight.SFML.Components;
using Watertight.SFML.Input;
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
            TickPriority = TickFunction.Last,
        };

        TickFunction RenderStartFunc = new TickFunction
        {
            CanTick = true,
            TickPriority = TickFunction.HighPriority,
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

        private SFMLKeyboardInputSource KeyboardInput
        {
            get;
        } = new SFMLKeyboardInputSource();

        private SFMLMouseInputSource MouseInput
        {
            get;
        } = new SFMLMouseInputSource();

        public override void OnInit()
        {
            FileSystem.ScanAssemblyForResourceFactories(Assembly.GetExecutingAssembly());
            
            Window = new RenderWindow(new VideoMode((uint)ScreenSize.X, (uint)ScreenSize.Y), Name);
            Window.Closed += (s, e) => this.Shutdown();

            Window.Resized += Window_Resized;
            Window.KeyPressed += KeyboardInput.Window_KeyPressed;
            Window.KeyReleased += KeyboardInput.Window_KeyReleased;

            Window.MouseButtonPressed += MouseInput.Window_KeyPressed;
            Window.MouseButtonReleased += MouseInput.Window_KeyReleased;

            Window.SetKeyRepeatEnabled(false);

            InputProcessor.RegisterInputSource(KeyboardInput);
            InputProcessor.RegisterInputSource(MouseInput);

            RenderEndFunc.TickFunc = RenderEnd;
            AddTickfunc(RenderEndFunc);

            RenderStartFunc.TickFunc = RenderStart;
            AddTickfunc(RenderStartFunc);

            base.OnInit();
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
