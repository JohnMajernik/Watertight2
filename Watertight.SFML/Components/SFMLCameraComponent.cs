using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;
using Watertight.Framework.Components;

namespace Watertight.SFML.Components
{
    public class SFMLCameraComponent : SceneComponent
    {
        internal View Camera;

        public override void PostScriptApplied()
        {
            base.PostScriptApplied();

            Camera = new View();
            if(SFMLEngine.SFMLInstance.MainCamera == null)
            {
                MakeMainCamera();
            }
        }

        public void MakeMainCamera()
        {
            SFMLEngine.SFMLInstance.MainCamera = this;
        }

        public override void OnTick(float DeltaTime)
        {
            Camera = new View(new Vector2f(Location.X, Location.Y), new Vector2f(SFMLEngine.SFMLInstance.ScreenSize.X, SFMLEngine.SFMLInstance.ScreenSize.Y));
            Camera.Zoom(Scale.X);
            base.OnTick(DeltaTime);
        }

    }
}
