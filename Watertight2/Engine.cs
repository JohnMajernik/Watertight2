using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using Watertight.Filesystem;
using Watertight.Tickable;

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
                return (1 / (MaxFrameTime * 1000));
            }
            set
            {
                MaxFrameTime = 1 / (value / 1000);
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


        private float MaxFrameTime;

        private List<TickFunction> TickList = new List<TickFunction>();
        private List<TickFunction> RemoveTickList = new List<TickFunction>();
        private Stopwatch StopWatch = new Stopwatch();

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
                TickGroup = TickGroup.HighPriority,
                CanTick = true,
            };

            TickList.Add(EngineTick);

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
            StopWatch.Start();
            while (ExecuteTick())
            {

            }
        }

        private bool ExecuteTick()
        {
            long FrameStart = StopWatch.ElapsedTicks;
            TickList.Sort((x, y) => {
                return (int)x.TickGroup - (int)y.TickGroup;
                });
            for(int i = 0; i < TickList.Count; i++)
            {
                TickFunction tf = TickList[i];
                long TickStart = StopWatch.ElapsedTicks;
                long DeltaTick = TickStart - tf.LastTickTime;
                float DeltaTimeMs = (float)DeltaTick / (float)TimeSpan.TicksPerMillisecond;

                if(tf.CanTick)
                {
                    tf.TickFunc?.Invoke(DeltaTimeMs);
                }               

                tf.LastTickTime = TickStart;
            }

            foreach (TickFunction tickfunc in RemoveTickList)
            {
                TickList.Remove(tickfunc);
            }
            RemoveTickList.Clear();

            long FrameDelta = StopWatch.ElapsedTicks - FrameStart;
            float BeforeSleepDeltaTime = (float)FrameDelta / (float)TimeSpan.TicksPerMillisecond;

            if(BeforeSleepDeltaTime < MaxFrameTime)
            {
                Thread.Sleep((int)(MaxFrameTime - BeforeSleepDeltaTime));
            }

            FrameDelta = StopWatch.ElapsedTicks - FrameStart;
            float TotalFrameDeltaTime = (float)FrameDelta / (float)TimeSpan.TicksPerMillisecond;
            FPS = (1 / TotalFrameDeltaTime) * 1000;

            return Running;
        }

        public virtual void Tick(float DeltaTime)
        {

        }

        
        public void LoadWorld(World WorldToLoad)
        {
            if(ActiveWorld != null)
            {
                //Unload this world
                ActiveWorld.UnloadWorld();
            }

            ActiveWorld = WorldToLoad;           
            ActiveWorld.BeginLoadingWorld();
        }              

        public virtual void Shutdown()
        {
            Running = false;
        }

        protected internal void AddTickfunc(TickFunction TickFunc)
        {
            if(!TickList.Contains(TickFunc))
            {
                TickList.Add(TickFunc);
            }
        }

        protected internal void RemoveTickfunc(TickFunction TickFunc)
        {
            TickFunc.CanTick = false;
            RemoveTickList.Add(TickFunc);
            
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
