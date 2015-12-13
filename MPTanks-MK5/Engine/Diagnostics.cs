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
        //firestar23373: did you comment war and peace into it?

        private Dictionary<string, Measurement> _rootNodes =
            new Dictionary<string, Measurement>();
        public IReadOnlyDictionary<string, Measurement> Roots { get { return _rootNodes; } }
        public bool IsMeasuring(string name, params string[] ownerHierarchy)
        {
            return GetMeasurement(name, ownerHierarchy).Stopwatch.IsRunning;

        }
        #region IsMeasuring overloads
        public bool IsMeasuring(string name) => IsMeasuring(name, _noneArray);
        public bool IsMeasuring(string name, string h1)
        {
            _oneArray[0] = h1;
            return IsMeasuring(name, _oneArray);
        }
        public bool IsMeasuring(string name, string h1, string h2)
        {
            _twoArray[0] = h1; _twoArray[1] = h2;
            return IsMeasuring(name, _twoArray);
        }
        public bool IsMeasuring(string name, string h1, string h2, string h3)
        {
            _threeArray[0] = h1; _threeArray[1] = h2; _threeArray[2] = h3;
            return IsMeasuring(name, _threeArray);
        }
        public bool IsMeasuring(string name, string h1, string h2, string h3, string h4)
        {
            _fourArray[0] = h1; _fourArray[1] = h2; _fourArray[2] = h3; _fourArray[3] = h4;
            return IsMeasuring(name, _fourArray);
        }
        public bool IsMeasuring(string name, string h1, string h2, string h3, string h4, string h5)
        {
            _fiveArray[0] = h1; _fiveArray[1] = h2; _fiveArray[2] = h3; _fiveArray[3] = h4;
            _fiveArray[4] = h5;
            return IsMeasuring(name, _fiveArray);
        }
        public bool IsMeasuring(string name, string h1, string h2, string h3, string h4, string h5, string h6)
        {
            _sixArray[0] = h1; _sixArray[1] = h2; _sixArray[2] = h3;
            _sixArray[3] = h4; _sixArray[4] = h5; _sixArray[5] = h6;

            return IsMeasuring(name, _sixArray);
        }
        public bool IsMeasuring(string name, string h1, string h2, string h3, string h4, string h5,
            string h6, string h7)
        {
            _sevenArray[0] = h1; _sevenArray[1] = h2; _sevenArray[2] = h3; _sevenArray[3] = h4;
            _sevenArray[4] = h5; _sevenArray[5] = h6; _sevenArray[6] = h7;
            return IsMeasuring(name, _sevenArray);
        }
        public bool IsMeasuring(string name, string h1, string h2, string h3, string h4, string h5,
            string h6, string h7, string h8)
        {
            _eightArray[0] = h1; _eightArray[1] = h2; _eightArray[2] = h3; _eightArray[3] = h4;
            _eightArray[4] = h5; _eightArray[5] = h6; _eightArray[6] = h7; _eightArray[7] = h8;
            return IsMeasuring(name, _eightArray);
        }
        #endregion
        #region Static Arrays
        private string[] _noneArray = new string[0];
        private string[] _oneArray = new string[1];
        private string[] _twoArray = new string[2];
        private string[] _threeArray = new string[3];
        private string[] _fourArray = new string[4];
        private string[] _fiveArray = new string[5];
        private string[] _sixArray = new string[6];
        private string[] _sevenArray = new string[7];
        private string[] _eightArray = new string[8];
        #endregion
        public Measurement BeginMeasurement(string name, params string[] ownerHierarchy)
        {
            var m = GetMeasurement(name, ownerHierarchy);
            BeginMonitor(m);
            return m;
        }
        #region BeginMeasurement overloads
        public Measurement BeginMeasurement(string name) => BeginMeasurement(name, _noneArray);
        public Measurement BeginMeasurement(string name, string h1)
        {
            _oneArray[0] = h1;
            return BeginMeasurement(name, _oneArray);
        }
        public Measurement BeginMeasurement(string name, string h1, string h2)
        {
            _twoArray[0] = h1; _twoArray[1] = h2;
            return BeginMeasurement(name, _twoArray);
        }
        public Measurement BeginMeasurement(string name, string h1, string h2, string h3)
        {
            _threeArray[0] = h1; _threeArray[1] = h2; _threeArray[2] = h3;
            return BeginMeasurement(name, _threeArray);
        }
        public Measurement BeginMeasurement(string name, string h1, string h2, string h3, string h4)
        {
            _fourArray[0] = h1; _fourArray[1] = h2; _fourArray[2] = h3; _fourArray[3] = h4;
            return BeginMeasurement(name, _fourArray);
        }
        public Measurement BeginMeasurement(string name, string h1, string h2, string h3, string h4, string h5)
        {
            _fiveArray[0] = h1; _fiveArray[1] = h2; _fiveArray[2] = h3; _fiveArray[3] = h4;
            _fiveArray[4] = h5;
            return BeginMeasurement(name, _fiveArray);
        }
        public Measurement BeginMeasurement(string name, string h1, string h2, string h3, string h4, string h5, string h6)
        {
            _sixArray[0] = h1; _sixArray[1] = h2; _sixArray[2] = h3;
            _sixArray[3] = h4; _sixArray[4] = h5; _sixArray[5] = h6;

            return BeginMeasurement(name, _sixArray);
        }
        public Measurement BeginMeasurement(string name, string h1, string h2, string h3, string h4, string h5,
            string h6, string h7)
        {
            _sevenArray[0] = h1; _sevenArray[1] = h2; _sevenArray[2] = h3; _sevenArray[3] = h4;
            _sevenArray[4] = h5; _sevenArray[5] = h6; _sevenArray[6] = h7;
            return BeginMeasurement(name, _sevenArray);
        }
        public Measurement BeginMeasurement(string name, string h1, string h2, string h3, string h4, string h5,
            string h6, string h7, string h8)
        {
            _eightArray[0] = h1; _eightArray[1] = h2; _eightArray[2] = h3; _eightArray[3] = h4;
            _eightArray[4] = h5; _eightArray[5] = h6; _eightArray[6] = h7; _eightArray[7] = h8;
            return BeginMeasurement(name, _eightArray);
        }
        #endregion
        private Measurement GetMeasurement(string name, params string[] ownerHierarchy)
        {
            Measurement owner = null;
            //Build the tree
            for (var i = ownerHierarchy.Length - 1; i >= 0; i--)
            {
                owner = CreateOrGetSubNode(ownerHierarchy[i], owner);
            }

            //Get my node
            return CreateOrGetSubNode(name, owner);
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
            measurement.Stopwatch.Restart();

        }

        #region Function monitor helpers
        public TResult MonitorCall<TResult>(Func<TResult> call, string name, params string[] ownerHierarchy)
        {
            var measurement = BeginMeasurement(name, ownerHierarchy);
            var result = call();
            EndMeasurement(measurement);
            return result;
        }
        #region MonitorCall<TResult> Overloads
        public TResult MonitorCall<TResult>(Func<TResult> call, string name) => MonitorCall(call, name, _noneArray);
        public TResult MonitorCall<TResult>(Func<TResult> call, string name, string h1)
        {
            _oneArray[0] = h1;
            return MonitorCall(call, name, _oneArray);
        }
        public TResult MonitorCall<TResult>(Func<TResult> call, string name, string h1, string h2)
        {
            _twoArray[0] = h1; _twoArray[1] = h2;
            return MonitorCall(call, name, _twoArray);
        }
        public TResult MonitorCall<TResult>(Func<TResult> call, string name, string h1, string h2, string h3)
        {
            _threeArray[0] = h1; _threeArray[1] = h2; _threeArray[2] = h3;
            return MonitorCall(call, name, _threeArray);
        }
        public TResult MonitorCall<TResult>(Func<TResult> call, string name, string h1, string h2, string h3, string h4)
        {
            _fourArray[0] = h1; _fourArray[1] = h2; _fourArray[2] = h3; _fourArray[3] = h4;
            return MonitorCall(call, name, _fourArray);
        }
        public TResult MonitorCall<TResult>(Func<TResult> call, string name, string h1, string h2, string h3, string h4, string h5)
        {
            _fiveArray[0] = h1; _fiveArray[1] = h2; _fiveArray[2] = h3; _fiveArray[3] = h4;
            _fiveArray[4] = h5;
            return MonitorCall(call, name, _fiveArray);
        }
        public TResult MonitorCall<TResult>(Func<TResult> call, string name, string h1, string h2, string h3, string h4, string h5, string h6)
        {
            _sixArray[0] = h1; _sixArray[1] = h2; _sixArray[2] = h3;
            _sixArray[3] = h4; _sixArray[4] = h5; _sixArray[5] = h6;

            return MonitorCall(call, name, _sixArray);
        }
        public TResult MonitorCall<TResult>(Func<TResult> call, string name, string h1, string h2, string h3, string h4, string h5,
            string h6, string h7)
        {
            _sevenArray[0] = h1; _sevenArray[1] = h2; _sevenArray[2] = h3; _sevenArray[3] = h4;
            _sevenArray[4] = h5; _sevenArray[5] = h6; _sevenArray[6] = h7;
            return MonitorCall(call, name, _sevenArray);
        }
        public TResult MonitorCall<TResult>(Func<TResult> call, string name, string h1, string h2, string h3, string h4, string h5,
            string h6, string h7, string h8)
        {
            _eightArray[0] = h1; _eightArray[1] = h2; _eightArray[2] = h3; _eightArray[3] = h4;
            _eightArray[4] = h5; _eightArray[5] = h6; _eightArray[6] = h7; _eightArray[7] = h8;
            return MonitorCall(call, name, _eightArray);
        }
        #endregion
        public TResult MonitorCall<TArg1, TResult>(Func<TArg1, TResult> call, TArg1 arg1, string name, params string[] ownerHierarchy)
        {
            var measurement = BeginMeasurement(name, ownerHierarchy);
            var result = call(arg1);
            EndMeasurement(measurement);
            return result;
        }
        #region MonitorCall<TArg1, TResult> Overloads
        public TResult MonitorCall<TArg1, TResult>(Func<TArg1, TResult> call, TArg1 arg1, string name) => MonitorCall(call, arg1, name, _noneArray);
        public TResult MonitorCall<TArg1, TResult>(Func<TArg1, TResult> call, TArg1 arg1, string name, string h1)
        {
            _oneArray[0] = h1;
            return MonitorCall(call, arg1, name, _oneArray);
        }
        public TResult MonitorCall<TArg1, TResult>(Func<TArg1, TResult> call, TArg1 arg1, string name, string h1, string h2)
        {
            _twoArray[0] = h1; _twoArray[1] = h2;
            return MonitorCall(call, arg1, name, _twoArray);
        }
        public TResult MonitorCall<TArg1, TResult>(Func<TArg1, TResult> call, TArg1 arg1, string name, string h1, string h2, string h3)
        {
            _threeArray[0] = h1; _threeArray[1] = h2; _threeArray[2] = h3;
            return MonitorCall(call, arg1, name, _threeArray);
        }
        public TResult MonitorCall<TArg1, TResult>(Func<TArg1, TResult> call, TArg1 arg1, string name, string h1, string h2, string h3, string h4)
        {
            _fourArray[0] = h1; _fourArray[1] = h2; _fourArray[2] = h3; _fourArray[3] = h4;
            return MonitorCall(call, arg1, name, _fourArray);
        }
        public TResult MonitorCall<TArg1, TResult>(Func<TArg1, TResult> call, TArg1 arg1, string name, string h1, string h2, string h3, string h4, string h5)
        {
            _fiveArray[0] = h1; _fiveArray[1] = h2; _fiveArray[2] = h3; _fiveArray[3] = h4;
            _fiveArray[4] = h5;
            return MonitorCall(call, arg1, name, _fiveArray);
        }
        public TResult MonitorCall<TArg1, TResult>(Func<TArg1, TResult> call, TArg1 arg1, string name, string h1, string h2, string h3, string h4, string h5, string h6)
        {
            _sixArray[0] = h1; _sixArray[1] = h2; _sixArray[2] = h3;
            _sixArray[3] = h4; _sixArray[4] = h5; _sixArray[5] = h6;

            return MonitorCall(call, arg1, name, _sixArray);
        }
        public TResult MonitorCall<TArg1, TResult>(Func<TArg1, TResult> call, TArg1 arg1, string name, string h1, string h2, string h3, string h4, string h5,
            string h6, string h7)
        {
            _sevenArray[0] = h1; _sevenArray[1] = h2; _sevenArray[2] = h3; _sevenArray[3] = h4;
            _sevenArray[4] = h5; _sevenArray[5] = h6; _sevenArray[6] = h7;
            return MonitorCall(call, arg1, name, _sevenArray);
        }
        public TResult MonitorCall<TArg1, TResult>(Func<TArg1, TResult> call, TArg1 arg1, string name, string h1, string h2, string h3, string h4, string h5,
            string h6, string h7, string h8)
        {
            _eightArray[0] = h1; _eightArray[1] = h2; _eightArray[2] = h3; _eightArray[3] = h4;
            _eightArray[4] = h5; _eightArray[5] = h6; _eightArray[6] = h7; _eightArray[7] = h8;
            return MonitorCall(call, arg1, name, _eightArray);
        }
        #endregion
        public TResult MonitorCall<TArg1, TArg2, TResult>(Func<TArg1, TArg2, TResult> call, TArg1 arg1, TArg2 arg2,
            string name, params string[] ownerHierarchy)
        {
            var measurement = BeginMeasurement(name, ownerHierarchy);
            var result = call(arg1, arg2);
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
        #region MonitorCall Overloads
        public void MonitorCall(Action call, string name) => MonitorCall(call, name, _noneArray);
        public void MonitorCall(Action call, string name, string h1)
        {
            _oneArray[0] = h1;
            MonitorCall(call, name, _oneArray);
        }
        public void MonitorCall(Action call, string name, string h1, string h2)
        {
            _twoArray[0] = h1; _twoArray[1] = h2;
            MonitorCall(call, name, _twoArray);
        }
        public void MonitorCall(Action call, string name, string h1, string h2, string h3)
        {
            _threeArray[0] = h1; _threeArray[1] = h2; _threeArray[2] = h3;
            MonitorCall(call, name, _threeArray);
        }
        public void MonitorCall(Action call, string name, string h1, string h2, string h3, string h4)
        {
            _fourArray[0] = h1; _fourArray[1] = h2; _fourArray[2] = h3; _fourArray[3] = h4;
            MonitorCall(call, name, _fourArray);
        }
        public void MonitorCall(Action call, string name, string h1, string h2, string h3, string h4, string h5)
        {
            _fiveArray[0] = h1; _fiveArray[1] = h2; _fiveArray[2] = h3; _fiveArray[3] = h4;
            _fiveArray[4] = h5;
            MonitorCall(call, name, _fiveArray);
        }
        public void MonitorCall(Action call, string name, string h1, string h2, string h3, string h4, string h5, string h6)
        {
            _sixArray[0] = h1; _sixArray[1] = h2; _sixArray[2] = h3;
            _sixArray[3] = h4; _sixArray[4] = h5; _sixArray[5] = h6;

            MonitorCall(call, name, _sixArray);
        }
        public void MonitorCall(Action call, string name, string h1, string h2, string h3, string h4, string h5,
            string h6, string h7)
        {
            _sevenArray[0] = h1; _sevenArray[1] = h2; _sevenArray[2] = h3; _sevenArray[3] = h4;
            _sevenArray[4] = h5; _sevenArray[5] = h6; _sevenArray[6] = h7;
            MonitorCall(call, name, _sevenArray);
        }
        public void MonitorCall(Action call, string name, string h1, string h2, string h3, string h4, string h5,
            string h6, string h7, string h8)
        {
            _eightArray[0] = h1; _eightArray[1] = h2; _eightArray[2] = h3; _eightArray[3] = h4;
            _eightArray[4] = h5; _eightArray[5] = h6; _eightArray[6] = h7; _eightArray[7] = h8;
            MonitorCall(call, name, _eightArray);
        }
        #endregion
        public void MonitorCall<TArg1>(Action<TArg1> call, TArg1 arg1, string name, params string[] ownerHierarchy)
        {
            var measurement = BeginMeasurement(name, ownerHierarchy);
            call(arg1);
            EndMeasurement(measurement);
        }
        #region MonitorCall<TArg1> Overloads
        public void MonitorCall<TArg1>(Action<TArg1> call, TArg1 arg1, string name) => MonitorCall(call, arg1, name, _noneArray);
        public void MonitorCall<TArg1>(Action<TArg1> call, TArg1 arg1, string name, string h1)
        {
            _oneArray[0] = h1;
            MonitorCall(call, arg1, name, _oneArray);
        }
        public void MonitorCall<TArg1>(Action<TArg1> call, TArg1 arg1, string name, string h1, string h2)
        {
            _twoArray[0] = h1; _twoArray[1] = h2;
            MonitorCall(call, arg1, name, _twoArray);
        }
        public void MonitorCall<TArg1>(Action<TArg1> call, TArg1 arg1, string name, string h1, string h2, string h3)
        {
            _threeArray[0] = h1; _threeArray[1] = h2; _threeArray[2] = h3;
            MonitorCall(call, arg1, name, _threeArray);
        }
        public void MonitorCall<TArg1>(Action<TArg1> call, TArg1 arg1, string name, string h1, string h2, string h3, string h4)
        {
            _fourArray[0] = h1; _fourArray[1] = h2; _fourArray[2] = h3; _fourArray[3] = h4;
            MonitorCall(call, arg1, name, _fourArray);
        }
        public void MonitorCall<TArg1>(Action<TArg1> call, TArg1 arg1, string name, string h1, string h2, string h3, string h4, string h5)
        {
            _fiveArray[0] = h1; _fiveArray[1] = h2; _fiveArray[2] = h3; _fiveArray[3] = h4;
            _fiveArray[4] = h5;
            MonitorCall(call, arg1, name, _fiveArray);
        }
        public void MonitorCall<TArg1>(Action<TArg1> call, TArg1 arg1, string name, string h1, string h2, string h3, string h4, string h5, string h6)
        {
            _sixArray[0] = h1; _sixArray[1] = h2; _sixArray[2] = h3;
            _sixArray[3] = h4; _sixArray[4] = h5; _sixArray[5] = h6;

            MonitorCall(call, arg1, name, _sixArray);
        }
        public void MonitorCall<TArg1>(Action<TArg1> call, TArg1 arg1, string name, string h1, string h2, string h3, string h4, string h5,
            string h6, string h7)
        {
            _sevenArray[0] = h1; _sevenArray[1] = h2; _sevenArray[2] = h3; _sevenArray[3] = h4;
            _sevenArray[4] = h5; _sevenArray[5] = h6; _sevenArray[6] = h7;
            MonitorCall(call, arg1, name, _sevenArray);
        }
        public void MonitorCall<TArg1>(Action<TArg1> call, TArg1 arg1, string name, string h1, string h2, string h3, string h4, string h5,
            string h6, string h7, string h8)
        {
            _eightArray[0] = h1; _eightArray[1] = h2; _eightArray[2] = h3; _eightArray[3] = h4;
            _eightArray[4] = h5; _eightArray[5] = h6; _eightArray[6] = h7; _eightArray[7] = h8;
            MonitorCall(call, arg1, name, _eightArray);
        }
        #endregion
        public void MonitorCall<TArg1, TArg2>(Action<TArg1, TArg2> call, TArg1 arg1, TArg2 arg2,
            string name, params string[] ownerHierarchy)
        {
            var measurement = BeginMeasurement(name, ownerHierarchy);
            call(arg1, arg2);
            EndMeasurement(measurement);
        }
        #endregion
        #region Loop monitor helpers
        public void MonitorForEach<T>(IList<T> items, Action<T> itemIterator, string name, params string[] ownerHierarchy)
        {
            BeginMeasurement(name, ownerHierarchy);

            var timings = new long[items.Count];
            var iterationAverage = GetMeasurement(name + " (Loop Body Average)", ownerHierarchy);
            iterationAverage.Stopwatch.Restart();

            for (var i = 0; i < items.Count; i++)
            {
                iterationAverage.Stopwatch.Restart();
                itemIterator(items[i]);
                timings[i] = iterationAverage.Stopwatch.ElapsedTicks;
            }
            EndMeasurement(name, ownerHierarchy);

            iterationAverage.AddMeasurement(new TimeSpan((long)timings.Average()));
        }
        private object _monitorForEachTempVariable;
        private object _monitorForEachCallFunction;
        public void MonitorForEach<T>(IEnumerable<T> items, Action<T> itemIterator, string name, params string[] ownerHierarchy)
        {
            _monitorForEachTempVariable = items;
            _monitorForEachCallFunction = itemIterator;
            MonitorCall(() =>
            {
                var a = (IEnumerable<T>)_monitorForEachTempVariable;
                var c = _monitorForEachCallFunction as Action<T>;
                foreach (var itm in a) c(itm);
            }, name, ownerHierarchy);
            _monitorForEachCallFunction = null;
            _monitorForEachTempVariable = null;
        }

        public void MonitorForEach<T>(IEnumerable<T> items, Action<T> itemIterator, string name)
        {
            MonitorForEach(items, itemIterator, name, _noneArray);
        }
        public void MonitorForEach<T>(IEnumerable<T> items, Action<T> itemIterator, string name, string h1)
        {
            _oneArray[0] = h1;
            MonitorForEach(items, itemIterator, name, _oneArray);
        }

        public void MonitorWhile(Action loopBody, Func<bool> breakEvaluator, string name, params string[] ownerHierarchy)
        {
            BeginMeasurement(name, ownerHierarchy);

            var bodyTimes = new List<long>();
            var breakEvaluationTimes = new List<long>();
            var bodyMeasurement = GetMeasurement(name + " (Loop body)");
            var breakEvalautionMeasurement = GetMeasurement(name + " (Head/Break evaluator)");

            while (true)
            {
                breakEvalautionMeasurement.Stopwatch.Restart();
                var shouldBreak = breakEvaluator();
                breakEvaluationTimes.Add(breakEvalautionMeasurement.Stopwatch.ElapsedTicks);
                if (shouldBreak) break;

                bodyMeasurement.Stopwatch.Restart();
                loopBody();
                bodyTimes.Add(bodyMeasurement.Stopwatch.ElapsedTicks);
            }
            EndMeasurement(name, ownerHierarchy);

            bodyMeasurement.AddMeasurement(new TimeSpan((long)bodyTimes.Average()));
            breakEvalautionMeasurement.AddMeasurement(new TimeSpan((long)breakEvaluationTimes.Average()));
        }
        #endregion

        public void EndMeasurement(Measurement measurement)
        {
            measurement.Stopwatch.Stop();
            measurement.AddMeasurement(measurement.Stopwatch.Elapsed);
        }

        public void EndMeasurement(string name, params string[] ownerHierarchy) =>
            EndMeasurement(GetMeasurement(name, ownerHierarchy));
        #region Beginvoid overloads
        public void EndMeasurement(string name) => EndMeasurement(name, _noneArray);
        public void EndMeasurement(string name, string h1)
        {
            _oneArray[0] = h1;
            EndMeasurement(name, _oneArray);
        }
        public void EndMeasurement(string name, string h1, string h2)
        {
            _twoArray[0] = h1; _twoArray[1] = h2;
            EndMeasurement(name, _twoArray);
        }
        public void EndMeasurement(string name, string h1, string h2, string h3)
        {
            _threeArray[0] = h1; _threeArray[1] = h2; _threeArray[2] = h3;
            EndMeasurement(name, _threeArray);
        }
        public void EndMeasurement(string name, string h1, string h2, string h3, string h4)
        {
            _fourArray[0] = h1; _fourArray[1] = h2; _fourArray[2] = h3; _fourArray[3] = h4;
            EndMeasurement(name, _fourArray);
        }
        public void EndMeasurement(string name, string h1, string h2, string h3, string h4, string h5)
        {
            _fiveArray[0] = h1; _fiveArray[1] = h2; _fiveArray[2] = h3; _fiveArray[3] = h4;
            _fiveArray[4] = h5;
            EndMeasurement(name, _fiveArray);
        }
        public void EndMeasurement(string name, string h1, string h2, string h3, string h4, string h5, string h6)
        {
            _sixArray[0] = h1; _sixArray[1] = h2; _sixArray[2] = h3;
            _sixArray[3] = h4; _sixArray[4] = h5; _sixArray[5] = h6;

            EndMeasurement(name, _sixArray);
        }
        public void EndMeasurement(string name, string h1, string h2, string h3, string h4, string h5,
            string h6, string h7)
        {
            _sevenArray[0] = h1; _sevenArray[1] = h2; _sevenArray[2] = h3; _sevenArray[3] = h4;
            _sevenArray[4] = h5; _sevenArray[5] = h6; _sevenArray[6] = h7;
            EndMeasurement(name, _sevenArray);
        }
        public void EndMeasurement(string name, string h1, string h2, string h3, string h4, string h5,
            string h6, string h7, string h8)
        {
            _eightArray[0] = h1; _eightArray[1] = h2; _eightArray[2] = h3; _eightArray[3] = h4;
            _eightArray[4] = h5; _eightArray[5] = h6; _eightArray[6] = h7; _eightArray[7] = h8;
            EndMeasurement(name, _eightArray);
        }
        #endregion

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
            var tabs = new string('\t', depth);
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
            internal Stopwatch Stopwatch { get; private set; }
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
                Stopwatch = new Stopwatch();
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
