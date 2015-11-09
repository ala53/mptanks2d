using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ZSB.Drm.Client
{
    public class Promise<T>
    {
        /// <summary>
        /// Gets whether the promise has succeeded.
        /// </summary>
        public bool Succeeded { get; internal set; }
        /// <summary>
        /// Gets whether the promise failed. If so, the <see cref="State"/> will contain the relevant exception object.
        /// </summary>
        public bool Failed { get; internal set; }
        /// <summary>
        /// Gets whether the execution of the promise has completed, regardless of whether if failed or succeeded.
        /// </summary>
        public bool Finished => Succeeded || Failed;
        /// <summary>
        /// Gets the current status of the promise as a human readable string
        /// </summary>
        public string Status { get; internal set; }
        public object State { get; set; }
        public T Result => (T)Result;

        public T Wait()
        {
            while (!Finished) Thread.Sleep(0);

            if (Succeeded)
                return Result;
            else throw (Exception)State;
        }
    }
}
