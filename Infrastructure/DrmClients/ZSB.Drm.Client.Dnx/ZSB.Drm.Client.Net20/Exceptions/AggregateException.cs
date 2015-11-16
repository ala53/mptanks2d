using System;
using System.Collections.Generic;
using System.Text;

namespace ZSB.Drm.Client.Exceptions
{
    /// <summary>
    /// A synchronous mapping of an exception that occurred in an Async function
    /// </summary>
    public class AggregateException : Exception
    {
        public List<Exception> InnerExceptions { get; set; }
        public AggregateException(Exception inner) : base("Exceptions: Count = 1", inner)
        {
            InnerExceptions = new List<Exception>();
            InnerExceptions.Add(inner);
        }
    }
}
