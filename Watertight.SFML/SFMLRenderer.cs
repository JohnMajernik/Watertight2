using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Text;
using Watertight.Interfaces;
using Watertight.Rendering;
using Watertight.SFML.Components;
using Watertight.Tickable;

namespace Watertight.SFML
{
    public class SFMLRenderer : Renderer
    {       
        const int DoRenderPriority = TickFunction.World;
        const int SortRenderingPriority = DoRenderPriority - 0x000F;


        public static SFMLRenderer Instance
        {
            get
            {
                return SFMLEngine.SFMLInstance.SFMLRenderer;
            }
        }

        public RenderWindow Window
        {
            get;
            set;
        }

        public Vector2i ScreenSize
        {
            get;
            protected set;
        } = new Vector2i(800, 600);


        TickFunction RenderEndFunc = new TickFunction
        {
            CanTick = true,
            TickPriority = TickFunction.Last,
        };

        TickFunction SortRendererFunc = new TickFunction
        {
            CanTick = true,
            TickPriority = SortRenderingPriority,
        };

        TickFunction DoRenderFunc = new TickFunction
        {
            CanTick = true,
            TickPriority = DoRenderPriority,
        };

        TickFunction RenderStartFunc = new TickFunction
        {
            CanTick = true,
            TickPriority = TickFunction.HighPriority,
        };

        internal SFMLCameraComponent MainCamera
        {
            get
            {
                return _MainCamera;
            }
            set
            {
                _MainCamera = value;
                if (_MainCamera != null && Window != null)
                {
                    Window.SetView(_MainCamera.Camera);
                }
            }
        }
        private SFMLCameraComponent _MainCamera;

        private List<IRenderable> Renderables = new List<IRenderable>();

        public override void CreateRenderer()
        {
            //SFML doesn't have a seperate renderer.  

            RenderStartFunc.TickFunc = RenderStart;
            TickManager.AddTick(RenderStartFunc);

            SortRendererFunc.TickFunc = SortRenderer;
            TickManager.AddTick(SortRendererFunc);

            DoRenderFunc.TickFunc = RenderAll;
            TickManager.AddTick(DoRenderFunc);

            RenderEndFunc.TickFunc = RenderEnd;
            TickManager.AddTick(RenderEndFunc);

            
        }

        protected override void CreateWindow_Internal()
        {
            Window = new RenderWindow(new VideoMode((uint)ScreenSize.X, (uint)ScreenSize.Y), Watertight.Engine.Instance.Name);
            Window.Closed += (s, e) => Watertight.Engine.Instance.Shutdown();

           
        }

        public void RenderStart(float DeltaTime)
        {
            Window.SetTitle(string.Format("{0} - FPS: {1}", Engine.Instance.Name, Engine.Instance.FPS.ToString("0")));
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

        public void SortRenderer(float DeltaTime)
        {
            //TODO: Sort based on Z-Value
            Renderables.Sort((x, y) => (int)x.GetLocation_WorldSpace().Z - (int)y.GetLocation_WorldSpace().Z);
        }

        public void PreRender(float DeltaTime)
        {
            foreach (IRenderable renderable in Renderables)
            {
                renderable?.PreRender(this);
            }
        }

        public void RenderAll(float DeltaTime)
        {
            foreach(IRenderable renderable in Renderables)
            {
                renderable?.Render(this);
            }
        }

        public override void AddRenderable(IRenderable Renderable)
        {
            Renderables.Add(Renderable);
        }

        public override void RemoveRenderable(IRenderable Renderable)
        {
            Renderables.Remove(Renderable);
        }
    }
}
