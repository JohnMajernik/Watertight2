using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Watertight.Filesystem;
using Watertight.Framework;
using Watertight.Framework.Components;
using Watertight.Interfaces;
using Watertight.Rendering;

namespace Watertight.SFML.Components
{
    public class SFMLVertexMeshComponent : SceneComponent, Drawable, IRenderable
    {
        public SFMLVertexMeshComponent(Actor Owner)
           : base(Owner)
        {

        }
        public SFMLVertexMeshComponent()
            : base()
        {
        }

        public ResourcePtr Texture
        {
            get;
            set;
        }

        Vertex[] VertexArray;

        PrimitiveType PrimitiveType = PrimitiveType.Quads;

        public void SetVertexQuadMesh(Vector3[] Verticies, Vector2[] UVs)
        {
            if(Verticies.Length != UVs.Length)
            {
                throw new ArgumentException(nameof(Verticies), "Verts and UVs must be the same length");
            }

            VertexArray = new Vertex[Verticies.Length];
            for(int i =0; i < VertexArray.Length; i++)
            {
                VertexArray[i] = new Vertex();
                VertexArray[i].Position = new Vector2f(Verticies[i].X, Verticies[i].Y);
                VertexArray[i].TexCoords = new Vector2f(UVs[i].X, UVs[i].Y);
                VertexArray[i].Color = Color.White;
            }

        }

        public override void OnTick(float DeltaTime)
        {
            base.OnTick(DeltaTime);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            if(VertexArray == null || VertexArray.Length == 0 || VertexArray.Length % 4 != 0)
            {
                return;
            }

            System.Numerics.Vector3 Location = GetLocation_WorldSpace();
            System.Numerics.Quaternion Rotation = GetRotation_WorldSpace(); //TODO: Get the rotation out of this quat
            System.Numerics.Vector3 Scale = GetScale_WorldSpace();

            Transform tf = Transform.Identity;
            tf.Translate(Location.X, Location.Y);
            //tf.Scale(Scale.X, Scale.Y);

            states.Transform *= tf;

            if(Texture.Loaded)
            {
                states.Texture = Texture.Get<Texture>();
            }

            target.Draw(VertexArray, PrimitiveType, states);

        }

        public void PreRender(Renderer renderer)
        {
            
        }

        public void Render(Renderer renderer)
        {
            SFMLRenderer sfmlRenderer = renderer as SFMLRenderer;

            sfmlRenderer.Window.Draw(this);
        }
    }
}
