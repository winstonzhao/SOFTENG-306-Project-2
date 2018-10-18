using System;
using System.Threading;
using UltimateIsometricToolkit.examples;
using UnityEngine;

namespace Utils
{
    /// <summary>
    /// An action that is can only run once every <see cref="Delay"/> time period
    /// </summary>
    public class DebounceAction : Action
    {
        private TimeSpan Delay;

        private int ActionNo;

        private DateTime LastRunAt = DateTime.MinValue;

        public DebounceAction(TimeSpan delay, Runnable runnable) : base(runnable)
        {
            Delay = delay;
        }

        private ThreadStart Schedule()
        {
            int no;

            lock (this)
            {
                no = ++ActionNo;
            }

            return () =>
            {
                if (ActionNo != no)
                {
                    return;
                }

                var delay = DateTime.Now - LastRunAt;

                // Let delay limit
                if (delay > Delay)
                {
                    delay = Delay;
                }

                // Sleep for the duration to next
                if (delay > TimeSpan.Zero)
                {
                    Thread.Sleep(Delay);
                }

                if (ActionNo == no)
                {
                    LastRunAt = DateTime.Now;
                    base.Run();
                }
            };
        }

        public override void Run()
        {
            var scheduled = Schedule();
            new Thread(scheduled).Start();
        }
    }
}
