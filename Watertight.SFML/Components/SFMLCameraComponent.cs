using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;
using Watertight.Framework;
using Watertight.Framework.Components;

namespace Watertight.SFML.Components
{
    public class SFMLCameraComponent : SceneComponent
    {
        internal View Camera;

        public SFMLCameraComponent(Actor Owner) : base(Owner)
        {
        }

        public SFMLCameraComponent()
            : base()
        {
        }

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
            System.Numerics.Vector3 Location = GetLocation_WorldSpace();
            System.Numerics.Quaternion Rotation = GetRotation_WorldSpace(); //TODO: Get the rotation out of this quat
            System.Numerics.Vector3 Scale = GetScale_WorldSpace();

            Camera = new View(new Vector2f(Location.X, -Location.Y), new Vector2f(SFMLEngine.SFMLInstance.ScreenSize.X, SFMLEngine.SFMLInstance.ScreenSize.Y));
            Camera.Zoom(Scale.X);
            base.OnTick(DeltaTime);
        }

    }
}
