using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Numerics;
using System.Reflection;
using Watertight.Filesystem;
using Watertight.Input;
using Watertight.Rendering;
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
        

        

        public Vector2i ScreenSize
        {
            get;
            protected set;
        } = new Vector2i(800, 600);

        public RenderWindow Window
        {
            get
            {
                return SFMLRenderer.Window;
            }
        }

        protected internal SFMLRenderer SFMLRenderer
        { 
            get
            {
                return Renderer as SFMLRenderer;
            }
        }
        

        public override SubclassOf<Renderer> RendererType => typeof(SFMLRenderer);
               

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

            base.OnInit();

            Renderer.OnWindowCreated += () =>
            {
                Window.Resized += Window_Resized;
                Window.KeyPressed += KeyboardInput.Window_KeyPressed;
                Window.KeyReleased += KeyboardInput.Window_KeyReleased;

                Window.MouseButtonPressed += MouseInput.Window_KeyPressed;
                Window.MouseButtonReleased += MouseInput.Window_KeyReleased;

                Window.SetKeyRepeatEnabled(false);                
            };

            InputProcessor.RegisterInputSource(KeyboardInput);
            InputProcessor.RegisterInputSource(MouseInput);




        }
              

        private void Window_Resized(object sender, SizeEventArgs e)
        {
            if (SFMLRenderer.MainCamera != null)
            {
                ScreenSize = new Vector2i((int)e.Width, (int)e.Height);
            }
        }

        public override void Tick(float DeltaTime)
        {
            Window.DispatchEvents();

            base.Tick(DeltaTime);
        }


    }
}
