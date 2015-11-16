using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ZSB.Drm.Client
{
    public class AsyncScheduler
    {
        public delegate T AsyncReturnsMethod<T>();
        public static Task<T> Schedule<T>(AsyncReturnsMethod<T> asyncTask)
        {
            var promise = new Task<T>();
            ThreadPool.QueueUserWorkItem(state =>
            {
                try
                {
                    promise.Status = TaskStatus.Running;
                    promise.Result = asyncTask();
                    promise.Status = TaskStatus.RanToCompletion;
                }
                catch (Exception ex)
                {
                    promise.Status = TaskStatus.Faulted;
                    promise.SetException(ex);
                }
            });

            return promise;
        }

        public delegate void AsyncNoReturns();
        public static Task Schedule(AsyncNoReturns asyncTask)
        {
            var promise = new Task();
            ThreadPool.QueueUserWorkItem(state =>
            {
                try
                {
                    promise.Status = TaskStatus.Running;
                    asyncTask();
                    promise.Status = TaskStatus.RanToCompletion;
                }
                catch (Exception ex)
                {
                    promise.Status = TaskStatus.Faulted;
                    promise.SetException(ex);
                }
            });

            return promise;
        }
    }
}
