using NLog;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Watertight;
using Watertight.Filesystem;
using Watertight.Framework;

namespace InfiniDungeon2D.TestWorld
{
    public class TestActor : Actor
    {

        static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
               
        public string TestString
        {
            get;
            set;
        }

        public override void BeginPlay()
        {
            Logger.Info("I exist! {0}", TestString);

            base.BeginPlay();
        }

        public override void Tick(float DeltaTime)
        {
          
            base.Tick(DeltaTime);
        }

        public override void CollectResources(IList<ResourcePtr> ResourceCollector)
        {            
            base.CollectResources(ResourceCollector);
        }
    }
}
