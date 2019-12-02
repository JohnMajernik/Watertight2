using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Watertight.Filesystem;
using Watertight.Framework;
using Watertight.SFML.Components;

namespace InfiniDungeon2D.TestWorld
{
    public class TileWorld : Actor
    {
        const int Tile_Width = 32;
        const int Tile_Height = 32;

        const int TextureSize = 32;

        static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        int[,] TileMap = new int[Tile_Width, Tile_Height];

        public SFMLVertexMeshComponent VertexMesh
        {
            get;
            private set;
        }

        public ResourcePtr TexturePath
        {
            get;
        } = new ResourcePtr("texture:Textures/TerrainTiles/DirtTile.png");

        public override void CollectResources(IList<ResourcePtr> ResourceCollector)
        {
            ResourceCollector.Add(TexturePath);
            base.CollectResources(ResourceCollector);
        }

        public override void PostConstruct()
        {
            VertexMesh = new SFMLVertexMeshComponent(this);
            VertexMesh.Register();
            base.PostConstruct();
        }

        public override void PostInitializeComponents()
        {
            BuildTileMap();
            base.PostInitializeComponents();
        }

        void BuildTileMap()
        {
            Vector3[] Verticies = new Vector3[Tile_Width * Tile_Height * 4];
            Vector2[] UVs = new Vector2[Verticies.Length];

            for(int x = 0; x < Tile_Width; x++)
            {
                for(int y = 0; y < Tile_Height; y++)
                {
                    int TileIndex = TileMap[x, y];

                    int TexU = TileIndex % (TextureSize / TextureSize);
                    int TexV = TileIndex / (TextureSize / TextureSize);

                    ArraySegment<Vector3> Quad = new ArraySegment<Vector3>(Verticies, (x + y * Tile_Width) * 4, 4);
                    Quad[0] = new Vector3(x * TextureSize, y * TextureSize, 0);
                    Quad[1] = new Vector3((x + 1) * TextureSize, y * TextureSize, 0);
                    Quad[2] = new Vector3((x + 1) * TextureSize, (y + 1) * TextureSize, 0);
                    Quad[3] = new Vector3(x * TextureSize, (y + 1) * TextureSize, 0);

                    ArraySegment<Vector2> UVQuad = new ArraySegment<Vector2>(UVs, (x + y * Tile_Width) * 4, 4);
                    UVQuad[0] = new Vector2(TexU * TextureSize, TexV * TextureSize);
                    UVQuad[1] = new Vector2((TexU + 1) * TextureSize, TexV * TextureSize);
                    UVQuad[2] = new Vector2((TexU + 1) * TextureSize, (TexV + 1) * TextureSize);
                    UVQuad[3] = new Vector2(TexU * TextureSize, (TexV + 1) * TextureSize);


                }
            }

            VertexMesh.SetVertexQuadMesh(Verticies, UVs);
            VertexMesh.Texture = TexturePath;
        }

    }
}
