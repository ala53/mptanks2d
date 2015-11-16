using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ZSB.Drm.Client.Exceptions;

namespace System.Threading.Tasks { } //Empty: so we don't need to replace the "using System.Threading.Tasks" for .Net 2.0
namespace ZSB.Drm.Client
{
    public enum TaskStatus
    {
        WaitingToRun,
        Running,
        RanToCompletion,
        Faulted
    }
    public class Task
    {
        /// <summary>
        /// Gets whether the promise has succeeded.
        /// </summary>
        public bool IsCompleted => Status == TaskStatus.RanToCompletion;
        /// <summary>
        /// Gets whether the promise failed. If so, the <see cref="Exception"/> will contain the relevant exception object.
        /// </summary>
        public bool IsFaulted => Status == TaskStatus.Faulted;
        /// <summary>
        /// Gets whether the execution of the promise has completed, regardless of whether if failed or succeeded.
        /// </summary>
        public bool Running => Status == TaskStatus.Running;
        /// <summary>
        /// Gets the current status of the promise as a human readable string
        /// </summary>
        public TaskStatus Status { get; set; } = TaskStatus.WaitingToRun;
        private Exception _ex;
        public AggregateException Exception { get { return new AggregateException(_ex); } }
        public void SetException(Exception ex) => _ex = ex;
        public void Wait()
        {
            while (!Running) Thread.Sleep(0);

            if (IsCompleted)
                return;
            else throw new AggregateException(Exception);
        }

        public static Task<T> Run<T>(AsyncScheduler.AsyncReturnsMethod<T> action) =>
            AsyncScheduler.Schedule(action);
        public static Task Run(AsyncScheduler.AsyncNoReturns action) =>
            AsyncScheduler.Schedule(action);
    }
    /// <summary>
    /// A promise of an asynchronous task that
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Task<T> : Task
    {
        private T _backingResult;
        public T Result { get { Wait(); return _backingResult; } set { _backingResult = value; } }
        public void SetResult(T result) => Result = result;
    }
}
