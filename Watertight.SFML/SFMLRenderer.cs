using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Text;
using Watertight.Rendering;

namespace Watertight.SFML
{
    class SFMLRenderer : Renderer
    {
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

        public override void CreateRenderer()
        {
           
        }

        public override void CreateWindow()
        {
            Window = new RenderWindow(new VideoMode((uint)ScreenSize.X, (uint)ScreenSize.Y), Watertight.Engine.Instance.Name);
            Window.Closed += (s, e) => Watertight.Engine.Instance.Shutdown();

           
        }
    }
}
