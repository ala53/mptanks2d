//#define DIAG_ENABLED
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ZSB.Infrastructure.Apis.Login
{
    public class Diagnostic
    {
        public struct Id
        {
            public string Name;
            public int Line;

            public override int GetHashCode()
            {
                return Name.GetHashCode() ^ Line.GetHashCode();
            }
        }
        public static Dictionary<Id, List<TimeSpan>> times = new Dictionary<Id, List<TimeSpan>>();
        public static void AddTime(string name, int line, TimeSpan time)
        {
#if DEBUG && DIAG_ENABLED
            if (!times.ContainsKey(new Id { Name = name, Line = line }))
                times.Add(new Id { Name = name, Line = line }, new List<TimeSpan>());
            var ind = times[new Id { Name = name, Line = line }]; 
            ind.Add(time);
            Console.WriteLine($"{name}:{line}\t\t\tNow: {time.TotalMilliseconds.ToString("N0")}ms\t\tAvg: {ind.Average(a=>a.TotalMilliseconds).ToString("N0")}ms");
#else
            //Do 'nuffin
#endif
        }
        private static void WriteLine(string ln)
        {
#if DEBUG && DIAG_ENABLED
            Console.WriteLine(ln);
#else
            //Do 'nuffin
#endif
        }
        public static async Task<T> Log<T1, T2, T>(Func<T1, T2, Task<T>> task, T1 t, T2 t2, [CallerMemberName] string name = null, [CallerLineNumber] int line = 0)
        {
            var sw = Stopwatch.StartNew();
            var res = await task(t, t2);
            AddTime(name, line, sw.Elapsed);
            return res;
        }
        public static async Task<T> Log<T1, T2, T3, T>(Func<T1, T2, T3, Task<T>> task, T1 t, T2 t2, T3 t3, [CallerMemberName] string name = null, [CallerLineNumber] int line = 0)
        {
            var sw = Stopwatch.StartNew();
            var res = await task(t, t2, t3);
            AddTime(name, line, sw.Elapsed);
            return res;
        }
        public static async Task<T> Log<T1, T>(Func<T1, Task<T>> task, T1 t, [CallerMemberName] string name = null, [CallerLineNumber] int line = 0)
        {
            var sw = Stopwatch.StartNew();
            var res = await task(t);
            AddTime(name, line, sw.Elapsed);
            return res;
        }
        public static async Task<T> Log<T>(Func<Task<T>> task, [CallerMemberName] string name = null, [CallerLineNumber] int line = 0)
        {
            var sw = Stopwatch.StartNew();
            var res = await task();
            AddTime(name, line, sw.Elapsed);
            return res;
        }





        public static async Task Log<T1, T2>(Func<T1, T2, Task> task, T1 t, T2 t2, [CallerMemberName] string name = null, [CallerLineNumber] int line = 0)
        {
            var sw = Stopwatch.StartNew();
            await task(t, t2);
            AddTime(name, line, sw.Elapsed);
        }
        public static async Task Log<T1, T2, T3>(Func<T1, T2, T3, Task> task, T1 t, T2 t2, T3 t3, [CallerMemberName] string name = null, [CallerLineNumber] int line = 0)
        {
            var sw = Stopwatch.StartNew();
            await task(t, t2, t3);
            AddTime(name, line, sw.Elapsed);
        }
        public static async Task Log<T1>(Func<T1, Task> task, T1 t, [CallerMemberName] string name = null, [CallerLineNumber] int line = 0)
        {
            var sw = Stopwatch.StartNew();
            await task(t);
            AddTime(name, line, sw.Elapsed);
        }
        public static async Task Log(Func<Task> task, [CallerMemberName] string name = null, [CallerLineNumber] int line = 0)
        {
            var sw = Stopwatch.StartNew();
            await task();
            AddTime(name, line, sw.Elapsed);
        }


























        public static T LogSync<T1, T2, T>(Func<T1, T2, T> task, T1 t, T2 t2, [CallerMemberName] string name = null, [CallerLineNumber] int line = 0)
        {
            var sw = Stopwatch.StartNew();
            var res = task(t, t2);
            AddTime(name, line, sw.Elapsed);
            return res;
        }
        public static T LogSync<T1, T2, T3, T>(Func<T1, T2, T3, T> task, T1 t, T2 t2, T3 t3, [CallerMemberName] string name = null, [CallerLineNumber] int line = 0)
        {
            var sw = Stopwatch.StartNew();
            var res = task(t, t2, t3);
            AddTime(name, line, sw.Elapsed);
            return res;
        }
        public static T LogSync<T1, T>(Func<T1, T> task, T1 t, [CallerMemberName] string name = null, [CallerLineNumber] int line = 0)
        {
            var sw = Stopwatch.StartNew();
            var res = task(t);
            AddTime(name, line, sw.Elapsed);
            return res;
        }
        public static T LogSync<T>(Func<T> task, [CallerMemberName] string name = null, [CallerLineNumber] int line = 0)
        {
            var sw = Stopwatch.StartNew();
            var res = task();
            AddTime(name, line, sw.Elapsed);
            return res;
        }





        public static void LogSync<T1, T2>(Action<T1, T2> task, T1 t, T2 t2, [CallerMemberName] string name = null, [CallerLineNumber] int line = 0)
        {
            var sw = Stopwatch.StartNew();
            task(t, t2);
            AddTime(name, line, sw.Elapsed);
        }
        public static void LogSync<T1, T2, T3>(Action<T1, T2, T3> task, T1 t, T2 t2, T3 t3, [CallerMemberName] string name = null, [CallerLineNumber] int line = 0)
        {
            var sw = Stopwatch.StartNew();
            task(t, t2, t3);
            AddTime(name, line, sw.Elapsed);
        }
        public static void LogSync<T1>(Action<T1> task, T1 t, [CallerMemberName] string name = null, [CallerLineNumber] int line = 0)
        {
            var sw = Stopwatch.StartNew();
            task(t);
            AddTime(name, line, sw.Elapsed);
        }
        public static void LogSync(Action task, [CallerMemberName] string name = null, [CallerLineNumber] int line = 0)
        {
            var sw = Stopwatch.StartNew();
            task();
            AddTime(name, line, sw.Elapsed);
        }


    }
}
