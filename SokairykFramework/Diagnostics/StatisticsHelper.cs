using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SokairykFramework.Diagnostics
{
    public class StatisticsHelper
    {
        public static long GetExecutionTimeElapsedMilliseconds(Action action)
        {
            if (action == null) return -1;

            var counter = new Stopwatch();
            counter.Start();
            action();
            counter.Stop();
            return counter.ElapsedMilliseconds;
        }
    }
}
