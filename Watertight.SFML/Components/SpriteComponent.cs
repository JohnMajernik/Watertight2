using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Watertight.Filesystem;
using Watertight.Framework.Components;
using Watertight.Interfaces;

namespace Watertight.SFML.Components
{
    class SpriteComponent : SceneComponent, IHasResources
    {
        public ResourcePtr Texture
        {
            get;
            set;
        } = new ResourcePtr("sprite:Textures/steambulbasaur256.png");

        public override void PostScriptApplied()
        {
            base.PostScriptApplied();
        }

        public void CollectResources(IList<ResourcePtr> ResourceCollector)
        {
            ResourceCollector.Add(Texture);
        }

        public override void OnTick(float DeltaTime)
        {
            System.Numerics.Vector3 Location = GetLocation_WorldSpace();
            System.Numerics.Quaternion Rotation = GetRotation_WorldSpace(); //TODO: Get the rotation out of this quat
            System.Numerics.Vector3 Scale = GetScale_WorldSpace();
                       

            Transform tf = Transform.Identity;            
            tf.Translate(Location.X, Location.Y);
            tf.Scale(Scale.X, Scale.Y);

            RenderStates RS = RenderStates.Default;
            RS.Transform = tf;


            SFMLEngine engine = Engine.Instance as SFMLEngine;
            if (Texture.Loaded)
            {
                //Texture.Get<Sprite>().Position = new Vector2f(Location.X, Location.Y);
                engine?.Window.Draw(Texture.Get<Sprite>(), RS);
            }

            base.OnTick(DeltaTime);
        }

    }
}
