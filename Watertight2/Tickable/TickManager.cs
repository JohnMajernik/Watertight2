using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;
using System.Threading;

namespace Watertight.Tickable
{
    class TickManager
    {


        private List<TickFunction> TickList = new List<TickFunction>();
        private List<TickFunction> RemoveTickList = new List<TickFunction>();
        private Stopwatch StopWatch = new Stopwatch();


        public float MaxFrameTime
        {
            get;
            set;
        }
        
        public void AddTick(TickFunction tickFunction)
        {
            if(TickList.Contains(tickFunction))
            {
                return;
            }

            TickList.Add(tickFunction);
        }

        public void RemoveTick(TickFunction tickFunction)
        {
            if(tickFunction == null)
            {
                return;
            }
            tickFunction.CanTick = false;
            RemoveTickList.Add(tickFunction);
        }




        public void Init()
        {
            StopWatch.Start();
        }

        public float ExecuteSingleTick()
        {
            long FrameStart = StopWatch.ElapsedTicks;
            TickList.Sort((x, y) => {
                return y.TickPriority - x.TickPriority;
            });
            for (int i = 0; i < TickList.Count; i++)
            {
                TickFunction tf = TickList[i];
                long TickStart = StopWatch.ElapsedTicks;
                long DeltaTick = TickStart - tf.LastTickTime;
                float DeltaTimeMs = (float)DeltaTick / (float)TimeSpan.TicksPerMillisecond;

                if (tf.CanTick)
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

            if (BeforeSleepDeltaTime < MaxFrameTime)
            {
                Thread.Sleep((int)(MaxFrameTime - BeforeSleepDeltaTime));
            }

            FrameDelta = StopWatch.ElapsedTicks - FrameStart;
            float TotalFrameDeltaTime = (float)FrameDelta / (float)TimeSpan.TicksPerMillisecond;
            

            return TotalFrameDeltaTime;
        }

    }
}
