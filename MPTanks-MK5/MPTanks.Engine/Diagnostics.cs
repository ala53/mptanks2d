//Whether to track every measurement that ever occurred
//Or just the last 100
//#define TRACK_ALL_MEASUREMENTS

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine
{
    public class Diagnostics
    {
        //A warning to future code readers:
        //This code was made by someone who thought copy pasting this was a good idea
        // (and is friends with the guy who said that)
        //firestar23373: did you comment war and peace into it?

        private Dictionary<string, Measurement> _rootNodes =
            new Dictionary<string, Measurement>();
        public IReadOnlyDictionary<string, Measurement> Roots { get { return _rootNodes; } }

        private Dictionary<string, Stopwatch> _monitors = new Dictionary<string, Stopwatch>();

        public bool IsMeasuring(string name, params string[] ownerHierarchy)
        {
            if (ownerHierarchy.Length == 0)
            {
                if (_monitors.ContainsKey(name))
                {
                    return _monitors[name].IsRunning;
                }
            }

            var ownerName = String.Join("_", ownerHierarchy.Reverse());
            if (_monitors.ContainsKey(ownerName + "_" + name))
                return _monitors[ownerName + "_" + name].IsRunning;

            return false;

        }
        public Measurement BeginMeasurement(string name, params string[] ownerHierarchy)
        {
            Measurement owner = null;
            //Build the tree
            for (var i = ownerHierarchy.Length - 1; i >= 0; i--)
            {
                owner = CreateOrGetSubNode(ownerHierarchy[i], owner);
            }

            //Get my node
            var m = CreateOrGetSubNode(name, owner);
            BeginMonitor(m);
            return m;
        }

        private Measurement CreateOrGetSubNode(string name, Measurement parent = null)
        {
            if (parent == null)
            {
                if (_rootNodes.ContainsKey(name))
                    return _rootNodes[name];
                else
                {
                    var m = new Measurement(name);
                    _rootNodes.Add(name, m);
                    return m;
                }
            }
            else
            {
                if (parent.Submeasurements.ContainsKey(name))
                    return parent.Submeasurements[name];
                else
                    return new Measurement(name, parent);
            }
        }

        private void BeginMonitor(Measurement measurement)
        {
            if (!_monitors.ContainsKey(measurement.GetUID()))
            {
                _monitors.Add(measurement.GetUID(), Stopwatch.StartNew());
            }
            else
            {
                if (_monitors[measurement.GetUID()].IsRunning)
                    throw new Exception("Already monitoring");

                _monitors[measurement.GetUID()].Restart();
            }
        }

        #region Function monitor helpers
        public TResult MonitorCall<TResult>(Func<TResult> call, string name, params string[] ownerHierarchy)
        {
            var measurement = BeginMeasurement(name, ownerHierarchy);
            var result = call();
            EndMeasurement(measurement);
            return result;
        }
        public TResult MonitorCall<TArg1, TResult>(Func<TArg1, TResult> call, TArg1 arg1, string name, params string[] ownerHierarchy)
        {
            var measurement = BeginMeasurement(name, ownerHierarchy);
            var result = call(arg1);
            EndMeasurement(measurement);
            return result;
        }
        public TResult MonitorCall<TArg1, TArg2, TResult>(Func<TArg1, TArg2, TResult> call, TArg1 arg1, TArg2 arg2,
            string name, params string[] ownerHierarchy)
        {
            var measurement = BeginMeasurement(name, ownerHierarchy);
            var result = call(arg1, arg2);
            EndMeasurement(measurement);
            return result;
        }
        public TResult MonitorCall<TArg1, TArg2, TArg3, TResult>(Func<TArg1, TArg2, TArg3, TResult> call,
            TArg1 arg1, TArg2 arg2, TArg3 arg3,
            string name, params string[] ownerHierarchy)
        {
            var measurement = BeginMeasurement(name, ownerHierarchy);
            var result = call(arg1, arg2, arg3);
            EndMeasurement(measurement);
            return result;
        }
        #endregion
        #region Delegate monitors
        public TResult MonitorCall<TResult>(Delegate call, string name, string[] ownerHierarchy, params object[] parameters)
        {
            var measurement = BeginMeasurement(name, ownerHierarchy);
            var result = (TResult)call.DynamicInvoke(parameters);
            EndMeasurement(measurement);
            return result;
        }
        public void MonitorCall(Delegate call, string name, string[] ownerHierarchy, params object[] parameters)
        {
            var measurement = BeginMeasurement(name, ownerHierarchy);
            call.DynamicInvoke(parameters);
            EndMeasurement(measurement);
        }
        #endregion
        #region Routine monitor helpers
        public void MonitorCall(Action call, string name, params string[] ownerHierarchy)
        {
            var measurement = BeginMeasurement(name, ownerHierarchy);
            call();
            EndMeasurement(measurement);
        }
        public void MonitorCall<TArg1>(Action<TArg1> call, TArg1 arg1, string name, params string[] ownerHierarchy)
        {
            var measurement = BeginMeasurement(name, ownerHierarchy);
            call(arg1);
            EndMeasurement(measurement);
        }
        public void MonitorCall<TArg1, TArg2>(Action<TArg1, TArg2> call, TArg1 arg1, TArg2 arg2,
            string name, params string[] ownerHierarchy)
        {
            var measurement = BeginMeasurement(name, ownerHierarchy);
            call(arg1, arg2);
            EndMeasurement(measurement);
        }
        public void MonitorCall<TArg1, TArg2, TArg3>(Action<TArg1, TArg2, TArg3> call,
            TArg1 arg1, TArg2 arg2, TArg3 arg3,
            string name, params string[] ownerHierarchy)
        {
            var measurement = BeginMeasurement(name, ownerHierarchy);
            call(arg1, arg2, arg3);
            EndMeasurement(measurement);
        }
        #endregion

        public void EndMeasurement(Measurement measurement)
        {
            _monitors[measurement.GetUID()].Stop();
            measurement.AddMeasurement(_monitors[measurement.GetUID()].Elapsed);
        }

        public void EndMeasurement(string name, params string[] ownerHierarchy)
        {
            Measurement owner = null;
            //Build the tree
            for (var i = ownerHierarchy.Length - 1; i >= 0; i--)
            {
                owner = CreateOrGetSubNode(ownerHierarchy[i], owner);
            }

            //Get my node
            var m = CreateOrGetSubNode(name, owner);
            EndMeasurement(m);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var m in _rootNodes)
            {
                sb.AppendLine(m.Value.ToString());
                RecursiveGetSubNodes(m.Value, sb, 1);
            }

            return sb.ToString();
        }

        private void RecursiveGetSubNodes(Measurement m, StringBuilder sb, int depth)
        {
            var tabs = new String('\t', depth);
            foreach (var mx in m.Submeasurements)
            {
                sb.AppendLine(tabs + mx.Value.ToString());
                RecursiveGetSubNodes(mx.Value, sb, depth + 1);
            }
        }

        public class Measurement
        {
            private string uid;
            public string Name { get; set; }
            private Dictionary<string, Measurement> _sub = new Dictionary<string, Measurement>();
            public IReadOnlyDictionary<string, Measurement> Submeasurements { get { return _sub; } }
            public Measurement Owner { get; private set; }
#if TRACK_ALL_MEASUREMENTS
            private List<TimeSpan> _all = new List<TimeSpan>(1000);
#else
            private int _nextInsertionPosition = 0;
            private int _currentPosition;
            private TimeSpan[] _all = new TimeSpan[100];
#endif
            public IList<TimeSpan> AllMeasurements { get { return _all; } }
            public TimeSpan MostRecent
            {
                get
                {
#if TRACK_ALL_MEASUREMENTS
                    if (AllMeasurements.Count == 0) return TimeSpan.Zero;
                    return AllMeasurements[_all.Length - 1];
#else
                    return _all[_currentPosition];
#endif
                }
                set { AllMeasurementsAdd(value); }
            }
            private TimeSpan[] _lastThirty = new TimeSpan[30];
            public IEnumerable<TimeSpan> LastThirty { get { return _lastThirty; } }
            public TimeSpan AverageLastThirty { get; private set; }

            private void AllMeasurementsAdd(TimeSpan span)
            {
#if TRACK_ALL_MEASUREMENTS
                _all.Add(span);
#else
                _currentPosition = _nextInsertionPosition;
                _all[_currentPosition] = span;
                _nextInsertionPosition++;
                _nextInsertionPosition = _nextInsertionPosition % _all.Length;
#endif
            }

            public Measurement(string name, Measurement owner = null)
            {
                uid = Guid.NewGuid().ToString();
                Name = name;
                if (owner != null)
                    owner._sub.Add(name, this);

                Owner = owner;
            }

            public void AddMeasurement(TimeSpan time)
            {
                AllMeasurementsAdd(time);
                ShiftArray(time);
                AverageLastThirty = CalculateAverage();
            }

            private void ShiftArray(TimeSpan newSpan)
            {
                for (var i = 0; i < _lastThirty.Length - 1; i++)
                    _lastThirty[i] = _lastThirty[i + 1];

                _lastThirty[_lastThirty.Length - 1] = newSpan;
            }

            private TimeSpan CalculateAverage()
            {
                long elapsed = 0;
                foreach (var ts in LastThirty)
                    elapsed += ts.Ticks;

                return new TimeSpan(elapsed / _lastThirty.Length);
            }

            public override string ToString()
            {
                return Name + ", Avg: " +
                    AverageLastThirty.TotalMilliseconds.ToString("N3") + "ms, Now: " +
                    MostRecent.TotalMilliseconds.ToString("N3") + "ms";
            }

            public string GetUID()
            {
                return uid;
            }
        }
    }
}
