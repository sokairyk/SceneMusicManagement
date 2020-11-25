using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SokairykFramework.Diagnostics
{
    public static class StatisticsHelper
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

        public static async Task<long> GetExecutionTimeElapsedMillisecondsAsync(Func<Task> asyncFunc)
        {
            if (asyncFunc == null) return -1;

            var counter = new Stopwatch();
            counter.Start();
            var asyncFuncTask = asyncFunc();
            var asyncFuncMetricsTask = asyncFuncTask.ContinueWith(x =>
            {
                counter.Stop();
                return counter.ElapsedMilliseconds;
            });

            await Task.WhenAll(asyncFuncTask, asyncFuncMetricsTask);

            return asyncFuncMetricsTask.Result;
        }
    }
}