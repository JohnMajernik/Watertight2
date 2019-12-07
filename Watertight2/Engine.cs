using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using Watertight.Filesystem;
using Watertight.Tickable;
using Watertight.Scripts;

namespace Watertight
{
    public abstract class Engine
    {
        /// <summary>
        /// Gets the name of the Watertight Game
        /// </summary>
        /// <returns></returns>
        public abstract string Name
        {
            get;
        }

        /// <summary>
        /// Gets the version of the Watertight Engine
        /// </summary>
        /// <returns></returns>
        public abstract string Version
        {
            get;
        }

        public static Engine Instance
        {
            get;
            private set;
        }

        public bool Running
        {
            get;
            set;
        }

        public float FPS
        {
            get;
            set;
        }

        public float MaxFPS
        {
            get
            {
                return (1 / (GameThreadTickManager.MaxFrameTime * 1000));
            }
            set
            {
                GameThreadTickManager.MaxFrameTime = 1 / (value / 1000);
            }

        }

        public World ActiveWorld
        {
            get;
            internal set;
        }


        public virtual IEnumerable<ResourcePtr> PreloadResources
        {
            get;
        }

        TickManager GameThreadTickManager
        {
            get;
            set;
        } = new TickManager();

        static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public Engine()
        {
            Instance = this;
        }

        public void Init()
        {
            ConfigureLogger();

            Logger.Info("Starting Up Watertight Engine!  Game {0}, Version {1}", Name, Version);

            //Init the Filesystem
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(FileSystem).TypeHandle);

            TickFunction EngineTick = new TickFunction
            {
                TickFunc = Tick,
                TickPriority = TickFunction.HighPriority,
                CanTick = true,
            };

            GameThreadTickManager.AddTick(EngineTick);

            MaxFPS = 250;           

            foreach (ResourcePtr resourcePtr in PreloadResources)
            {
               Logger.Info("Preloading Asset: {0}", resourcePtr.ToString());
               resourcePtr.Load();
            }

            OnInit();

            Logger.Info("Finished Loading Engine.  Ready to Begin");
        }      

        public virtual void OnInit()
        {
                    
        }

        public void Run()
        {
            Running = true;
            GameThreadTickManager.Init();
            while (ExecuteTick())
            {

            }
        }

        private bool ExecuteTick()
        {
            float TotalFrameDeltaTime = GameThreadTickManager.ExecuteSingleTick();
            FPS = (1 / TotalFrameDeltaTime) * 1000;

            return Running;
        }

        public virtual void Tick(float DeltaTime)
        {

        }

        
        public World LoadWorld(World WorldToLoad)
        {
            if(ActiveWorld != null)
            {
                //Unload this world
                ActiveWorld.UnloadWorld();
            }

            ActiveWorld = WorldToLoad;           
            ActiveWorld.BeginLoadingWorld();

            return ActiveWorld;
        }  
        
        public World LoadWorld(WorldScript WorldScript)
        {
            return LoadWorld(WorldScript.CreateInstance<World>());
        }

        public virtual void Shutdown()
        {
            Running = false;
        }

        protected internal void AddTickfunc(TickFunction TickFunc)
        {
            GameThreadTickManager.AddTick(TickFunc);
        }

        protected internal void RemoveTickfunc(TickFunction TickFunc)
        {
            GameThreadTickManager.RemoveTick(TickFunc);
        }

        private static void ConfigureLogger()
        {
            var Config = new NLog.Config.LoggingConfiguration();
            var LogConsole = new NLog.Targets.ConsoleTarget("LogConsole");
            var FileTarget = new NLog.Targets.FileTarget("File")
            {
                FileName = "WatertightLog.txt",
                //Layout = "${message}"
            };

            Config.AddRuleForAllLevels(LogConsole);
            Config.AddRuleForAllLevels(FileTarget);

            NLog.LogManager.Configuration = Config;
        }

    }
}
