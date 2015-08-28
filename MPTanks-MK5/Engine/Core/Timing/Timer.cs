using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Core.Timing
{

    public class Timer
    {

        public static Timer Default { get { return new Timer { Completed = true }; } }
        public TimeSpan Elapsed { get; set; }
        public Action<Timer> Callback { get; private set; }
        public object UserData { get; private set; }
        public TimeSpan Interval { get; set; }
        public bool Repeat { get; private set; }

        public bool Completed { get; private set; }

        public Factory Creator { get; private set; }

        private Timer()
        {

        }

        public Timer Reset()
        {
            Creator.ResetTimer(this);
            return this;
        }

        public Timer Complete()
        {
            Elapsed += Interval;
            Callback(this);
            return this;
        }

        public class Factory
        {
            public int ActiveTimersCount { get { return timers.Count; } }

            //We're using a hashset so the removals are faster
            private HashSet<Timer> timers = new HashSet<Timer>();
            private bool inUpdateLoop = false;
            private List<Timer> timersToRemove = new List<Timer>();
            private List<Timer> timersToAdd = new List<Timer>();

            public Timer CreateTimer(Action<Timer> callback, TimeSpan timeout, object userdata = null)
            {
                if (timeout.TotalMilliseconds < 0)
                    timeout = TimeSpan.FromMilliseconds(0);

                var timer = new Timer()
                {
                    Callback = callback,
                    Completed = (timeout.TotalMilliseconds == 0),
                    Interval = timeout,
                    Repeat = false,
                    UserData = userdata,
                    Creator = this
                };

                if (timeout.TotalMilliseconds == 0)
                {
                    callback(timer);
                    timer.Completed = true;
                    return timer;
                }
                if (inUpdateLoop)
                    timersToAdd.Add(timer);
                else
                    timers.Add(timer);
                return timer;
            }

            public Timer CreateTimer(TimeSpan timeout, object userdata = null)
            {
                return CreateTimer((t) => { }, timeout, userdata);
            }

            public Timer CreateReccuringTimer(Action<Timer> callback, TimeSpan interval, object userdata = null)
            {
                if (interval.TotalMilliseconds == 0 || interval.TotalMilliseconds < 0)
                    throw new ArgumentException("Cannot have infinitely fast tickrate");

                var timer = CreateTimer(callback, interval, userdata);
                timer.Repeat = true;
                timer.Completed = false;

                return timer;
            }

            public void RemoveTimer(Timer timer, bool throwOnNotFound = false)
            {
                bool found = timers.Contains(timer);
                if (!found && throwOnNotFound)
                    throw new IndexOutOfRangeException("Timer not found in collection");

                if (inUpdateLoop)
                {
                    if (found)
                        timersToRemove.Add(timer);
                }
                else
                {
                    if (found)
                        timers.Remove(timer);
                }
            }

            public void ResetTimer(Timer timer)
            {
                bool found = timers.Contains(timer);

                timer.Elapsed = TimeSpan.FromMilliseconds(0);
                timer.Completed = false;

                if (!found)
                    if (inUpdateLoop)
                        timersToAdd.Add(timer);
                    else timers.Add(timer);
            }

            public void Update(GameTime gameTime)
            {
                inUpdateLoop = true;
                foreach (var timer in timers)
                {
                    timer.Elapsed += gameTime.ElapsedGameTime;
                    if (timer.Elapsed > timer.Interval)
                    {
                        timer.Callback(timer); //Invoke the callback

                        if (!timer.Repeat)
                        {
                            //And mark for deletion if we're not supposed to repeat
                            timersToRemove.Add(timer);
                            timer.Completed = true;
                        }
                        else timer.Elapsed = TimeSpan.Zero;
                    }
                }
                inUpdateLoop = false;

                //Process the queue of timers
                foreach (var timer in timersToAdd)
                    timers.Add(timer);

                foreach (var timer in timersToRemove)
                    timers.Remove(timer);

                timersToRemove.Clear();
                timersToAdd.Clear();

            }
        }
    }
}
