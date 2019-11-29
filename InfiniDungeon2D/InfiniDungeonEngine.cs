using InfiniDungeon2D.TestWorld;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Watertight;
using Watertight.Filesystem;
using Watertight.Framework;
using Watertight.Scripts;
using Watertight.SFML;
using Watertight.Tickable;

namespace InfiniDungeon2D
{
    class InfiniDungeonEngine : SFMLEngine
    {
        public override string Name
        {
            get
            {
                return "InfiniDungeon2D";
            }
        }


        public override string Version
        {
            get
            {
                return "InDev";
            }
        }

        public override IEnumerable<ResourcePtr> PreloadResources
        {
            get
            {
                return new ResourcePtr[]
                {
                    new ResourcePtr("font:Fonts/Work_Sans/WorkSans-Regular.ttf"),
                    TestActor,
                    SecondActor,
                };
            }
        }

        ResourcePtr TestActor = new ResourcePtr("ascript:Scripts/Actors/TestObject.ascript");
        ResourcePtr SecondActor = new ResourcePtr("ascript:Scripts/Actors/TestObjectChild.ascript");
        
        Text Text;

        public override void OnInit()
        {
            base.OnInit();

            ResourcePtr Font = new ResourcePtr("font:Fonts/Work_Sans/WorkSans-Regular.ttf");

            Text = new Text("Hello World", Font.Get<Font>());
            Text.CharacterSize = 24;
            Text.FillColor = Color.Red;

            World newWorld = new World();
            newWorld.SpawnActors = new SubclassOf<Actor>[]
            {
             //   typeof(TestActor),
            };

            LoadWorld(newWorld);

            var actor = newWorld.CreateActor<Actor>(TestActor.Get<ActorScript>());
            actor.Location = new Vector3(100, 100, 0);

            Actor secondACtor = newWorld.CreateActor<Actor>(SecondActor.Get<ActorScript>());
            secondACtor.Location = new Vector3(500, 100, 0);
        }

        public override void Tick(float DeltaTime)
        {
            base.Tick(DeltaTime);

          
            Text.DisplayedString = FPS.ToString("F2") + " Hello World";
            Window.Draw(Text);            
        }

       
    }
}
