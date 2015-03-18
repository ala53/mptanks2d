using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Timing
{

    public class Timer
    {
        public double ElapsedMilliseconds { get; private set; }
        public Action<Timer> Callback { get; private set; }
        public object UserData { get; private set; }
        public double Interval { get; private set; }
        public bool Repeat { get; private set; }

        public bool Completed { get; private set; }

        private Timer()
        {

        }

        public class Factory
        {
            public int ActiveTimersCount { get { return timers.Count; } }

            //We're using a hashset so the removals are faster
            private HashSet<Timer> timers = new HashSet<Timer>();
            private bool inUpdateLoop = false;
            private List<Timer> timersToRemove = new List<Timer>();
            private List<Timer> timersToAdd = new List<Timer>();

            public Timer CreateTimer(Action<Timer> callback, double milliseconds, object userdata = null)
            {
                if (milliseconds < 0)
                    milliseconds = 0;

                var timer = new Timer()
                {
                    Callback = callback,
                    Completed = (milliseconds == 0),
                    Interval = milliseconds,
                    Repeat = false,
                    UserData = userdata
                };

                if (milliseconds == 0)
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

            public Timer CreateReccuringTimer(Action<Timer> callback, double interval, object userdata = null)
            {
                if (interval == 0 || interval < 0)
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

            public void Update(GameTime gameTime)
            {
                inUpdateLoop = true;
                foreach (var timer in timers)
                {
                    timer.ElapsedMilliseconds += gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (timer.ElapsedMilliseconds > timer.Interval)
                    {
                        timer.Callback(timer); //Invoke the callback

                        if (!timer.Repeat)
                        {
                            //And mark for deletion if we're not supposed to repeat
                            timersToRemove.Add(timer);
                            timer.Completed = true;
                        }
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
